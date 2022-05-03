using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.GatewayCommon;
using ApplicationGateway.Domain.Entities;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Commands.UpdateTransformerCommand;
using ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand;
using ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerByName;
using ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery;
using ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery;
using ApplicationGateway.Application.Features.Key.Queries.GetAllKeys;
using ApplicationGateway.Application.Features.Certificate.Queries.GetCertificateById;
using ApplicationGateway.Application.Features.Certificate.Queries.GetAllCertificate;

namespace ApplicationGateway.Application.Profiles
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.GatewayCommon.Api, CreateApiCommand>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Api, CreateApiDto>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Api, MultipleApiModel>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Api, MultipleApiModelDto>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Api, UpdateApiCommand>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Api, UpdateApiDto>().ReverseMap();
            CreateMap<RateLimit, UpdateRateLimit>().ReverseMap();
            CreateMap<VersioningInfo, UpdateVersioningInfo>().ReverseMap();
            CreateMap<VersionModel, UpdateVersionModel>().ReverseMap();
            CreateMap<ExtendedPaths, UpdateExtendedPaths>().ReverseMap();
            CreateMap<MethodTransform, UpdateMethodTransform>().ReverseMap();
            CreateMap<ValidateJson, UpdateValidateJson>().ReverseMap();
            CreateMap<UrlRewrite, UpdateUrlRewrite>().ReverseMap();
            CreateMap<Trigger, UpdateTrigger>().ReverseMap();
            CreateMap<Option, UpdateOption>().ReverseMap();
            CreateMap<QueryValMatch, UpdateQueryValMatch>().ReverseMap();
            CreateMap<HeaderMatch, UpdateHeaderMatch>().ReverseMap();
            CreateMap<PathPartMatch, UpdatePathPartMatch>().ReverseMap();
            CreateMap<RequestContexMatch, UpdateRequestContexMatch>().ReverseMap();
            CreateMap<SessionMetaMatch, UpdateSessionMetaMatch>().ReverseMap();
            CreateMap<PayloadMatch, UpdatePayloadMatch>().ReverseMap();
            CreateMap<Culprits, UpdateCulprits>().ReverseMap();
            CreateMap<OpenIdOptions, UpdateOpenIdOptions>().ReverseMap();
            CreateMap<Provider, UpdateProvider>().ReverseMap();
            CreateMap<ClientPolicy, UpdateClientPolicy>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Api, GetAllApiModel>().ReverseMap();
          
            CreateMap<Domain.GatewayCommon.Api, GetApiByIdDto>().ReverseMap();
            CreateMap<RateLimit, GetRateLimit>().ReverseMap();
            CreateMap<VersioningInfo, GetVersioningInfo>().ReverseMap();
            CreateMap<VersionModel, GetVersionModel>().ReverseMap();
            CreateMap<OpenIdOptions, GetOpenIdOptions>().ReverseMap();
            CreateMap<Provider, GetProvider>().ReverseMap();
            CreateMap<ClientPolicy, GetClientPolicy>().ReverseMap();
            CreateMap<ExtendedPaths,GetExtendedPaths>().ReverseMap();
            CreateMap<MethodTransform, GetMethodTransform>().ReverseMap();
            CreateMap<ValidateJson, GetValidateJson>().ReverseMap();
            CreateMap<UrlRewrite, GetUrlRewrite>().ReverseMap();
            CreateMap<Trigger, GetTrigger>().ReverseMap();
            CreateMap<Option, GetOption>().ReverseMap();
            CreateMap<QueryValMatch, GetQueryValMatch>().ReverseMap();
            CreateMap<HeaderMatch, GetHeaderMatch>().ReverseMap();
            CreateMap<PathPartMatch, GetPathPartMatch>().ReverseMap();
            CreateMap<RequestContexMatch, GetRequestContexMatch>().ReverseMap();
            CreateMap<SessionMetaMatch, GetSessionMetaMatch>().ReverseMap();
            CreateMap<PayloadMatch, GetPayloadMatch>().ReverseMap();
            CreateMap<Culprits, GetCulprits>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Policy, CreatePolicyCommand>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Policy, CreatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, CreatePolicyApi>().ReverseMap();
            CreateMap<Partition, CreatePartition>().ReverseMap();
            CreateMap<AllowedUrl, CreateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, CreatePerApiLimit>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Policy, UpdatePolicyCommand>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Policy, UpdatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, UpdatePolicyApi>().ReverseMap();
            CreateMap<Partition, UpdatePartition>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, UpdatePerApiLimit>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Policy, GetAllPoliciesDto>().ReverseMap();
          
            CreateMap<Domain.GatewayCommon.Policy, GetPolicyByIdDto>().ReverseMap();
            CreateMap<PolicyApi, GetPolicyApi>().ReverseMap();
            CreateMap<Partition, GetPartition>().ReverseMap();
            CreateMap<AllowedUrl, GetAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, GetPerApiLimit>().ReverseMap();

            CreateMap<Transformer, CreateTransformerCommand>().ReverseMap();
            CreateMap<Transformer, CreateTransformerDto>().ReverseMap();

            CreateMap<Transformer, UpdateTransformerCommand>().ReverseMap();
            CreateMap<Transformer, UpdateTransformerDto>().ReverseMap();

            CreateMap<Transformer, GetAllTransformersDto>().ReverseMap();
            CreateMap<Transformer, GetTransformerByIdDto>().ReverseMap();

            CreateMap<Transformer, GetTransformerByNameDto>().ReverseMap();
            CreateMap<Transformer, GetTransformerByNameQuery>().ReverseMap();

            CreateMap<Domain.GatewayCommon.Key, CreateKeyCommand>().ReverseMap();
            CreateMap<AllowedUrl, KeyAllowedUrl>().ReverseMap();
            CreateMap<AccessRightsModel, KeyAccessRightsModel>().ReverseMap();
            CreateMap<ApiLimit, KeyApiLimit>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Key, UpdateKeyCommand>().ReverseMap();
            CreateMap<AllowedUrl, UpdateKeyAllowedUrl>().ReverseMap();
            CreateMap<AccessRightsModel, UpdateKeyAccessRightsModel>().ReverseMap();
            CreateMap<Domain.GatewayCommon.Key, UpdateKeyCommandDto>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrlDto>().ReverseMap();
            CreateMap<AccessRightsModel, UpdateAccessRightsModelDto>().ReverseMap();
            CreateMap<ApiLimit, UpdateKeyLimit>().ReverseMap();
            CreateMap<ApiLimit, UpdateKeyLimitDto>().ReverseMap();

            CreateMap<Certificate, GetCertificateByIdDto>().ReverseMap();
            CreateMap<Certificate, CertificateDto>().ReverseMap();


            CreateMap<Domain.Entities.Api, GetAllApiModel>().ReverseMap();
            CreateMap<Domain.Entities.Key, GetAllKeyModel>().ReverseMap();
            CreateMap<Domain.Entities.Policy, GetAllPolicyModel>().ReverseMap();

            CreateMap<TransformHeader, UpdateTransformHeader>().ReverseMap();
            CreateMap<TransformHeader, GetTransformHeader>().ReverseMap();
            CreateMap<TransformResponseHeader, UpdateTransformResponseHeader>().ReverseMap();
            CreateMap<TransformResponseHeader, GetTransformResponseHeader>().ReverseMap();

            CreateMap<Transform, UpdateTransform>().ReverseMap();
            CreateMap<Transform, GetTransform>().ReverseMap();
            CreateMap<TransformResponse, UpdateTransformResponse>().ReverseMap();
            CreateMap<TransformResponse, GetTransformResponse>().ReverseMap();
            CreateMap<TemplateData,UpdateTemplateData>().ReverseMap();
            CreateMap<TemplateData, GetTemplateData>().ReverseMap();



        }

    }
}
