using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     学生服务接口。
    /// </summary>
    public interface IStudentService {
        /// <summary>
        ///     根据学号获得学生。
        /// </summary>
        /// <param name="studentId">学号。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult<Student>> GetStudentByStudentIdAsync(
            string studentId);

        /// <summary>
        ///     绑定账号。
        /// </summary>
        /// <param name="studentId">学号。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult> BindAccountAsync(string studentId);

        /// <summary>
        ///     获得我。
        /// </summary>
        /// <returns>服务结果。</returns>
        Task<ServiceResult<Student>> GetMeAsync();
    }
}