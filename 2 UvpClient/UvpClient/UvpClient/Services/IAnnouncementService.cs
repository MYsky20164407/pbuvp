using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    /// 通知服务接口。
    /// </summary>
    public interface IAnnouncementService {
        /// <summary>
        /// 获得所有通知。
        /// </summary>
        /// <returns>所有通知。</returns>
        Task<ServiceResult<IEnumerable<Announcement>>> GetAsync();
    }
}