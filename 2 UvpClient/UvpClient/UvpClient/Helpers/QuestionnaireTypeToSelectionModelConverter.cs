using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using UvpClient.Models;

namespace UvpClient.Helpers {
    public class QuestionnaireTypeToSelectionModelConverter : IValueConverter {
        public object Convert(object v, Type t, object p, string l) {
            return (QuestionnaireType) v == QuestionnaireType.ExclusiveChoice ?
                ListViewSelectionMode.Single : ListViewSelectionMode.Multiple;
        }

        public object ConvertBack(object v, Type t, object p, string l) {
            return null;
        }
    }
}