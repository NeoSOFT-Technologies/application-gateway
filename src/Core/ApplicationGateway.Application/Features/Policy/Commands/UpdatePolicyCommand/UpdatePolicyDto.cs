namespace ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand
{
    public class UpdatePolicyDto
    {
        public Guid PolicyId { get; set; }
        public string Name { get; set; }
    }
}
