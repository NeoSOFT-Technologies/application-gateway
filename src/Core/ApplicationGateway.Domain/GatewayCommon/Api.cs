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
        public List<TransformHeader>? TransformHeaders { get; set; }
        public List<TransformResponseHeader>? TransformResponseHeaders { get; set; }
        public List<Transform>? Transform { get; set; }
        public List<TransformResponse>? TransformResponse{ get; set;}
#nullable disable
    }

    public class MethodTransform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string ToMethod { get; set; }
    }
    public class TransformHeader
    {
        public bool ActOn { get; set; }
#nullable enable
        public Dictionary<string, string>? AddHeaders { get; set; }
#nullable disable
        public List<string> DeleteHeaders { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
    }

    public class TransformResponseHeader
    {
        public bool ActOn { get; set; }
#nullable enable
        public Dictionary<string, string>? AddHeaders { get; set; }
#nullable disable
        public List<string> DeleteHeaders { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
    }
    public class Transform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public TemplateData TemplateData { get; set; }
    }
    public class TransformResponse
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public TemplateData TemplateData { get; set; }
    }
    public class TemplateData
    {
        public bool EnableSession { get; set; }   
        public string InputType { get; set; }
        public string TemplateMode { get; set; }
        public string TemplateSource { get; set; }

    }

    public enum VersioningLocation
    {
        none,
        header,
        url,
        url_param
    }
}
