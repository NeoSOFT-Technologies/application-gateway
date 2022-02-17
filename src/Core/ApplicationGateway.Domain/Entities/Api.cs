namespace ApplicationGateway.Domain.Entities
{
    public class Api
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public string TargetUrl { get; set; }
        public RateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public VersioningInfo? VersioningInfo { get; set; }
        public string DefaultVersion { get; set; }
        public List<VersionModel> Versions { get; set; }
        public string AuthType { get; set; }
        public OpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
    }

    public class RateLimit
    {
        public int Rate { get; set; }
        public int Per { get; set; }
    }

    public class OpenIdOptions
    {
        public List<Provider> Providers { get; set; }
    }

    public class Provider
    {
        public string Issuer { get; set; }
        public List<ClientPolicy> Client_ids { get; set; }
    }

    public class ClientPolicy
    {
        public string ClientId { get; set; }
        public string Policy { get; set; }
    }

    public class VersioningInfo
    {
        public string Location { get; set; }
        public string Key { get; set; }
    }

    public class VersionModel
    {
        public string Name { get; set; }
        public string? OverrideTarget { get; set; }
    }
}
