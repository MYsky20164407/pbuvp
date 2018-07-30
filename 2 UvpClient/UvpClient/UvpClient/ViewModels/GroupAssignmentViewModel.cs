using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     小组作业ViewModel。
    /// </summary>
    public class GroupAssignmentViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     小组作业服务。
        /// </summary>
        private readonly IGroupAssignmentService _groupAssignmentService;

        /// <summary>
        ///     小组作业。
        /// </summary>
        private GroupAssignment _groupAssignment;

        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand _refreshCommand;

        /// <summary>
        ///     正在刷新。
        /// </summary>
        private bool _refreshing;

        /// <summary>
        ///     提交命令。
        /// </summary>
        private RelayCommand _submitCommand;

        /// <summary>
        ///     正在提交。
        /// </summary>
        private bool _submitting;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="groupAssignmentService">小组作业服务。</param>
        public GroupAssignmentViewModel(IDialogService dialogService,
            IGroupAssignmentService groupAssignmentService) {
            _dialogService = dialogService;
            _groupAssignmentService = groupAssignmentService;
        }

        /// <summary>
        ///     正在刷新。
        /// </summary>
        public bool Refreshing {
            get => _refreshing;
            set => Set(nameof(Refreshing), ref _refreshing, value);
        }

        /// <summary>
        ///     小组作业id。
        /// </summary>
        public int GroupAssignmentId { get; set; }

        /// <summary>
        ///     小组作业。
        /// </summary>
        public GroupAssignment GroupAssignment {
            get => _groupAssignment;
            set => Set(nameof(GroupAssignment), ref _groupAssignment, value);
        }

        /// <summary>
        ///     刷新命令。
        /// </summary>
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                Refreshing = true;
                _refreshCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _groupAssignmentService.GetAsync(GroupAssignmentId);
                Refreshing = false;
                _refreshCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        GroupAssignment = serviceResult.Result;
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }
            }));

        /// <summary>
        ///     提交命令。
        /// </summary>
        public RelayCommand SubmitCommand =>
            _submitCommand ?? (_submitCommand = new RelayCommand(async () => {
                if (!Uri.TryCreate(GroupAssignment.Solution, UriKind.Absolute,
                    out var myUri)) {
                    await _dialogService.ShowAsync(App.SolutionUrlErrorMessage);
                    return;
                }

                Submitting = true;
                _submitCommand.RaiseCanExecuteChanged();
                var servideResult =
                    await _groupAssignmentService.SubmitAsync(GroupAssignmentId,
                        GroupAssignment);
                Submitting = false;
                _submitCommand.RaiseCanExecuteChanged();

                switch (servideResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.NoContent:
                        await _dialogService.ShowAsync(
                            App.SolutionSubmittedMessage);
                        break;
                    case ServiceResultStatus.BadRequest:
                        await _dialogService.ShowAsync(servideResult.Message);
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + servideResult.Message);
                        break;
                }
            }, () => !Submitting));

        /// <summary>
        ///     正在提交。
        /// </summary>
        public bool Submitting {
            get => _submitting;
            set => Set(nameof(Submitting), ref _submitting, value);
        }
    }
}