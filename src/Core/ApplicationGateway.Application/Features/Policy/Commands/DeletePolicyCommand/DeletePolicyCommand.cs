using MediatR;

namespace ApplicationGateway.Application.Features.Policy.Commands.DeletePolicyCommand
{
    public class DeletePolicyCommand : IRequest
    {
        public Guid PolicyId { get; set; }
    }
}