using MediatR;

namespace ApplicationGateway.Application.Features.Transformers.Commands.DeleteTransformerCommand
{
    public class DeleteTransformerCommand : IRequest
    {
        public Guid TransformerId { get; set; }
    }
}
