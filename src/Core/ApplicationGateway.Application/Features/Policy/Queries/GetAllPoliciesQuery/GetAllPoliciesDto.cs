namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesDto
    {
        public List<GetAllPolicyModel> Policies { get; set; }
    }

    public class GetAllPolicyModel
    {
        public string Name { get; set; }
        public List<string> Apis { get; set; }
        public string AuthType { get; set; }
        public string State { get; set; }

    }
    
}
