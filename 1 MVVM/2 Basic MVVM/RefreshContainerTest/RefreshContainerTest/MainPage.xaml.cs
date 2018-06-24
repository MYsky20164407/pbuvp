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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RefreshContainerTest {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public ObservableCollection<ListViewItem> ListViewItemCollection =
            new ObservableCollection<ListViewItem>();

        public MainPage() {
            this.InitializeComponent();
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e) {
            ListView.ItemsSource = ListViewItemCollection;
        }

        private void RefreshContainer_OnRefreshRequested(
            RefreshContainer sender, RefreshRequestedEventArgs args) {
            AddData();
        }

        private void AddData() {
            for (int i = 0; i < 5; i++) {
                var item = new ListViewItem();
                item.Content = i.ToString();
                ListViewItemCollection.Add(item);
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e) {
            RefreshContainer.RequestRefresh();
        }

        private void RefreshAppBarButton_OnClick(object sender, RoutedEventArgs e) {
            RefreshContainer.RequestRefresh();
        }
    }
}