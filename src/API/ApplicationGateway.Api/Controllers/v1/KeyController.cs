using ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand;
using ApplicationGateway.Application.Features.Key.Commands.DeleteKeyCommand;
using ApplicationGateway.Application.Features.Key.Queries.GetKey;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.TykData;
using JUST;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        readonly ILogger<KeyController> _logger;
        readonly IMediator _mediator;

        public KeyController(ILogger<KeyController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<Key>>> GetKey(string getKeyQuery)
        {
            _logger.LogInformation($"GetKey initiated in controller for {getKeyQuery}");
            var response = await _mediator.Send(getKeyQuery);
            _logger.LogInformation($"GetKey completed in controller for {getKeyQuery}");
            return Ok();
            //string key;
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
            //    HttpResponseMessage httpResponse = await httpClient.GetAsync("http://localhost:8080/tyk/keys/" + keyId);
            //    if (!httpResponse.IsSuccessStatusCode)
            //        return NotFound("Failed to get key");
            //    key = await httpResponse.Content.ReadAsStringAsync();
            //}
            //return Ok(key);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateKey(CreateKeyCommand createKeyCommand)
        {
            _logger.LogInformation($"CreateKey initiated in controller for {createKeyCommand}");
            Response<Key> response = await _mediator.Send(createKeyCommand);
            _logger.LogInformation($"CreateKey completed for {createKeyCommand}");
            return Ok(response);
        }


        //[HttpPut]
        //public async Task<ActionResult<string>> UpdateKey(UpdateKeyRequest request)
        //{
        //    string path = Directory.GetCurrentDirectory();
        //    string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\Tyk\UpdateKeyTransformer.json");
        //    string requestString = JsonConvert.SerializeObject(request);

        //    string transformedObj = new JsonTransformer().Transform(transformer, requestString);
        //    JObject jsonObj = JObject.Parse(transformedObj);
        //    if (request.policyId.Any())
        //    {
        //        JArray policies = new JArray();
        //        request.policyId.ForEach(policy => policies.Add(policy));
        //        jsonObj["apply_policies"] = policies;
        //    }
        //    if (request.accessRights.Any())
        //    {
        //        jsonObj["access_rights"] = new JObject();
        //        foreach (var api in request.accessRights)
        //        {
        //            string jsonString = JsonConvert.SerializeObject(api);
        //            JObject obj = JObject.Parse(jsonString);
        //            JArray versions = new JArray();
        //            api.versions.ForEach(v => versions.Add(v));
        //            JObject accObj = new JObject();
        //            accObj.Add("api_id", obj["apiId"]);
        //            accObj.Add("api_name", obj["apiName"]);
        //            accObj.Add("versions", versions);
        //            (jsonObj["access_rights"] as JObject).Add(obj["apiId"].ToString(), accObj);
        //        }
        //    }
        //    string key;
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        StringContent stringContent = new StringContent(jsonObj.ToString(), System.Text.Encoding.UTF8, "application/json");
        //        httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
        //        string url = "http://localhost:8080/tyk/keys/"+request.keyId;
        //        HttpResponseMessage httpResponse = await httpClient.PutAsync(url, stringContent);
        //        //HotReload();
        //        if (!httpResponse.IsSuccessStatusCode)
        //        {
        //            return NotFound();
        //        }
        //        key = await httpResponse.Content.ReadAsStringAsync();
        //    }

        //    return Ok(key);

        //}

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteKey(DeleteKeyCommand deleteKeyCommand)
        {
            _logger.LogInformation($"DeleteKey initiated in controller for {deleteKeyCommand}");
            var response = await _mediator.Send(deleteKeyCommand);
            _logger.LogInformation($"DeleteKey completed in controller for {deleteKeyCommand}");
            return NoContent();
        }

    }
}
