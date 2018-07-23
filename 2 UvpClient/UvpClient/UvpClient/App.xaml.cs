using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using UvpClient.Pages;
using UvpClient.Services;
using UwpSample;

namespace UvpClient {
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App {
        /// <summary>
        ///     应用全名。
        /// </summary>
        public const string QualifiedAppName = "cn.edu.neu.uvp.client";

        /// <summary>
        ///     HttpClient错误信息。
        /// </summary>
        public const string HttpClientErrorMessage =
            "Sorry!!!\n\nAn error occurred when we tried to send your request to our server.\nPlease screenshot this dialog and send it to your teacher.\n\nError:\n";

        /// <summary>
        ///     服务端点。
        /// </summary>
        public static readonly string ServerEndpoint = ResourceLoader
            .GetForCurrentView("AppSettings").GetString("UvpServerEndpoint");

        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null) {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState ==
                    ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null)
                    rootFrame.Navigate(typeof(MyUvpPage), e.Arguments);

                // Ensure the current window is active
                Window.Current.Activate();
            }

            DispatcherHelper.Initialize();
        }

        /// <summary>
        ///     Invoked when the application is launched through a custom URI scheme, such as
        ///     is the case in an OAuth 2.0 authorization flow.
        /// </summary>
        /// <param name="args">Details about the URI that activated the app.</param>
        protected override void OnActivated(IActivatedEventArgs args) {
            // When the app was activated by a Protocol (custom URI scheme), forwards
            // the URI to the SystemBrowser through a static method.
            if (args.Kind == ActivationKind.Protocol) {
                // Extracts the authorization response URI from the arguments.
                var protocolArgs = (ProtocolActivatedEventArgs) args;
                var uri = protocolArgs.Uri;
                Debug.WriteLine("Authorization Response: " + uri.AbsoluteUri);
                SystemBrowser.ProcessResponse(uri);
            }
        }

        /// <summary>
        ///     Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender,
            NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " +
                                e.SourcePageType.FullName);
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();

            var identityService =
                SimpleIoc.Default.GetInstance<IIdentityService>();
            identityService.Save();

            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}