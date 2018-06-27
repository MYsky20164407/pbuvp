using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight.Design;
using MvvmLight.Models;
using MvvmLight.Services;

namespace MvvmLight.ViewModels {
    /// <summary>
    /// 主页ViewModel类。
    /// </summary>
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

        /// <summary>
        /// 刷新命令。
        /// </summary>
        private RelayCommand _listCommand;

        /// <summary>
        /// 保存命令。
        /// </summary>
        private RelayCommand<Contact> _updateCommand;

        /// <summary>
        /// 详细信息命令。
        /// </summary>
        private RelayCommand<Contact> _showDetailsCommand;

        /******** 公开属性 ********/

        /// <summary>
        /// 联系人集合。
        /// </summary>
        public ObservableCollection<Contact> ContactCollection {
            get;
            private set;
        }

        /// <summary>
        /// 选中的联系人。
        /// </summary>
        public Contact SelectedContact {
            get => _selectedContact;
            set => Set(nameof(SelectedContact), ref _selectedContact, value);
        }

        /// <summary>
        /// 刷新命令。
        /// </summary>
        public RelayCommand ListCommand =>
            _listCommand ?? (_listCommand =
                new RelayCommand(async () => { await List(); }));

        /// <summary>
        /// 更新命令。
        /// </summary>
        public RelayCommand<Contact> UpdateCommand =>
            _updateCommand ?? (_updateCommand = new RelayCommand<Contact>(
                async contact => {
                    var service = _contactService;
                    await service.UpdateAsync(contact);
                }));

        /******** 继承方法 ********/

        /******** 公开方法 ********/

        public MainPageViewModel(IContactService contactService) {
            _contactService = contactService;
            ContactCollection = new ObservableCollection<Contact>();
        }

        public MainPageViewModel() : this(DesignMode.DesignModeEnabled ?
            (IContactService) new DesignContactService() :
            new ContactService()) {
#if DEBUG
            if (DesignMode.DesignModeEnabled) {
                List();
                SelectedContact = ContactCollection[0];
            }
#endif
        }

        /******** 私有方法 ********/

        /// <summary>
        /// 执行刷新操作。
        /// </summary>
        private async Task List() {
            ContactCollection.Clear();

            var contacts = await _contactService.ListAsync();
            foreach (var contact in contacts) {
                ContactCollection.Add(contact);
            }
        }
    }
}