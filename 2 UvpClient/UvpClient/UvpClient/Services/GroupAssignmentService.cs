using System;
using System.Net;
using System.Net.Http;
using System.Text;
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
        ///     提交小组作业。
        /// </summary>
        /// <param name="id">小组作业id。</param>
        /// <param name="groupAssignment">小组作业。</param>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult> SubmitAsync(int id,
            GroupAssignment groupAssignment) {
            var groupAssignmentToSubmit = new GroupAssignment {
                GroupID = groupAssignment.GroupID,
                HomeworkID = groupAssignment.HomeworkID,
                Score = groupAssignment.Score,
                Solution = groupAssignment.Solution
            };

            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                var json = JsonConvert.SerializeObject(groupAssignmentToSubmit);
                HttpResponseMessage response;
                try {
                    response = await httpClient.PutAsync(
                        App.ServerEndpoint + "/api/GroupAssignment/" + id,
                        new StringContent(json, Encoding.UTF8,
                            "application/json"));
                } catch (Exception e) {
                    return new ServiceResult {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult {
                    Status =
                        ServiceResultStatusHelper.FromHttpStatusCode(
                            response.StatusCode)
                };

                switch (response.StatusCode) {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        break;
                    case HttpStatusCode.NoContent:
                        break;
                    case HttpStatusCode.BadRequest:
                        serviceResult.Message =
                            await response.Content.ReadAsStringAsync();
                        break;
                    default:
                        serviceResult.Message = response.ReasonPhrase;
                        break;
                }

                return serviceResult;
            }
        }

        /// <summary>
        ///     获得小组作业。
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