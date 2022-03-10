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

namespace ApplicationGateway.Application.Profiles
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Api, CreateApiCommand>().ReverseMap();
            CreateMap<Api, CreateApiDto>().ReverseMap();
            CreateMap<Api, MultipleApiModel>().ReverseMap();
            CreateMap<Api, MultipleApiModelDto>().ReverseMap();
            CreateMap<Api, UpdateApiCommand>().ReverseMap();
            CreateMap<Api, UpdateApiDto>().ReverseMap();
            CreateMap<RateLimit, UpdateRateLimit>().ReverseMap();
            CreateMap<VersioningInfo, UpdateVersioningInfo>().ReverseMap();
            CreateMap<VersionModel, UpdateVersionModel>().ReverseMap();
            CreateMap<OpenIdOptions, UpdateOpenIdOptions>().ReverseMap();
            CreateMap<Provider, UpdateProvider>().ReverseMap();
            CreateMap<ClientPolicy, UpdateClientPolicy>().ReverseMap();
            CreateMap<Api, GetAllApiModel>().ReverseMap();
            //CreateMap<RateLimit, GetAllRateLimit>().ReverseMap();
            //CreateMap<VersioningInfo, GetAllVersioningInfo>().ReverseMap();
            //CreateMap<VersionModel, GetAllVersionModel>().ReverseMap();
            //CreateMap<OpenIdOptions, GetAllOpenIdOptions>().ReverseMap();
            //CreateMap<Provider, GetAllProvider>().ReverseMap();
            //CreateMap<ClientPolicy, GetAllClientPolicy>().ReverseMap();
            CreateMap<Api, GetApiByIdDto>().ReverseMap();
            CreateMap<RateLimit, GetRateLimit>().ReverseMap();
            CreateMap<VersioningInfo, GetVersioningInfo>().ReverseMap();
            CreateMap<VersionModel, GetVersionModel>().ReverseMap();
            CreateMap<OpenIdOptions, GetOpenIdOptions>().ReverseMap();
            CreateMap<Provider, GetProvider>().ReverseMap();
            CreateMap<ClientPolicy, GetClientPolicy>().ReverseMap();

            CreateMap<Policy, CreatePolicyCommand>().ReverseMap();
            CreateMap<Policy, CreatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, CreatePolicyApi>().ReverseMap();
            CreateMap<Partition, CreatePartition>().ReverseMap();
            CreateMap<AllowedUrl, CreateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, CreatePerApiLimit>().ReverseMap();
            CreateMap<Policy, UpdatePolicyCommand>().ReverseMap();
            CreateMap<Policy, UpdatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, UpdatePolicyApi>().ReverseMap();
            CreateMap<Partition, UpdatePartition>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, UpdatePerApiLimit>().ReverseMap();
            CreateMap<Policy, GetAllPoliciesDto>().ReverseMap();
            //CreateMap<PolicyApi, GetAllPolicyApi>().ReverseMap();
            //CreateMap<Partition, GetAllPartition>().ReverseMap();
            //CreateMap<AllowedUrl, GetAllAllowedUrl>().ReverseMap();
            //CreateMap<PerApiLimit, GetAllPerApiLimit>().ReverseMap();
            CreateMap<Policy, GetPolicyByIdDto>().ReverseMap();
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

            CreateMap<Key,CreateKeyCommand>().ReverseMap();
            CreateMap<AllowedUrl, KeyAllowedUrl>().ReverseMap();
            CreateMap<AccessRightsModel, KeyAccessRightsModel>().ReverseMap();
            CreateMap<ApiLimit, KeyApiLimit>().ReverseMap();
            CreateMap<Key, UpdateKeyCommand>().ReverseMap();
            CreateMap<AllowedUrl, UpdateKeyAllowedUrl>().ReverseMap();
            CreateMap<AccessRightsModel, UpdateKeyAccessRightsModel>().ReverseMap();
            CreateMap<Key, UpdateKeyCommandDto>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrlDto>().ReverseMap();
            CreateMap<AccessRightsModel, UpdateAccessRightsModelDto>().ReverseMap();
            CreateMap<ApiLimit, UpdateKeyLimit>().ReverseMap();
            CreateMap<ApiLimit, UpdateKeyLimitDto>().ReverseMap();



            CreateMap<ApiDto, GetAllApiModel>().ReverseMap();
            CreateMap<KeyDto, GetAllKeyModel>().ReverseMap();
            CreateMap<PolicyDto, GetAllPolicyModel>().ReverseMap();
        }

    }
}
