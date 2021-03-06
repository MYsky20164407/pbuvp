﻿using System;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using MvvmLight.Models;
using MvvmLight.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MvvmLight {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }

        // 绕过点击AppBarButton不会导致TextBox失去焦点问题的方法。
        /*private void LoseFocusAppBarButton_OnClick(object sender,
            RoutedEventArgs e) {
            var textbox = FocusManager.GetFocusedElement() as TextBox;
            if (textbox != null) {
                var binding =
                    textbox.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
        }*/

        private void AdaptiveStates_OnCurrentStateChanged(object sender,
            VisualStateChangedEventArgs e) {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState,
            VisualState oldState = null) {
            var viewModel = (MainPageViewModel) this.DataContext;

            var isNarrow = newState == NarrowState;

            if (isNarrow && oldState == DefaultState &&
                viewModel.SelectedContact != null) {
                Frame.Navigate(typeof(DetailPage), null,
                    new SuppressNavigationTransitionInfo());
            }
        }

        private void MasterListView_OnItemClick(object sender,
            ItemClickEventArgs e) {
            var viewModel = (MainPageViewModel) this.DataContext;
            viewModel.SelectedContact = (Contact) e.ClickedItem;

            if (AdaptiveStates.CurrentState == NarrowState) {
                Frame.Navigate(typeof(DetailPage), null,
                    new DrillInNavigationTransitionInfo());
            }
        }
    }
}