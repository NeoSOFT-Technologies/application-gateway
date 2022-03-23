using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.GatewayCommon;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand
{
    public class UpdateApiCommand : IRequest<Response<UpdateApiDto>>
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
        public UpdateRateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public UpdateVersioningInfo? VersioningInfo { get; set; }
#nullable disable
        public string DefaultVersion { get; set; }
        public List<UpdateVersionModel> Versions { get; set; }
        public string AuthType { get; set; }
#nullable enable
        public UpdateOpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
#nullable disable
        public bool IsQuotaDisabled { get; set; }
    }

    public class UpdateRateLimit
    {
        public int Rate { get; set; }
        public int Per { get; set; }
        public bool IsDisabled { get; set; }
    }

    public class UpdateOpenIdOptions
    {
        public List<UpdateProvider> Providers { get; set; }
    }

    public class UpdateProvider
    {
        public string Issuer { get; set; }
        public List<UpdateClientPolicy> Client_ids { get; set; }
    }

    public class UpdateClientPolicy
    {
        public string ClientId { get; set; }
        public string Policy { get; set; }
    }

    public class UpdateVersioningInfo
    {
        public VersioningLocation Location { get; set; }
        public string Key { get; set; }
    }

    public class UpdateVersionModel
    {
        public string Name { get; set; }
#nullable enable
        public string? OverrideTarget { get; set; }
        public string? Expires { get; set; }
        public Dictionary<string, string>? GlobalRequestHeaders { get; set; }
        public List<string>? GlobalRequestHeadersRemove { get; set; }
        public Dictionary<string, string>? GlobalResponseHeaders { get; set; }
        public List<string>? GlobalResponseHeadersRemove { get; set; }
        public UpdateExtendedPaths? ExtendedPaths { get; set; }
#nullable disable
    }

    public class UpdateExtendedPaths
    {
#nullable enable
        public List<UpdateMethodTransform>? MethodTransforms { get; set; }
        public List<UpdateUrlRewrite>? UrlRewrites { get; set; }
        public List<UpdateValidateJson>? ValidateJsons { get; set; }
#nullable disable
    }

    public class UpdateValidateJson
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public int ErrorResponseCode { get; set; }
    }

    public class UpdateMethodTransform
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string ToMethod { get; set; }
    }

    public class UpdateUrlRewrite
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string MatchPattern { get; set; }
        public string RewriteTo { get; set; }
        public List<UpdateTrigger> Triggers { get; set; }
    }

    public class UpdateTrigger
    {
        public string On { get; set; }
        public UpdateOption Options { get; set; }
        public string RewriteTo { get; set; }

    }

    public class UpdateOption
    {
        public UpdateHeaderMatch HeaderMatches { get; set; }
        public UpdateQueryValMatch QueryValMatches { get; set; }
        public UpdatePathPartMatch PathPartMatches { get; set; }
        public UpdateRequestContexMatch RequestContexMatches { get; set; }
        public UpdateSessionMetaMatch SessionMetaMatches { get; set; }
        public UpdatePayloadMatch PayloadMatches { get; set; }
    }

    public class UpdateHeaderMatch
    {
        public UpdateCulprits Culprit { get; set; }
    }

    public class UpdatePathPartMatch
    {
        public UpdateCulprits Culprit { get; set; }
    }

    public class UpdatePayloadMatch
    {
        public string MatchRx { get; set; }
    }

    public class UpdateQueryValMatch
    {
        public UpdateCulprits Culprit { get; set; }
    }

    public class UpdateRequestContexMatch
    {

    }

    public class UpdateSessionMetaMatch
    {

    }

    public class UpdateCulprits
    {
        public string Key { get; set; }
        public string MatchRx { get; set; }
        public bool Reverse { get; set; }
    }
}
