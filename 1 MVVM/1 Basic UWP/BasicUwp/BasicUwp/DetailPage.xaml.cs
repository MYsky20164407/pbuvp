using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BasicUwp {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page {
        private Contact _contact;

        public DetailPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            _contact = (Contact) e.Parameter;

            var backStack = Frame.BackStack;
            var backStackCount = backStack.Count;

            if (backStackCount > 0) {
                var masterPageEntry = backStack[backStackCount - 1];
                backStack.RemoveAt(backStackCount - 1);

                var modifiedEntry = new PageStackEntry(
                    masterPageEntry.SourcePageType, _contact,
                    masterPageEntry.NavigationTransitionInfo);
                backStack.Add(modifiedEntry);
            }

            SystemNavigationManager systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += DetailPage_BackRequested;

            FirstNameTextBox.Text = _contact.FirstName;
            LastNameTextBox.Text = _contact.LastName;

            PreviewFirstNameTextBlock.Text = _contact.FirstName;
            PreviewLastNameTextBlock.Text = _contact.LastName;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            SystemNavigationManager systemNavigationManager =
                SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= DetailPage_BackRequested;
        }

        private void OnBackRequested() {
            Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private void NavigateBackForWideState(bool useTransition) {
            NavigationCacheMode = NavigationCacheMode.Disabled;

            if (useTransition) {
                Frame.GoBack(new EntranceNavigationTransitionInfo());
            } else {
                Frame.GoBack(new SuppressNavigationTransitionInfo());
            }
        }

        private bool ShouldGoToWideState() {
            return Window.Current.Bounds.Width >= 720;
        }

        private void DetailPage_OnLoaded(object sender, RoutedEventArgs e) {
            if (ShouldGoToWideState()) {
                NavigateBackForWideState(true);
            }

            Window.Current.SizeChanged += Window_SizeChanged;
        }

        private void DetailPage_OnUnloaded(object sender, RoutedEventArgs e) {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender,
            WindowSizeChangedEventArgs e) {
            if (ShouldGoToWideState()) {
                Window.Current.SizeChanged -= Window_SizeChanged;
                NavigateBackForWideState(false);
            }
        }

        private void DetailPage_BackRequested(object sender,
            BackRequestedEventArgs e) {
            e.Handled = true;
            OnBackRequested();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            OnBackRequested();
        }

        private void FirstNameTextBox_OnTextChanged(object sender,
            TextChangedEventArgs e) {
            _contact.FirstName = FirstNameTextBox.Text;
            PreviewFirstNameTextBlock.Text = _contact.FirstName;
        }

        private void LastNameTextBox_OnTextChanged(object sender,
            TextChangedEventArgs e) {
            _contact.LastName = LastNameTextBox.Text;
            PreviewLastNameTextBlock.Text = _contact.LastName;
        }

        private async void
            SaveButton_OnClick(object sender, RoutedEventArgs e) {
            var contactService = new ContactService();
            await contactService.UpdateAsync(_contact);
        }
    }
}