using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UvpClient.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BindingPage : Page {
        public BindingPage() {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.BindingViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Frame.BackStack.Clear();
        }
    }
}