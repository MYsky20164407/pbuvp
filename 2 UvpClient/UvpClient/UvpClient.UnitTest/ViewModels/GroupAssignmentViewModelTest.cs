using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class GroupAssignmentViewModelTest {
        [TestMethod]
        public void TestSubmitCommandOther() {
            var groupAssignmentToSubmit = new GroupAssignment {
                HomeworkID = -1, Solution = "http://www.bing.com/",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            GroupAssignment groupAssignmentSubmitted = null;
            var submitRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.NotFound,
                    Message = messageToShow
                };
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, null);
            groupAssignmentViewModel.GroupAssignment = groupAssignmentToSubmit;
            groupAssignmentViewModel.GroupAssignmentId =
                groupAssignmentToSubmit.HomeworkID;
            groupAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandError() {
            var groupAssignmentToSubmit = new GroupAssignment {
                HomeworkID = -1, Solution = "http://www.bing.com/",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            GroupAssignment groupAssignmentSubmitted = null;
            var submitRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.BadRequest,
                    Message = messageToShow
                };
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, null);
            groupAssignmentViewModel.GroupAssignment = groupAssignmentToSubmit;
            groupAssignmentViewModel.GroupAssignmentId =
                groupAssignmentToSubmit.HomeworkID;
            groupAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(messageToShow, messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandUnauthorized() {
            var groupAssignmentToSubmit = new GroupAssignment {
                HomeworkID = -1, Solution = "http://www.bing.com/",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var submitRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                return new ServiceResult
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, null);
            groupAssignmentViewModel.GroupAssignment = groupAssignmentToSubmit;
            groupAssignmentViewModel.GroupAssignmentId =
                groupAssignmentToSubmit.HomeworkID;
            groupAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommand() {
            var groupAssignmentToSubmit = new GroupAssignment {
                HomeworkID = -1, Solution = "http://www.bing.com/",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            GroupAssignment groupAssignmentSubmitted = null;
            var submitRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                groupAssignmentSubmitted = groupAssignment;
                return new ServiceResult
                    {Status = ServiceResultStatus.NoContent};
            });

            var stubIRootNavigationService = new StubIRootNavigationService();
            var backNavigated = false;
            stubIRootNavigationService.GoBack(() => backNavigated = true);

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, stubIRootNavigationService);
            groupAssignmentViewModel.GroupAssignment = groupAssignmentToSubmit;
            groupAssignmentViewModel.GroupAssignmentId =
                groupAssignmentToSubmit.HomeworkID;
            groupAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreSame(UvpClient.App.SolutionSubmittedMessage,
                messageShown);
            Assert.IsTrue(submitRequested);
            Assert.AreSame(groupAssignmentToSubmit, groupAssignmentSubmitted);
            Assert.IsTrue(backNavigated);
        }

        [TestMethod]
        public void TestSubmitCommandInvalidUrl() {
            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService, null, null);
            groupAssignmentViewModel.GroupAssignment = new GroupAssignment {
                Solution = "not a url",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };
            groupAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(UvpClient.App.SolutionUrlErrorMessage,
                messageShown);
        }

        [TestMethod]
        public void TestRefreshCommandUnauthorized() {
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var refreshRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.GetAsync(async id => {
                refreshRequested = true;
                return new ServiceResult<GroupAssignment>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, null);
            groupAssignmentViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(refreshRequested);
        }

        [TestMethod]
        public void TestRefreshCommandSucceeded() {
            var groupAssignmentToReturn = new GroupAssignment
                {Solution = "http://www.163.com/"};

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var refreshRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.GetAsync(async id => {
                refreshRequested = true;
                return new ServiceResult<GroupAssignment> {
                    Status = ServiceResultStatus.OK,
                    Result = groupAssignmentToReturn
                };
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, null);
            groupAssignmentViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(refreshRequested);
            Assert.AreSame(groupAssignmentToReturn,
                groupAssignmentViewModel.GroupAssignment);
        }

        [TestMethod]
        public void TestRefreshCommandOther() {
            var messageToShow = "Error Message";

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var refreshRequested = false;
            var stubIGroupAssignmentService = new StubIGroupAssignmentService();
            stubIGroupAssignmentService.GetAsync(async id => {
                refreshRequested = true;
                return new ServiceResult<GroupAssignment> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var groupAssignmentViewModel =
                new GroupAssignmentViewModel(stubIDialogService,
                    stubIGroupAssignmentService, null);
            groupAssignmentViewModel.RefreshCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(refreshRequested);
        }
    }
}