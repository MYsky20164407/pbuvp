using System.Text;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace UvpClient.Models {
    /// <summary>
    ///     组内自评互评表。
    /// </summary>
    public class PeerWorkGroupEvaluation : ObservableObject {
        private bool _q1;

        private int _q2 = 1;

        private int _q3 = 1;

        private int _q4 = 1;

        private int _q5;

        private int _q6;

        private int _q7;

        private bool _q8;

        private string _q9;

        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     评价人。
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        ///     评价人。
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        ///     被评价人。
        /// </summary>
        public int TargetID { get; set; }

        /// <summary>
        ///     被评价人。
        /// </summary>
        public Student Target { get; set; }

        public bool Finished { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Q1 {
            get => _q1;
            set => Set(nameof(Q1), ref _q1, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Q2 {
            get => _q2;
            set => Set(nameof(Q2), ref _q2, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Q3 {
            get => _q3;
            set => Set(nameof(Q3), ref _q3, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Q4 {
            get => _q4;
            set => Set(nameof(Q4), ref _q4, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Q5 {
            get => _q5;
            set => Set(nameof(Q5), ref _q5, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Q6 {
            get => _q6;
            set => Set(nameof(Q6), ref _q6, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Q7 {
            get => _q7;
            set => Set(nameof(Q7), ref _q7, value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Q8 {
            get => _q8;
            set => Set(nameof(Q8), ref _q8, value);
        }

        public string Q9 {
            get => _q9;
            set => Set(nameof(Q9), ref _q9, value);
        }
    }
}