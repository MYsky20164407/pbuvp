using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UvpClient.Models;
using UvpClient.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VotePage : Page {
        public VotePage() {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.VoteViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var vote = e.Parameter as Vote;
            if (vote == null) {
                Frame.GoBack();
                return;
            }

            ((VoteViewModel) DataContext).Vote = vote;

            var systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested +=
                SystemNavigationManagerOnBackRequested;
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

        private void MarkdownTextBlock_OnLoaded(object sender,
            RoutedEventArgs e) {
            MarkdownTextBlock.UriPrefix = App.ServerEndpoint;
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e) {
            var voteViewModel = (VoteViewModel) DataContext;
            voteViewModel.Vote.AnswerCollection.Clear();
            foreach (QuestionnaireOption questionnaireOption in ListView
                .SelectedItems)
                voteViewModel.Vote.AnswerCollection.Add(questionnaireOption);

            voteViewModel.SubmitCommand.Execute(null);
        }
    }
}