using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetAllKeys
{
    public class GetAllKeysDto
    {
     public List<GetAllKeyModel> Keys { get; set; }
    } 

    public class GetAllKeyModel
    {
        public string Id { get; set; }
        public string KeyName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Expires { get; set; }
        public List<string> Policies { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
