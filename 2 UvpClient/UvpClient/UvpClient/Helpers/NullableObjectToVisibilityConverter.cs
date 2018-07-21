using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UvpClient.Helpers {
    public class NullableObjectToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter,
            string language) {
            return GetVisibility(value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string language) {
            throw new NotImplementedException();
        }

        private object GetVisibility(object value) {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}