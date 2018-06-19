using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;
using BasicUwp.Services;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace BasicUwp {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
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

        private void MasterListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            // throw new NotImplementedException();
        }

        private void AdaptiveStates_OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e) {
            // throw new NotImplementedException();
        }
    }
}