using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    public class StudentAssignmentService : IStudentAssignmentService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public StudentAssignmentService(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     获得个人作业。
        /// </summary>
        /// <param name="id">个人作业Id。</param>
        /// <returns>个人作业</returns>
        public async Task<ServiceResult<StudentAssignment>> GetAsync(int id) {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response = await httpClient.GetAsync(
                        App.ServerEndpoint + "/api/StudentAssignment/" + id);
                } catch (Exception e) {
                    return new ServiceResult<StudentAssignment> {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult<StudentAssignment> {
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
                            .DeserializeObject<StudentAssignment>(json);
                        break;
                    default:
                        serviceResult.Message = response.ReasonPhrase;
                        break;
                }

                return serviceResult;
            }
        }

        /// <summary>
        ///     提交个人作业。
        /// </summary>
        /// <param name="groupAssignment">个人作业。</param>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult> SubmitAsync(
            StudentAssignment studentAssignment) {
            var studentAssignmentToSubmit = new StudentAssignment {
                StudentID = studentAssignment.StudentID,
                HomeworkID = studentAssignment.HomeworkID,
                Score = studentAssignment.Score,
                Solution = studentAssignment.Solution
            };

            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                var json =
                    JsonConvert.SerializeObject(studentAssignmentToSubmit);
                HttpResponseMessage response;
                try {
                    response = await httpClient.PutAsync(
                        App.ServerEndpoint + "/api/StudentAssignment/" +
                        studentAssignment.HomeworkID,
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
    }
}