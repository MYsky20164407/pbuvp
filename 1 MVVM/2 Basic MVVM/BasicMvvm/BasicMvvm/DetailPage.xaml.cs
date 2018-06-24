using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BasicMvvm {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page {
        public DetailPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += DetailPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            var systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= DetailPage_BackRequested;
        }

        private void OnBackRequested() {
            Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private void NavigateBackForWideState(bool useTransition) {
            NavigationCacheMode = NavigationCacheMode.Disabled;

            if (useTransition)
                Frame.GoBack(new EntranceNavigationTransitionInfo());
            else
                Frame.GoBack(new SuppressNavigationTransitionInfo());
        }

        private bool ShouldGoToWideState() {
            return Window.Current.Bounds.Width >= 720;
        }

        private void DetailPage_OnLoaded(object sender, RoutedEventArgs e) {
            if (ShouldGoToWideState())
                NavigateBackForWideState(true);

            Window.Current.SizeChanged += Window_SizeChanged;
        }

        private void DetailPage_OnUnloaded(object sender, RoutedEventArgs e) {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender,
            WindowSizeChangedEventArgs e) {
            if (ShouldGoToWideState()) {
                Window.Current.SizeChanged -= Window_SizeChanged;
                NavigateBackForWideState(false);
            }
        }

        private void DetailPage_BackRequested(object sender,
            BackRequestedEventArgs e) {
            e.Handled = true;
            OnBackRequested();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            OnBackRequested();
        }
    }
}