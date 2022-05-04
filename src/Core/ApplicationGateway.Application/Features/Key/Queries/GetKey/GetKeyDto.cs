using ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery;
using ApplicationGateway.Domain.GatewayCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    [ExcludeFromCodeCoverage]
    public class GetKeyDto
    {
        public string KeyId { get; set; }
        public int Rate { get; set; }
        public int Per { get; set; }
        public int Quota { get; set; }
        public int QuotaRenewalRate { get; set; }
        public int ThrottleInterval { get; set; }
        public int ThrottleRetries { get; set; }
        public int Expires { get; set; }
        public bool IsInActive { get; set; }
        public List<AccessRightsModel> AccessRights { get; set; }
        public List<string> Policies { get; set; }
        public List<PolicyById> PolicyByIds { get; set; } 
        public class PolicyById
        {
            public Guid PolicyId { get; set; }
            public string Name { get; set; }
            public GlobalPolicy Global { get; set; }
            public List<PolicyApi> APIs{get;set;}
        }

        public class GlobalPolicy
        {
            public string Name { get; set; }
            public int MaxQuota { get; set; }
            public int QuotaRate { get; set; }
            public int Per { get; set; }
            public int Rate { get; set; }
            public int ThrottleInterval { get; set; }
            public int ThrottleRetries { get; set; }
        }

        public class AccessRightsModel
        {
            public Guid ApiId { get; set; }
            public string ApiName { get; set; }
            public List<string> Versions { get; set; }
            public List<string> MasterVersions { get; set; }
            public string AuthType { get; set; }
            public List<AllowedUrl> AllowedUrls { get; set; }
            public ApiLimit Limit { get; set; }
        }

        public class ApiLimit
        {
            public int Rate { get; set; }
            public int Per { get; set; }
            public int Throttle_interval { get; set; }
            public int Throttle_retry_limit { get; set; }
            public int Max_query_depth { get; set; }
            public int Quota_max { get; set; }
            public int Quota_renews { get; set; }
            public int Quota_remaining { get; set; }
            public int Quota_renewal_rate { get; set; }
        }
           public class AllowedUrl
        {
            public string Url { get; set; }
            public List<string> Methods { get; set; }
        }
    }
}
