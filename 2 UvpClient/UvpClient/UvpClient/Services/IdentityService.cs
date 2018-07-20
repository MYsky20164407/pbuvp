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

            var refreshTokenHandler =
                (RefreshTokenHandler) loginResult.RefreshTokenHandler;
            _refreshTokenHandler = CreateRefreshTokenHandler(
                refreshTokenHandler.RefreshToken,
                refreshTokenHandler.AccessToken);

            return new LoginReturn {Succeeded = true, Error = ""};
        }


        /// <summary>
        ///     获得RefreshTokenHandler。
        /// </summary>
        /// <returns>RefreshTokenHandler</returns>
        public RefreshTokenHandler GetRefreshTokenHandler() {
            return _refreshTokenHandler;
        }

        public async Task SaveAsync() {
            var passwordVault = new PasswordVault();
            passwordVault.Add(new PasswordCredential(
                App.QualifiedAppName + "." +
                nameof(_refreshTokenHandler.RefreshToken), DefaultUsername,
                _refreshTokenHandler.RefreshToken));
            passwordVault.Add(new PasswordCredential(
                App.QualifiedAppName + "." +
                nameof(_refreshTokenHandler.AccessToken), DefaultUsername,
                _refreshTokenHandler.AccessToken));
        }

        /// <summary>
        ///     创建RefreshTokenHandler。
        /// </summary>
        /// <param name="refreshToken">RefreshToken</param>
        /// <param name="accessToken">AccessToken。</param>
        /// <returns>RefreshTokenHandler</returns>
        private RefreshTokenHandler CreateRefreshTokenHandler(
            string refreshToken, string accessToken) {
            var info = _oidcClientOptions.ProviderInformation;
            var handler = _oidcClientOptions.BackchannelHandler ??
                          new HttpClientHandler();

            var tokenClient = new TokenClient(info.TokenEndpoint,
                _oidcClientOptions.ClientId, handler);
            tokenClient.Timeout = _oidcClientOptions.BackchannelTimeout;

            return new RefreshTokenHandler(tokenClient, refreshToken,
                accessToken, _oidcClientOptions.RefreshTokenInnerHttpHandler);
        }
    }
}