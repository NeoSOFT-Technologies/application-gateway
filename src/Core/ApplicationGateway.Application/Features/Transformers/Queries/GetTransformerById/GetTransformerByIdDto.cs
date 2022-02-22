using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById
{
    public class GetTransformerByIdDto
    {
        public Guid TransformerId { get; set; }

        public string TemplateName { get; set; }

        public string TransformerTemplate { get; set; }

        public string Gateway { get; set; }
    }
}
