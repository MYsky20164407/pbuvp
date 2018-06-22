using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicMvvm.Helpers;
using BasicMvvm.Models;
using BasicMvvm.Services;

namespace BasicMvvm.ViewModels {
    /// <summary>
    /// 主页ViewModel类。
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged {
        /******** 私有变量 ********/

        /// <summary>
        /// 联系人服务。
        /// </summary>
        private IContactService _contactService;

        /// <summary>
        /// 导航服务。
        /// </summary>
        private INavigationService _navigationService;

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
        private RelayCommand<Contact> _saveCommand;

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

            set {
                if (_selectedContact == value) {
                    return;
                }

                _selectedContact = value;
                RaisePropertyChanged(nameof(SelectedContact));
            }
        }

        /// <summary>
        /// 刷新命令。
        /// </summary>
        public RelayCommand ListCommand =>
            _listCommand ?? (_listCommand =
                new RelayCommand(async () => { await List(); }));

        /// <summary>
        /// 保存命令。
        /// </summary>
        public RelayCommand<Contact> SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand<Contact>(
                async contact => {
                    var service = _contactService;
                    await service.UpdateAsync(contact);
                }));

        /// <summary>
        /// 详细信息命令。
        /// </summary>
        public RelayCommand<Contact> ShowDetailsCommand =>
            _showDetailsCommand ?? (_showDetailsCommand =
                new RelayCommand<Contact>(contact => {
                    SelectedContact = contact;
                    _navigationService.NavigateTo(typeof(DetailPage));
                }));

        /******** 继承方法 ********/

        /// <summary>
        /// INotifyPropertyChanged。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /******** 公开方法 ********/

        public MainPageViewModel(IContactService contactService,
            INavigationService navigationService) {
            _contactService = contactService;
            _navigationService = navigationService;
        }

        public MainPageViewModel() : this(new ContactService(),
            new NavigationService()) { }

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

        /******** 私有方法 ********/

        protected virtual void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
    }
}