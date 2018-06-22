using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicMvvm.Models;

namespace BasicMvvm.Services {
    /// <summary>
    /// 联系人服务接口。
    /// </summary>
    public interface IContactService {
        /// <summary>
        /// 列出所有联系人。
        /// </summary>
        /// <returns>所有联系人。</returns>
        Task<IEnumerable<Contact>> ListAsync();

        /// <summary>
        /// 更新联系人。
        /// </summary>
        /// <param name="contact">要更新的联系人。</param>
        Task UpdateAsync(Contact contact);
    }
}