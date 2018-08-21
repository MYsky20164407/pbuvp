using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class PrivacyDataViewModelTest {
        [TestMethod]
        public void TestGetCommandUnauthorized() {
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getMeRequested = false;
            var stubIPrivacyDataService = new StubIPrivacyDataService();
            stubIPrivacyDataService.GetAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<PrivacyData>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var privacyDataViewModel =
                new PrivacyDataViewModel(stubIDialogService,
                    stubIPrivacyDataService);
            privacyDataViewModel.GetCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getMeRequested);
        }

        [TestMethod]
        public void TestGetCommandSucceeded() {
            var privacyDataToReturn = new PrivacyData();

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getRequested = false;
            var stubIPrivacyDataService = new StubIPrivacyDataService();
            stubIPrivacyDataService.GetAsync(async () => {
                getRequested = true;
                return new ServiceResult<PrivacyData> {
                    Status = ServiceResultStatus.OK,
                    Result = privacyDataToReturn
                };
            });

            var privacyDataViewModel =
                new PrivacyDataViewModel(stubIDialogService,
                    stubIPrivacyDataService);
            privacyDataViewModel.GetCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getRequested);
            Assert.AreSame(privacyDataToReturn,
                privacyDataViewModel.PrivacyData);
        }

        [TestMethod]
        public void TestGetCommandOther() {
            var messageToShow = "Error Message";

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var getMeRequested = false;
            var stubIPrivacyDataService = new StubIPrivacyDataService();
            stubIPrivacyDataService.GetAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<PrivacyData> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var privacyDataViewModel =
                new PrivacyDataViewModel(stubIDialogService,
                    stubIPrivacyDataService);
            privacyDataViewModel.GetCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(getMeRequested);
        }
    }
}