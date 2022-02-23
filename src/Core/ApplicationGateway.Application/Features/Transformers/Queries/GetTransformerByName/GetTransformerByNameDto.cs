using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerByName
{
    public class GetTransformerByNameDto
    {
        public string TemplateName { get; set; }
        public string TransformerTemplate { get; set; }
        public Gateway Gateway { get; set; }
    }
}
