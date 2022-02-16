﻿using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.TykData;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Transformer.Commands.CreateTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Commands.UpdateTransformerCommand;
using ApplicationGateway.Application.Features.Transformer.Queries.GetTransformer;
using ApplicationGateway.Application.Features.Transformer.Queries.GetTransformerById;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using static ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand.CreateKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand;

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
            CreateMap<Policy, UpdatePolicyCommand>().ReverseMap();
            CreateMap<Policy, UpdatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, UpdatePolicyApi>().ReverseMap();
            CreateMap<Partition, UpdatePartition>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, UpdatePerApiLimit>().ReverseMap();

            CreateMap<Transformers, CreateTransformerCommand>().ReverseMap();
            CreateMap<Transformers, CreateTransformerDto>().ReverseMap();

            CreateMap<Transformers, UpdateTransformerCommand>().ReverseMap();
            CreateMap<Transformers, UpdateTransformerDto>().ReverseMap();

            CreateMap<Transformers, GetTransformerDto>().ReverseMap();
            CreateMap<Transformers, GetTransformerByIdDto>().ReverseMap();

            CreateMap<Key,CreateKeyCommand>().ReverseMap();
            CreateMap<AllowedUrl, KeyAllowedUrl>().ReverseMap();
            CreateMap<AccessRightsModel, KeyAccessRightsModel>().ReverseMap();
            CreateMap<Key, UpdateKeyCommand>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrl>().ReverseMap();
            CreateMap<AccessRightsModel, UpdateAccessRightsModel>().ReverseMap();
            CreateMap<Key, UpdateKeyCommandDto>().ReverseMap();
            CreateMap<AllowedUrl, UpdateAllowedUrlDto>().ReverseMap();
            CreateMap<AccessRightsModel, UpdateAccessRightsModelDto>().ReverseMap();


        }

    }
}
