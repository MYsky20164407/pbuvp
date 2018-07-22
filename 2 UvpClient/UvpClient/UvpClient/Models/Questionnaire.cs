using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string Options { get; set; }

        /// <summary>
        ///     选项。
        /// </summary>
        public ReadOnlyCollection<string> OptionCollection {
            get =>
                JsonConvert.DeserializeObject<ReadOnlyCollection<string>>(
                    Options ?? "[]");
            set => Options = JsonConvert.SerializeObject(value);
        }

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
}