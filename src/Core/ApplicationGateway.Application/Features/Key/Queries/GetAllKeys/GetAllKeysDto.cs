using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetAllKeys
{
    public class GetAllKeysDto
    {
     public List<AllKeyDto> KeyDto { get; set; }
    } 

    public class AllKeyDto
    {
        public string KeyId { get; set; }
        public string AuthType { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}
