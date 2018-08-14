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
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _navigationService;

        /// <summary>
        ///     个人作业服务。
        /// </summary>
        private readonly IStudentAssignmentService _studentAssignmentService;

        /// <summary>
        ///     磁贴服务。
        /// </summary>
        private readonly ITileService _tileService;

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
        /// <param name="navigationService">根导航服务。</param>
        /// <param name="tileService">磁贴服务。</param>
        public StudentAssignmentViewModel(IDialogService dialogService,
            IStudentAssignmentService studentAssignmentService,
            IRootNavigationService navigationService,
            ITileService tileService) {
            _dialogService = dialogService;
            _studentAssignmentService = studentAssignmentService;
            _navigationService = navigationService;
            _tileService = tileService;
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
                SubmitCommand.RaiseCanExecuteChanged();

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

                SubmitCommand.RaiseCanExecuteChanged();
            }));

        /// <summary>
        ///     提交命令。
        /// </summary>
        public RelayCommand SubmitCommand =>
            _submitCommand ?? (_submitCommand = new RelayCommand(async () => {
                    if (!Uri.TryCreate(StudentAssignment.Solution,
                        UriKind.Absolute,
                        out var myUri)) {
                        await _dialogService.ShowAsync(
                            App.SolutionUrlErrorMessage);
                        return;
                    }

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
                            _tileService.ForceUpdate();
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
                () => !Refreshing && !Submitting && StudentAssignment != null &&
                      StudentAssignment.Homework.Deadline > DateTime.Now));

        /// <summary>
        ///     正在提交。
        /// </summary>
        public bool Submitting {
            get => _submitting;
            set => Set(nameof(Submitting), ref _submitting, value);
        }
    }
}