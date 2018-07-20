using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var options = new OidcClientOptions
            {
                Authority = "https://localhost:5090",
                ClientId = "native.hybrid",
                Scope = "openid profile api offline_access",
                RedirectUri = "cn.edu.neu.uvp.client://callback",
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Browser = new SystemBrowser()
            };

            var client = new OidcClient(options);
            var result = await client.LoginAsync(new LoginRequest());

            var sb = new StringBuilder(128);

            foreach (var claim in result.User.Claims)
            {
                sb.AppendLine($"{claim.Type}: {claim.Value}");
            }

            sb.AppendLine($"refresh token: {result.RefreshToken}");
            sb.AppendLine($"access token: {result.AccessToken}");

            Result.Text = sb.ToString();
        }
    }
}