namespace UvpClient.Models {
    /// <summary>
    ///     隐私数据。
    /// </summary>
    public class PrivacyData {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     学生。
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        ///     学生。
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        ///     隐私数据。
        /// </summary>
        public string Data { get; set; }
    }
}