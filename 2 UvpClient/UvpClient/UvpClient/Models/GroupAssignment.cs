using GalaSoft.MvvmLight;

namespace UvpClient.Models {
    /// <summary>
    ///     小组作业。
    /// </summary>
    public class GroupAssignment : ObservableObject {
        /// <summary>
        ///     作业。
        /// </summary>
        public int HomeworkID { get; set; }

        /// <summary>
        ///     作业。
        /// </summary>
        public Homework Homework { get; set; }

        /// <summary>
        ///     小组。
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        ///     小组。
        /// </summary>
        public Group Group { get; set; }

        private string _solution;

        /// <summary>
        ///     答案。
        /// </summary>
        public string Solution {
            get => _solution;
            set => Set(nameof(Solution), ref _solution, value);
        }

        /// <summary>
        ///     成绩。
        /// </summary>
        public int? Score { get; set; }
    }
}