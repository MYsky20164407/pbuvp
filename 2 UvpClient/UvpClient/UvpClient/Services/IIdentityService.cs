using System.Threading.Tasks;
using IdentityModel.Client;

namespace UvpClient.Services {
    /// <summary>
    ///     身份服务接口。
    /// </summary>
    public interface IIdentityService {
        /// <summary>
        ///     获得RefreshTokenHandler。
        /// </summary>
        /// <returns>RefreshTokenHandler</returns>
        RefreshTokenHandler GetRefreshTokenHandler();

        /// <summary>
        /// 归还RefreshTokenHandler。
        /// </summary>
        /// <param name="refreshTokenHandler">要归还的RefreshTokenHandler。</param>
        void ReturnRefreshTokenHandler(RefreshTokenHandler refreshTokenHandler);

        /// <summary>
        ///     登录。
        /// </summary>
        /// <returns>是否成功登录。</returns>
        Task<LoginReturn> LoginAsync();

        /// <summary>
        ///     保存。
        /// </summary>
        void Save();
    }

    /// <summary>
    ///     登录结果。
    /// </summary>
    public class LoginReturn {
        /// <summary>
        ///     是否成功。
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        ///     错误信息。
        /// </summary>
        public string Error { get; set; }
    }
}