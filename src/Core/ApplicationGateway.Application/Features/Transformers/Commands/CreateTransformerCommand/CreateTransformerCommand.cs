using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using MediatR;

namespace ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand
{
    public class CreateTransformerCommand:IRequest<Response<CreateTransformerDto>>
    {
        public string TemplateName { get; set; }
        public string TransformerTemplate { get; set; }
        public Gateway Gateway { get; set; }
    }
}
