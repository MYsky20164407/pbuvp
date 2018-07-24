using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     我ViewModel。
    /// </summary>
    public class MeViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     学生服务。
        /// </summary>
        private readonly IStudentService _studentService;

        /// <summary>
        ///     获取命令。
        /// </summary>
        private RelayCommand _getCommand;

        /// <summary>
        ///     正在加载。
        /// </summary>
        private bool _loading;

        /// <summary>
        ///     我。
        /// </summary>
        private Student _me;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="studentService">学生服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        public MeViewModel(IStudentService studentService,
            IDialogService dialogService) {
            _studentService = studentService;
            _dialogService = dialogService;
        }

        /// <summary>
        ///     我。
        /// </summary>
        public Student Me {
            get => _me;
            set => Set(nameof(Me), ref value, value);
        }

        /// <summary>
        ///     获取命令。
        /// </summary>
        public RelayCommand GetCommand =>
            _getCommand ?? (_getCommand = new RelayCommand(async () => {
                Loading = true;
                _getCommand.RaiseCanExecuteChanged();
                var serviceResult = await _studentService.GetMeAsync();
                Loading = false;
                _getCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        Me = serviceResult.Result;
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }
            }));

        /// <summary>
        ///     正在加载。
        /// </summary>
        public bool Loading {
            get => _loading;
            set => Set(nameof(Loading), ref _loading, value);
        }
    }
}