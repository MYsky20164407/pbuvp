using GalaSoft.MvvmLight.Ioc;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     ViewModel定位器。
    /// </summary>
    public class ViewModelLocator {
        /// <summary>
        ///     构造函数。
        /// </summary>
        public ViewModelLocator() {
            SimpleIoc.Default.Register<IIdentityService, IdentityService>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MyUvpViewModel>();
        }

        /// <summary>
        ///     获得登录ViewModel。
        /// </summary>
        public LoginViewModel LoginViewModel =>
            SimpleIoc.Default.GetInstance<LoginViewModel>();

        /// <summary>
        /// 我的uvp ViewModel。
        /// </summary>
        public MyUvpViewModel MyUvpViewModel =>
            SimpleIoc.Default.GetInstance<MyUvpViewModel>();
    }
}