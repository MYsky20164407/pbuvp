using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class StudentAssignmentViewModelTest {
        [TestMethod]
        public void TestSubmitCommandOther() {
            var studentAssignmentToSubmit = new StudentAssignment {
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

            StudentAssignment studentAssignmentSubmitted = null;
            var submitRequested = false;
            var stubIStudentAssignmentService =
                new StubIStudentAssignmentService();
            stubIStudentAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.NotFound,
                    Message = messageToShow
                };
            });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentAssignmentService);
            studentAssignmentViewModel.StudentAssignment =
                studentAssignmentToSubmit;
            studentAssignmentViewModel.StudentAssignmentId =
                studentAssignmentToSubmit.HomeworkID;
            studentAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandError() {
            var studentAssignmentToSubmit = new StudentAssignment {
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

            StudentAssignment studentAssignmentSubmitted = null;
            var submitRequested = false;
            var stubIStudentAssignmentService =
                new StubIStudentAssignmentService();
            stubIStudentAssignmentService.SubmitAsync(
                async studentAssignment => {
                    submitRequested = true;
                    return new ServiceResult {
                        Status = ServiceResultStatus.BadRequest,
                        Message = messageToShow
                    };
                });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentAssignmentService);
            studentAssignmentViewModel.StudentAssignment =
                studentAssignmentToSubmit;
            studentAssignmentViewModel.StudentAssignmentId =
                studentAssignmentToSubmit.HomeworkID;
            studentAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(messageToShow, messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandUnauthorized() {
            var studentAssignmentToSubmit = new StudentAssignment {
                HomeworkID = -1, Solution = "http://www.bing.com/",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var submitRequested = false;
            var stubIStudentAssignmentService =
                new StubIStudentAssignmentService();
            stubIStudentAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                return new ServiceResult
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentAssignmentService);
            studentAssignmentViewModel.StudentAssignment =
                studentAssignmentToSubmit;
            studentAssignmentViewModel.StudentAssignmentId =
                studentAssignmentToSubmit.HomeworkID;
            studentAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommand() {
            var studentAssignmentToSubmit = new StudentAssignment {
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

            StudentAssignment studentAssignmentSubmitted = null;
            var submitRequested = false;
            var stubIStudentAssignmentService =
                new StubIStudentAssignmentService();
            stubIStudentAssignmentService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                studentAssignmentSubmitted = groupAssignment;
                return new ServiceResult
                    {Status = ServiceResultStatus.NoContent};
            });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentAssignmentService);
            studentAssignmentViewModel.StudentAssignment =
                studentAssignmentToSubmit;
            studentAssignmentViewModel.StudentAssignmentId =
                studentAssignmentToSubmit.HomeworkID;
            studentAssignmentViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreSame(UvpClient.App.SolutionSubmittedMessage,
                messageShown);
            Assert.IsTrue(submitRequested);
            Assert.AreSame(studentAssignmentToSubmit,
                studentAssignmentSubmitted);
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

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService, null);
            studentAssignmentViewModel.StudentAssignment = new StudentAssignment {
                Solution = "not a url",
                Homework = new Homework {Deadline = DateTime.MaxValue}
            };
            studentAssignmentViewModel.SubmitCommand.Execute(null);

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
            var stubIStudentAssignmentService =
                new StubIStudentAssignmentService();
            stubIStudentAssignmentService.GetAsync(async id => {
                refreshRequested = true;
                return new ServiceResult<StudentAssignment>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentAssignmentService);
            studentAssignmentViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(refreshRequested);
        }

        [TestMethod]
        public void TestRefreshCommandSucceeded() {
            var studentAssignmentToReturn = new StudentAssignment();

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var refreshRequested = false;
            var stubIStudentService = new StubIStudentAssignmentService();
            stubIStudentService.GetAsync(async id => {
                refreshRequested = true;
                return new ServiceResult<StudentAssignment> {
                    Status = ServiceResultStatus.OK,
                    Result = studentAssignmentToReturn
                };
            });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentService);
            studentAssignmentViewModel.RefreshCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(refreshRequested);
            Assert.AreSame(studentAssignmentToReturn,
                studentAssignmentViewModel.StudentAssignment);
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
            var stubIStudentAssignmentService =
                new StubIStudentAssignmentService();
            stubIStudentAssignmentService.GetAsync(async id => {
                refreshRequested = true;
                return new ServiceResult<StudentAssignment> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var studentAssignmentViewModel =
                new StudentAssignmentViewModel(stubIDialogService,
                    stubIStudentAssignmentService);
            studentAssignmentViewModel.RefreshCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(refreshRequested);
        }
    }
}