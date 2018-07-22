using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace UvpClient.Models {
    /// <summary>
    ///     投票。
    /// </summary>
    public class Vote {
        /// <summary>
        ///     问卷。
        /// </summary>
        public int QuestionnaireID { get; set; }

        /// <summary>
        ///     问卷。
        /// </summary>
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
        public string Answers { get; set; }

        /// <summary>
        ///     答案。
        /// </summary>
        public ReadOnlyCollection<int> AnswerCollection {
            get =>
                JsonConvert.DeserializeObject<ReadOnlyCollection<int>>(
                    Answers ?? "[]");
            set => Answers = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///     答案。
        /// </summary>
        public string AnswerCollectionString =>
            string.Join(",", AnswerCollection);
    }
}