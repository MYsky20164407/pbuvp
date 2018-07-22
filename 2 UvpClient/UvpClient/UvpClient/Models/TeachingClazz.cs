using System.Collections.Generic;

namespace UvpClient.Models {
    /// <summary>
    ///     授课班级。
    /// </summary>
    public class TeachingClazz {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     授课班级名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     小组。
        /// </summary>
        public IList<Group> Groups { get; set; }

        /// <summary>
        ///     学生。
        /// </summary>
        public IList<Student> Students { get; set; }
    }
}