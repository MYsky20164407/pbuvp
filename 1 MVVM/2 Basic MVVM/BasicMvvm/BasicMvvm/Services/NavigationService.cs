using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BasicMvvm.Services {
    public class NavigationService : INavigationService {
        /******** 私有变量 ********/

        /// <summary>
        ///     Frame。
        /// </summary>
        private Frame _mainFrame;

        /******** 公开属性 ********/

        /******** 继承方法 ********/

        /// <summary>
        ///     返回。
        /// </summary>
        public void GoBack() {
            if (EnsureMainFrame() && _mainFrame.CanGoBack)
                _mainFrame.GoBack();
        }

        /// <summary>
        ///     导航。
        /// </summary>
        /// <param name="pageType">目标页面类型。</param>
        public void NavigateTo(Type pageType) {
            if (EnsureMainFrame())
                _mainFrame.Navigate(pageType);
        }

        /******** 公开方法 ********/

        /******** 私有方法 ********/

        private bool EnsureMainFrame() {
            if (_mainFrame != null)
                return true;

            _mainFrame = Window.Current.Content as Frame;
            return _mainFrame != null;
        }
    }
}