using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace UvpClient.Services {
    /// <summary>
    ///     根导航服务接口。
    /// </summary>
    public class RootNavigationService : IRootNavigationService {
        private Frame _mainFrame;

        /// <summary>
        ///     导航。
        /// </summary>
        public bool Navigate(Type sourcePageType) {
            if (!EnsureMainFrame())
                return false;

            return _mainFrame.Navigate(sourcePageType);
        }

        /// <summary>
        ///     导航。
        /// </summary>
        public bool Navigate(Type sourcePageType, object parameter) {
            if (!EnsureMainFrame())
                return false;

            return _mainFrame.Navigate(sourcePageType, parameter);
        }

        /// <summary>
        ///     导航。
        /// </summary>
        public bool Navigate(Type sourcePageType, object parameter,
            NavigationTransitionInfo navigationTransitionInfo) {
            if (!EnsureMainFrame())
                return false;

            return _mainFrame.Navigate(sourcePageType, parameter, navigationTransitionInfo);
        }

        private bool EnsureMainFrame() {
            if (_mainFrame != null)
                return true;

            _mainFrame = Window.Current.Content as Frame;
            return _mainFrame != null;
        }
    }
}