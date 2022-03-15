
using ApplicationGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ApplicationGateway.Domain.TykData
{
    [ExcludeFromCodeCoverage]
    public class CreateRequest
    {
        public string name { get; set; }
        public string listenPath { get; set; }
        public string targetUrl { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Docker
    {
#nullable enable
        public args? args { get; set; }
        public headers? headers { get; set; }
#nullable disable
        public string origin { get; set; }
        public string url { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class args
    {

    }

    [ExcludeFromCodeCoverage]
    public class headers
    {
        public string Accept { get; set; }
        public string Accept_Encoding { get; set; }
        public string Host { get; set; }
        public string Postman_Token { get; set; }
        public string User_Agent { get; set; }
        public string X_Amzn_Trace_Id { get; set; }

    }
}
