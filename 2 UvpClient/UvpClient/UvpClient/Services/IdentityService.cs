using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Security.Credentials;
using Windows.Storage;
using IdentityModel.OidcClient;
using UwpSample;

namespace UvpClient.Services {
    /// <summary>
    ///     身份服务。
    /// </summary>
    // TODO test
    public class IdentityService : IIdentityService {
        /// <summary>
        ///     默认用户名。
        /// </summary>
        private const string DefaultUsername =
            App.QualifiedAppName + ".defaultusername";

        /// <summary>
        ///     服务端点。
        /// </summary>
        private static readonly string ServerEndpoint = ResourceLoader
            .GetForCurrentView("AppSettings").GetString("UvpServerEndpoint");

        /// <summary>
        ///     Oidc客户端登录结果存储。
        /// </summary>
        private LoginResultStorage _loginResultStorage;

        /// <summary>
        ///     获得refresh token handler。
        /// </summary>
        /// <returns>Refresh token handler</returns>
        public async Task<HttpMessageHandler> GetRefreshTokenHandlerAsync() {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        ///     登录。
        /// </summary>
        /// <returns>是否成功登录。</returns>
        public async Task<LoginReturn> LoginAsync() {
            var oidcClientOptions = new OidcClientOptions {
                Authority = ServerEndpoint, ClientId = "native.hybrid",
                Scope = "openid profile api offline_access",
                RedirectUri = App.QualifiedAppName + "://callback",
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Browser = new SystemBrowser()
            };

            var oidcClient = new OidcClient(oidcClientOptions);
            var loginResult = await oidcClient.LoginAsync(new LoginRequest());

            if (!string.IsNullOrEmpty(loginResult.Error))
                return new LoginReturn
                    {Succeeded = false, Error = loginResult.Error};

            _loginResultStorage = new LoginResultStorage {
                AccessToken = loginResult.AccessToken,
                IdentityToken = loginResult.IdentityToken,
                RefreshToken = loginResult.RefreshToken,
                AccessTokenExpiration = loginResult.AccessTokenExpiration,
                AuthenticationTime = loginResult.AuthenticationTime
            };
            foreach (var userClaim in loginResult.User.Claims)
                switch (userClaim.Type) {
                    case nameof(_loginResultStorage.sid):
                        _loginResultStorage.sid = userClaim.Value;
                        break;
                    case nameof(_loginResultStorage.sub):
                        _loginResultStorage.sub = userClaim.Value;
                        break;
                    case nameof(_loginResultStorage.idp):
                        _loginResultStorage.idp = userClaim.Value;
                        break;
                    case nameof(_loginResultStorage.amr):
                        _loginResultStorage.amr = userClaim.Value;
                        break;
                    case nameof(_loginResultStorage.name):
                        _loginResultStorage.name = userClaim.Value;
                        break;
                    case nameof(_loginResultStorage.preferred_username):
                        _loginResultStorage.preferred_username =
                            userClaim.Value;
                        break;
                }

            var loginResultStorageCompositeValue =
                new ApplicationDataCompositeValue {
                    [nameof(_loginResultStorage.sid)] = _loginResultStorage.sid,
                    [nameof(_loginResultStorage.sub)] = _loginResultStorage.sub,
                    [nameof(_loginResultStorage.idp)] = _loginResultStorage.idp,
                    [nameof(_loginResultStorage.amr)] = _loginResultStorage.amr,
                    [nameof(_loginResultStorage.name)] =
                        _loginResultStorage.name,
                    [_loginResultStorage.preferred_username] =
                        _loginResultStorage.preferred_username,
                    [nameof(_loginResultStorage.AccessTokenExpiration)] =
                        _loginResultStorage.AccessTokenExpiration,
                    [nameof(_loginResultStorage.AuthenticationTime)] =
                        _loginResultStorage.AuthenticationTime
                };
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[nameof(_loginResultStorage)] =
                loginResultStorageCompositeValue;

            var passwordVault = new PasswordVault();
            passwordVault.Add(new PasswordCredential(
                App.QualifiedAppName + "." +
                nameof(_loginResultStorage.AccessToken), DefaultUsername,
                _loginResultStorage.AccessToken));
            passwordVault.Add(new PasswordCredential(
                App.QualifiedAppName + "." +
                nameof(_loginResultStorage.IdentityToken), DefaultUsername,
                _loginResultStorage.IdentityToken));
            passwordVault.Add(new PasswordCredential(
                App.QualifiedAppName + "." +
                nameof(_loginResultStorage.RefreshToken), DefaultUsername,
                _loginResultStorage.RefreshToken));

            return new LoginReturn {Succeeded = true, Error = ""};
        }

        public IdentityService() {
            // TODO Load data from roaming settings and password vault
        }

        /// <summary>
        ///     Oidc客户端登录结果存储。
        /// </summary>
        private class LoginResultStorage {
            public string sid { get; set; }
            public string sub { get; set; }
            public string idp { get; set; }
            public string amr { get; set; }
            public string name { get; set; }
            public string preferred_username { get; set; }
            public string AccessToken { get; set; }
            public string IdentityToken { get; set; }
            public string RefreshToken { get; set; }
            public DateTime AccessTokenExpiration { get; set; }
            public DateTime AuthenticationTime { get; set; }
        }
    }
}