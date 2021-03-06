﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UvpClient.Models;

namespace UvpClient.Services {
    /// <summary>
    ///     组内自评/互评表服务。
    /// </summary>
    public class
        PeerWorkGroupEvaluationService : IPeerWorkGroupEvaluationService {
        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        public PeerWorkGroupEvaluationService(
            IIdentityService identityService) {
            _identityService = identityService;
        }

        /// <summary>
        ///     提交组内自评/互评表。
        /// </summary>
        /// <param name="peerWorkGroupEvaluation">组内自评互评表。</param>
        /// <returns>服务结果。</returns>
        public async Task<ServiceResult> SubmitAsync(
            PeerWorkGroupEvaluation peerWorkGroupEvaluation) {
            var workGroupEvaluationToSubmit = new PeerWorkGroupEvaluation {
                ID = peerWorkGroupEvaluation.ID,
                StudentID = peerWorkGroupEvaluation.StudentID,
                TargetID = peerWorkGroupEvaluation.TargetID,
                Finished = peerWorkGroupEvaluation.Finished,
                Q1 = peerWorkGroupEvaluation.Q1,
                Q2 = peerWorkGroupEvaluation.Q2,
                Q3 = peerWorkGroupEvaluation.Q3,
                Q4 = peerWorkGroupEvaluation.Q4,
                Q5 = peerWorkGroupEvaluation.Q5,
                Q6 = peerWorkGroupEvaluation.Q6,
                Q7 = peerWorkGroupEvaluation.Q7,
                Q8 = peerWorkGroupEvaluation.Q8, Q9 = peerWorkGroupEvaluation.Q9
            };

            var identifiedHttpMessageHandler =
                _identityService.GetIdentifiedHttpMessageHandler();
            using (var httpClient =
                new HttpClient(identifiedHttpMessageHandler)) {
                var json =
                    JsonConvert.SerializeObject(workGroupEvaluationToSubmit);
                HttpResponseMessage response;
                try {
                    response = await httpClient.PutAsync(
                        App.ServerEndpoint + "/api/PeerWorkGroupEvaluation/" +
                        peerWorkGroupEvaluation.TargetID,
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