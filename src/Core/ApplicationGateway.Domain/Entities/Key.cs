using ApplicationGateway.Domain.Common;

namespace ApplicationGateway.Domain.Entities
{
    public class Key: AuditableEntity
    {
        public string Id { get; set; }   
        public string KeyName { get; set; }
        public bool IsActive{ get; set; }    
        public DateTime? Expires { get; set; }
        public List<string> Policies { get; set; }
    }
}
