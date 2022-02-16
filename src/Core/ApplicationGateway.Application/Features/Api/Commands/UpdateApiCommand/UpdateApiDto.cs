namespace ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand
{
    public class UpdateApiDto
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
    }
}
