using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class MyUvpViewModelTest {
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