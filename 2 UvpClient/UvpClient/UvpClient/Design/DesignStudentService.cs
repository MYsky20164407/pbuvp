using System;
using System.Threading.Tasks;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.Design {
    public class DesignStudentService : IStudentService {
        public async Task<ServiceResult<Student>> GetStudentByStudentIdAsync(
            string studentId) {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> BindAccountAsync(string studentId) {
            throw new NotImplementedException();
        }
    }
}