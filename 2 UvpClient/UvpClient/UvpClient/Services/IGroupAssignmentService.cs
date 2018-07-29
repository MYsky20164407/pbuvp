using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     小组作业服务接口。
    /// </summary>
    public interface IGroupAssignmentService {
        /// <summary>
        ///     获得个人作业。
        /// </summary>
        /// <param name="id">小组作业id。</param>
        /// <returns>小组作业。</returns>
        Task<ServiceResult<GroupAssignment>> GetAsync(int id);
    }
}