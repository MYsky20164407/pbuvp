using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     登录ViewModel。
    /// </summary>
    public class LoginViewModel : ViewModelBase {
        /// <summary>
        /// 身份服务。
        /// </summary>
        private IIdentityService _identityService;

        /// <summary>
        ///     登录命令。
        /// </summary>
        private RelayCommand _loginCommand;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public LoginViewModel(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        /// 登录命令。
        /// </summary>
        public RelayCommand RelayCommand =>
            _loginCommand ?? (_loginCommand =
                new RelayCommand(
                    async () => await _identityService.LoginAsync()));
    }
}