using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using UvpClient.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrivacyDataPage : Page {
        public PrivacyDataPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested +=
                SystemNavigationManagerOnBackRequested;

            SimpleIoc.Default.GetInstance<ILogService>()
                .Log(nameof(PrivacyDataPage));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            var systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -=
                SystemNavigationManagerOnBackRequested;
        }

        private void SystemNavigationManagerOnBackRequested(object sender,
            BackRequestedEventArgs e) {
            e.Handled = true;
            Frame.GoBack();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }
    }
}