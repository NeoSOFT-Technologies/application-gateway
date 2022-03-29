using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.GatewayCommon;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ApplicationGateway.Infrastructure.Gateway.Tyk
{
    [ExcludeFromCodeCoverage]
    public class TykApiService : IApiService
    {
        private readonly IBaseService _baseService;
        private readonly ILogger<TykApiService> _logger;
        private readonly TemplateTransformer _templateTransformer;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public TykApiService(IBaseService baseService, ILogger<TykApiService> logger, TemplateTransformer templateTransformer, IOptions<TykConfiguration> tykConfiguration)
        {
            _baseService = baseService;
            _logger = logger;
            _templateTransformer = templateTransformer;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/apis", _headers);
        }

        public async Task<List<Api>> GetAllApisAsync()
        {
            _logger.LogInformation("GetAllApisAsync Initiated");
            string inputJson = await _restClient.GetAsync(null);
            JArray inputObject = JArray.Parse(inputJson);
            JArray transformedObject = new JArray();

            #region Transorm individual api
            foreach (var inputApi in inputObject)
            {
                string transformed = await _templateTransformer.Transform(inputApi.ToString(), TemplateHelper.GETAPI_TEMPLATE, Domain.Entities.Gateway.Tyk);
                JObject apiObject = JObject.Parse(transformed);

                apiObject = GetApiVersioning(apiObject, inputApi as JObject);
                apiObject = GetAuthType(apiObject, inputApi as JObject);

                if (inputApi["openid_options"]["providers"] as JArray is not null)
                {
                    apiObject = GetOIDC(apiObject, inputApi as JObject);
                }
                #region Get VersioningLocation
                if (apiObject["versioningInfo"]["location"].ToString() == "")
                {
                    (apiObject["versioningInfo"] as JObject).Remove("location");
                    (apiObject["versioningInfo"] as JObject).Add("location", VersioningLocation.none.ToString());
                }
                else if (apiObject["versioningInfo"]["location"].ToString() == "url-param")
                {
                    (apiObject["versioningInfo"] as JObject).Remove("location");
                    (apiObject["versioningInfo"] as JObject).Add("location", VersioningLocation.url_param.ToString());
                }
                #endregion
                transformedObject.Add(apiObject);
            }
            #endregion

            List<Api> apiList = JsonConvert.DeserializeObject<List<Api>>(transformedObject.ToString(), new Newtonsoft.Json.Converters.StringEnumConverter());
            _logger.LogInformation("GetAllApisAsync Completed: {@List<Api>}", apiList);
            return apiList;
        }

        public async Task<Api> GetApiByIdAsync(Guid apiId)
        {
            _logger.LogInformation("GetApiByIdAsync Initiated with {@Guid}", apiId);
            string inputJson = await _restClient.GetAsync(apiId.ToString());
            JObject inputObject = JObject.Parse(inputJson);

            #region Transorm Api
            string transformed = await _templateTransformer.Transform(inputObject.ToString(), TemplateHelper.GETAPI_TEMPLATE, Domain.Entities.Gateway.Tyk);
            JObject transformedObject = JObject.Parse(transformed);

            transformedObject = GetApiVersioning(transformedObject, inputObject);
            transformedObject = GetAuthType(transformedObject, inputObject);

            if (inputObject["openid_options"]["providers"] as JArray is not null)
            {
                transformedObject = GetOIDC(transformedObject, inputObject);
            }

            #region Get VersioningLocation
            if (transformedObject["versioningInfo"]["location"].ToString() == "")
            {
                (transformedObject["versioningInfo"] as JObject).Remove("location");
                (transformedObject["versioningInfo"] as JObject).Add("location", VersioningLocation.none.ToString());
            }
            else if (transformedObject["versioningInfo"]["location"].ToString() == "url-param")
            {
                (transformedObject["versioningInfo"] as JObject).Remove("location");
                (transformedObject["versioningInfo"] as JObject).Add("location", VersioningLocation.url_param.ToString());
            }
            #endregion
            #endregion

            Api api = JsonConvert.DeserializeObject<Api>(transformedObject.ToString(), new Newtonsoft.Json.Converters.StringEnumConverter());
            _logger.LogInformation("GetApiByIdAsync Completed: {@Api}", api);
            return api;
        }

        public async Task<Api> CreateApiAsync(Api api)
        {
            _logger.LogInformation("CreateApiAsync Initiated with {@Api}", api);
            api.ApiId = Guid.NewGuid();
            string requestJson = JsonConvert.SerializeObject(api);
            string transformed = await _templateTransformer.Transform(requestJson, TemplateHelper.CREATEAPI_TEMPLATE, Domain.Entities.Gateway.Tyk);

            #region Add ApiId to Api
            JObject transformedObject = JObject.Parse(transformed);
            transformedObject.Add("api_id", api.ApiId);
            #endregion

            await _restClient.PostAsync(transformedObject);

            await _baseService.HotReload();

            _logger.LogInformation("CreateApiAsync Completed: {@Api}", api);
            return api;
        }

        public async Task<Api> UpdateApiAsync(Api api)
        {
            _logger.LogInformation("UpdateApiAsync Initiated with {@Api}", api);
            string inputJson = JsonConvert.SerializeObject(api, new Newtonsoft.Json.Converters.StringEnumConverter());
            string transformed = await _templateTransformer.Transform(inputJson, TemplateHelper.UPDATEAPI_TEMPLATE, Domain.Entities.Gateway.Tyk);

            JObject inputObject = JObject.Parse(inputJson);
            JObject transformedObject = JObject.Parse(transformed);

            #region Add version_data to API
            if (inputObject["Versions"].Count() != 0)
            {
                transformedObject["version_data"]["versions"] = new JObject();
                foreach (JToken version in inputObject["Versions"])
                {
                    (version as JObject).Add("use_extended_paths", true);
                    JArray removeGlobalHeaders = new JArray();
                    removeGlobalHeaders.Add("Authorization");
                    (version as JObject).Add("global_headers_remove", removeGlobalHeaders);
                    (transformedObject["version_data"]["versions"] as JObject).Add($"{version["Name"]}", version);
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("override_target", version["OverrideTarget"]);
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Property("OverrideTarget").Remove();
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("expires", version["Expires"]);
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Property("Expires").Remove();
                    if (version.Value<JObject>("ExtendedPaths") is not null)
                    {
                        transformedObject = SetExtendedPaths(transformedObject, version as JObject);
                    }
                    transformedObject = SetAddRemoveGlobalHeaders(transformedObject, version as JObject);
                }
            }
            #endregion

            #region Add OIDC Configuration to API
            if (inputObject["AuthType"].ToString() == "openid")
            {
                transformedObject["openid_options"]["providers"] = new JArray();
                foreach (JToken provider in inputObject["OpenidOptions"]["Providers"])
                {
                    JObject newProvider = new JObject();
                    newProvider.Add("issuer", provider["Issuer"]);
                    JObject newClient = new JObject();
                    foreach (JToken client in provider["Client_ids"])
                    {
                        string base64ClientId = Convert.ToBase64String(Encoding.UTF8.GetBytes(client["ClientId"].ToString()));
                        newClient.Add(base64ClientId, client["Policy"]);
                    }
                    newProvider.Add("client_ids", newClient);
                    (transformedObject["openid_options"]["providers"] as JArray).Add(newProvider);
                }
            }
            #endregion

            #region Set VersioningLocation
            if (transformedObject["definition"]["location"].ToString() == "none")
            {
                (transformedObject["definition"] as JObject).Remove("location");
                (transformedObject["definition"] as JObject).Add("location", "");
            }
            else if (transformedObject["definition"]["location"].ToString() == "url_param")
            {
                (transformedObject["definition"] as JObject).Remove("location");
                (transformedObject["definition"] as JObject).Add("location", VersioningLocation.url_param.ToString().Replace('_', '-'));
            }
            #endregion

            await _restClient.PutAsync(transformedObject);

            await _baseService.HotReload();

            _logger.LogInformation("UpdateApiAsync Completed: {@Api}", api);
            return api;
        }

        public async Task DeleteApiAsync(Guid apiId)
        {
            _logger.LogInformation("DeleteApiAsync Initiated with {@Guid}", apiId);
            await _restClient.DeleteAsync(apiId.ToString());
            await _baseService.HotReload();
            _logger.LogInformation("DeleteApiAsync Completed: {@Guid}", apiId);
        }

        public async Task<bool> CheckUniqueListenPathAsync(Api api)
        {
            #region Get All Existsing APIs
            List<Api> apiList = await GetAllApisAsync();
            #endregion

            #region Check for existing listenPath
            foreach (Api existingApi in apiList)
            {
                if (api.ApiId != existingApi.ApiId && api.ListenPath.Trim('/') == existingApi.ListenPath.Trim('/'))
                {
                    return false;
                }
            }
            #endregion
            return true;
        }
        private static JObject GetApiVersioning(JObject apiObject, JObject inputApi)
        {
            apiObject["versions"] = new JArray();
            JObject versionDataObject = inputApi["version_data"]["versions"] as JObject;
            foreach (var item in versionDataObject)
            {
                JObject tempObj = new JObject();
                tempObj.Add("name", item.Key);
                tempObj.Add("overrideTarget", item.Value["override_target"]);
                tempObj.Add("expires", item.Value["expires"]);
                if (item.Value["extended_paths"] is not null && item.Value["extended_paths"].ToString() != "{}")
                {
                    tempObj = GetExtendedPaths(tempObj, item.Value["extended_paths"] as JObject);
                    tempObj = GetAddRemoveGlobalHeaders(tempObj, item.Value as JObject);
                }
                (apiObject["versions"] as JArray).Add(tempObj);
            }
            return apiObject;
        }

        private static JObject GetAuthType(JObject apiObject, JObject inputApi)
        {
            if (inputApi.Value<bool>("use_keyless"))
            {
                apiObject["authType"] = "keyless";
            }
            else if (inputApi.Value<bool>("use_basic_auth"))
            {
                apiObject["authType"] = "basic";
            }
            else if (inputApi.Value<bool>("use_standard_auth"))
            {
                apiObject["authType"] = "standard";
            }
            else if (inputApi.Value<bool>("use_openid"))
            {
                apiObject["authType"] = "openid";
            }
            return apiObject;
        }

        private static JObject GetOIDC(JObject apiObject, JObject inputApi)
        {
            apiObject["openidOptions"]["providers"] = new JArray();
            JArray openIdProviders = inputApi["openid_options"]["providers"] as JArray;
            foreach (var item in openIdProviders)
            {
                var tempObj = new JObject();
                tempObj.Add("issuer", item["issuer"]);
                tempObj.Add("client_ids", new JArray());
                foreach (var innerItem in item["client_ids"] as JObject)
                {
                    JObject clientObj = new JObject();
                    byte[] decodedBytes = Convert.FromBase64String(innerItem.Key.ToString());
                    string decodedText = Encoding.UTF8.GetString(decodedBytes);
                    clientObj.Add("clientId", decodedText);
                    clientObj.Add("policy", innerItem.Value);
                    (tempObj["client_ids"] as JArray).Add(clientObj);
                }
                (apiObject["openidOptions"]["providers"] as JArray).Add(tempObj);
            }
            return apiObject;
        }

        private static JObject GetExtendedPaths(JObject apiObject, JObject extendedPaths)
        {
            apiObject.Add("extendedPaths", new JObject());
            JArray methodTransforms = new JArray();
            if (extendedPaths["method_transforms"] is not null)
            {
                foreach (JToken methodTransform in extendedPaths["method_transforms"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", methodTransform.Value<string>("method"));
                    tempObj.Add("path", methodTransform.Value<string>("path"));
                    tempObj.Add("toMethod", methodTransform.Value<string>("to_method"));
                    methodTransforms.Add(tempObj);
                }
                (apiObject["extendedPaths"] as JObject).Add("methodTransforms", methodTransforms);
            }
            if (extendedPaths["url_rewrites"] is not null)
            {
                GetUrlRewrite(apiObject, extendedPaths);
            }
            if(extendedPaths["validate_json"] is not null)
            {
                GetValidateJson(apiObject,extendedPaths);
            }
            GetTransformHeaders(apiObject, extendedPaths);
            GetTransformResponseHeaders(apiObject, extendedPaths);
            GetTransform(apiObject, extendedPaths);
            GetTransformResponse(apiObject, extendedPaths);
            return apiObject;
        }

        private static JObject GetValidateJson(JObject apiObject, JObject extendedPaths)
        {
            JArray validate = new JArray();
            foreach (JToken jsonvalidate in extendedPaths["validate_json"])
            {
                
                JObject tempObj = new JObject();
                var schema = (jsonvalidate.Value<JObject>("schema")).ToString();
                tempObj.Add("method", jsonvalidate.Value<string>("method"));
                tempObj.Add("path", jsonvalidate.Value<string>("path"));
                tempObj.Add("schema", schema);
                tempObj.Add("errorResponseCode", jsonvalidate.Value<int>("error_response_code"));
                validate.Add(tempObj);
            }
            (apiObject["extendedPaths"] as JObject).Add("ValidateJsons", validate);
            return apiObject;
        }

        private static JObject GetUrlRewrite(JObject apiObject, JObject extendedPaths)
        {
            JArray urlRewrite = new JArray();
            if (extendedPaths["url_rewrites"] is not null)
            {
                foreach (JToken urlRewrites in extendedPaths["url_rewrites"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("path", urlRewrites.Value<string>("path"));
                    tempObj.Add("method", urlRewrites.Value<string>("method"));
                    tempObj.Add("matchPattern", urlRewrites.Value<string>("match_pattern"));
                    tempObj.Add("rewriteTo", urlRewrites.Value<string>("rewrite_to"));
                    urlRewrite.Add(tempObj);
                    (apiObject["extendedPaths"] as JObject).Add("urlRewrites", urlRewrite);
                    if (urlRewrites.Value<JArray>("triggers") is not null)
                    {
                        var trans = GetTriggers(apiObject, extendedPaths);
                        apiObject = trans;
                    }
                }
                
            }
            return apiObject;
        }

        private static JObject GetTriggers(JObject apiObject, JObject extendedPaths)
        {
            JArray trigger = new JArray();
            for(var j=0;j< extendedPaths["url_rewrites"].Count(); j++)
            {
                for (var i = 0; i < extendedPaths["url_rewrites"][j]["triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("on", extendedPaths["url_rewrites"][j]["triggers"][i].Value<string>("on"));
                    tempObj.Add("options", extendedPaths["url_rewrites"][j]["triggers"][i].Value<JObject>("options"));
                    tempObj.Add("rewriteTo", extendedPaths["url_rewrites"][j]["triggers"][i].Value<string>("rewrite_to"));
                    trigger.Add(tempObj);
                    (apiObject["extendedPaths"]["urlRewrites"][j] as JObject).Add("triggers", trigger);
                    var trans = GetOptions(apiObject, extendedPaths);
                    apiObject = trans;
                }
            }
            return apiObject;
        }

        private static JObject GetOptions(JObject apiObject, JObject extendedPaths)
        {
            JArray trigger = new JArray();
            for (var j = 0; j < extendedPaths["url_rewrites"].Count(); j++)
            {
                for (var i = 0; i < extendedPaths["url_rewrites"][j]["triggers"].Count(); i++)
                {
                    (apiObject["extendedPaths"]["urlRewrites"][j]["triggers"][i] as JObject).Property("options").Remove();
                    (apiObject["extendedPaths"]["urlRewrites"][j]["triggers"][i] as JObject).Add("options",new JObject());
                    JObject options = (extendedPaths["url_rewrites"][j]["triggers"][i]["options"] as JObject);
                    if (options.Value<JObject>("query_val_matches") is not null)
                    {
                        var trans = GetTrigger(apiObject, extendedPaths, "queryValMatches");
                        apiObject = trans;
                    }
                    if (options.Value<JObject>("header_matches") is not null)
                    {
                        var trans = GetTrigger(apiObject, extendedPaths, "headerMatches");
                        apiObject = trans;
                    }
                    if (options.Value<JObject>("path_part_matches") is not null)
                    {
                        var trans = GetTrigger(apiObject, extendedPaths, "pathPartMatches");
                        apiObject = trans;
                    }
                    if (options.Value<JObject>("payload_matches") is not null)
                    {
                        var trans = GetPayload(apiObject, extendedPaths);
                        apiObject = trans;
                    }
                }
            }
            return apiObject;
        }

        private static JObject GetTrigger(JObject apiObject, JObject extendedPaths, string triggerType)
        {
            for (var j = 0; j < extendedPaths["url_rewrites"].Count(); j++)
            {
                for (var i = 0; i < extendedPaths["url_rewrites"][j]["triggers"].Count(); i++)
                {
                    (apiObject["extendedPaths"]["urlRewrites"][j]["triggers"][i]["options"] as JObject).Add("queryValMatches", new JObject());
                    var trans = GetCulprit(apiObject, extendedPaths, triggerType);
                    apiObject = trans;
                }
            }
            return apiObject;
        }

        private static JObject GetPayload(JObject apiObject, JObject extendedPaths)
        {
            for (var j = 0; j < extendedPaths["url_rewrites"].Count(); j++)
            {
                for (var i = 0; i < extendedPaths["url_rewrites"][j]["triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("matchRx", extendedPaths["url_rewrites"][j]["triggers"][i]["options"]["payload_matches"].Value<string>("match_rx"));
                    (apiObject["extendedPaths"]["urlRewrites"][j]["triggers"][i]["options"] as JObject).Add("payloadMatches", new JObject());
                    
                }
            }
            return apiObject;
        }

        private static JObject GetCulprit(JObject apiObject, JObject extendedPaths,string culpritname)
        {
            for (var j = 0; j < extendedPaths["url_rewrites"].Count(); j++)
            {
                for (var i = 0; i < extendedPaths["url_rewrites"][j]["triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    var culprit = culpritname;
                    if(culpritname == "queryValMatches")
                    {
                        culprit = "query_val_matches";
                    }else if(culpritname == "headerMatches")
                    {
                        culprit = "header_matches";
                    }
                    else if(culpritname == "pathPartMatches")
                    {
                        culprit = "path_part_matches";
                    }
                    var value = extendedPaths["url_rewrites"][j]["triggers"][i]["options"].Value<JObject>(culprit);
                    var propertyName = "";
                    foreach(var item in value.Properties())
                    {
                        propertyName = item.Name;
                    }
                    tempObj.Add("key",propertyName);
                    tempObj.Add("matchRx", extendedPaths["url_rewrites"][j]["triggers"][i]["options"][culprit][propertyName].Value<string>("match_rx"));
                    tempObj.Add("reverse", extendedPaths["url_rewrites"][j]["triggers"][i]["options"][culprit][propertyName].Value<bool>("reverse"));
                    (apiObject["extendedPaths"]["urlRewrites"][j]["triggers"][i]["options"][culpritname] as JObject).Add("Culprit", tempObj);
                }
            }
            return apiObject;
        }

        private static JObject GetTransform(JObject apiObject, JObject extendedPaths)
        {
            JArray transforms = new JArray();
            if (extendedPaths["transform"] is not null)
            {
                foreach (JToken transform in extendedPaths["transform"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", transform.Value<string>("method"));
                    tempObj.Add("path", transform.Value<string>("path"));
                    transforms.Add(tempObj);
                }
                (apiObject["extendedPaths"] as JObject).Add("transform", transforms);
                GetTemplateData(apiObject, extendedPaths, false);
            }
            return apiObject;
        }

        private static JObject GetTransformResponse(JObject apiObject, JObject extendedPaths)
        {
            JArray transforms = new JArray();
            if (extendedPaths["transform_response"] is not null)
            {
                foreach (JToken transform in extendedPaths["transform_response"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", transform.Value<string>("method"));
                    tempObj.Add("path", transform.Value<string>("path"));
                    transforms.Add(tempObj);
                }
                (apiObject["extendedPaths"] as JObject).Add("transformResponse", transforms);
                GetTemplateData(apiObject, extendedPaths, true);

            }
            return apiObject;
        }

        private static JObject GetTemplateData(JObject apiObject, JObject extendedPaths, bool isResponse)
        {
            string apiProperty = isResponse ? "transform_response" : "transform";
            string transformedProperty = isResponse ? "transformResponse" : "transform";
            JArray transform = extendedPaths.Value<JArray>(apiProperty);
            var item = 0;
            foreach (JToken templatedata in transform.Values<JObject>("template_data"))
            {
                JObject tempObj = new JObject();
                tempObj.Add("enableSession", templatedata.Value<bool>("enable_session"));
                tempObj.Add("inputType", templatedata.Value<string>("input_type"));
                tempObj.Add("templateMode", templatedata.Value<string>("template_mode"));
                tempObj.Add("templateSource", templatedata.Value<string>("template_source"));
                (apiObject["extendedPaths"][transformedProperty][item] as JObject).Add("templateData", tempObj);
                item++;
            }
            return apiObject;
        }

        private static JObject GetTransformHeaders(JObject apiObject, JObject extendedPaths)
        {
            JArray headerTransforms = new JArray();
            if (extendedPaths["transform_headers"] is not null)
            {
                foreach (JToken headerTransform in extendedPaths["transform_headers"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("actOn", headerTransform.Value<bool>("act_on"));
                    tempObj.Add("addHeaders", headerTransform.Value<JObject>("add_headers"));
                    tempObj.Add("deleteHeaders", headerTransform.Value<JArray>("delete_headers"));
                    tempObj.Add("method", headerTransform.Value<string>("method"));
                    tempObj.Add("path", headerTransform.Value<string>("path"));
                    headerTransforms.Add(tempObj);
                }
               (apiObject["extendedPaths"] as JObject).Add("transformHeaders", headerTransforms);
            }
            return apiObject;
        }
        private static JObject GetTransformResponseHeaders(JObject apiObject, JObject extendedPaths)
        {
            JArray headerResponseTransforms = new JArray();
            if (extendedPaths["transform_response_headers"] is not null)
            {
                foreach (JToken headerResponseTransform in extendedPaths["transform_response_headers"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("actOn", headerResponseTransform.Value<bool>("act_on"));
                    tempObj.Add("addHeaders", headerResponseTransform.Value<JObject>("add_headers"));
                    tempObj.Add("deleteHeaders", headerResponseTransform.Value<JArray>("delete_headers"));
                    tempObj.Add("method", headerResponseTransform.Value<string>("method"));
                    tempObj.Add("path", headerResponseTransform.Value<string>("path"));
                    headerResponseTransforms.Add(tempObj);
                }
               (apiObject["extendedPaths"] as JObject).Add("transformResponseHeaders", headerResponseTransforms);
            }
            return apiObject;
        }

        private static JObject GetAddRemoveGlobalHeaders(JObject apiObject, JObject version)
        {
            if (version.Value<JObject>("global_headers") is not null)
            {
                JObject globalHeaders = version.Value<JObject>("global_headers");
                apiObject.Add("globalRequestHeaders", new JObject());
                foreach (var item in globalHeaders)
                {
                    (apiObject["globalRequestHeaders"] as JObject).Add(item.Key, item.Value);
                }
            }
            if (version.Value<JArray>("global_headers_remove") is not null)
            {
                apiObject.Add("globalRequestHeadersRemove", new JArray());
                foreach (var item in version.Value<JArray>("global_headers_remove"))
                {
                    (apiObject["globalRequestHeadersRemove"] as JArray).Add(item);
                }
            }
            if (version.Value<JArray>("global_response_headers_remove") is not null)
            {
                apiObject.Add("globalResponseHeadersRemove", new JArray());
                foreach (var item in version.Value<JArray>("global_response_headers_remove"))
                {
                    (apiObject["globalResponseHeadersRemove"] as JArray).Add(item);
                }
            }
            if (version.Value<JObject>("global_response_headers") is not null)
            {
                JObject globalResponseHeaders = version.Value<JObject>("global_response_headers");
                apiObject.Add("globalResponseHeaders", new JObject());
                foreach (var item in globalResponseHeaders)
                {
                    (apiObject["globalResponseHeaders"] as JObject).Add(item.Key, item.Value);
                }
            }
            return apiObject;
        }
        private static JObject SetExtendedPaths(JObject transformedObject, JObject version)
        {
            JObject extendedPaths = version.Value<JObject>("ExtendedPaths");
            (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("extended_paths", new JObject());
            if (extendedPaths["MethodTransforms"] is not null)
            {
                foreach (JToken methodTransform in extendedPaths["MethodTransforms"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", methodTransform.Value<string>("Method"));
                    tempObj.Add("path", methodTransform.Value<string>("Path"));
                    tempObj.Add("to_method", methodTransform.Value<string>("ToMethod"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("method_transforms", new JArray());
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["method_transforms"] as JArray).Add(tempObj);
                   
                }
            }
            if (extendedPaths["UrlRewrites"] is not null)
            {
                SetUrlRewrite(transformedObject, version, extendedPaths);
            }
            if (extendedPaths["ValidateJsons"] is not null)
            {
                SetJsonValidate(transformedObject, version, extendedPaths);
            }
            SetTransformHeaders(transformedObject, version, extendedPaths);
            SetTransformResponseHeaders(transformedObject, version, extendedPaths);
            SetTransform(transformedObject, version, extendedPaths);
            SetTransformResponse(transformedObject, version, extendedPaths);
            (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Property("ExtendedPaths").Remove();
            return transformedObject;
        }

        private static JObject SetJsonValidate(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            foreach (JToken jsonvalidate in extendedPaths["ValidateJsons"])
            {
                JObject tempObj = new JObject();
                JObject schema = JObject.Parse(jsonvalidate.Value<string>("Schema"));
                tempObj.Add("method", jsonvalidate.Value<string>("Method"));
                tempObj.Add("path", jsonvalidate.Value<string>("Path"));
                tempObj.Add("schema", schema);
                tempObj.Add("error_response_code", jsonvalidate.Value<int>("ErrorResponseCode"));
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("validate_json", new JArray());
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["validate_json"] as JArray).Add(tempObj);
            }
            return transformedObject;
        }
        private static JObject SetUrlRewrite(JObject transformedObject, JObject version,JObject extendedPaths)
        {
            if (extendedPaths["UrlRewrites"] is not null)
            {
                foreach (JToken urlRewrites in extendedPaths["UrlRewrites"])
                { 
                    JObject tempObj = new JObject();
                    tempObj.Add("path", urlRewrites.Value<string>("Path"));
                    tempObj.Add("method", urlRewrites.Value<string>("Method"));
                    tempObj.Add("match_pattern", urlRewrites.Value<string>("MatchPattern"));
                    tempObj.Add("rewrite_to", urlRewrites.Value<string>("RewriteTo"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("url_rewrites", new JArray());
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"] as JArray).Add(tempObj);
                    if(urlRewrites.Value<JArray>("Triggers") is not null)
                    {
                        var transobj = SetTrigger(transformedObject, version, extendedPaths);
                        transformedObject = transobj;
                    }
                }
            }
            return transformedObject;
        }
        
        private static JObject SetTrigger(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            JArray trig = extendedPaths.Value<JArray>("UrlRewrites");
            var length = transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"].Count();
            for (var i = 0; i < length; i++)
            {
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][i] as JObject).Add("triggers", new JArray());
            } 
         
            for(var j=0;j<trig.Count();j++)    
            {
                for (var i = 0; i < trig[j]["Triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("on", trig[j]["Triggers"][i].Value<string>("On"));
                    tempObj.Add("options", trig[j]["Triggers"][i].Value<JObject>("Options"));
                    tempObj.Add("rewrite_to", trig[j]["Triggers"][i].Value<string>("RewriteTo"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][j]["triggers"] as JArray).Add(tempObj);
                    var transobj = SetOptions(transformedObject, version, extendedPaths);
                    transformedObject = transobj;
                }
               
            }
            return transformedObject;
        }

        private static JObject SetOptions(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            JArray trig = extendedPaths.Value<JArray>("UrlRewrites");
            for (var j = 0; j < trig.Count(); j++)
            {
                for (var i = 0; i < trig[j]["Triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();                    
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][j]["triggers"][i] as JObject).Property("options").Remove();
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][j]["triggers"][i] as JObject).Add("options", tempObj);
                    if (trig[j]["Triggers"][i]["Options"].Value<JObject>("QueryValMatches") is not null)
                    {
                        var trans = Trigger(transformedObject, version, extendedPaths, "query_val_matches");
                        transformedObject = trans;
                    }
                    if (trig[j]["Triggers"][i]["Options"].Value<JObject>("HeaderMatches") is not null)
                    {
                        var trans = Trigger(transformedObject, version, extendedPaths, "header_matches");
                        transformedObject = trans;
                    }
                    if (trig[j]["Triggers"][i]["Options"].Value<JObject>("PathPartMatches") is not null)
                    {
                        var trans = Trigger(transformedObject, version, extendedPaths, "path_part_matches");
                        transformedObject = trans;
                    }
                    if (trig[j]["Triggers"][i]["Options"].Value<JObject>("PayloadMatches") is not null)
                    {
                        var trans = Payload(transformedObject, version, extendedPaths);
                        transformedObject = trans;
                    }
                }
            }
            return transformedObject;

        }

        private static JObject Trigger(JObject transformedObject, JObject version, JObject extendedPaths, string triggerType)
        {
            JArray trig = extendedPaths.Value<JArray>("UrlRewrites");
            for (var j = 0; j < trig.Count(); j++)
            {
                for (var i = 0; i < trig[j]["Triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][j]["triggers"][i]["options"] as JObject).Add(triggerType, tempObj);
                    var trans = Culprit(transformedObject, version, extendedPaths);
                    transformedObject = trans;
                }
            }
            return transformedObject;
        }

        private static JObject Payload(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            JArray trig = extendedPaths.Value<JArray>("UrlRewrites");
            for (var j = 0; j < trig.Count(); j++)
            {
                for (var i = 0; i < trig[j]["Triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("match_rx", trig[j]["Triggers"][i]["Options"]["PayloadMatches"].Value<JObject>("MatchRx"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][j]["triggers"][i]["options"] as JObject).Add("payload_matches", tempObj);
                }
            }           
            return transformedObject;
        }

        private static JObject Culprit(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            JArray trig = extendedPaths.Value<JArray>("UrlRewrites");
            for (var j = 0; j < trig.Count(); j++)
            {
                for (var i = 0; i < trig[j]["Triggers"].Count(); i++)
                {
                    JObject tempObj = new JObject();
                    var key = trig[j]["Triggers"][i]["Options"]["QueryValMatches"]["Culprit"].Value<string>("Key");
                    tempObj.Add("match_rx", trig[j]["Triggers"][i]["Options"]["QueryValMatches"]["Culprit"].Value<string>("MatchRx"));
                    tempObj.Add("reverse", trig[j]["Triggers"][i]["Options"]["QueryValMatches"]["Culprit"].Value<bool>("Reverse"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["url_rewrites"][j]["triggers"][i]["options"]["query_val_matches"] as JObject).Add(key, tempObj);

                }
            }
            return transformedObject;
        }
        private static JObject SetTransform(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            if (extendedPaths["Transform"] is not null)
            {
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform", new JArray());
                foreach (JToken transform in extendedPaths["Transform"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", transform.Value<string>("Method"));
                    tempObj.Add("path", transform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform"] as JArray).Add(tempObj);
                }
                var tempdata = SetTemplateData(transformedObject, version, extendedPaths, false);
                transformedObject = tempdata;
            }
            return transformedObject;
        }
        private static JObject SetTransformResponse(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            if (extendedPaths["TransformResponse"] is not null)
            {
                JObject resObj = new JObject();
                resObj.Add("name", "response_body_transform");
                resObj.Add("options", new JObject());
                (transformedObject["response_processors"] as JArray).Add(resObj);
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform_response", new JArray());
                foreach (JToken transform in extendedPaths["TransformResponse"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", transform.Value<string>("Method"));
                    tempObj.Add("path", transform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform_response"] as JArray).Add(tempObj);
                }
                var tempdata = SetTemplateData(transformedObject, version, extendedPaths, true);
                transformedObject = tempdata;
            }
            return transformedObject;
        }
        private static JObject SetTemplateData(JObject transformedObject, JObject version, JObject extendedPaths, bool isResponse)
        {
            string apiProperty = isResponse ? "transform_response" : "transform";
            string transformedProperty = isResponse ? "TransformResponse" : "Transform";
            JArray transform = extendedPaths.Value<JArray>(transformedProperty);
            var item = 0;
            foreach (JToken templatedata in transform.Values<JObject>("TemplateData"))
            {
                JObject tempObj = new JObject();
                tempObj.Add("enable_session", templatedata.Value<bool>("EnableSession"));
                tempObj.Add("input_type", templatedata.Value<string>("InputType"));
                tempObj.Add("template_mode", templatedata.Value<string>("TemplateMode"));
                tempObj.Add("template_source", templatedata.Value<string>("TemplateSource"));
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"][apiProperty][item] as JObject).Add("template_data", tempObj);
                item++;
            }
            return transformedObject;
        }

        private static JObject SetTransformHeaders(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            if (extendedPaths["TransformHeaders"] is not null)
            {
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform_headers", new JArray());
                foreach (JToken headerTransform in extendedPaths["TransformHeaders"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("act_on", headerTransform.Value<bool>("ActOn"));
                    tempObj.Add("add_headers", headerTransform.Value<JObject>("AddHeaders"));
                    tempObj.Add("delete_headers", headerTransform.Value<JArray>("DeleteHeaders"));
                    tempObj.Add("method", headerTransform.Value<string>("Method"));
                    tempObj.Add("path", headerTransform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform_headers"] as JArray).Add(tempObj);
                }
            }
            return transformedObject;
        }
        private static JObject SetTransformResponseHeaders(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            if (extendedPaths["TransformResponseHeaders"] is not null)
            {
                JObject resObj = new JObject();
                resObj.Add("name", "header_injector");
                resObj.Add("options", new JObject());
                transformedObject.Add("response_processors", new JArray());
                (transformedObject["response_processors"] as JArray).Add(resObj);
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform_response_headers", new JArray());
                foreach (JToken headerResponseTransform in extendedPaths["TransformResponseHeaders"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("act_on", headerResponseTransform.Value<bool>("ActOn"));
                    tempObj.Add("add_headers", headerResponseTransform.Value<JObject>("AddHeaders"));
                    tempObj.Add("delete_headers", headerResponseTransform.Value<JArray>("DeleteHeaders"));
                    tempObj.Add("method", headerResponseTransform.Value<string>("Method"));
                    tempObj.Add("path", headerResponseTransform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform_response_headers"] as JArray).Add(tempObj);
                }
            }
            return transformedObject;
        }
           
        private static JObject SetAddRemoveGlobalHeaders(JObject transformedObject, JObject version)
        {
            if (version.Value<JObject>("GlobalRequestHeaders") is not null)
            {
                JObject globalRequestHeaders = version.Value<JObject>("GlobalRequestHeaders");
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("global_headers", new JObject());
                foreach (var item in globalRequestHeaders)
                {
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["global_headers"] as JObject).Add(item.Key, item.Value);
                }
            }
            if (version.Value<JArray>("GlobalRequestHeadersRemove") is not null)
            {
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Property("global_headers_remove").Remove();
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("global_headers_remove", new JArray());
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["global_headers_remove"] as JArray).Add("Authorization");
                foreach (var item in version.Value<JArray>("GlobalRequestHeadersRemove"))
                {
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["global_headers_remove"] as JArray).Add(item);
                }
            }
            if (version.Value<JArray>("GlobalResponseHeadersRemove") is not null)
            {
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("global_response_headers_remove", new JArray());
                foreach (var item in version.Value<JArray>("GlobalResponseHeadersRemove"))
                {
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["global_response_headers_remove"] as JArray).Add(item);
                }
            }
            if (version.Value<JObject>("GlobalResponseHeaders") is not null)
            {
                JObject globalRequestHeaders = version.Value<JObject>("GlobalResponseHeaders");
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("global_response_headers", new JObject());
                foreach (var item in globalRequestHeaders)
                {
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["global_response_headers"] as JObject).Add(item.Key, item.Value);
                }
            }
            return transformedObject;
        }
    }
}
