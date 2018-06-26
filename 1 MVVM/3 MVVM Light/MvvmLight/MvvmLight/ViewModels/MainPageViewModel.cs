using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MvvmLight.Models;
using MvvmLight.Services;

namespace MvvmLight.ViewModels {
    public class MainPageViewModel : ViewModelBase {
        /******** 私有变量 ********/

        /// <summary>
        /// 联系人服务。
        /// </summary>
        private IContactService _contactService;

        /// <summary>
        /// 选中的联系人。
        /// </summary>
        private Contact _selectedContact;

        /******** 公开属性 ********/

        /// <summary>
        /// 选中的联系人。
        /// </summary>
        public Contact SelectedContact {
            get => _selectedContact;
            set => Set(nameof(SelectedContact), ref _selectedContact, value);
        }

        /******** 继承方法 ********/

        /******** 公开方法 ********/

        /******** 私有方法 ********/
    }
}