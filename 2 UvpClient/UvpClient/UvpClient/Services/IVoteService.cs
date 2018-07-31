using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     投票服务接口。
    /// </summary>
    public interface IVoteService {
        /// <summary>
        ///     获得投票。
        /// </summary>
        /// <param name="id">投票id。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult<Vote>> GetAsync(int id);

        /// <summary>
        ///     提交投票。
        /// </summary>
        /// <param name="vote">投票。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult> SubmitAsync(Vote vote);
    }
}