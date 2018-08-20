using Microsoft.Services.Store.Engagement;

namespace UvpClient.Services {
    /// <summary>
    ///     日志服务。
    /// </summary>
    public class LogService : ILogService {
        /// <summary>
        ///     日志工具。
        /// </summary>
        private readonly StoreServicesCustomEventLogger logger =
            StoreServicesCustomEventLogger.GetDefault();

        /// <summary>
        ///     记录日志。
        /// </summary>
        /// <param name="log">日志。</param>
        public void Log(string log) {
            logger.Log(log);
        }
    }
}