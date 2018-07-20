using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     登录ViewModel。
    /// </summary>
    public class LoginViewModel : ViewModelBase {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     登录命令。
        /// </summary>
        private RelayCommand _loginCommand;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        public LoginViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService) {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
        }

        /// <summary>
        ///     登录命令。
        /// </summary>
        public RelayCommand RelayCommand =>
            _loginCommand ?? (_loginCommand = new RelayCommand(async () => {
                var loginReturn = await _identityService.LoginAsync();

                // TODO
                if (loginReturn.Succeeded) { }
            }));
    }
}