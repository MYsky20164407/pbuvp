using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Pages;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     通知ViewModel。
    /// </summary>
    public class AnnouncementViewModel : ViewModelBase {
        /// <summary>
        ///     通知服务。
        /// </summary>
        private readonly IAnnouncementService _announcementService;

        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;


        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     通知。
        /// </summary>
        private Announcement _announcement;

        /// <summary>
        ///     全部通知。
        /// </summary>
        private IEnumerable<Announcement> _announcements;

        /// <summary>
        ///     获取命令。
        /// </summary>
        private RelayCommand _getCommand;


        /// <summary>
        ///     正在加载。
        /// </summary>
        private bool _loading;


        /// <summary>
        ///     查看通知命令。
        /// </summary>
        private RelayCommand<Announcement> _openCommand;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="announcementService">通知服务。</param>
        public AnnouncementViewModel(IDialogService dialogService,
            IAnnouncementService announcementService,
            IRootNavigationService rootNavigationService) {
            _dialogService = dialogService;
            _announcementService = announcementService;
            _rootNavigationService = rootNavigationService;
        }

        /// <summary>
        ///     通知。
        /// </summary>
        public Announcement Announcement {
            get => _announcement;
            set => Set(nameof(Announcement), ref _announcement, value);
        }

        /// <summary>
        ///     全部通知。
        /// </summary>
        public IEnumerable<Announcement> Announcements {
            get => _announcements;
            set => Set(nameof(Announcements), ref _announcements, value);
        }

        /// <summary>
        ///     查看通知命令。
        /// </summary>
        public RelayCommand<Announcement> OpenCommand =>
            _openCommand ?? (_openCommand = new RelayCommand<Announcement>(
                announcement => _rootNavigationService.Navigate(
                    typeof(AnnouncementPage), announcement,
                    NavigationTransition.DrillInNavigationTransition)));

        /// <summary>
        ///     获取命令。
        /// </summary>
        public RelayCommand GetCommand =>
            _getCommand ?? (_getCommand = new RelayCommand(async () => {
                Loading = true;
                _getCommand.RaiseCanExecuteChanged();
                var serviceResult = await _announcementService.GetAsync();
                Loading = false;
                _getCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        Announcements = serviceResult.Result;
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