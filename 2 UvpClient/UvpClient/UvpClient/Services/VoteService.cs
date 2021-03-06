﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     投票服务。
    /// </summary>
    public class VoteService : IVoteService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public VoteService(IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     提交投票。
        /// </summary>
        /// <param name="vote">投票。</param>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult> SubmitAsync(Vote vote) {
            var voteToSubmit = new Vote {
                QuestionnaireID = vote.QuestionnaireID,
                StudentID = vote.StudentID,
                AnswerCollection = vote.AnswerCollection
            };

            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                var json = JsonConvert.SerializeObject(voteToSubmit);
                HttpResponseMessage response;
                try {
                    response = await httpClient.PutAsync(
                        App.ServerEndpoint + "/api/Vote/" +
                        vote.QuestionnaireID,
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