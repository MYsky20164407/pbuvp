using System.Collections.Generic;

namespace UvpClient.Models {
    /// <summary>
    ///     小组。
    /// </summary>
    public class Group {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     授课班级。
        /// </summary>
        public int TeachingClazzID { get; set; }

        /// <summary>
        ///     授课班级。
        /// </summary>
        public TeachingClazz TeachingClazz { get; set; }

        /// <summary>
        ///     组号。
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     组名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     学生。
        /// </summary>
        public IList<Student> Students { get; set; }

        /// <summary>
        ///     小组作业。
        /// </summary>
        public IList<GroupAssignment> GroupAssignments { get; set; }
    }
}