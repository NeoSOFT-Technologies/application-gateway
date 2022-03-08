namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesDto
    {
        public string PolicyName { get; set;}
        public string Status { get; set;}
        public string AuthType { get; set; }
        public List<string> AccessRights { get; set; }
    }
    
    //public class GetAllPoliciesDto
    //{
    //    public Guid PolicyId { get; set; }
    //    public string Name { get; set; }
    //    public bool Active { get; set; }
    //    public bool KeysInactive { get; set; }
    //    public int MaxQuota { get; set; }
    //    public int QuotaRate { get; set; }
    //    public int Rate { get; set; }
    //    public int Per { get; set; }
    //    public int ThrottleInterval { get; set; }
    //    public int ThrottleRetries { get; set; }
    //    public string State { get; set; }
    //    public int KeyExpiresIn { get; set; }
    //    public List<string> Tags { get; set; }
    //    public List<GetAllPolicyApi> APIs { get; set; }
    //    public GetAllPartition? Partitions { get; set; }
    //}

    //public class GetAllPolicyApi
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public List<string> Versions { get; set; }
    //    public List<GetAllAllowedUrl> AllowedUrls { get; set; }
    //    public GetAllPerApiLimit? Limit { get; set; }
    //}

    //public class GetAllAllowedUrl
    //{
    //    public string url { get; set; }
    //    public List<string> methods { get; set; }
    //}

    //public class GetAllPerApiLimit
    //{
    //    public int rate { get; set; }
    //    public int per { get; set; }
    //    public int throttle_interval { get; set; }
    //    public int throttle_retry_limit { get; set; }
    //    public int max_query_depth { get; set; }
    //    public int quota_max { get; set; }
    //    public int quota_renews { get; set; }
    //    public int quota_remaining { get; set; }
    //    public int quota_renewal_rate { get; set; }
    //    public bool set_by_policy { get; set; }
    //}

    //public class GetAllPartition
    //{
    //    public bool quota { get; set; }
    //    public bool rate_limit { get; set; }
    //    public bool complexity { get; set; }
    //    public bool acl { get; set; }
    //    public bool per_api { get; set; }
    //}
}
