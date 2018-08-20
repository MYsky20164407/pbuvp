namespace UvpClient.Services {
    /// <summary>
    ///     日志服务。
    /// </summary>
    public interface ILogService {
        /// <summary>
        ///     记录日志。
        /// </summary>
        /// <param name="log">日志。</param>
        void Log(string log);
    }
}