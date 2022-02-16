namespace ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand
{
    public class CreateApiDto
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
    }
}
