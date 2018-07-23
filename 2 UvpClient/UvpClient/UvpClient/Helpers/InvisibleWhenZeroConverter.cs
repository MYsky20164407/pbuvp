using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UvpClient.Helpers {
    internal class InvisibleWhenZeroConverter : IValueConverter {
        public object Convert(object v, Type t, object p, string l) {
            return (int) v == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object v, Type t, object p, string l) {
            return null;
        }
    }
}