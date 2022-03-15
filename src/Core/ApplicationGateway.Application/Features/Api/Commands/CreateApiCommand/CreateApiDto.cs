namespace ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand
{
    public class CreateApiDto
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public bool StripListenPath { get; set; }
        public string TargetUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
