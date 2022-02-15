using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand
{
    public class UpdateApiCommand : IRequest<Response<UpdateApiDto>>
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public string TargetUrl { get; set; }
        public UpdateRateLimit? RateLimit { get; set; }
        public List<string>? Blacklist { get; set; }
        public List<string>? Whitelist { get; set; }
        public UpdateVersioningInfo? VersioningInfo { get; set; }
        public string DefaultVersion { get; set; }
        public List<UpdateVersionModel> Versions { get; set; }
        public string AuthType { get; set; }
        public UpdateOpenIdOptions? OpenidOptions { get; set; }
        public List<string>? LoadBalancingTargets { get; set; }
    }

    public class UpdateRateLimit
    {
        public int Rate { get; set; }
        public int Per { get; set; }
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
        public string Location { get; set; }
        public string Key { get; set; }
    }

    public class UpdateVersionModel
    {
        public string Name { get; set; }
        public string? OverrideTarget { get; set; }
    }
}
