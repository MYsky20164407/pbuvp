using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IdentityModel.OidcClient;
using UvpClient.Services;
using UwpSample;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UvpClient {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }

        private async void
            ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            var identityService = new IdentityService();
            var refreshTokenHandler = identityService.GetRefreshTokenHandler();
            if (refreshTokenHandler == null) {
                await identityService.LoginAsync();
                refreshTokenHandler = identityService.GetRefreshTokenHandler();
            }

            using (var httpClient = new HttpClient(refreshTokenHandler)) {
                Json.Text =
                    await httpClient.GetStringAsync(
                        "https://localhost:5090/api/Values");
            }

            identityService.ReturnRefreshTokenHandler(refreshTokenHandler);

            identityService.Save();
        }
    }
}