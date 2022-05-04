using ApplicationGateway.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Domain.Entities
{
    public class Api: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TargetUrl { get; set; }
        public string AuthType { get; set; }
        public string Version { get; set; }
        public bool IsActive { get; set; }
    }
}
