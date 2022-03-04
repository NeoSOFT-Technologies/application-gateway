using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById
{
    public class GetTransformerByIdDto
    {
        public Guid TransformerId { get; set; }
        public string TemplateName { get; set; }
        public string TransformerTemplate { get; set; }
        public Gateway Gateway { get; set; }
    }
}
