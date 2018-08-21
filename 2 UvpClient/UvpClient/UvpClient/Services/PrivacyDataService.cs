using System;
using System.Net.Http;
using System.Threading.Tasks;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     隐私数据服务。
    /// </summary>
    public class PrivacyDataService : IPrivacyDataService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public PrivacyDataService(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     获得隐私数据。
        /// </summary>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult<PrivacyData>> GetAsync() {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response =
                        await httpClient.GetAsync(
                            App.ServerEndpoint + "/api/PrivacyData");
                } catch (Exception e) {
                    return new ServiceResult<PrivacyData> {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                // TODO

                throw new NotImplementedException();
            }
        }
    }
}