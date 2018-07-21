﻿// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using UvpClient.Services;

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyUvpPage {
        public MyUvpPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Frame.BackStack.Clear();
        }
    }
}