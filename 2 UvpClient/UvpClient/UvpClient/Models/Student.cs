using System.Collections.Generic;

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

        /// <summary>
        ///     授课班级。
        /// </summary>
        public int TeachingClazzID { get; set; }

        /// <summary>
        ///     授课班级。
        /// </summary>
        public TeachingClazz TeachingClazz { get; set; }

        /// <summary>
        ///     小组。
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        ///     小组。
        /// </summary>
        public Group Group { get; set; }

        /// <summary>
        ///     用户。
        /// </summary>
        public string ApplicationUserID { get; set; }

        /// <summary>
        ///     个人作业。
        /// </summary>
        public IList<StudentAssignment> StudentAssignments { get; set; }

        /// <summary>
        ///     组内自评互评表。
        /// </summary>
        public IList<PeerWorkGroupEvaluation> PeerWorkGroupEvaluations {
            get;
            set;
        }

        /// <summary>
        ///     隐私数据。
        /// </summary>
        public PrivacyData PrivacyData { get; set; }
    }
}