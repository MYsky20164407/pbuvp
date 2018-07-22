

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvpClient.Models
{
    /// <summary>
    /// 我的uvp。
    /// </summary>
    public class MyUvp
    {
        /// <summary>
        /// 通知。
        /// </summary>
        public List<Announcement> Announcements { get; set; }
        public List<StudentAssignment> StudentAssignments { get; set; }
        public List<GroupAssignment> GroupAssignments { get; set; }
        public List<Vote> Votes { get; set; }

        public List<PeerWorkGroupEvaluation> PeerWorkGroupEvaluations
        {
            get;
            set;
        }
    }
}
