using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     个人作业服务接口。
    /// </summary>
    public interface IStudentAssignmentService {
        /// <summary>
        ///     获得个人作业。
        /// </summary>
        /// <param name="id">个人作业id。</param>
        /// <returns>个人作业。</returns>
        Task<ServiceResult<StudentAssignment>> GetAsync(int id);

        /// <summary>
        ///     提交个人作业。
        /// </summary>
        /// <param name="groupAssignment">个人作业。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult> SubmitAsync(StudentAssignment studentAssignment);
    }
}