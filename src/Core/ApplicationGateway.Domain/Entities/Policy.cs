using ApplicationGateway.Domain.Common;
using System.Collections.Generic;

namespace ApplicationGateway.Domain.Entities
{
    public class Policy: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Apis { get; set; }  
        public string AuthType { get; set; }
        public string State { get; set; }
    }
}
