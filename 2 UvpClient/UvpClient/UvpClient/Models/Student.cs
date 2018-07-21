namespace UvpClient.Models {
    /// <summary>
    ///     学生。
    /// </summary>
    public class Student {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     学号。
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        ///     姓名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     班级。
        /// </summary>
        public int ClazzID { get; set; }

        /// <summary>
        ///     班级。
        /// </summary>
        public Clazz Clazz { get; set; }
    }
}