namespace UvpClient.Services {
    /// <summary>
    ///     磁贴服务。
    /// </summary>
    public interface ITileService {
        /// <summary>
        ///     设置更新。
        /// </summary>
        /// <param name="studentId">学号。</param>
        void SetUpdate(string studentId);

        /// <summary>
        ///     强制更新。
        /// </summary>
        void ForceUpdate();

        /// <summary>
        ///     重置更新。
        /// </summary>
        void Reset();
    }
}