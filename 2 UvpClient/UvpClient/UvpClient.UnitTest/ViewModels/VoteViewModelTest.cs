using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class VoteViewModelTest {
        [TestMethod]
        public void TestSubmitCommandEmptyCollection() {
            var voteToSubmit = new Vote {
                Questionnaire = new Questionnaire {Deadline = DateTime.MaxValue}
            };

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, null, null);
            voteViewModel.Vote = voteToSubmit;
            voteViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(VoteViewModel.AnswerCollectionEmptyErrorMessage,
                messageShown);
        }

        [TestMethod]
        public void TestSubmitCommandOther() {
            var voteToSubmit = new Vote {
                Questionnaire =
                    new Questionnaire {Deadline = DateTime.MaxValue},
                AnswerCollection = new List<QuestionnaireOption>
                    {new QuestionnaireOption()}
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            Vote voteSubmitted = null;
            var submitRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.SubmitAsync(async vote => {
                submitRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.NotFound,
                    Message = messageToShow
                };
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, stubIVoteService, null);
            voteViewModel.Vote = voteToSubmit;
            voteViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandError() {
            var voteToSubmit = new Vote {
                Questionnaire =
                    new Questionnaire {Deadline = DateTime.MaxValue},
                AnswerCollection = new List<QuestionnaireOption>
                    {new QuestionnaireOption()}
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            Vote voteSubmitted = null;
            var submitRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.SubmitAsync(async vote => {
                submitRequested = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.BadRequest,
                    Message = messageToShow
                };
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, stubIVoteService, null);
            voteViewModel.Vote = voteToSubmit;
            voteViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(messageToShow, messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandUnauthorized() {
            var voteToSubmit = new Vote {
                Questionnaire =
                    new Questionnaire {Deadline = DateTime.MaxValue},
                AnswerCollection = new List<QuestionnaireOption>
                    {new QuestionnaireOption()}
            };

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => dialogShown = true);

            var submitRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.SubmitAsync(async groupAssignment => {
                submitRequested = true;
                return new ServiceResult
                    {Status = ServiceResultStatus.Unauthorized};
            });

            var voteViewModel =
                new VoteViewModel(stubIDialogService, stubIVoteService, null);
            voteViewModel.Vote = voteToSubmit;
            voteViewModel.SubmitCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommand() {
            var voteToSubmit = new Vote {
                Questionnaire =
                    new Questionnaire {Deadline = DateTime.MaxValue},
                AnswerCollection = new List<QuestionnaireOption>
                    {new QuestionnaireOption()}
            };

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            Vote voteSubmitted = null;
            var submitRequested = false;
            var stubIVoteService = new StubIVoteService();
            stubIVoteService.SubmitAsync(async vote => {
                submitRequested = true;
                voteSubmitted = vote;
                return new ServiceResult
                    {Status = ServiceResultStatus.NoContent};
            });

            var stubIRootNavigationService = new StubIRootNavigationService();
            var backNavigated = false;
            stubIRootNavigationService.GoBack(() => backNavigated = true);

            var voteViewModel = new VoteViewModel(stubIDialogService,
                stubIVoteService, stubIRootNavigationService);
            voteViewModel.Vote = voteToSubmit;
            voteViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreSame(VoteViewModel.AnswerSubmittedMessage, messageShown);
            Assert.IsTrue(submitRequested);
            Assert.AreSame(voteToSubmit, voteSubmitted);
            Assert.IsTrue(backNavigated);
        }
    }
}