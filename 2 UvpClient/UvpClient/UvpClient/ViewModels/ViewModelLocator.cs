using GalaSoft.MvvmLight.Ioc;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     ViewModel定位器。
    /// </summary>
    public class ViewModelLocator {
        /// <summary>
        ///     ViewModel定位器单件。
        /// </summary>
        public static readonly ViewModelLocator Instance =
            new ViewModelLocator();

        /// <summary>
        ///     构造函数。
        /// </summary>
        private ViewModelLocator() {
            SimpleIoc.Default
                .Register<IRootNavigationService, RootNavigationService>();
            SimpleIoc.Default.Register<IIdentityService, IdentityService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IStudentService, StudentService>();
            SimpleIoc.Default.Register<IMyUvpService, MyUvpService>();
            SimpleIoc.Default
                .Register<IAnnouncementService, AnnouncementService>();
            SimpleIoc.Default
                .Register<IStudentAssignmentService, StudentAssignmentService
                >();
            SimpleIoc.Default
                .Register<IGroupAssignmentService, GroupAssignmentService>();
            SimpleIoc.Default.Register<IVoteService, VoteService>();
            SimpleIoc.Default
                .Register<IPeerWorkGroupEvaluationService,
                    PeerWorkGroupEvaluationService>();

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<BindingViewModel>();
            SimpleIoc.Default.Register<MyUvpViewModel>();
            SimpleIoc.Default.Register<MeViewModel>();
            SimpleIoc.Default.Register<AnnouncementViewModel>();
            SimpleIoc.Default.Register<StudentAssignmentViewModel>();
            SimpleIoc.Default.Register<GroupAssignmentViewModel>();
            SimpleIoc.Default.Register<VoteViewModel>();
            SimpleIoc.Default.Register<PeerWorkGroupEvaluationViewModel>();
        }

        /// <summary>
        ///     获得登录ViewModel。
        /// </summary>
        public LoginViewModel LoginViewModel =>
            SimpleIoc.Default.GetInstance<LoginViewModel>();

        /// <summary>
        ///     我的uvp ViewModel。
        /// </summary>
        public MyUvpViewModel MyUvpViewModel =>
            SimpleIoc.Default.GetInstance<MyUvpViewModel>();

        /// <summary>
        ///     绑定ViewModel。
        /// </summary>
        public BindingViewModel BindingViewModel =>
            SimpleIoc.Default.GetInstance<BindingViewModel>();

        /// <summary>
        ///     我ViewModel。
        /// </summary>
        public MeViewModel MeViewModel =>
            SimpleIoc.Default.GetInstance<MeViewModel>();

        /// <summary>
        ///     通知ViewModel。
        /// </summary>
        public AnnouncementViewModel AnnouncementViewModel =>
            SimpleIoc.Default.GetInstance<AnnouncementViewModel>();

        /// <summary>
        ///     个人作业ViewModel。
        /// </summary>
        public StudentAssignmentViewModel StudentAssignmentViewModel =>
            SimpleIoc.Default.GetInstance<StudentAssignmentViewModel>();

        /// <summary>
        ///     小组作业ViewModel。
        /// </summary>
        public GroupAssignmentViewModel GroupAssignmentViewModel =>
            SimpleIoc.Default.GetInstance<GroupAssignmentViewModel>();

        /// <summary>
        ///     投票ViewModel。
        /// </summary>
        public VoteViewModel VoteViewModel =>
            SimpleIoc.Default.GetInstance<VoteViewModel>();

        /// <summary>
        ///     组内自评/互评表ViewModel。
        /// </summary>
        public PeerWorkGroupEvaluationViewModel
            PeerWorkGroupEvaluationViewModel =>
            SimpleIoc.Default.GetInstance<PeerWorkGroupEvaluationViewModel>();
    }
}