using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     小组作业服务接口。
    /// </summary>
    public interface IGroupAssignmentService {
        /// <summary>
        ///     获得小组作业。
        /// </summary>
        /// <param name="id">小组作业id。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult<GroupAssignment>> GetAsync(int id);

        /// <summary>
        ///     提交小组作业。
        /// </summary>
        /// <param name="groupAssignment">小组作业。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult> SubmitAsync(GroupAssignment groupAssignment);
    }
}