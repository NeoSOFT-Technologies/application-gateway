using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.TykData;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Features.Transformer.Commands.CreateTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Commands.UpdateTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Queries.GetTransformer;
using ApplicationGateway.Application.Features.Transformer.Queries.GetTransformerById;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;

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
            CreateMap<RateLimit, GetAllRateLimit>().ReverseMap();
            CreateMap<VersioningInfo, GetAllVersioningInfo>().ReverseMap();
            CreateMap<VersionModel, GetAllVersionModel>().ReverseMap();
            CreateMap<OpenIdOptions, GetAllOpenIdOptions>().ReverseMap();
            CreateMap<Provider, GetAllProvider>().ReverseMap();
            CreateMap<ClientPolicy, GetAllClientPolicy>().ReverseMap();
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

            
            CreateMap<Transformers, CreateTransformerCommand>().ReverseMap();
            CreateMap<Transformers, CreateTransformerDto>().ReverseMap();

            CreateMap<Transformers, UpdateTransformerCommand>().ReverseMap();
            CreateMap<Transformers, UpdateTransformerDto>().ReverseMap();

            CreateMap<Transformers, GetTransformerDto>().ReverseMap();
            CreateMap<Transformers, GetTransformerByIdDto>().ReverseMap();

            CreateMap<Key,CreateKeyCommand>().ReverseMap();
            CreateMap<Key.AllowedUrl, KeyAllowedUrl>().ReverseMap();
            CreateMap<Key.AccessRightsModel, KeyAccessRightsModel>().ReverseMap();
            
        }

    }
}
