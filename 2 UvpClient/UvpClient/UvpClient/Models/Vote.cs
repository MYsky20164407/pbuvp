using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace UvpClient.Models {
    /// <summary>
    ///     投票。
    /// </summary>
    public class Vote {
        /// <summary>
        ///     答案。
        /// </summary>
        private ObservableCollection<QuestionnaireOption> _answerCollection =
            new ObservableCollection<QuestionnaireOption>();

        /// <summary>
        ///     问卷。
        /// </summary>
        public int QuestionnaireID { get; set; }

        /// <summary>
        ///     问卷。
        /// </summary>
        [JsonProperty(Order = 1)]
        public Questionnaire Questionnaire { get; set; }

        /// <summary>
        ///     学生。
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        ///     学生。
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        ///     答案。
        /// </summary>
        [JsonProperty(Order = 2)]
        public string Answers {
            get =>
                JsonConvert.SerializeObject(AnswerCollection.Select(m => m.Id));
            set {
                AnswerCollection.Clear();
                if (value != null)
                    JsonConvert.DeserializeObject<List<int>>(value).ForEach(m =>
                        AnswerCollection.Add(
                            Questionnaire.OptionCollection[m]));
            }
        }

        /// <summary>
        ///     答案。
        /// </summary>
        public List<QuestionnaireOption> AnswerCollection {
            get;
            set;
        } = new List<QuestionnaireOption>();
    }
}