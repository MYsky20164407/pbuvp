using System;
using System.Net;
using System.Net.Http;
using System.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     绑定ViewModel。
    /// </summary>
    public class BindingViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     正在绑定学号。
        /// </summary>
        private bool _bindingStudentId;

        /// <summary>
        ///     正在检查学号。
        /// </summary>
        private bool _checkingStudentId;

        /// <summary>
        ///     检查学号命令。
        /// </summary>
        private RelayCommand _checkStudentIdCommand;

        /// <summary>
        ///     学生。
        /// </summary>
        private Student _student;

        /// <summary>
        ///     学号。
        /// </summary>
        private string _studentId;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        public BindingViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService,
            IDialogService dialogService) {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
        }

        /// <summary>
        ///     学号。
        /// </summary>
        public string StudentId {
            get => _studentId;
            set => Set(nameof(StudentId), ref _studentId, value);
        }

        /// <summary>
        ///     学生。
        /// </summary>
        public Student Student {
            get => _student;
            set => Set(nameof(Student), ref _student, value);
        }

        /// <summary>
        ///     检查学号命令。
        /// </summary>
        public RelayCommand CheckStudentIdCommand =>
            _checkStudentIdCommand ?? (_checkStudentIdCommand =
                new RelayCommand(async () => {
                    var identifiedHttpMessageHandler = _identityService
                        .GetIdentifiedHttpMessageHandler();
                    using (var httpClient =
                        new HttpClient(identifiedHttpMessageHandler)) {
                        CheckingStudentId = true;
                        _checkStudentIdCommand.RaiseCanExecuteChanged();

                        HttpResponseMessage response = null;
                        try {
                            response = await httpClient.GetAsync(
                                App.ServerEndpoint + "/api/Student?studentId=" +
                                HttpUtility.UrlEncode(StudentId));
                        } catch (Exception e) {
                            await _dialogService.ShowAsync(
                                App.HttpClientErrorMessage + e.Message);
                            return;
                        } finally {
                            CheckingStudentId = false;
                            _checkStudentIdCommand.RaiseCanExecuteChanged();
                        }

                        if (response.StatusCode == HttpStatusCode.NotFound) {
                            await _dialogService.ShowAsync(
                                "We could not find your Student ID in our database.\nPlease check if you have entered a correct Student ID.\n\nIf this error continues, please contact your teacher.");
                            return;
                        }

                        if (!response.IsSuccessStatusCode)
                            return;

                        var json = await response.Content.ReadAsStringAsync();
                        Student = JsonConvert.DeserializeObject<Student>(json);
                    }
                }, () => !CheckingStudentId));

        /// <summary>
        ///     正在检查学号。
        /// </summary>
        public bool CheckingStudentId {
            get => _checkingStudentId;
            set =>
                Set(nameof(CheckingStudentId), ref _checkingStudentId, value);
        }

        /// <summary>
        ///     正在绑定学号。
        /// </summary>
        public bool BindingStudentId {
            get => _bindingStudentId;
            set => Set(nameof(BindingStudentId), ref _bindingStudentId, value);
        }
    }
}