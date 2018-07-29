using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     通知服务。
    /// </summary>
    public class AnnouncementService : IAnnouncementService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public AnnouncementService(IIdentityService identityService) {
            _identityService = identityService;
        }

        public async Task<ServiceResult<IEnumerable<Announcement>>> GetAsync() {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response =
                        await httpClient.GetAsync(
                            App.ServerEndpoint + "/api/Announcement");
                } catch (Exception e) {
                    return new ServiceResult<IEnumerable<Announcement>> {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult<IEnumerable<Announcement>> {
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
                        serviceResult.Result = JsonConvert
                            .DeserializeObject<List<Announcement>>(json);
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