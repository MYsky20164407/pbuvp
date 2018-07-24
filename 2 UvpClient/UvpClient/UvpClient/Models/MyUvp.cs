using System.Collections.Generic;
using System.Linq;

namespace UvpClient.Models {
    /// <summary>
    ///     我的uvp。
    /// </summary>
    public class MyUvp {
        /// <summary>
        ///     我。
        /// </summary>
        public Student Me { get; set; }

        /// <summary>
        ///     通知。
        /// </summary>
        public List<Announcement> Announcements { get; set; }

        /// <summary>
        ///     个人作业。
        /// </summary>
        public List<StudentAssignment> StudentAssignments { get; set; }

        /// <summary>
        ///     小组作业。
        /// </summary>
        public List<GroupAssignment> GroupAssignments { get; set; }

        /// <summary>
        ///     投票。
        /// </summary>
        public List<Vote> Votes { get; set; }

        /// <summary>
        ///     组内自评互评表。
        /// </summary>
        public List<PeerWorkGroupEvaluation> PeerWorkGroupEvaluations {
            get;
            set;
        }

        /// <summary>
        ///     未完成的个人作业。
        /// </summary>
        public List<StudentAssignment> IncompleteStudentAssignments =>
            StudentAssignments.Where(m => string.IsNullOrEmpty(m.Solution))
                .ToList();

        /// <summary>
        ///     未完成的小组作业。
        /// </summary>
        public List<GroupAssignment> IncompleteGroupAssignments =>
            GroupAssignments.Where(m => string.IsNullOrEmpty(m.Solution))
                .ToList();

        /// <summary>
        ///     未完成的投票。
        /// </summary>
        public List<Vote> IncompleteVotes =>
            Votes.Where(m => m.AnswerCollection.Count == 0).ToList();

        /// <summary>
        ///     未完成的组内自评互评表。
        /// </summary>
        public List<PeerWorkGroupEvaluation>
            IncompletePeerWorkGroupEvaluations =>
            PeerWorkGroupEvaluations.Where(m => !m.Finished).ToList();
    }
}