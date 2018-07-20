using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Security.Credentials;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using UwpSample;

namespace UvpClient.Services {
    /// <summary>
    ///     身份服务。
    /// </summary>
    // TODO read and test
    public class IdentityService : IIdentityService {
        /// <summary>
        ///     默认用户名。
        /// </summary>
        private const string DefaultUsername =
            App.QualifiedAppName + ".defaultusername";

        /// <summary>
        ///     RefreshToken资源名。
        /// </summary>
        private const string RefreshTokenResource =
            App.QualifiedAppName + ".RefreshToken";

        /// <summary>
        ///     AccessToken资源名。
        /// </summary>
        private const string AccessTokenResource =
            App.QualifiedAppName + ".AccessToken";

        /// <summary>
        ///     服务端点。
        /// </summary>
        private static readonly string ServerEndpoint = ResourceLoader
            .GetForCurrentView("AppSettings").GetString("UvpServerEndpoint");

        /// <summary>
        ///     Token锁。
        /// </summary>
        private readonly object _tokenLock = new object();

        /// <summary>
        ///     AccessToken。
        /// </summary>
        private string _accessToken;

        /// <summary>
        ///     RefreshToken。
        /// </summary>
        private string _refreshToken;

        /// <summary>
        ///     构造函数。
        /// </summary>
        public IdentityService() {
            var passwordVault = new PasswordVault();

            PasswordCredential refreshTokenCredential = null;
            try {
                refreshTokenCredential =
                    passwordVault.Retrieve(RefreshTokenResource,
                        DefaultUsername);
            } catch {
                // ignored
            }

            if (refreshTokenCredential != null) {
                refreshTokenCredential.RetrievePassword();
                _refreshToken = refreshTokenCredential.Password;
            }

            PasswordCredential accessTokenCredential = null;
            try {
                accessTokenCredential =
                    passwordVault.Retrieve(AccessTokenResource,
                        DefaultUsername);
            } catch {
                // ignored
            }

            if (accessTokenCredential != null) {
                accessTokenCredential.RetrievePassword();
                _accessToken = accessTokenCredential.Password;
            }
        }

        /// <summary>
        ///     获得RefreshTokenHandler。
        /// </summary>
        /// <returns>RefreshTokenHandler</returns>
        public RefreshTokenHandler GetRefreshTokenHandler() {
            if (string.IsNullOrEmpty(_refreshToken) ||
                string.IsNullOrEmpty(_accessToken))
                return null;

            var oidcClientOptions = CreateOidcClientOptions();
            var tokenClient =
                new TokenClient(ServerEndpoint + "/connect/token",
                        oidcClientOptions.ClientId,
                        oidcClientOptions.BackchannelHandler)
                    {Timeout = oidcClientOptions.BackchannelTimeout};

            var refreshTokenHandler = new RefreshTokenHandler(tokenClient,
                _refreshToken, _accessToken, new HttpClientHandler());
            refreshTokenHandler.TokenRefreshed +=
                RefreshTokenHandler_TokenRefreshed;

            return refreshTokenHandler;
        }

        /// <summary>
        ///     归还RefreshTokenHandler。
        /// </summary>
        /// <param name="refreshTokenHandler">要归还的RefreshTokenHandler。</param>
        public void ReturnRefreshTokenHandler(
            RefreshTokenHandler refreshTokenHandler) {
            refreshTokenHandler.TokenRefreshed -=
                RefreshTokenHandler_TokenRefreshed;
        }

        /// <summary>
        ///     登录。
        /// </summary>
        /// <returns>是否成功登录。</returns>
        public async Task<LoginReturn> LoginAsync() {
            var oidcClient = new OidcClient(CreateOidcClientOptions());
            var loginResult = await oidcClient.LoginAsync(new LoginRequest());

            if (!string.IsNullOrEmpty(loginResult.Error))
                return new LoginReturn
                    {Succeeded = false, Error = loginResult.Error};

            var refreshTokenHandler =
                (RefreshTokenHandler) loginResult.RefreshTokenHandler;
            _refreshToken = refreshTokenHandler.RefreshToken;
            _accessToken = refreshTokenHandler.AccessToken;

            return new LoginReturn {Succeeded = true, Error = ""};
        }

        /// <summary>
        ///     保存。
        /// </summary>
        public void Save() {
            var passwordVault = new PasswordVault();
            passwordVault.Add(new PasswordCredential(RefreshTokenResource,
                DefaultUsername, _refreshToken));
            passwordVault.Add(new PasswordCredential(AccessTokenResource,
                DefaultUsername, _accessToken));
        }


        /// <summary>
        ///     创建OidcClientOptions。
        /// </summary>
        private OidcClientOptions CreateOidcClientOptions() {
            return new OidcClientOptions {
                Authority = ServerEndpoint, ClientId = "native.hybrid",
                Scope = "openid profile api offline_access",
                RedirectUri = App.QualifiedAppName + "://callback",
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Browser = new SystemBrowser()
            };
        }

        /// <summary>
        ///     RefreshTokenHandler TokenRefreshed事件处理函数。
        /// </summary>
        private void RefreshTokenHandler_TokenRefreshed(object sender,
            TokenRefreshedEventArgs e) {
            lock (_tokenLock) {
                _refreshToken = e.RefreshToken;
                _accessToken = e.AccessToken;
            }
        }
    }
}