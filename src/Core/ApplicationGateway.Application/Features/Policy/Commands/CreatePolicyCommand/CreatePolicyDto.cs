namespace ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand
{
    public class CreatePolicyDto
    {
        public Guid PolicyId { get; set; }
        public string Name { get; set; }
    }
}
