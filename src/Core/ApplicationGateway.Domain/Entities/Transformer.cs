using ApplicationGateway.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Domain.Entities
{
    public class Transformer : AuditableEntity
    {
        public Guid TransformerId { get; set; }
        public string TemplateName { get; set; }
        public string TransformerTemplate { get; set; }
        public Gateway Gateway { get; set; }
    }

    public enum Gateway
    {
        Tyk,
        Envoy,
        Kong
    }
}
