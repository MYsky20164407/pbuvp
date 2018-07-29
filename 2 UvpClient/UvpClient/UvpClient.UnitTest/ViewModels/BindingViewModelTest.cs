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
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var getRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetStudentByStudentIdAsync(async studentId => {
                getRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.CheckCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getRequested);
        }

        [TestMethod]
        public void TestCheckCommandSucceeded() {
            var studentIdToRequest = "Student ID";

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var studentIdRequested = "";
            var getRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetStudentByStudentIdAsync(async studentId => {
                getRequested = true;
                studentIdRequested = studentId;
                return new ServiceResult<Student> {
                    Status = ServiceResultStatus.OK,
                    Result = new Student {StudentId = studentIdToRequest}
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.StudentId = studentIdToRequest;
            bindingViewModel.CheckCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(getRequested);
            Assert.AreEqual(studentIdToRequest, studentIdRequested);
            Assert.AreEqual(studentIdToRequest,
                bindingViewModel.Student.StudentId);
        }

        [TestMethod]
        public void TestCheckCommandError() {
            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var bindRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetStudentByStudentIdAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult<Student>
                    {Status = ServiceResultStatus.NotFound};
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.CheckCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(BindingViewModel.CheckNotFoundErrorMessage,
                messageShown);
            Assert.IsTrue(bindRequested);
        }

        [TestMethod]
        public void TestCheckCommandOther() {
            var messageToShow = "Error Message";

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var bindRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.GetStudentByStudentIdAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult<Student> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.CheckCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(bindRequested);
        }

        [TestMethod]
        public void TestBindCommandUnauthorized() {
            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var bindRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.BindCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(bindRequested);
        }

        [TestMethod]
        public void TestBindCommandSucceeded() {
            var studentIdToRequest = "Student ID";

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = sourcePageType == typeof(MyUvpPage) &&
                                         parameter == null &&
                                         navigationTransition ==
                                         NavigationTransition
                                             .EntranceNavigationTransition);

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var studentIdRequested = "";
            var bindRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                studentIdRequested = studentId;
                return new ServiceResult
                    {Status = ServiceResultStatus.NoContent};
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.StudentId = studentIdToRequest;
            bindingViewModel.BindCommand.Execute(null);

            Assert.IsTrue(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
            Assert.IsTrue(bindRequested);
            Assert.AreEqual(studentIdToRequest, studentIdRequested);
        }

        [TestMethod]
        public void TestBindCommandError() {
            var messageToShow = "Error Message";

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var bindRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.BadRequest,
                    Message = messageToShow
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.BindCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(messageToShow, messageShown);
            Assert.IsTrue(bindRequested);
        }

        [TestMethod]
        public void TestBindCommandOther() {
            var messageToShow = "Error Message";

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var bindRequested = false;
            var stubIStudentService = new StubIStudentService();
            stubIStudentService.BindAccountAsync(async studentId => {
                bindRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var bindingViewModel = new BindingViewModel(
                stubIRootNavigationService, stubIDialogService,
                stubIStudentService);
            bindingViewModel.BindCommand.Execute(null);

            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(bindRequested);
        }
    }
}