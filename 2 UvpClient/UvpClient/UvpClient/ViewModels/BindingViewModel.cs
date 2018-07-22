using System;
using System.Net;
using System.Net.Http;
using System.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using UvpClient.Models;
using UvpClient.Pages;
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
        ///     绑定命令。
        /// </summary>
        private RelayCommand _bindCommand;

        /// <summary>
        ///     正在绑定学号。
        /// </summary>
        private bool _binding;

        /// <summary>
        ///     检查命令。
        /// </summary>
        private RelayCommand _checkCommand;

        /// <summary>
        ///     正在检查学号。
        /// </summary>
        private bool _checking;

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
        ///     绑定命令。
        /// </summary>
        public RelayCommand BindCommand =>
            _bindCommand ?? (_bindCommand = new RelayCommand(async () => {
                var identifiedHttpMessageHandler =
                    _identityService.GetIdentifiedHttpMessageHandler();
                using (var httpClient =
                    new HttpClient(identifiedHttpMessageHandler)) {
                    Binding = true;
                    _bindCommand.RaiseCanExecuteChanged();

                    HttpResponseMessage response = null;
                    try {
                        response = await httpClient.PutAsync(
                            App.ServerEndpoint + "/api/Student?studentId=" +
                            HttpUtility.UrlEncode(StudentId),
                            new StringContent(""));
                    } catch (Exception e) {
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + e.Message);
                        return;
                    } finally {
                        Binding = false;
                        _bindCommand.RaiseCanExecuteChanged();
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return;

                    if (response.StatusCode == HttpStatusCode.BadRequest) {
                        var responseContent =
                            await response.Content.ReadAsStringAsync();
                        await _dialogService.ShowAsync(responseContent);
                        return;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent) {
                        _rootNavigationService.Navigate(typeof(MyUvpPage), null,
                            NavigationTransition.EntranceNavigationTransition);
                        return;
                    }

                    await _dialogService.ShowAsync(
                        App.HttpClientErrorMessage + response.ReasonPhrase);
                }
            }));

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
        ///     检查命令。
        /// </summary>
        public RelayCommand CheckCommand =>
            _checkCommand ?? (_checkCommand = new RelayCommand(async () => {
                var identifiedHttpMessageHandler =
                    _identityService.GetIdentifiedHttpMessageHandler();
                using (var httpClient =
                    new HttpClient(identifiedHttpMessageHandler)) {
                    Checking = true;
                    _checkCommand.RaiseCanExecuteChanged();

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
                        Checking = false;
                        _checkCommand.RaiseCanExecuteChanged();
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return;

                    if (response.StatusCode == HttpStatusCode.NotFound) {
                        await _dialogService.ShowAsync(
                            "We could not find your Student ID in our database.\nPlease check if you have entered a correct Student ID.\n\nIf this error continues, please contact your teacher.");
                        return;
                    }

                    if (response.IsSuccessStatusCode) {
                        var json = await response.Content.ReadAsStringAsync();
                        Student = JsonConvert.DeserializeObject<Student>(json);
                        return;
                    }

                    await _dialogService.ShowAsync(
                        App.HttpClientErrorMessage + response.ReasonPhrase);
                }
            }, () => !Checking));

        /// <summary>
        ///     正在检查学号。
        /// </summary>
        public bool Checking {
            get => _checking;
            set => Set(nameof(Checking), ref _checking, value);
        }

        /// <summary>
        ///     正在绑定学号。
        /// </summary>
        public bool Binding {
            get => _binding;
            set => Set(nameof(Binding), ref _binding, value);
        }
    }
}