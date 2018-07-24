using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Pages;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class MyUvpViewModelTest {
        [TestMethod]
        public void TestOpenAnnouncementCommand() {
            var announcementToNavigate = new Announcement();

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(AnnouncementPage) &&
                                     parameter == announcementToNavigate &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenAnnouncementCommand.Execute(
                announcementToNavigate);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenMeCommand() {
            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(MePage) &&
                                     parameter == null &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenMeCommand.Execute(null);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenPrivacyDataCommand() {
            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(PrivacyDataPage) &&
                                     parameter == null &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenPrivacyDataCommand.Execute(null);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenVoteCommand() {
            var voteToNavigate = new Vote();

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(VotePage) &&
                                     parameter == voteToNavigate &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenVoteCommand.Execute(voteToNavigate);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenPeerWorkGroupEvaluationCommand() {
            var peerWorkGroupEvaluationToNavigate =
                new PeerWorkGroupEvaluation();

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated =
                    type == typeof(PeerWorkGroupEvaluationPage) &&
                    parameter == peerWorkGroupEvaluationToNavigate &&
                    transition == NavigationTransition
                        .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenPeerWorkGroupEvaluationCommand.Execute(
                peerWorkGroupEvaluationToNavigate);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenGroupAssignmentCommand() {
            var groupAssignmentToNavigate = new GroupAssignment();

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(GroupAssignmentPage) &&
                                     parameter == groupAssignmentToNavigate &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenGroupAssignmentCommand.Execute(
                groupAssignmentToNavigate);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenStudentAssignmentCommand() {
            var studentAssignmentToNavigate = new StudentAssignment();

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(StudentAssignmentPage) &&
                                     parameter == studentAssignmentToNavigate &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var myUvpViewModel =
                new MyUvpViewModel(null, null, stubIRootNavigationService);
            myUvpViewModel.OpenStudentAssignmentCommand.Execute(
                studentAssignmentToNavigate);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestRefreshCommandUnauthorized() {
            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var checkRequested = false;
            var stubMyUvpService = new StubIMyUvpService();
            stubMyUvpService.GetAsync(async () => {
                checkRequested = true;
                return new ServiceResult<MyUvp>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var myUvpViewModel = new MyUvpViewModel(stubMyUvpService,
                stubDialogService, stubRootNavigationService);
            myUvpViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(checkRequested);
        }

        [TestMethod]
        public void TestRefreshCommandSucceeded() {
            var myUvpToReturn = new MyUvp();

            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var checkRequested = false;
            var myUvpService = new StubIMyUvpService();
            myUvpService.GetAsync(async () => {
                checkRequested = true;
                return new ServiceResult<MyUvp>
                    {Status = ServiceResultStatus.OK, Result = myUvpToReturn};
            });

            var myUvpViewModel = new MyUvpViewModel(myUvpService,
                stubDialogService, stubRootNavigationService);
            myUvpViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(checkRequested);
            Assert.AreSame(myUvpToReturn, myUvpViewModel.MyUvp);
        }

        [TestMethod]
        public void TestCheckCommandOther() {
            var messageToShow = "Error Message";

            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var messageShown = "";
            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var bindRequested = false;
            var myUvpService = new StubIMyUvpService();
            myUvpService.GetAsync(async () => {
                bindRequested = true;
                return new ServiceResult<MyUvp> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var myUvpViewModel = new MyUvpViewModel(myUvpService,
                stubDialogService, stubRootNavigationService);
            myUvpViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(bindRequested);
        }
    }
}