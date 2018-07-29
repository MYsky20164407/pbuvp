using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvpClient.Models;
using UvpClient.Pages;

namespace UvpClient.Services {
    /// <summary>
    /// 个人作业服务接口。
    /// </summary>
    public interface IStudentAssignmentService {
        /// <summary>
        /// 获得个人作业。
        /// </summary>
        /// <param name="id">个人作业id。</param>
        /// <returns>个人作业。</returns>
        Task<ServiceResult<StudentAssignment>> GetAsync(int id);
    }
}