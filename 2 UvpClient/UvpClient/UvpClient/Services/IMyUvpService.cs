using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     我的uvp服务接口。
    /// </summary>
    public interface IMyUvpService {
        /// <summary>
        ///     获得我的uvp。
        /// </summary>
        /// <returns>我的uvp。</returns>
        Task<ServiceResult<MyUvp>> GetAsync();
    }
}