namespace UvpClient.Models {
    /// <summary>
    ///     组内自评互评表。
    /// </summary>
    public class PeerWorkGroupEvaluation {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     评价人。
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        ///     评价人。
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        ///     被评价人。
        /// </summary>
        public int TargetID { get; set; }

        /// <summary>
        ///     被评价人。
        /// </summary>
        public Student Target { get; set; }

        public bool Finished { get; set; }

        public bool? Q1 { get; set; }

        public int? Q2 { get; set; }

        public int? Q3 { get; set; }

        public int? Q4 { get; set; }

        public int? Q5 { get; set; }

        public int? Q6 { get; set; }

        public int? Q7 { get; set; }

        public bool? Q8 { get; set; }

        public string Q9 { get; set; }
    }
}