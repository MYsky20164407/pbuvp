using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UvpClient.Pages;
using UvpClient.Services;
using UvpClient.ViewModels;

namespace UvpClient.UnitTest.ViewModels {
    [TestClass]
    public class LoginViewModelTest {
        [TestMethod]
        public void TestLoginCommand() {
            bool loginRequired = false;
            var identityService = new StubIIdentityService();
            identityService.LoginAsync(async () => new LoginReturn
                {Succeeded = loginRequired = true});

            bool rootFrameNavigated = false;
            var rootNavigationService = new StubIRootNavigationService();
            rootNavigationService.Navigate(
                (sourcePageType, parameter, navigationTransitionInfo) =>
                    rootFrameNavigated =
                        sourcePageType == typeof(MyUvpPage) &&
                        parameter == null &&
                        navigationTransitionInfo is
                            EntranceNavigationTransitionInfo);

            bool dialogShown = false;
            var dialogService = new StubIDialogService();
            dialogService.Show(async (message) => dialogShown = true);

            var loginViewModel = new LoginViewModel(identityService,
                rootNavigationService, dialogService);
            loginViewModel.LoginCommand.Execute(null);

            Assert.IsTrue(loginRequired);
            Assert.IsTrue(rootFrameNavigated);
            Assert.IsFalse(dialogShown);
        }
    }
}