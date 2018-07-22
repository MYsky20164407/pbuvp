using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     学生服务。
        /// </summary>
        private readonly IStudentService _studentService;

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
        /// <param name="rootNavigationService">根导航服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="studentService">学生服务。</param>
        public BindingViewModel(IRootNavigationService rootNavigationService,
            IDialogService dialogService, IStudentService studentService) {
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
            _studentService = studentService;
        }

        /// <summary>
        ///     绑定命令。
        /// </summary>
        public RelayCommand BindCommand =>
            _bindCommand ?? (_bindCommand = new RelayCommand(async () => {
                Binding = true;
                _bindCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _studentService.BindAccountAsync(StudentId);
                Binding = false;
                _bindCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                        break;
                    case ServiceResultStatus.NoContent:
                        _rootNavigationService.Navigate(typeof(MyUvpPage), null,
                            NavigationTransition.EntranceNavigationTransition);
                        break;
                    case ServiceResultStatus.BadRequest:
                        await _dialogService.ShowAsync(serviceResult.Message);
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
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
                Checking = true;
                _checkCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _studentService.GetStudentByStudentIdAsync(StudentId);
                Checking = false;
                _checkCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                        break;
                    case ServiceResultStatus.OK:
                        Student = serviceResult.Result;
                        break;
                    case ServiceResultStatus.NotFound:
                        await _dialogService.ShowAsync(
                            "We could not find your Student ID in our database.\nPlease check if you have entered a correct Student ID.\n\nIf this error continues, please contact your teacher.");
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
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