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
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _navigationService;

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
        /// <param name="navigationService">根导航服务。</param>
        public GroupAssignmentViewModel(IDialogService dialogService,
            IGroupAssignmentService groupAssignmentService,
            IRootNavigationService navigationService) {
            _dialogService = dialogService;
            _groupAssignmentService = groupAssignmentService;
            _navigationService = navigationService;
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
                SubmitCommand.RaiseCanExecuteChanged();

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

                SubmitCommand.RaiseCanExecuteChanged();
            }));

        /// <summary>
        ///     提交命令。
        /// </summary>
        public RelayCommand SubmitCommand =>
            _submitCommand ?? (_submitCommand = new RelayCommand(async () => {
                    if (!Uri.TryCreate(GroupAssignment.Solution,
                        UriKind.Absolute,
                        out var myUri)) {
                        await _dialogService.ShowAsync(
                            App.SolutionUrlErrorMessage);
                        return;
                    }

                    Submitting = true;
                    _submitCommand.RaiseCanExecuteChanged();
                    var serviceResult =
                        await _groupAssignmentService.SubmitAsync(
                            GroupAssignment);
                    Submitting = false;
                    _submitCommand.RaiseCanExecuteChanged();

                    switch (serviceResult.Status) {
                        case ServiceResultStatus.Unauthorized:
                        case ServiceResultStatus.Forbidden:
                            break;
                        case ServiceResultStatus.NoContent:
                            await _dialogService.ShowAsync(
                                App.SolutionSubmittedMessage);
                            _navigationService.GoBack();
                            break;
                        case ServiceResultStatus.BadRequest:
                            await _dialogService.ShowAsync(
                                serviceResult.Message);
                            break;
                        default:
                            await _dialogService.ShowAsync(
                                App.HttpClientErrorMessage +
                                serviceResult.Message);
                            break;
                    }
                },
                () => !Refreshing && !Submitting && GroupAssignment != null &&
                      GroupAssignment.Homework.Deadline > DateTime.Now));

        /// <summary>
        ///     正在提交。
        /// </summary>
        public bool Submitting {
            get => _submitting;
            set => Set(nameof(Submitting), ref _submitting, value);
        }
    }
}