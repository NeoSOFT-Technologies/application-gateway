using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Domain.GatewayCommon;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationGateway.Application.UnitTests.Mocks
{
    public class PolicyServiceMocks
    {
        public static Mock<IPolicyService> GetPolicyService()
        {
            var Policies = new List<Domain.GatewayCommon.Policy>()
            {
                new Domain.GatewayCommon.Policy()
                {
                    PolicyId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name = "policy1",
                    Active = true,
                    KeysInactive = true,
                    MaxQuota =5,
                    QuotaRate =5,
                    Rate =3,
                    Per=20,
                    ThrottleInterval=10,
                    ThrottleRetries=10,
                    State="state",
                    KeyExpiresIn=10,
                    Tags=new List<string> { "tag1", "tag2"},
                    APIs=new List<PolicyApi> 
                             { new PolicyApi() 
                                { Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                                 Name = "policyApiName", 
                                 Versions = new List<string> { "version1", "version2" },
                                 AllowedUrls = new List<AllowedUrl>{new AllowedUrl() { url = "url", methods = new List<string> { "method1", "method2" } } },
                                 Limit=new PerApiLimit() { rate=10,per=10,throttle_interval=10,throttle_retry_limit=10,max_query_depth=10,quota_max=10,quota_renews = 10,quota_remaining =10,quota_renewal_rate=10,set_by_policy=true} } },
                    Partitions=new Partition() { quota=true,rate_limit=true,complexity=true,acl=true,per_api=true },


                },
                 new Domain.GatewayCommon.Policy()
                {
                    PolicyId = Guid.Parse("{7cca2947-221d-4314-971e-911d542622b2}"),
                    Name = "policy2",
                    Active = true,
                    KeysInactive = true,
                    MaxQuota =5,
                    QuotaRate =5,
                    Rate =3,
                    Per=20,
                    ThrottleInterval=10,
                    ThrottleRetries=10,
                    State="state2",
                    KeyExpiresIn=10,
                    Tags=new List<string> { "tag3", "tag4"},
                    APIs=new List<PolicyApi>
                             { new PolicyApi()
                                { Id = Guid.Parse("{d29cd198-03a1-46bc-965e-56d5c6748429}"),
                                 Name = "policyApiName",
                                 Versions = new List<string> { "version3", "version4" },
                                 AllowedUrls = new List<AllowedUrl>{new AllowedUrl() { url = "url", methods = new List<string> { "method1", "method2" } } },
                                 Limit=new PerApiLimit() { rate=10,per=10,throttle_interval=10,throttle_retry_limit=10,max_query_depth=10,quota_max=10,quota_renews = 10,quota_remaining =10,quota_renewal_rate=10,set_by_policy=true} } },
                    Partitions=new Partition() { quota=true,rate_limit=true,complexity=true,acl=true,per_api=true },


                }
            };

            var mockPolicyService = new Mock<IPolicyService>();
            mockPolicyService.Setup(repo => repo.GetAllPoliciesAsync()).ReturnsAsync(Policies);
            mockPolicyService.Setup(repo => repo.GetPolicyByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                (Guid policyId) =>
                {
                    return Policies.SingleOrDefault(x => x.PolicyId == policyId);
                });
            mockPolicyService.Setup(repo => repo.CreatePolicyAsync(It.IsAny<Domain.GatewayCommon.Policy>())).ReturnsAsync(
                (Domain.GatewayCommon.Policy policy) =>
                {
                    policy.PolicyId = Guid.NewGuid();
                    Policies.Add(policy);
                    return policy;
                }
                );
            mockPolicyService.Setup(repo => repo.DeletePolicyAsync(It.IsAny<Guid>())).Callback(
                (Guid id) =>
                {
                    Policies.RemoveAll(x => x.PolicyId == id);
                }
                );
            mockPolicyService.Setup(repo => repo.UpdatePolicyAsync(It.IsAny<Domain.GatewayCommon.Policy>())).ReturnsAsync(
                (Domain.GatewayCommon.Policy policy) =>
                {
                    Policies[0] = policy;
                    return policy;
                }
                );
            return mockPolicyService;
        }
    }
}
