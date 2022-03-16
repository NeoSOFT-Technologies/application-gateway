namespace ApplicationGateway.Domain.GatewayCommon
{
    public class Key
    {
        #nullable enable
        public string? KeyId { get; set; }
        #nullable disable
        public string KeyName { get; set; }
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
        public List<string> Tags { get; set; }

    }
    
    public class AccessRightsModel
    {
        public Guid ApiId { get; set; }
        public string ApiName { get; set; }
        public List<string> Versions { get; set; }
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
}
