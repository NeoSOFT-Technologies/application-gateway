using ApplicationGateway.Application.Contracts.Persistence;
using JUST;

namespace ApplicationGateway.Application.Helper
{
    public class TemplateTransformer
    {
        private readonly ITransformerRepository _transformerRepository;

        public TemplateTransformer(ITransformerRepository transformerRepository)
        {
            _transformerRepository = transformerRepository;
        }

        public async Task<string> Transform(string requestJson, string templateName, Domain.Entities.Gateway gateway)
        {
            Domain.Entities.Transformer transformer = await _transformerRepository.GetTransformerByNameAndGateway(templateName, gateway);
            return new JsonTransformer().Transform(transformer.TransformerTemplate, requestJson);
        }
    }
}
