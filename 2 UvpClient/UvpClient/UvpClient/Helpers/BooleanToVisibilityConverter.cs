// https://stackoverflow.com/questions/20295118/how-to-implement-a-booltovisibilityconverter/20344739#20344739

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UvpClient.Helpers {
    public class BooleanToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter,
            string language) {
            return GetVisibility(value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string language) {
            throw new NotImplementedException();
        }

        private object GetVisibility(object value) {
            if (!(value is bool))
                return Visibility.Collapsed;
            var objValue = (bool) value;
            if (objValue)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}