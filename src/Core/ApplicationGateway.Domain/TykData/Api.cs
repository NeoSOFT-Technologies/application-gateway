namespace ApplicationGateway.Domain.TykData
{
    public class Api
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public string TargetUrl { get; set; }
        public RateLimit? rateLimit { get; set; }
        public List<string>? blacklist { get; set; }
        public List<string>? whitelist { get; set; }
        public VersioningInfo? versioningInfo { get; set; }
        public string defaultVersion { get; set; }
        public List<VersionModel> versions { get; set; }
        public string authType { get; set; }
        public OpenIdOptions? openidOptions { get; set; }
        public List<string>? loadBalancingTargets { get; set; }
    }

    public class RateLimit
    {
        public int rate { get; set; }
        public int per { get; set; }
    }
    public class VersioningInfo
    {
        public string location { get; set; }
        public string key { get; set; }
    }
    public class OpenIdOptions
    {
        public List<Provider> providers { get; set; }
    }

    public class VersionModel
    {
        public string name { get; set; }
        public string? overrideTarget { get; set; }
    }
}
