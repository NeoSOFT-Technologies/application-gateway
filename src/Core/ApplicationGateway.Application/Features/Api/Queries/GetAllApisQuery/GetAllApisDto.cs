namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{

    public class GetAllApisDto
    {
        public List<GetAllApiModel> Apis { get; set; }
    }


    public class GetAllApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TargetUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Version { get; set; }

    }
}
