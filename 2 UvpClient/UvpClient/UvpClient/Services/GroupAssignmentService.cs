using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    public class GroupAssignmentService : IGroupAssignmentService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public GroupAssignmentService(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     获得个人作业。
        /// </summary>
        /// <param name="id">小组作业id。</param>
        /// <returns>小组作业。</returns>
        public async Task<ServiceResult<GroupAssignment>> GetAsync(int id) {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response = await httpClient.GetAsync(
                        App.ServerEndpoint + "/api/GroupAssignment/" + id);
                } catch (Exception e) {
                    return new ServiceResult<GroupAssignment> {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult<GroupAssignment> {
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
                            JsonConvert
                                .DeserializeObject<GroupAssignment>(json);
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