using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BasicUwp.Models;
using Newtonsoft.Json;

namespace BasicUwp.Services {
    /// <summary>
    /// 联系人服务。
    /// </summary>
    public class ContactService : IContactService {
        /// <summary>
        /// 服务端点。
        /// </summary>
        private const string ServiceEndpoint =
            "http://localhost:54652/api/Contacts";

        /// <summary>
        /// 列出所有联系人。
        /// </summary>
        /// <returns>所有联系人。</returns>
        public async Task<IEnumerable<Contact>> ListAsync() {
            using (var client = new HttpClient()) {
                var json = await client.GetStringAsync(ServiceEndpoint);
                return JsonConvert.DeserializeObject<Contact[]>(json);
            }
        }

        /// <summary>
        /// 更新联系人。
        /// </summary>
        /// <param name="contact">要更新的联系人。</param>
        public async Task UpdateAsync(Contact contact) {
            using (var client = new HttpClient()) {
                var json = JsonConvert.SerializeObject(contact);
                await client.PutAsync(ServiceEndpoint + "/" + contact.Id,
                    new StringContent(json));
            }
        }
    }
}