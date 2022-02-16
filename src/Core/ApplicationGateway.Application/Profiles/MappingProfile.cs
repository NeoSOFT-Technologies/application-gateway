using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.TykData;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using static ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand.CreateKeyCommand;

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

            CreateMap<Policy, CreatePolicyCommand>().ReverseMap();
            CreateMap<Policy, CreatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, CreatePolicyApi>().ReverseMap();
            CreateMap<Partition, CreatePartition>().ReverseMap();
            CreateMap<AllowedUrl, CreateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, CreatePerApiLimit>().ReverseMap();

            CreateMap<Key,CreateKeyCommand>().ReverseMap();
            CreateMap<Key.AllowedUrl, KeyAllowedUrl>().ReverseMap();
            CreateMap<Key.AccessRightsModel, KeyAccessRightsModel>().ReverseMap();
            
        }
    }
}
