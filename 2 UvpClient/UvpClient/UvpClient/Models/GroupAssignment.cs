namespace UvpClient.Models {
    /// <summary>
    ///     小组作业。
    /// </summary>
    public class GroupAssignment {
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

        /// <summary>
        ///     答案。
        /// </summary>
        public string Solution { get; set; }
        /// <summary>
        ///     成绩。
        /// </summary>
        public int? Score { get; set; }
    }
}