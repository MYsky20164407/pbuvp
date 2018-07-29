using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class StudentAssignmentViewModelTest {
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
            stubIStudentService.GetAsync(async (id) => {
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
            stubIStudentAssignmentService.GetAsync(async (id) => {
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