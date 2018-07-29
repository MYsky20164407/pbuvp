using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class MeViewModelTest {
        [TestMethod]
        public void TestGetCommandUnauthorized() {
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getMeRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetMeAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var meViewModel =
                new MeViewModel(stubIStudentService, stubIDialogService);
            meViewModel.GetCommand.Execute(null);
            
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getMeRequested);
        }

        [TestMethod]
        public void TestGetCommandSucceeded() {
            var studentToReturn = new Student();

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getMeRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetMeAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.OK, Result = studentToReturn};
            });

            var meViewModel =
                new MeViewModel(stubIStudentService, stubIDialogService);
            meViewModel.GetCommand.Execute(null);
            
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getMeRequested);
            Assert.AreSame(studentToReturn, meViewModel.Me);
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
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetMeAsync(async () => {
                getMeRequested = true;
                return new ServiceResult<Student> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var meViewModel =
                new MeViewModel(stubIStudentService, stubIDialogService);
            meViewModel.GetCommand.Execute(null);
            
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(getMeRequested);
        }
    }
}