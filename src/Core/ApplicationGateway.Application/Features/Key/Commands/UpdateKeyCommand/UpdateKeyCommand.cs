using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand
{
    public class UpdateKeyCommand:IRequest<Response<UpdateKeyCommandDto>>
    {
            public string KeyId { get; set; }
            public int Rate { get; set; }
            public int Per { get; set; }
            public int Quota { get; set; }
            public int QuotaRenewalRate { get; set; }
            public int ThrottleInterval { get; set; }
            public int ThrottleRetries { get; set; }
            public int Expires { get; set; }
            public List<UpdateAccessRightsModel> AccessRights { get; set; }
            public List<string> Policies { get; set; }

        }

        public class UpdateAccessRightsModel
        {
            public Guid ApiId { get; set; }
            public string ApiName { get; set; }
            public List<string> Versions { get; set; }
            public UpdateAllowedUrl AllowedUrls { get; set; }
        }

        public class UpdateAllowedUrl
        {
            public string Url { get; set; }
            public List<string> Methods { get; set; }
        }
    
}
