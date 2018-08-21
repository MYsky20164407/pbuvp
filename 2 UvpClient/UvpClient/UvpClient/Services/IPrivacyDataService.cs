using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     隐私数据服务。
    /// </summary>
    public interface IPrivacyDataService {
        /// <summary>
        ///     获得隐私数据。
        /// </summary>
        /// <returns>服务结果。</returns>
        Task<ServiceResult<PrivacyData>> GetAsync();
    }
}