// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UvpClient.Models;
using UvpClient.ViewModels;

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyUvpPage {
        public MyUvpPage() {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.MyUvpViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Frame.BackStack.Clear();

            var myUvpViewModel = (MyUvpViewModel) DataContext;
            if (myUvpViewModel.MyUvp == null)
                myUvpViewModel.RefreshCommand.Execute(null);
        }

        private void StudentAssignmentListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var studentAssignment = (StudentAssignment) e.ClickedItem;
            ((MyUvpViewModel) DataContext).OpenStudentAssignmentCommand.Execute(
                studentAssignment);
        }

        private void GroupAssignmentListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var groupAssignment = (GroupAssignment) e.ClickedItem;
            ((MyUvpViewModel) DataContext).OpenGroupAssignmentCommand.Execute(
                groupAssignment);
        }

        private void VoteListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var vote = (Vote) e.ClickedItem;
            ((MyUvpViewModel) DataContext).OpenVoteCommand.Execute(vote);
        }

        private void PeerWorkGroupEvaluationListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var peerWorkGroupEvaluation =
                (PeerWorkGroupEvaluation) e.ClickedItem;
            ((MyUvpViewModel) DataContext).OpenPeerWorkGroupEvaluationCommand
                .Execute(peerWorkGroupEvaluation);
        }

        private void AnnouncementListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var announcement = (Announcement) e.ClickedItem;
            ((MyUvpViewModel) DataContext).OpenAnnouncementCommand.Execute(
                announcement);
        }
    }
}