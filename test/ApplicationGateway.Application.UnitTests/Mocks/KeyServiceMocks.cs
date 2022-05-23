using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Domain.GatewayCommon;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationGateway.Application.UnitTests.Mocks
{
    public class KeyServiceMocks
    {
        public static Mock<IKeyService> GetKeyService()
        {
            var keys = new List<Domain.GatewayCommon.Key>()
            {
                new Domain.GatewayCommon.Key()
                {
                    KeyId = "KeyId1",
                    Rate = 10,
                    Per = 10,
                    Quota = 10,
                    QuotaRenewalRate = 10,
                    ThrottleInterval = 10,
                    ThrottleRetries = 10,
                    Expires = 10,
                    IsInActive = false,
                    AccessRights = new List<AccessRightsModel>
                            {
                                new AccessRightsModel()
                                    { ApiId= Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                                      ApiName="apiName",
                                      Versions = new List<string> { "version1","version2"},
                                      AllowedUrls = new List<AllowedUrl>{new AllowedUrl() { url = "url", methods = new List<string> { "method1", "method2" } } },
                                      Limit = new ApiLimit(){Rate=10,Per=10,Throttle_interval=10,Throttle_retry_limit=10,Max_query_depth=10,Quota_max=10,Quota_renews = 10,Quota_remaining =10,Quota_renewal_rate=10}
                                     } 
                            },
                    Policies = new List<string>{ "EE272F8B-6096-4CB6-8625-BB4BB2D89E8B", "7cca2947-221d-4314-971e-911d542622b2" }
                },
                new Domain.GatewayCommon.Key()
                {
                    KeyId = "KeyId2",
                    Rate = 10,
                    Per = 10,
                    Quota = 10,
                    QuotaRenewalRate = 10,
                    ThrottleInterval = 10,
                    ThrottleRetries = 10,
                    Expires = 10,
                    IsInActive = false,
                    AccessRights = new List<AccessRightsModel>
                            {
                                new AccessRightsModel()
                                    { ApiId= Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                                      ApiName="apiName2",
                                      Versions = new List<string> { "version1","version2"},
                                      AllowedUrls = new List<AllowedUrl>{new AllowedUrl() { url = "url", methods = new List<string> { "method1", "method2" } } },
                                      Limit = new ApiLimit(){Rate=10,Per=10,Throttle_interval=10,Throttle_retry_limit=10,Max_query_depth=10,Quota_max=10,Quota_renews = 10,Quota_remaining =10,Quota_renewal_rate=10}
                                     }
                            },
                    Policies = new List<string>{ "EE272F8B-6096-4CB6-8625-BB4BB2D89E8B", "7cca2947-221d-4314-971e-911d542622b2" }
                }
            };

           var mockKeyService = new Mock<IKeyService>();

           mockKeyService.Setup(repo => repo.GetAllKeysAsync()).ReturnsAsync(new List<string>());
           mockKeyService.Setup(repo => repo.GetKeyAsync(It.IsAny<string>())).ReturnsAsync(
                (string keyId) =>
                {
                    return keys.SingleOrDefault(x => x.KeyId == keyId);
                });

            mockKeyService.Setup(repo => repo.CreateKeyAsync(It.IsAny<Domain.GatewayCommon.Key>())).ReturnsAsync(
                 (Domain.GatewayCommon.Key key) =>
                 {
                     key.KeyId = "keyId";
                     keys.Add(key);
                     return key;

                 }
                );
            mockKeyService.Setup(repo => repo.DeleteKeyAsync(It.IsAny<string>())).Callback(

                (string id) =>
                {
                    keys.RemoveAll(x=>x.KeyId==id);
                }
                );
            mockKeyService.Setup(repo => repo.UpdateKeyAsync(It.IsAny<Domain.GatewayCommon.Key>())).ReturnsAsync(
                (Domain.GatewayCommon.Key key) =>
                {
                    keys[0].KeyId = key.KeyId;
                    keys[0].Rate = key.Rate;
                    keys[0].Per = key.Per;
                    keys[0].Quota = key.Quota;
                    keys[0].QuotaRenewalRate = key.QuotaRenewalRate;
                    keys[0].ThrottleInterval = key.ThrottleInterval;
                    keys[0].ThrottleRetries = key.ThrottleRetries;
                    keys[0].Expires = key.Expires;
                    keys[0].IsInActive = key.IsInActive;
                    keys[0].AccessRights = key.AccessRights;
                    keys[0].Policies = key.Policies;
                    return key;
                    

                }
                );

            return mockKeyService;

            
        }
    }
}
