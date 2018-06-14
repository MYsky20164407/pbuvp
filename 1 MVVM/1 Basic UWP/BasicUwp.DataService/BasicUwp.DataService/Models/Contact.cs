using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BasicUwp.DataService.Models {
    /// <summary>
    /// 联系人类。
    /// </summary>
    public class Contact {
        /// <summary>
        /// 主键。
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 名。
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// 姓。
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// 生日。
        /// </summary>
        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 网址。
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}