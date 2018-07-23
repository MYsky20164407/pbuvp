using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using UvpClient.Models;
using UvpClient.Pages;
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
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     我的uvp。
        /// </summary>
        private MyUvp _myUvp;

        /// <summary>
        ///     查看通知命令。
        /// </summary>
        private RelayCommand<Announcement> _openAnnouncementCommand;

        /// <summary>
        ///     查看小组作业命令。
        /// </summary>
        private RelayCommand<GroupAssignment> _openGroupAssignmentCommand;

        /// <summary>
        ///     查看我命令。
        /// </summary>
        private RelayCommand _openMeCommand;

        /// <summary>
        ///     查看组内自评互评表命令。
        /// </summary>
        private RelayCommand<PeerWorkGroupEvaluation>
            _openPeerWorkGroupEvaluationCommand;

        /// <summary>
        ///     查看隐私数据命令。
        /// </summary>
        private RelayCommand _openPrivacyDataCommand;

        /// <summary>
        ///     查看个人作业命令。
        /// </summary>
        private RelayCommand<StudentAssignment> _openStudentAssignmentCommand;

        /// <summary>
        ///     查看投票命令。
        /// </summary>
        private RelayCommand<Vote> _openVoteCommand;

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
        /// <param name="rootNavigationService">根导航服务。</param>
        [PreferredConstructor]
        public MyUvpViewModel(IMyUvpService myUvpService,
            IDialogService dialogService,
            IRootNavigationService rootNavigationService) {
            _myUvpService = myUvpService;
            _dialogService = dialogService;
            _rootNavigationService = rootNavigationService;
        }

        /// <summary>
        ///     查看通知命令。
        /// </summary>
        public RelayCommand<Announcement> OpenAnnouncementCommand =>
            _openAnnouncementCommand ?? (_openAnnouncementCommand =
                new RelayCommand<Announcement>(announcement =>
                    _rootNavigationService.Navigate(typeof(AnnouncementPage),
                        announcement,
                        NavigationTransition.DrillInNavigationTransition)));

        /// <summary>
        ///     查看我命令。
        /// </summary>
        public RelayCommand OpenMeCommand =>
            _openMeCommand ?? (_openMeCommand = new RelayCommand(() =>
                _rootNavigationService.Navigate(typeof(MePage), null,
                    NavigationTransition.DrillInNavigationTransition)));

        /// <summary>
        ///     查看隐私数据命令。
        /// </summary>
        public RelayCommand OpenPrivacyDataCommand =>
            _openPrivacyDataCommand ?? (_openPrivacyDataCommand =
                new RelayCommand(() =>
                    _rootNavigationService.Navigate(typeof(PrivacyDataPage),
                        null, NavigationTransition.DrillInNavigationTransition))
            );

        /// <summary>
        ///     查看投票命令。
        /// </summary>
        public RelayCommand<Vote> OpenVoteCommand =>
            _openVoteCommand ?? (_openVoteCommand =
                new RelayCommand<Vote>(vote =>
                    _rootNavigationService.Navigate(typeof(VotePage), vote,
                        NavigationTransition.DrillInNavigationTransition)));

        /// <summary>
        ///     查看组内自评互评表命令。
        /// </summary>
        public RelayCommand<PeerWorkGroupEvaluation>
            OpenPeerWorkGroupEvaluationCommand =>
            _openPeerWorkGroupEvaluationCommand ??
            (_openPeerWorkGroupEvaluationCommand =
                new RelayCommand<PeerWorkGroupEvaluation>(
                    peerWorkGroupEvaluation => _rootNavigationService.Navigate(
                        typeof(PeerWorkGroupEvaluation),
                        peerWorkGroupEvaluation,
                        NavigationTransition.DrillInNavigationTransition)));

        /// <summary>
        ///     查看小组作业命令。
        /// </summary>
        public RelayCommand<GroupAssignment> OpenGroupAssignmentCommand =>
            _openGroupAssignmentCommand ?? (_openGroupAssignmentCommand =
                new RelayCommand<GroupAssignment>(groupAssignment =>
                    _rootNavigationService.Navigate(typeof(GroupAssignmentPage),
                        groupAssignment,
                        NavigationTransition.DrillInNavigationTransition)));

        /// <summary>
        ///     查看个人作业命令。
        /// </summary>
        public RelayCommand<StudentAssignment> OpenStudentAssignmentCommand =>
            _openStudentAssignmentCommand ?? (_openStudentAssignmentCommand =
                new RelayCommand<StudentAssignment>(studentAssignment =>
                    _rootNavigationService.Navigate(
                        typeof(StudentAssignmentPage), studentAssignment,
                        NavigationTransition.DrillInNavigationTransition)));

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