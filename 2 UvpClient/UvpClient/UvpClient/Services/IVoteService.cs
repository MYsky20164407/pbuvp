using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     投票服务接口。
    /// </summary>
    public interface IVoteService {
        /// <summary>
        ///     提交投票。
        /// </summary>
        /// <param name="vote">投票。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult> SubmitAsync(Vote vote);
    }
}