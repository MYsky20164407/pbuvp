using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace WindowsCommunityToolkit.Models {
    /// <summary>
    /// 联系人类。
    /// </summary>
    public class Contact : ObservableObject {
        /// <summary>
        /// 主键。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名。
        /// </summary>
        private string _firstName;

        /// <summary>
        /// 名。
        /// </summary>
        public string FirstName {
            get => _firstName;
            set => Set(nameof(FirstName), ref _firstName, value);
        }

        private string _lastName;

        /// <summary>
        /// 姓。
        /// </summary>
        public string LastName {
            get => _lastName;
            set => Set(nameof(LastName), ref _lastName, value);
        }

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