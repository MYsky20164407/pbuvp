using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UvpClient.Helpers {
    internal class InvisibleWhenZeroConverter : IValueConverter {
        public object Convert(object v, Type t, object p, string l) {
            return ((IList) v).Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object v, Type t, object p, string l) {
            return null;
        }
    }
}