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
        public bool IsVersioningDisabled { get; set; }
        public string DefaultVersion { get; set; }
        public List<GetVersionModel> Versions { get; set; }
        public string AuthType { get; set; }
        public bool EnableMTLS { get; set; }
#nullable enable
        public List<Guid>? CertIds { get; set; }
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

        public List<GetUrlRewrite>? UrlRewrites { get; set; }
        public List<GetValidateJson>? ValidateJsons { get; set; }

        public List<GetTransformHeader>? TransformHeaders { get; set; }
        public List<GetTransformResponseHeader>? TransformResponseHeaders { get; set; }
        public List<GetTransform>? Transform { get; set; }
        public List<GetTransformResponse>? TransformResponse { get; set; }
#nullable disable
    }
    public class GetValidateJson
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public int ErrorResponseCode { get; set; }
    }

    public class GetMethodTransform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string ToMethod { get; set; }
    }

    public class GetUrlRewrite
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string MatchPattern { get; set; }
        public string RewriteTo { get; set; }
        public List<GetTrigger> Triggers { get; set; }
    }

    public class GetTrigger
    {
        public string On { get; set; }
        public GetOption Options { get; set; }
        public string RewriteTo { get; set; }
    }

    public class GetOption
    {
        public GetHeaderMatch HeaderMatches { get; set; }
        public GetPathPartMatch PathPartMatches { get; set; }
        public GetPayloadMatch PayloadMatches { get; set; }
        public GetQueryValMatch QueryValMatches { get; set; }
        public GetRequestContexMatch RequestContexMatches { get; set; }
        public GetSessionMetaMatch SessionMetaMatches { get; set; }
    }

    public class GetHeaderMatch
    {
        public GetCulprits Culprit { get; set; }
    }

    public class GetPathPartMatch
    {
        public GetCulprits Culprit { get; set; }
    }

    public class GetPayloadMatch
    {
        public string MatchRx { get; set; }
    }

    public class GetQueryValMatch
    {
        public GetCulprits Culprit { get; set; }
    }

    public class GetRequestContexMatch
    {

    }

    public class GetSessionMetaMatch
    {

    }

    public class GetCulprits
    {
        public string Key { get; set; }
        public string MatchRx { get; set; }
        public bool Reverse { get; set; }
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
    public class GetTransform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public GetTemplateData TemplateData { get; set; }
    }
    public class GetTransformResponse
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public GetTemplateData TemplateData { get; set; }
    }
    public class GetTemplateData
    {
        public bool EnableSession { get; set; }
        public string InputType { get; set; }
        public string TemplateMode { get; set; }
        public string TemplateSource { get; set; }

    }
}
