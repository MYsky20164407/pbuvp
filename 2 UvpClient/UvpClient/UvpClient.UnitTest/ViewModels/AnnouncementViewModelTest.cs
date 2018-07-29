using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Pages;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class AnnouncementViewModelTest {
        [TestMethod]
        public void TestOpenCommand() {
            var announcementToNavigate = new Announcement();

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate((type, parameter, transition) =>
                rootFrameNavigated = type == typeof(AnnouncementPage) &&
                                     parameter == announcementToNavigate &&
                                     transition == NavigationTransition
                                         .DrillInNavigationTransition);

            var announcementViewModel =
                new AnnouncementViewModel(null, null,
                    stubIRootNavigationService);
            announcementViewModel.OpenCommand.Execute(announcementToNavigate);

            Assert.IsTrue(rootFrameNavigated);
        }

        [TestMethod]
        public void TestOpenCommandUnauthorized() {
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getRequested = false;
            var stubIAnnouncementService = new StubIAnnouncementService();
            stubIAnnouncementService.GetAsync(async () => {
                getRequested = true;
                return new ServiceResult<IEnumerable<Announcement>>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var announcementViewModel =
                new AnnouncementViewModel(stubIDialogService,
                    stubIAnnouncementService, null);
            announcementViewModel.GetCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getRequested);
        }

        [TestMethod]
        public void TestOpenCommandSucceeded() {
            var announcementToReturn = new List<Announcement>
                {new Announcement()};
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getRequested = false;
            var stubIAnnouncementService = new StubIAnnouncementService();
            stubIAnnouncementService.GetAsync(async () => {
                getRequested = true;
                return new ServiceResult<IEnumerable<Announcement>> {
                    Status = ServiceResultStatus.OK,
                    Result = announcementToReturn
                };
            });

            var announcementViewModel =
                new AnnouncementViewModel(stubIDialogService,
                    stubIAnnouncementService, null);
            announcementViewModel.GetCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getRequested);
            Assert.AreSame(announcementToReturn,
                announcementViewModel.Announcements);
        }

        [TestMethod]
        public void TestOpenCommandOther() {
            var messageToShow = "Error Message";

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var getRequested = false;
            var stubIAnnouncementService = new StubIAnnouncementService();
            stubIAnnouncementService.GetAsync(async () => {
                getRequested = true;
                return new ServiceResult<IEnumerable<Announcement>> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var announcementViewModel =
                new AnnouncementViewModel(stubIDialogService,
                    stubIAnnouncementService, null);
            announcementViewModel.GetCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(getRequested);
        }
    }
}