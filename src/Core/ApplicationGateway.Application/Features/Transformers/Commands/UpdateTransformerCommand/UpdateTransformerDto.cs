namespace ApplicationGateway.Application.Features.Transformers.Commands.UpdateTransformerCommand
{
    public class UpdateTransformerDto
    {
        public Guid TransformerId { get; set; }
        public string TemplateName { get; set; }
    }
}
