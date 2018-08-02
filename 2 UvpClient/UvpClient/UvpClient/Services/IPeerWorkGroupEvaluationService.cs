using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     组内自评/互评表服务接口。
    /// </summary>
    public interface IPeerWorkGroupEvaluationService {
        /// <summary>
        ///     提交组内自评/互评表。
        /// </summary>
        /// <param name="peerWorkGroupEvaluation">组内自评互评表。</param>
        /// <returns>服务结果。</returns>
        Task<ServiceResult> SubmitAsync(
            PeerWorkGroupEvaluation peerWorkGroupEvaluation);
    }
}