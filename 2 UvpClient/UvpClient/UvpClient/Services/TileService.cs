using System;
using Windows.UI.Notifications;

namespace UvpClient.Services {
    /// <summary>
    ///     磁贴服务。
    /// </summary>
    public class TileService : ITileService {
        /// <summary>
        ///     学号。
        /// </summary>
        private string _studentId;

        /// <summary>
        ///     更新已设置。
        /// </summary>
        private bool _updateSet;

        /// <summary>
        ///     设置更新。
        /// </summary>
        /// <param name="studentId">学号。</param>
        public void SetUpdate(string studentId) {
            if (!_updateSet) {
                _studentId = studentId;
                TileUpdateManager.CreateTileUpdaterForApplication()
                    .StartPeriodicUpdate(
                        new Uri(App.InsecureServerEndpoint +
                                "/api/tile?studentId=" + studentId),
                        PeriodicUpdateRecurrence.HalfHour);
                _updateSet = true;
            }
        }

        /// <summary>
        ///     强制更新。
        /// </summary>
        public void ForceUpdate() {
            if (_updateSet)
                TileUpdateManager.CreateTileUpdaterForApplication()
                    .StartPeriodicUpdate(
                        new Uri(App.InsecureServerEndpoint +
                                "/api/tile?studentId=" + _studentId),
                        PeriodicUpdateRecurrence.HalfHour);
        }
    }
}