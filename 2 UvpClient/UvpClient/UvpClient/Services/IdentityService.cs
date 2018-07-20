using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Security.Credentials;
using Windows.Storage;
using IdentityModel.Client;
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

        private readonly OidcClientOptions _oidcClientOptions =
            new OidcClientOptions {
                Authority = ServerEndpoint, ClientId = "native.hybrid",
                Scope = "openid profile api offline_access",
                RedirectUri = App.QualifiedAppName + "://callback",
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Browser = new SystemBrowser()
            };

        /// <summary>
        ///     RefreshTokenHandler。
        /// </summary>
        private RefreshTokenHandler _refreshTokenHandler;

        /// <summary>
        ///     登录。
        /// </summary>
        /// <returns>是否成功登录。</returns>
        public async Task<LoginReturn> LoginAsync() {
            var oidcClient = new OidcClient(_oidcClientOptions);
            var loginResult = await oidcClient.LoginAsync(new LoginRequest());

            if (!string.IsNullOrEmpty(loginResult.Error))
                return new LoginReturn
                    {Succeeded = false, Error = loginResult.Error};

            var refreshTokenHandler = loginResult.RefreshTokenHandler;


            _refreshTokenHandler = new RefreshTokenHandler(TokenClientFactory);

            loginResult.RefreshTokenHandler _loginResultStorage =
                new LoginResultStorage {
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


        /// <summary>
        ///     获得RefreshTokenHandler。
        /// </summary>
        /// <returns>RefreshTokenHandler</returns>
        public RefreshTokenHandler GetRefreshTokenHandlerAsync() {
            return _refreshTokenHandler;
        }

        // https://github.com/IdentityModel/IdentityModel.OidcClient2/blob/dev/src/IdentityModel.OidcClient/Infrastructure/TokenClientFactory.cs
        private TokenClient CreateTokenClient(OidcClientOptions options) {
            var info = options.ProviderInformation;
            var handler = options.BackchannelHandler ?? new HttpClientHandler();

            var tokenClient = new TokenClient(info.TokenEndpoint,
                options.ClientId, handler);
            tokenClient.Timeout = options.BackchannelTimeout;
            return tokenClient;
        }

        /// <summary>
        ///     RefreshTokenHandler存储。
        /// </summary>
        private class RefreshTokenHandlerStorage {
            public string RefreshToken { get; set; }
            public string AccessToken { get; set; }
        }
    }
}