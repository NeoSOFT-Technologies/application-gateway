using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.TykData;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;

namespace ApplicationGateway.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Api, CreateApiCommand>().ReverseMap();
            CreateMap<Api, CreateApiDto>().ReverseMap();

            CreateMap<Policy, CreatePolicyCommand>().ReverseMap();
            CreateMap<Policy, CreatePolicyDto>().ReverseMap();
            CreateMap<PolicyApi, CreatePolicyApi>().ReverseMap();
            CreateMap<Partition, CreatePartition>().ReverseMap();
            CreateMap<AllowedUrl, CreateAllowedUrl>().ReverseMap();
            CreateMap<PerApiLimit, CreatePerApiLimit>().ReverseMap();
        }
    }
}
