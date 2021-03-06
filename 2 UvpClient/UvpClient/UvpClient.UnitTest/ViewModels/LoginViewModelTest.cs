﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Pages;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class LoginViewModelTest {
        [TestMethod]
        public void TestLoginCommandSucceeded() {
            var loginRequired = false;
            var stubIIdentityService = new StubIIdentityService();
            stubIIdentityService.LoginAsync(async () => {
                loginRequired = true;
                return new ServiceResult {Status = ServiceResultStatus.OK};
            });

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

            var loginViewModel = new LoginViewModel(stubIIdentityService,
                stubIRootNavigationService, stubIDialogService);
            loginViewModel.LoginCommand.Execute(null);

            Assert.IsTrue(loginRequired);
            Assert.IsTrue(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
        }

        [TestMethod]
        public void TestLoginCommandFailed() {
            var errorMessageToShow = "Error Message";

            var loginRequired = false;
            var stubIIdentityService = new StubIIdentityService();
            stubIIdentityService.LoginAsync(async () => {
                loginRequired = true;
                return new ServiceResult {
                    Status = ServiceResultStatus.BadRequest,
                    Message = errorMessageToShow
                };
            });

            var rootFrameNavigated = false;
            var stubIRootNavigationService = new StubIRootNavigationService();
            stubIRootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransition) =>
                    rootFrameNavigated = true);


            var dialogShown = false;
            var errorMessageShown = "";
            var stubIDialogService = new StubIDialogService();
            stubIDialogService.ShowAsync(async message => {
                dialogShown = true;
                errorMessageShown = message;
            });

            var loginViewModel = new LoginViewModel(stubIIdentityService,
                stubIRootNavigationService, stubIDialogService);
            loginViewModel.LoginCommand.Execute(null);

            Assert.IsTrue(loginRequired);
            Assert.IsFalse(rootFrameNavigated);
            Assert.IsTrue(dialogShown);
            Assert.AreEqual(
                LoginViewModel.LoginErrorMessage + errorMessageToShow,
                errorMessageShown);
        }
    }
}