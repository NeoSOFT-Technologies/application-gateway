using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand
{
    public class UpdateKeyCommand:IRequest<Response<UpdateKeyCommandDto>>
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
        public List<UpdateKeyAccessRightsModel> AccessRights { get; set; }
        public List<string> Policies { get; set; }

        }

        public class UpdateKeyAccessRightsModel
        {
            public Guid ApiId { get; set; }
            public string ApiName { get; set; }
            public List<string> Versions { get; set; }
            public List<UpdateKeyAllowedUrl> AllowedUrls { get; set; }
            public UpdateKeyLimit? Limit { get; set; }

    }

        public class UpdateKeyLimit
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
    public class UpdateKeyAllowedUrl
        {
            public string Url { get; set; }
            public List<string> Methods { get; set; }
        }
    
}
