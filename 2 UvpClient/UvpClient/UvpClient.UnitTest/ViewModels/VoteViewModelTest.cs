using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class VoteViewModelTest {
        [TestMethod]
        public void TestLoadCommandUnauthorized() {
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var loadRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.GetAsync(async id => {
                loadRequested = true;
                return new ServiceResult<Vote>
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, stubIVoteService);
            voteViewModel.LoadCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(loadRequested);
        }

        [TestMethod]
        public void TestLoadCommandSucceeded() {
            var voteToReturn = new Vote {
                Questionnaire = new Questionnaire {Deadline = DateTime.MaxValue}
            };

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var loadRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.GetAsync(async id => {
                loadRequested = true;
                return new ServiceResult<Vote>
                    {Status = ServiceResultStatus.OK, Result = voteToReturn};
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, stubIVoteService);
            voteViewModel.LoadCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(loadRequested);
            Assert.AreSame(voteToReturn, voteViewModel.Vote);
        }

        [TestMethod]
        public void TestLoadCommandOther() {
            var messageToShow = "Error Message";

            var messageShown = "";
            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var loadRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.GetAsync(async id => {
                loadRequested = true;
                return new ServiceResult<Vote> {
                    Status = ServiceResultStatus.Exception,
                    Message = messageToShow
                };
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, stubIVoteService);
            voteViewModel.LoadCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(loadRequested);
        }
    }
}