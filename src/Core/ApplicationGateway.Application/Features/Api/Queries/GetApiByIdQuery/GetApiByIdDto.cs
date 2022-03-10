namespace ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery
{
    public class GetApiByIdDto
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public bool StripListenPath { get; set; }
        public string TargetUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsInternal { get; set; }
        public string? Protocol { get; set; }
        public GetRateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public GetVersioningInfo? VersioningInfo { get; set; }
        public string DefaultVersion { get; set; }
        public List<GetVersionModel> Versions { get; set; }
        public string AuthType { get; set; }
        public GetOpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
        public bool IsQuotaDisabled { get; set; }
    }

    public class GetRateLimit
    {
        public int Rate { get; set; }
        public int Per { get; set; }
        public bool IsDisabled { get; set; }
    }

    public class GetOpenIdOptions
    {
        public List<GetProvider> Providers { get; set; }
    }

    public class GetProvider
    {
        public string Issuer { get; set; }
        public List<GetClientPolicy> Client_ids { get; set; }
    }

    public class GetClientPolicy
    {
        public string ClientId { get; set; }
        public string Policy { get; set; }
    }

    public class GetVersioningInfo
    {
        public string Location { get; set; }
        public string Key { get; set; }
    }

    public class GetVersionModel
    {
        public string Name { get; set; }
        public string? OverrideTarget { get; set; }
        public string? Expires { get; set; }
    }
}
