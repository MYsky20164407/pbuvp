using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmLight.Models;
using MvvmLight.Services;

namespace MvvmLight.Design {
    /// <summary>
    /// 设计时联系人服务。
    /// </summary>
    public class DesignContactService : IContactService {
        /******** 私有变量 ********/

        /******** 公开属性 ********/

        /******** 继承方法 ********/

        /// <summary>
        /// 列出所有联系人。
        /// </summary>
        /// <returns>所有联系人。</returns>
        public async Task<IEnumerable<Contact>> ListAsync() {
            var result = new List<Contact>();

            for (int i = 1; i <= 12; i++) {
                result.Add(new Contact {
                    FirstName = "FirstName" + i, LastName = "LastName" + i,
                    Id = i,
                    Avatar =
                        String.Format("http://localhost:54652/images/{0}.jpg",
                            i)
                });
            }

            return result;
        }

        /// <summary>
        /// 更新联系人。
        /// </summary>
        /// <param name="contact">要更新的联系人。</param>
        public Task UpdateAsync(Contact contact) {
            throw new NotImplementedException();
        }

        /******** 公开方法 ********/

        /******** 私有方法 ********/
    }
}