using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    public class MyUvpService : IMyUvpService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public MyUvpService(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     获得我的uvp。
        /// </summary>
        /// <returns>我的uvp。</returns>
        public async Task<ServiceResult<MyUvp>> GetAsync() {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response =
                        await httpClient.GetAsync(
                            App.ServerEndpoint + "/api/MyUvp");
                } catch (Exception e) {
                    return new ServiceResult<MyUvp> {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult<MyUvp> {
                    Status =
                        ServiceResultStatusHelper.FromHttpStatusCode(
                            response.StatusCode)
                };

                switch (response.StatusCode) {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        break;
                    case HttpStatusCode.OK:
                        var json = await response.Content.ReadAsStringAsync();
                        serviceResult.Result =
                            JsonConvert.DeserializeObject<MyUvp>(json);
                        break;
                    default:
                        serviceResult.Message = response.ReasonPhrase;
                        break;
                }

                return serviceResult;
            }
        }
    }
}