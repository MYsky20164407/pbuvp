using System;

namespace UvpClient.Models {
    /// <summary>
    ///     通知。
    /// </summary>
    public class Announcement {
        /// <summary>
        ///     主键。
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     发布时间。
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     内容。
        /// </summary>
        public string Content { get; set; }
    }
}