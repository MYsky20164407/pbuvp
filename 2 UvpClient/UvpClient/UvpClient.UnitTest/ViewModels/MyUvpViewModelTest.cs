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

            var myUvpViewModel =
                new MyUvpViewModel(stubMyUvpService, stubDialogService);
            myUvpViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(checkRequested);
        }
    }
}