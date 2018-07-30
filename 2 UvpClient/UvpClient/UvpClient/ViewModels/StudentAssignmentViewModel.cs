using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    public class StudentAssignmentViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     个人作业服务。
        /// </summary>
        private readonly IStudentAssignmentService _studentAssignmentService;

        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand _refreshCommand;

        /// <summary>
        ///     正在刷新。
        /// </summary>
        private bool _refreshing;

        /// <summary>
        ///     个人作业。
        /// </summary>
        private StudentAssignment _studentAssignment;

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
        /// <param name="studentAssignmentService">个人作业服务。</param>
        public StudentAssignmentViewModel(IDialogService dialogService,
            IStudentAssignmentService studentAssignmentService) {
            _dialogService = dialogService;
            _studentAssignmentService = studentAssignmentService;
        }

        /// <summary>
        ///     正在刷新。
        /// </summary>
        public bool Refreshing {
            get => _refreshing;
            set => Set(nameof(Refreshing), ref _refreshing, value);
        }

        /// <summary>
        ///     个人作业id。
        /// </summary>
        public int StudentAssignmentId { get; set; }

        /// <summary>
        ///     个人作业。
        /// </summary>
        public StudentAssignment StudentAssignment {
            get => _studentAssignment;
            set =>
                Set(nameof(StudentAssignment), ref _studentAssignment, value);
        }

        /// <summary>
        ///     刷新命令。
        /// </summary>
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                Refreshing = true;
                _refreshCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _studentAssignmentService.GetAsync(
                        StudentAssignmentId);
                Refreshing = false;
                _refreshCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        StudentAssignment = serviceResult.Result;
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }
            }));

        public RelayCommand SubmitCommand =>
            _submitCommand ?? (_submitCommand = new RelayCommand(async () => {
                if (!Uri.TryCreate(StudentAssignment.Solution, UriKind.Absolute,
                    out var myUri))
                    await _dialogService.ShowAsync(App.SolutionUrlErrorMessage);

                Submitting = true;
                _submitCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _studentAssignmentService.SubmitAsync(
                        StudentAssignment);
                Submitting = false;
                _submitCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.NoContent:
                        await _dialogService.ShowAsync(
                            App.SolutionSubmittedMessage);
                        break;
                    case ServiceResultStatus.BadRequest:
                        await _dialogService.ShowAsync(serviceResult.Message);
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
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