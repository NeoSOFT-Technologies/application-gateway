using AutoMapper;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Profiles
{
    public class MappingProfile
    {
        public MappingProfile()
        {
            //CreateMap<Policy, CreatePolicyCommand>().ReverseMap();
            //CreateMap<Policy, CreatePolicyDto>().ReverseMap();
        }
    }
}
