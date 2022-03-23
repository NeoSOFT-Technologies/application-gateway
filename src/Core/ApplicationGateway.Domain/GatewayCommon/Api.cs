namespace ApplicationGateway.Domain.GatewayCommon
{
    public class Api
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public bool StripListenPath { get; set; }
        public string TargetUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsInternal { get; set; }
#nullable enable
        public string? Protocol { get; set; }
        public RateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public VersioningInfo? VersioningInfo { get; set; }
#nullable disable
        public string DefaultVersion { get; set; }
        public List<VersionModel> Versions { get; set; }
        public string AuthType { get; set; }
#nullable enable
        public OpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
#nullable disable
        public bool IsQuotaDisabled { get; set; }
    }

    public class RateLimit
    {
        public int Rate { get; set; }
        public int Per { get; set; }
        public bool IsDisabled { get; set; }
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
        public VersioningLocation Location { get; set; }
        public string Key { get; set; }
    }

    public class VersionModel
    {
        public string Name { get; set; }
#nullable enable
        public string? OverrideTarget { get; set; }
        public string? Expires { get; set; }
        public Dictionary<string, string>? GlobalRequestHeaders { get; set; }
        public List<string>? GlobalRequestHeadersRemove { get; set; }
        public Dictionary<string, string>? GlobalResponseHeaders { get; set; }
        public List<string>? GlobalResponseHeadersRemove { get; set; }
        public ExtendedPaths? ExtendedPaths { get; set; }
#nullable disable
    }

    public class ExtendedPaths
    {
#nullable enable
        public List<MethodTransform>? MethodTransforms { get; set; }
        public List<UrlRewrite>? UrlRewrites { get; set; }
        public List<ValidateJson>? ValidateJsons { get; set; } 
#nullable disable
    }

    public class ValidateJson
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public int  ErrorResponseCode { get; set; }
    }

    public class MethodTransform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string ToMethod { get; set; }
    }

    public enum VersioningLocation
    {
        none,
        header,
        url,
        url_param
    }

    public class UrlRewrite
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string MatchPattern { get; set; }
        public string RewriteTo { get; set; }
        public List<Trigger> Triggers { get; set; }
    }

    public class Trigger
    {
        public string On { get; set; }
        public Option Options { get; set; }
        public string RewriteTo { get; set; }
    }

    public class Option
    {
        public HeaderMatch HeaderMatches { get; set; }
        public QueryValMatch QueryValMatches { get; set; }
        public PathPartMatch PathPartMatches { get; set; }
        public RequestContexMatch RequestContexMatches { get; set; }
        public SessionMetaMatch SessionMetaMatches { get; set; }
        public PayloadMatch PayloadMatches { get; set; }
    }

    public class HeaderMatch
    {
        public Culprits Culprit { get; set; }
    }

    public class PathPartMatch
    {
        public Culprits Culprit { get; set; }
    }

    public class PayloadMatch
    {
        public string MatchRx { get; set; }
    }

    public class QueryValMatch
    {
        public Culprits Culprit { get; set; }
    }

    public class RequestContexMatch
    {

    }

    public class SessionMetaMatch
    {

    }

    public class Culprits
    {
        public string Key { get; set; }
        public string MatchRx { get; set; }
        public bool Reverse { get; set; }
    }

    
}
