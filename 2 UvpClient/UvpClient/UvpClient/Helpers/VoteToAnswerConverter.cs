using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using UvpClient.Models;

namespace UvpClient.Helpers {
    public class VoteToAnswerConverter : IValueConverter {
        public object Convert(object v, Type t, object p, string l) {
            var answerList = ((Vote) v).AnswerCollection.Select(m => m.Option)
                .ToList();
            return answerList.Count == 0 ? "You have not give an answer." :
                string.Join(", ", answerList);
        }

        public object ConvertBack(object v, Type t, object p, string l) {
            return null;
        }
    }
}