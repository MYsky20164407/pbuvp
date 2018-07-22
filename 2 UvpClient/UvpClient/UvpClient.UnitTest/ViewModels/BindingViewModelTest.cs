using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Pages;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class BindingViewModelTest {
        [TestMethod]
        public void TestCheckCommandUnauthorized() {
            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var checkRequested = false;
            var stubStudentService = new StubIStudentService();
            stubStudentService.GetStudentByStudentIdAsync(async studentId => {
                checkRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var bindingViewModel = new BindingViewModel(
                stubRootNavigationService, stubDialogService,
                stubStudentService);
            bindingViewModel.CheckCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(checkRequested);
        }

        [TestMethod]
        public void TestCheckCommandSucceeded() {
            var studentIdToRequest = "Student ID";

            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var studentIdRequested = "";
            var checkRequested = false;
            var stubStudentService = new StubIStudentService();
            stubStudentService.GetStudentByStudentIdAsync(async studentId => {
                checkRequested = true;
                studentIdRequested = studentId;
                return new ServiceResult<Student> {
                    Status = ServiceResultStatus.NoContent,
                    Result = new Student {StudentId = studentIdToRequest}
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubRootNavigationService, stubDialogService,
                stubStudentService);
            bindingViewModel.CheckCommand.Execute(studentIdToRequest);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(checkRequested);
            Assert.AreEqual(studentIdToRequest, studentIdRequested);
            Assert.AreEqual(studentIdToRequest, bindingViewModel.Student.StudentId);
        }

        [TestMethod]
        public void TestBindCommandUnauthorized() {
            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            var bindRequested = false;
            var stubStudentService = new StubIStudentService();
            stubStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var bindingViewModel = new BindingViewModel(
                stubRootNavigationService, stubDialogService,
                stubStudentService);
            bindingViewModel.BindCommand.Execute("studentId");

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(bindRequested);
        }

        [TestMethod]
        public void TestBindCommandSucceeded() {
            var studentIdToRequest = "Student ID";

            var rootFrameNavigated = false;
            var stubRootNavigationService = new StubIRootNavigationService();
            stubRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = sourcePageType == typeof(MyUvpPage) &&
                                         parameter == null &&
                                         navigationTransition ==
                                         NavigationTransition
                                             .EntranceNavigationTransition);

            var dialogShown = false;
            var stubDialogService = new StubIDialogService();
            stubDialogService.ShowAsync(async message => dialogShown = true);

            string studentIdRequested = "";
            var bindRequested = false;
            var stubStudentService = new StubIStudentService();
            stubStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                studentIdRequested = studentId;
                return new ServiceResult
                    {Status = ServiceResultStatus.NoContent};
            });

            var bindingViewModel = new BindingViewModel(
                stubRootNavigationService, stubDialogService,
                stubStudentService);
            bindingViewModel.StudentId = studentIdToRequest;
            bindingViewModel.BindCommand.Execute(null);

            Assert.IsTrue(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(bindRequested);
            Assert.AreEqual(studentIdToRequest, studentIdRequested);
        }

        [TestMethod]
        public void TestBindCommandError() {
            string messageToShow = "Error Message";

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
            var stubStudentService = new StubIStudentService();
            stubStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.BadRequest,
                    Message = messageToShow
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubRootNavigationService, stubDialogService,
                stubStudentService);
            bindingViewModel.BindCommand.Execute("studentId");

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(messageToShow, messageShown);
            Assert.IsTrue(bindRequested);
        }

        [TestMethod]
        public void TestBindCommandOther() {
            string messageToShow = "Error Message";

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
            var stubStudentService = new StubIStudentService();
            stubStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubRootNavigationService, stubDialogService,
                stubStudentService);
            bindingViewModel.BindCommand.Execute("studentId");

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(bindRequested);
        }
    }
}