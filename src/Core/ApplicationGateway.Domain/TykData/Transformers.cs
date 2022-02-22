using ApplicationGateway.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Domain.TykData
{
    public class Transformers : AuditableEntity
    {
        public Guid Id { get; set; }

        public string TemplateName { get; set; }

        public string TransformerTemplate { get; set; }

        public string Gateway { get; set; }

    }
}
