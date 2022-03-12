using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Domain.GatewayCommon;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Policies = new List<string>{"policy1","Policy2"}
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
                    Policies = new List<string>{"policy1","Policy2"}
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

            return mockKeyService;

            
        }
    }
}
