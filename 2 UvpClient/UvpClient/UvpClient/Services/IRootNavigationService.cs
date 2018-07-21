using System;
using Windows.UI.Xaml.Media.Animation;

namespace UvpClient.Services {
    /// <summary>
    ///     根导航服务接口。
    /// </summary>
    public interface IRootNavigationService {
        /// <summary>
        ///     导航。
        /// </summary>
        bool Navigate(Type sourcePageType);

        /// <summary>
        ///     导航。
        /// </summary>
        bool Navigate(Type sourcePageType, object parameter);

        /// <summary>
        ///     导航。
        /// </summary>
        bool Navigate(Type sourcePageType, object parameter,
            NavigationTransitionInfo navigationTransitionInfo);
    }
}