using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand
{
    public class CreateKeyCommand:IRequest<Response<Domain.GatewayCommon.Key>>
    {
        public string KeyName { get; set; }
        public int Rate { get; set; }
        public int Per { get; set; }
        public int Quota { get; set; }
        public int QuotaRenewalRate { get; set; }
        public int ThrottleInterval { get; set; }
        public int ThrottleRetries { get; set; }
        public int Expires { get; set; }
        public List<KeyAccessRightsModel> AccessRights { get; set; }
        public List<string> Policies { get; set; }



    }
        public class KeyAccessRightsModel
        {
            public Guid ApiId { get; set; }
            public string ApiName { get; set; }
            public List<string> Versions { get; set; }
            public List<KeyAllowedUrl>? AllowedUrls { get; set; }
            public KeyApiLimit? Limit { get; set; }
        }
        public class KeyApiLimit
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
        public class KeyAllowedUrl
        {
            public string Url { get; set; }
            public List<string> Methods { get; set; }
        }
}
