using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using BasicUwp.Models;
using BasicUwp.Services;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace BasicUwp {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
        private Contact _lastSelectedContact;

        public MainPage() {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var contactList = MasterListView.ItemsSource as List<Contact>;

            if (contactList == null) {
                var contactService = new ContactService();
                contactList = (await contactService.ListAsync()).ToList();
                MasterListView.ItemsSource = contactList;
            }

            if (e.Parameter is Contact contact) {
                _lastSelectedContact =
                    contactList.FirstOrDefault(p => p.Id == contact.Id);
            }

            UpdateForVisualState(AdaptiveStates.CurrentState);
        }

        private void AdaptiveStates_OnCurrentStateChanged(object sender,
            VisualStateChangedEventArgs e) {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState,
            VisualState oldState = null) {
            var isNarrow = newState == NarrowState;

            if (isNarrow && oldState == DefaultState &&
                _lastSelectedContact != null) {
                Frame.Navigate(typeof(DetailPage), _lastSelectedContact,
                    new SuppressNavigationTransitionInfo());
            }
        }

        private void MasterListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var clickedContact = (Contact) e.ClickedItem;
            _lastSelectedContact = clickedContact;

            if (AdaptiveStates.CurrentState == NarrowState) {
                Frame.Navigate(typeof(DetailPage), clickedContact,
                    new DrillInNavigationTransitionInfo());
            } else {
                FirstNameTextBox.Text = clickedContact.FirstName;
                LastNameTextBox.Text = clickedContact.LastName;

                PreviewFirstNameTextBlock.Text = clickedContact.FirstName;
                PreviewLastNameTextBlock.Text = clickedContact.LastName;
            }
        }

        private void LayoutRoot_OnLoaded(object sender, RoutedEventArgs e) {
            MasterListView.SelectedItem = _lastSelectedContact;
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e) {
            RefreshContainer.RequestRefresh();
        }

        private async void RefreshContainer_OnRefreshRequested(
            RefreshContainer sender, RefreshRequestedEventArgs args) {
            using (var refrechCompletionDeferral = args.GetDeferral()) {
                var contactService = new ContactService();
                var contactList = (await contactService.ListAsync()).ToList();
                MasterListView.ItemsSource = contactList;
            }
        }

        private void RefreshVisualizer_OnRefreshStateChanged(
            RefreshVisualizer sender, RefreshStateChangedEventArgs args) {
            if (args.NewState == RefreshVisualizerState.Refreshing) {
                RefreshButton.IsEnabled = false;
            } else {
                RefreshButton.IsEnabled = true;
            }
        }

        private void FirstNameTextBox_OnTextChanged(object sender,
            TextChangedEventArgs e)
        {
            _lastSelectedContact.FirstName = FirstNameTextBox.Text;
            PreviewFirstNameTextBlock.Text = _lastSelectedContact.FirstName;
        }

        private void LastNameTextBox_OnTextChanged(object sender,
            TextChangedEventArgs e)
        {
            _lastSelectedContact.LastName = LastNameTextBox.Text;
            PreviewLastNameTextBlock.Text = _lastSelectedContact.LastName;
        }

        private async void
            SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contactService = new ContactService();
            await contactService.UpdateAsync(_lastSelectedContact);
        }
    }
}