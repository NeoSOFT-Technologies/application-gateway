using MediatR;

namespace ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand
{
    public class DeleteApiCommand : IRequest
    {
        public Guid ApiId { get; set; }
    }
}
