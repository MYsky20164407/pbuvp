using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     学生服务。
    /// </summary>
    public class StudentService : IStudentService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public StudentService(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     根据学号获得学生。
        /// </summary>
        /// <param name="studentId">学号。</param>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult<Student>> GetStudentByStudentIdAsync(
            string studentId) {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response = await httpClient.GetAsync(
                        App.ServerEndpoint + "/api/Student?studentId=" +
                        HttpUtility.UrlEncode(studentId));
                } catch (Exception e) {
                    return new ServiceResult<Student> {
                        Status = ServiceResultStatus.Exception,
                        Message = e.Message
                    };
                }

                var serviceResult = new ServiceResult<Student> {
                    Status =
                        ServiceResultStatusHelper.FromHttpStatusCode(
                            response.StatusCode)
                };

                switch (response.StatusCode) {
                    case HttpStatusCode.Unauthorized:
                        break;
                    case HttpStatusCode.OK:
                        var json = await response.Content.ReadAsStringAsync();
                        serviceResult.Result =
                            JsonConvert.DeserializeObject<Student>(json);
                        break;
                    case HttpStatusCode.NotFound:
                        break;
                    default:
                        serviceResult.Message = response.ReasonPhrase;
                        break;
                }

                return serviceResult;
            }
        }

        /// <summary>
        ///     绑定账号。
        /// </summary>
        /// <param name="studentId">学号。</param>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult> BindAccountAsync(string studentId) {
            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                HttpResponseMessage response;
                try {
                    response = await httpClient.PutAsync(
                        App.ServerEndpoint + "/api/Student?studentId=" +
                        HttpUtility.UrlEncode(studentId),
                        new StringContent(""));
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