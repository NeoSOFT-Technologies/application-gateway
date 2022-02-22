namespace ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand
{
    public class CreateMultipleApisDto
    {
        public List<MultipleApiModelDto> APIs { get; set; }
    }

    public class MultipleApiModelDto
    {
        public Guid ApiId { get; set; }
        public string Name { get; set; }
    }
}
