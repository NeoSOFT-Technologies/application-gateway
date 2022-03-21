using ApplicationGateway.Domain.GatewayCommon;

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
#nullable enable
        public string? Protocol { get; set; }
        public GetRateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public GetVersioningInfo? VersioningInfo { get; set; }
#nullable disable
        public string DefaultVersion { get; set; }
        public List<GetVersionModel> Versions { get; set; }
        public string AuthType { get; set; }
#nullable enable
        public GetOpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
#nullable disable
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
        public VersioningLocation Location { get; set; }
        public string Key { get; set; }
    }

    public class GetVersionModel
    {
        public string Name { get; set; }
#nullable enable
        public string? OverrideTarget { get; set; }
        public string? Expires { get; set; }
        public Dictionary<string, string>? GlobalRequestHeaders { get; set; }
        public List<string>? GlobalRequestHeadersRemove { get; set; }
        public Dictionary<string, string>? GlobalResponseHeaders { get; set; }
        public List<string>? GlobalResponseHeadersRemove { get; set; }
        public GetExtendedPaths? ExtendedPaths { get; set; }
#nullable disable
    }

    public class GetExtendedPaths
    {
#nullable enable
        public List<GetMethodTransform>? MethodTransforms { get; set; }
        public List<GetTransformHeader>? TransformHeaders { get; set; }
        public List<GetTransformResponseHeader>? TransformResponseHeaders { get; set; }
#nullable disable
    }

    public class GetMethodTransform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string ToMethod { get; set; }
    }

    public class GetTransformHeader
    {
        public bool ActOn { get; set; }
#nullable enable
        public Dictionary<string, string>? AddHeaders { get; set; }
#nullable disable
        public List<string> DeleteHeaders { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
    }
    public class GetTransformResponseHeader
    {
        public bool ActOn { get; set; }
#nullable enable
        public Dictionary<string, string>? AddHeaders { get; set; }
#nullable disable
        public List<string> DeleteHeaders { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
    }
}
