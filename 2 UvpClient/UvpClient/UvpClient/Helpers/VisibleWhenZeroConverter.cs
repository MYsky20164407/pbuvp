using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UvpClient.Helpers {
    public class VisibleWhenZeroConverter : IValueConverter {
        public object Convert(object v, Type t, object p, string l) {
            return (int) v == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object v, Type t, object p, string l) {
            return null;
        }
    }
}