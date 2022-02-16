using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand
{
    public class UpdatePolicyCommand : IRequest<Response<UpdatePolicyDto>>
    {
        public Guid PolicyId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool KeysInactive { get; set; }
        public int MaxQuota { get; set; }
        public int QuotaRate { get; set; }
        public int Rate { get; set; }
        public int Per { get; set; }
        public int ThrottleInterval { get; set; }
        public int ThrottleRetries { get; set; }
        public string State { get; set; }
        public int KeyExpiresIn { get; set; }
        public List<string> Tags { get; set; }
        public List<UpdatePolicyApi> APIs { get; set; }
        public UpdatePartition? Partitions { get; set; }
    }

    public class UpdatePolicyApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Versions { get; set; }
        public List<UpdateAllowedUrl> AllowedUrls { get; set; }
        public UpdatePerApiLimit? Limit { get; set; }
    }

    public class UpdateAllowedUrl
    {
        public string url { get; set; }
        public List<string> methods { get; set; }
    }

    public class UpdatePerApiLimit
    {
        public int rate { get; set; }
        public int per { get; set; }
        public int throttle_interval { get; set; }
        public int throttle_retry_limit { get; set; }
        public int max_query_depth { get; set; }
        public int quota_max { get; set; }
        public int quota_renews { get; set; }
        public int quota_remaining { get; set; }
        public int quota_renewal_rate { get; set; }
        public bool set_by_policy { get; set; }
    }

    public class UpdatePartition
    {
        public bool quota { get; set; }
        public bool rate_limit { get; set; }
        public bool complexity { get; set; }
        public bool acl { get; set; }
        public bool per_api { get; set; }
    }
}
