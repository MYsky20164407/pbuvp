﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

                var serviceResult = new ServiceResult<PrivacyData> {
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
                            JsonConvert.DeserializeObject<PrivacyData>(json);
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