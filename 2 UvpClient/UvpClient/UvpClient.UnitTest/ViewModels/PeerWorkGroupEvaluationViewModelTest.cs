using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Models;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class PeerWorkGroupEvaluationViewModelTest {
        [TestMethod]
        public void TestSubmitCommandOther() {
            var peerWorkGroupEvaluationToSubmit = new PeerWorkGroupEvaluation {
                Q1 = false, Q2 = 1, Q3 = 1, Q4 = 1, Q5 = 1, Q6 = 1, Q7 = 1,
                Q8 = false, Q9 = ""
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            PeerWorkGroupEvaluation peerWorkGroupEvaluationSubmitted = null;
            var submitRequested = false;
            var stubIPeerWorkGroupEvaluationService =
                new StubIPeerWorkGroupEvaluationService();
            stubIPeerWorkGroupEvaluationService.SubmitAsync(
                async peerWorkGroupEvaluation => {
                    submitRequested = true;
                    peerWorkGroupEvaluationSubmitted = peerWorkGroupEvaluation;
                    return new ServiceResult {
                        Status = ServiceResultStatus.NotFound,
                        Message = messageToShow
                    };
                });

            var peerWorkGroupEvaluationViewModel =
                new PeerWorkGroupEvaluationViewModel(stubIDialogService, null,
                    stubIPeerWorkGroupEvaluationService, null);
            peerWorkGroupEvaluationViewModel.PeerWorkGroupEvaluation =
                peerWorkGroupEvaluationToSubmit;
            peerWorkGroupEvaluationViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                UvpClient.App.HttpClientErrorMessage + messageToShow,
                messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandError() {
            var peerWorkGroupEvaluationToSubmit = new PeerWorkGroupEvaluation {
                Q1 = false, Q2 = 1, Q3 = 1, Q4 = 1, Q5 = 1, Q6 = 1, Q7 = 1,
                Q8 = false, Q9 = ""
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            PeerWorkGroupEvaluation peerWorkGroupEvaluationSubmitted = null;
            var submitRequested = false;
            var stubIPeerWorkGroupEvaluationService =
                new StubIPeerWorkGroupEvaluationService();
            stubIPeerWorkGroupEvaluationService.SubmitAsync(
                async peerWorkGroupEvaluation => {
                    submitRequested = true;
                    return new ServiceResult {
                        Status = ServiceResultStatus.BadRequest,
                        Message = messageToShow
                    };
                });

            var peerWorkGroupEvaluationViewModel =
                new PeerWorkGroupEvaluationViewModel(stubIDialogService, null,
                    stubIPeerWorkGroupEvaluationService, null);
            peerWorkGroupEvaluationViewModel.PeerWorkGroupEvaluation =
                peerWorkGroupEvaluationToSubmit;
            peerWorkGroupEvaluationViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreEqual(messageToShow, messageShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommandUnauthorized() {
            var peerWorkGroupEvaluationToSubmit = new PeerWorkGroupEvaluation {
                Q1 = false, Q2 = 1, Q3 = 1, Q4 = 1, Q5 = 1, Q6 = 1, Q7 = 1,
                Q8 = false, Q9 = ""
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
            });

            var submitRequested = false;
            var stubIPeerWorkGroupEvaluationService =
                new StubIPeerWorkGroupEvaluationService();
            stubIPeerWorkGroupEvaluationService.SubmitAsync(
                async peerWorkGroupEvaluation => {
                    submitRequested = true;
                    return new ServiceResult
                        {Status = ServiceResultStatus.Unauthorized};
                });

            var peerWorkGroupEvaluationViewModel =
                new PeerWorkGroupEvaluationViewModel(stubIDialogService, null,
                    stubIPeerWorkGroupEvaluationService, null);
            peerWorkGroupEvaluationViewModel.PeerWorkGroupEvaluation =
                peerWorkGroupEvaluationToSubmit;
            peerWorkGroupEvaluationViewModel.SubmitCommand.Execute(null);

            Assert.IsFalse(dialogShown);
            Assert.IsTrue(submitRequested);
        }

        [TestMethod]
        public void TestSubmitCommand() {
            var peerWorkGroupEvaluationToSubmit = new PeerWorkGroupEvaluation {
                Q1 = false, Q2 = 1, Q3 = 1, Q4 = 1, Q5 = 1, Q6 = 1, Q7 = 1,
                Q8 = false, Q9 = ""
            };
            var messageToShow = "Error Message";

            var dialogShown = false;
            var messageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                messageShown = message;
            });

            PeerWorkGroupEvaluation peerWorkGroupEvaluationSubmitted = null;
            var submitRequested = false;
            var stubIPeerWorkGroupEvaluationService =
                new StubIPeerWorkGroupEvaluationService();
            stubIPeerWorkGroupEvaluationService.SubmitAsync(
                async peerWorkGroupEvaluation => {
                    submitRequested = true;
                    peerWorkGroupEvaluationSubmitted = peerWorkGroupEvaluation;
                    return new ServiceResult
                        {Status = ServiceResultStatus.NoContent};
                });

            var stubIRootNavigationService = new StubIRootNavigationService();
            var backNavigated = false;
            stubIRootNavigationService.GoBack(() => backNavigated = true);

            var stubITileService = new StubITileService();
            var updateRequested = false;
            stubITileService.ForceUpdate(() => updateRequested = true);

            var peerWorkGroupEvaluationViewModel =
                new PeerWorkGroupEvaluationViewModel(stubIDialogService,
                    stubIRootNavigationService,
                    stubIPeerWorkGroupEvaluationService, stubITileService);
            peerWorkGroupEvaluationViewModel.PeerWorkGroupEvaluation =
                peerWorkGroupEvaluationToSubmit;
            peerWorkGroupEvaluationViewModel.SubmitCommand.Execute(null);

            Assert.IsTrue(dialogShown);
            Assert.AreSame(
                PeerWorkGroupEvaluationViewModel
                    .PeerWorkGroupEvaluationSubmittedMessage, messageShown);
            Assert.IsTrue(submitRequested);
            Assert.AreSame(peerWorkGroupEvaluationToSubmit,
                peerWorkGroupEvaluationSubmitted);
            Assert.IsTrue(backNavigated);
            Assert.IsTrue(updateRequested);
        }
    }
}