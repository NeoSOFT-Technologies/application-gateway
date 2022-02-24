namespace ApplicationGateway.Application.Features.Transformers.Commands.CreateTransformerCommand
{
    public class CreateTransformerDto
    {
        public Guid TransformerId { get; set; }
        public string TemplateName { get; set; }
    }
}
