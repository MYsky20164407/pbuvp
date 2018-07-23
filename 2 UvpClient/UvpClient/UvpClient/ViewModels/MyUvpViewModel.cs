using System.Collections.Generic;
using Windows.ApplicationModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     我的uvp ViewModel。
    /// </summary>
    public class MyUvpViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     我的uvp服务。
        /// </summary>
        private readonly IMyUvpService _myUvpService;

        /// <summary>
        ///     我的uvp。
        /// </summary>
        private MyUvp _myUvp;

        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand _refreshCommand;

        /// <summary>
        ///     正在刷新。
        /// </summary>
        private bool _refreshing;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="myUvpService">我的uvp服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        [PreferredConstructor]
        public MyUvpViewModel(IMyUvpService myUvpService,
            IDialogService dialogService) {
            _myUvpService = myUvpService;
            _dialogService = dialogService;
        }

        /// <summary>
        ///     我的uvp。
        /// </summary>
        public MyUvp MyUvp {
            get => _myUvp;
            set => Set(nameof(MyUvp), ref _myUvp, value);
        }

        /// <summary>
        ///     刷新命令。
        /// </summary>
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                Refreshing = true;
                _refreshCommand.RaiseCanExecuteChanged();
                var serviceResult = await _myUvpService.GetAsync();
                Refreshing = false;
                _refreshCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        MyUvp = serviceResult.Result;
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }
            }));

        /// <summary>
        ///     正在刷新。
        /// </summary>
        public bool Refreshing {
            get => _refreshing;
            set => Set(nameof(Refreshing), ref _refreshing, value);
        }
    }
}