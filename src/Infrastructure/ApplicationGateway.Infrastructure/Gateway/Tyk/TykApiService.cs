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
            GetTransformHeaders(apiObject, extendedPaths);
            GetTransformResponseHeaders(apiObject, extendedPaths);
            GetTransform(apiObject, extendedPaths);
            GetTransformResponse(apiObject, extendedPaths);
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
                GetTemplateData(apiObject, extendedPaths);

            }
            return apiObject;
        }
        private static JObject GetTemplateData(JObject apiObject, JObject extendedPaths)
        {
            JArray transform = extendedPaths.Value<JArray>("transform");
            foreach (JToken templatedata in transform.Values<JObject>("template_data"))
            {
                JObject tempObj = new JObject();
                tempObj.Add("enableSession", templatedata.Value<bool>("enable_session"));              
                tempObj.Add("inputType", templatedata.Value<string>("input_type"));
                tempObj.Add("templateMode", templatedata.Value<string>("template_mode"));
                tempObj.Add("templateSource", templatedata.Value<string>("template_source"));
                (apiObject["extendedPaths"]["transform"][0] as JObject).Add("templateData", tempObj);
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
                GetResponseTemplateData(apiObject, extendedPaths);

            }
            return apiObject;
        }
        private static JObject GetResponseTemplateData(JObject apiObject, JObject extendedPaths)
        {
            JArray transform = extendedPaths.Value<JArray>("transform_response");
            foreach (JToken templatedata in transform.Values<JObject>("template_data"))
            {
                JObject tempObj = new JObject();
                tempObj.Add("enableSession", templatedata.Value<bool>("enable_session"));
                tempObj.Add("inputType", templatedata.Value<string>("input_type"));
                tempObj.Add("templateMode", templatedata.Value<string>("template_mode"));
                tempObj.Add("templateSource", templatedata.Value<string>("template_source"));
                (apiObject["extendedPaths"]["transformResponse"][0] as JObject).Add("templateData", tempObj);
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
            SetTransformHeaders(transformedObject, version, extendedPaths);
            SetTransformResponseHeaders(transformedObject, version, extendedPaths);
            SetTransform(transformedObject, version, extendedPaths);
            SetTransformResponse(transformedObject, version, extendedPaths);
            (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Property("ExtendedPaths").Remove();
            return transformedObject;
        }
        private static JObject SetTransform(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            if (extendedPaths["Transform"] is not null)
            {
                foreach (JToken transform in extendedPaths["Transform"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", transform.Value<string>("Method"));
                    tempObj.Add("path", transform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform", new JArray());
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform"] as JArray).Add(tempObj);
                    var tempdata = SetTemplateData(transformedObject, version, extendedPaths);
                    transformedObject = tempdata;
                }
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
                foreach (JToken transform in extendedPaths["TransformResponse"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("method", transform.Value<string>("Method"));
                    tempObj.Add("path", transform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform_response", new JArray());
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform_response"] as JArray).Add(tempObj);
                    var tempdata = SetResponseTemplateData(transformedObject, version, extendedPaths);
                    transformedObject = tempdata;
                }
            }
            return transformedObject;
        }
        private static JObject SetTemplateData(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            JArray transform = extendedPaths.Value<JArray>("Transform");
            foreach (JToken templatedata in transform.Values<JObject>("TemplateData"))
            {
                JObject tempObj = new JObject();
                tempObj.Add("enable_session", templatedata.Value<bool>("EnableSession"));
                tempObj.Add("input_type", templatedata.Value<string>("InputType"));
                tempObj.Add("template_mode", templatedata.Value<string>("TemplateMode"));
                tempObj.Add("template_source", templatedata.Value<string>("TemplateSource"));
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform"][0] as JObject).Add("template_data", tempObj);
            }
            return transformedObject;
        }
        private static JObject SetResponseTemplateData(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            JArray transform = extendedPaths.Value<JArray>("TransformResponse");
            foreach (JToken templatedata in transform.Values<JObject>("TemplateData"))
            {
                JObject tempObj = new JObject();
                tempObj.Add("enable_session", templatedata.Value<bool>("EnableSession"));
                tempObj.Add("input_type", templatedata.Value<string>("InputType"));
                tempObj.Add("template_mode", templatedata.Value<string>("TemplateMode"));
                tempObj.Add("template_source", templatedata.Value<string>("TemplateSource"));
                (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"]["transform_response"][0] as JObject).Add("template_data", tempObj);
            }
            return transformedObject;
        }
       
        private static JObject SetTransformHeaders(JObject transformedObject, JObject version, JObject extendedPaths)
        {
            if (extendedPaths["TransformHeaders"] is not null)
            {
                foreach (JToken headerTransform in extendedPaths["TransformHeaders"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("act_on", headerTransform.Value<bool>("ActOn"));
                    tempObj.Add("add_headers", headerTransform.Value<JObject>("AddHeaders"));
                    tempObj.Add("delete_headers", headerTransform.Value<JArray>("DeleteHeaders"));
                    tempObj.Add("method", headerTransform.Value<string>("Method"));
                    tempObj.Add("path", headerTransform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform_headers", new JArray());
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
                transformedObject.Add("response_processors", new JArray());
                (transformedObject["response_processors"] as JArray).Add(resObj);
                foreach (JToken headerResponseTransform in extendedPaths["TransformResponseHeaders"])
                {
                    JObject tempObj = new JObject();
                    tempObj.Add("act_on", headerResponseTransform.Value<bool>("ActOn"));
                    tempObj.Add("add_headers", headerResponseTransform.Value<JObject>("AddHeaders"));
                    tempObj.Add("delete_headers", headerResponseTransform.Value<JArray>("DeleteHeaders"));
                    tempObj.Add("method", headerResponseTransform.Value<string>("Method"));
                    tempObj.Add("path", headerResponseTransform.Value<string>("Path"));
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"]["extended_paths"] as JObject).Add("transform_response_headers", new JArray());
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