using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace UvpClient.Models {
    /// <summary>
    ///     问卷。
    /// </summary>
    public class Questionnaire {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     问题。
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        ///     选项。
        /// </summary>
        public string Options {
            get =>
                JsonConvert.SerializeObject(OptionCollection
                    .Select(m => m.Option).ToArray());
            set {
                var optionStringList =
                    JsonConvert.DeserializeObject<List<string>>(value);
                for (var i = 0; i < optionStringList.Count; i++)
                    OptionCollection.Add(new QuestionnaireOption
                        {Id = i, Option = optionStringList[i]});
            }
        }

        /// <summary>
        ///     选项。
        /// </summary>
        public List<QuestionnaireOption> OptionCollection { get; set; } =
            new List<QuestionnaireOption>();

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        ///     截止日期。
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        ///     问卷类型。
        /// </summary>
        public QuestionnaireType Type { get; set; }

        /// <summary>
        ///     投票。
        /// </summary>
        public IList<Vote> Votes { get; set; }
    }

    /// <summary>
    ///     问卷类型。
    /// </summary>
    public enum QuestionnaireType {
        ExclusiveChoice,

        MultipleChoice
    }

    /// <summary>
    ///     问卷选项。
    /// </summary>
    public class QuestionnaireOption {
        /// <summary>
        ///     选项id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     选项内容。
        /// </summary>
        public string Option { get; set; }
    }
}