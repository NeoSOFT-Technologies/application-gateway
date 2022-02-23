using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using MediatR;

namespace ApplicationGateway.Application.Features.Transformers.Commands.UpdateTransformerCommand
{
    public class UpdateTransformerCommand : IRequest<Response<UpdateTransformerDto>>
    {
        public Guid TransformerId { get; set; }
        public string TemplateName { get; set; }
        public string TransformerTemplate { get; set; }
        public Gateway Gateway { get; set; }

    }
}
