using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicUwp.Models
{
    /// <summary>
    /// 联系人类。
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// 主键。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名。
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓。
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 生日。
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 网址。
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }
    }
}
