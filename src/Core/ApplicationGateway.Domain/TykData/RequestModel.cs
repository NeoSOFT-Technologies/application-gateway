//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationGateway.Domain.TykData
{
      public class Proxy
    {
        public string listen_path { get; set; }
        public string target_url { get; set; }
        public bool strip_listen_path { get; set; }
       /* public bool enable_load_balancing { get; set; }
        public List<string> target_list { get; set; }*/
    }


    public class Default
    {
        public string name { get; set; }
        public bool use_extended_paths { get; set; }
    }

    public class Versions
    {
        public Default Default { get; set; }
    }

    public class VersionData
    {
        public bool not_versioned { get; set; }
        public string default_version { get; set; }
        public Versions versions { get; set; }
    }



    public class RequestModel
    {
        /*[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }*/
        public string name { get; set; }
        public string api_id { get; set; }
        public bool use_keyless { get; set; }
        public bool active { get; set; }
        public Proxy proxy { get; set; }
        public VersionData version_data { get; set; }
    }

    public class global_rate_limit
    {
        public int rate { get; set; }
        public int per { get; set; }
    }
    public class RateLimit_RequestModel
    {
      
        public string name { get; set; }
        public string api_id { get; set; }
        public bool use_keyless { get; set; }
        public bool active { get; set; }
        public Proxy proxy { get; set; }
        public VersionData Version_data { get; set; }
        public global_rate_limit global_rate_limit { get; set; }
    }
    public class ResponseModel
    {

        public string key { get; set; }
        public string status { get; set; }
        public string action { get; set; }

    }
    public class CreateRequest
    {
        public string name { get; set; }
        public string listenPath { get; set; }
        public string targetUrl { get; set; }
    }

    public class UpdateRequest : CreateRequest
    {
        public Guid id { get; set; }
        public RateLimit? rateLimit { get; set; }
        public List<string>? blacklist { get; set; }
        public List<string>? whitelist { get; set; }
        public string defaultVersion { get; set; }
        public List<VersionModel> versions { get; set; }
        public string authType { get; set; }
        public OpenIdOptions? openidOptions { get; set; }
        public VersioningInfo versioningInfo { get; set; }
        public List<string>? loadBalancingTargets { get; set; }
    }
    public class AccessRightsModel
    {
        public string apiId { get; set; }
        public string apiName { get; set; }
        public List<VersionModel> versions { get; set; }
    }
    public class RateLimit
    {
        public int rate { get; set; }
        public int per { get; set; }
    }

    public class VersioningInfo
    {
        public string location { get; set; }
        public string key { get; set; }
        public bool strip_path { get; set; }
    }

    public class VersionModel
    {
        public string name { get; set; }
        public string? overrideTarget { get; set; }
    }

    public class CircuitBreakerRequest
    {
        public string apiId { get; set; }
        public string version { get; set; }
        public string method { get; set; }
        public string path { get; set; }
        public int samples { get; set; } 
        public double threshold_percent { get; set; }
        public int cooldownTime { get; set; }
        public bool disable_half_open_state { get; set; }
    }

    public class OpenIdOptions
    {
        public List<Provider> providers { get; set; }
    }

    public class Provider
    {
        public string issuer { get; set; }
        public List<ClientPolicy> client_ids { get; set; }
    }

    public class ClientPolicy
    {
        public string clientId { get; set; }
        public string policy { get; set; }
    }

    public class CreateKeyRequest
    {
        public int rate{ get; set;}
        public int perSec { get; set; }
        public int quota { get; set; }
        public int quotaRenewalRate { get; set; }
        public List<AccessRightsModel> accessRights { get; set; }
        public string policyId{ get; set; }
        public int expires { get; set; }

    }
}
