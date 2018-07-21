using System.Threading.Tasks;

namespace UvpClient.Services {
    /// <summary>
    ///     对话框服务接口。
    /// </summary>
    public interface IDialogService {
        /// <summary>
        ///     显示。
        /// </summary>
        /// <param name="message">消息。</param>
        Task ShowAsync(string message);
    }
}