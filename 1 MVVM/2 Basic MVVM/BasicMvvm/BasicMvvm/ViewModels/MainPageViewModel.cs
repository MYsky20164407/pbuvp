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
    public class MainPageViewModel : INotifyPropertyChanged {
        private IContactService _contactService;

        public ObservableCollection<Contact> ContactCollection {
            get;
            private set;
        }

        private Contact _selectedContact;

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

        private RelayCommand _listCommand;

        public RelayCommand ListCommand =>
            _listCommand ?? (_listCommand =
                new RelayCommand(async () => { await List(); }));

        private async Task List() {
            ContactCollection.Clear();

            var contacts = await _contactService.ListAsync();
            foreach (var contact in contacts) {
                ContactCollection.Add(contact);
            }
        }

        private RelayCommand<Contact> _saveCommand;

        public RelayCommand<Contact> SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand<Contact>(
                async contact => {
                    var service = _contactService;
                    await service.UpdateAsync(contact);
                }));

        public MainPageViewModel(IContactService contactService) {
            _contactService = contactService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
    }
}