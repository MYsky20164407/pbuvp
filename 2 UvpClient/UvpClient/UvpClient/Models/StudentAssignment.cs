namespace UvpClient.Models {
    /// <summary>
    ///     个人作业。
    /// </summary>
    public class StudentAssignment {
        /// <summary>
        ///     作业。
        /// </summary>
        public int HomeworkID { get; set; }

        /// <summary>
        ///     作业。
        /// </summary>
        public Homework Homework { get; set; }

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
        public string Solution { get; set; }

        /// <summary>
        ///     成绩。
        /// </summary>
        public int? Score { get; set; }
    }
}