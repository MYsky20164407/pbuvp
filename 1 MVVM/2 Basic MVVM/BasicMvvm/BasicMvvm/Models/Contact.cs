using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMvvm.Models {
    /// <summary>
    /// 联系人类。
    /// </summary>
    public class Contact : INotifyPropertyChanged {
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

            set {
                if (_firstName == value) {
                    return;
                }

                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        private string _lastName;

        /// <summary>
        /// 姓。
        /// </summary>
        public string LastName {
            get => _lastName;

            set {
                if (_lastName == value) {
                    return;
                }

                _lastName = value;
                RaisePropertyChanged("LastName");
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
    }
}