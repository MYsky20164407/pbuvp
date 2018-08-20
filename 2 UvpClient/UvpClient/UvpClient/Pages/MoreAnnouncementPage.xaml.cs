using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoreAnnouncementPage : Page {
        public MoreAnnouncementPage() {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.AnnouncementViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested +=
                SystemNavigationManagerOnBackRequested;

            SimpleIoc.Default.GetInstance<ILogService>()
                .Log(nameof(MoreAnnouncementPage));
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

        private void ListViewBase_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var announcement = (Announcement) e.ClickedItem;
            ((AnnouncementViewModel) DataContext).OpenCommand.Execute(
                announcement);
        }
    }
}