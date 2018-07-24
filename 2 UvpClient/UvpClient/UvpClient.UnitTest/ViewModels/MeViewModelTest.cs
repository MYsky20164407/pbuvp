using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class MeViewModelTest {
        [TestMethod]
        public void TestGetCommandUnauthorized() {
            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var checkRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetMeAsync(async () => {
                checkRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var meViewModel =
                new MeViewModel(stubIStudentService, stubDialogService);
            meViewModel.GetCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(checkRequested);
        }

        [TestMethod]
        public void TestGetCommandSucceeded() {
            var studentToReturn = new Student();

            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var getMeRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetMeAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.OK, Result = studentToReturn};
            });

            var meViewModel =
                new MeViewModel(stubIStudentService, stubDialogService);
            meViewModel.GetCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getMeRequested);
            Assert.AreSame(studentToReturn, meViewModel.Me);
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

            var getMeRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetMeAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<Student> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var meViewModel =
                new MeViewModel(stubIStudentService, stubDialogService);
            meViewModel.GetCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(getMeRequested);
        }
    }
}