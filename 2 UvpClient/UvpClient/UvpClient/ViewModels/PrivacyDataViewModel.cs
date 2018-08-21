using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     隐私数据ViewModel。
    /// </summary>
    public class PrivacyDataViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     隐私数据服务。
        /// </summary>
        private readonly IPrivacyDataService _privacyDataService;

        /// <summary>
        ///     获取命令。
        /// </summary>
        private RelayCommand _getCommand;

        /// <summary>
        ///     正在加载。
        /// </summary>
        private bool _loading;

        /// <summary>
        ///     隐私数据。
        /// </summary>
        private PrivacyData _privacyData;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="privacyDataService">隐私数据服务。</param>
        public PrivacyDataViewModel(IDialogService dialogService,
            IPrivacyDataService privacyDataService) {
            _dialogService = dialogService;
            _privacyDataService = privacyDataService;
        }

        /// <summary>
        ///     隐私数据。
        /// </summary>
        public PrivacyData PrivacyData {
            get => _privacyData;
            set => Set(nameof(PrivacyData), ref _privacyData, value);
        }

        /// <summary>
        ///     获取命令。
        /// </summary>
        public RelayCommand GetCommand =>
            _getCommand ?? (_getCommand = new RelayCommand(async () => {
                Loading = true;
                _getCommand.RaiseCanExecuteChanged();
                var serviceResult = await _privacyDataService.GetAsync();
                Loading = false;
                _getCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        PrivacyData = serviceResult.Result;
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