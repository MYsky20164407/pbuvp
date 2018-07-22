using System;
using System.Collections.Generic;

namespace UvpClient.Models {
    /// <summary>
    ///     作业。
    /// </summary>
    public class Homework {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     题目。
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        ///     截止日期。
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        ///     个人作业。
        /// </summary>
        public IList<StudentAssignment> StudentAssignments { get; set; }

        /// <summary>
        ///     小组作业。
        /// </summary>
        public IList<GroupAssignment> GroupAssignments { get; set; }

        /// <summary>
        ///     作业类型。
        /// </summary>
        public HomeworkType Type { get; set; }
    }

    /// <summary>
    ///     作业类型。
    /// </summary>
    public enum HomeworkType {
        Student,

        Group
    }
}