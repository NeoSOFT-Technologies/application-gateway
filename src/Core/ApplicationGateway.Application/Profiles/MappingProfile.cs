using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Policy, CreatePolicyCommand>().ReverseMap();
            CreateMap<Policy, CreatePolicyDto>().ReverseMap();
            CreateMap<Domain.TykData.PolicyApi, Features.Policy.Commands.CreatePolicyCommand.PolicyApi>().ReverseMap();
            CreateMap<Domain.TykData.Partition, Features.Policy.Commands.CreatePolicyCommand.Partition>().ReverseMap();
            CreateMap<Domain.TykData.AllowedUrl, Features.Policy.Commands.CreatePolicyCommand.AllowedUrl>().ReverseMap();
            CreateMap<Domain.TykData.PerApiLimit, Features.Policy.Commands.CreatePolicyCommand.PerApiLimit>().ReverseMap();
        }
    }
}
