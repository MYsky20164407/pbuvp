using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace UvpClient.Services {
    /// <summary>
    ///     对话框服务。
    /// </summary>
    public class DialogService : IDialogService {
        /// <summary>
        ///     显示。
        /// </summary>
        /// <param name="message">消息。</param>
        public async Task ShowAsync(string message) {
            await new MessageDialog(message).ShowAsync();
        }
    }
}