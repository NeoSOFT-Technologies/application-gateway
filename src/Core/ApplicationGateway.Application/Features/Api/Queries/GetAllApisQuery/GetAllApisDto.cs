namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisDto
    {
        public List<GetAllApiModel> Apis { get; set; }
    }

    public class GetAllApiModel
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public string TargetUrl { get; set; }
        public GetAllRateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public GetAllVersioningInfo? VersioningInfo { get; set; }
        public string DefaultVersion { get; set; }
        public List<GetAllVersionModel> Versions { get; set; }
        public string AuthType { get; set; }
        public GetAllOpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
    }

    public class GetAllRateLimit
    {
        public int Rate { get; set; }
        public int Per { get; set; }
    }

    public class GetAllOpenIdOptions
    {
        public List<GetAllProvider> Providers { get; set; }
    }

    public class GetAllProvider
    {
        public string Issuer { get; set; }
        public List<GetAllClientPolicy> Client_ids { get; set; }
    }

    public class GetAllClientPolicy
    {
        public string ClientId { get; set; }
        public string Policy { get; set; }
    }

    public class GetAllVersioningInfo
    {
        public string Location { get; set; }
        public string Key { get; set; }
    }

    public class GetAllVersionModel
    {
        public string Name { get; set; }
        public string? OverrideTarget { get; set; }
    }
}
