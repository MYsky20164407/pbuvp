﻿using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using UvpClient.Services;
using UvpClient.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UvpClient.Pages {
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage {
        public LoginPage() {
            InitializeComponent();
            DataContext = ViewModelLocator.Instance.LoginViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Frame.BackStack.Clear();

            SimpleIoc.Default.GetInstance<ILogService>().Log(nameof(LoginPage));
        }
    }
}