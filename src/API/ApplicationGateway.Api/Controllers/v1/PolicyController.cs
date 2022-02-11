using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using MediatR;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.TykData;
using Newtonsoft.Json;
using JUST;
using ApplicationGateway.Application.Models.Tyk;
using Microsoft.Extensions.Options;

namespace ApplicationGateway.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PolicyController> _logger;
        private readonly TykConfiguration _tykConfiguration;

        public PolicyController(IMediator mediator, ILogger<PolicyController> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _mediator = mediator;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllPolicies()
        {
            string folderPath = _tykConfiguration.PoliciesFolderPath;
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);

            return Ok(policiesObject.ToString());
        }

        [HttpGet("{policyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPolicyByid(Guid policyId)
        {
            string folderPath = _tykConfiguration.PoliciesFolderPath;
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                return NotFound($"Policy with id: {policyId} was not found");
            }

            string policy = (policiesObject[policyId.ToString()] as JObject).ToString();

            return Ok(policy);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePolicy(CreatePolicyCommand createPolicyCommand)
        {
            _logger.LogInformation("CreatePolicy Initiated with {@CreatePolicyCommand}", createPolicyCommand);
            Response<CreatePolicyDto> response = await _mediator.Send(createPolicyCommand);
            _logger.LogInformation("CreatePolicy Completed");
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdatePolicy(Policy request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string path = Directory.GetCurrentDirectory();
            string transformer = System.IO.File.ReadAllText(path + @"\JsonTransformers\Tyk\PolicyTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = new JObject();
                foreach (var api in inputObject["APIs"])
                {
                    var apiObject = new JObject()
                    {
                        { "api_id", api["Id"] },
                        { "api_name", api["Name"] },
                        { "versions", api["Versions"] },
                        { "allowed_urls", api["AllowedUrls"] },
                        { "limit", api["Limit"] }
                    };
                    (transformedObject["access_rights"] as JObject).Add($"{api["Id"]}", apiObject);
                }
            }

            string folderPath = _tykConfiguration.PoliciesFolderPath;
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            string policyId = request.PolicyId.ToString();
            if (!policiesObject.ContainsKey(policyId))
            {
                return NotFound($"Policy with id: {policyId} was not found");
            }

            policiesObject.Remove(policyId);
            policiesObject.Add(policyId, transformedObject);

            System.IO.File.WriteAllText(folderPath + @"\policies.json", policiesObject.ToString());

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
            }

            return Ok($"Policy with PolicyId: {policyId} updated successfully");
        }

        [HttpDelete("{policyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePolicy(Guid policyId)
        {
            string folderPath = _tykConfiguration.PoliciesFolderPath;
            if (!Directory.Exists(folderPath) || !System.IO.File.Exists(folderPath + @"\policies.json"))
            {
                return NotFound("Policies not found");
            }

            string policiesJson = System.IO.File.ReadAllText(folderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                return NotFound($"Policy with id: {policyId} was not found");
            }

            policiesObject.Remove(policyId.ToString());

            System.IO.File.WriteAllText(folderPath + @"\policies.json", policiesObject.ToString());

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
                HttpResponseMessage httpResponse = httpClient.GetAsync("http://localhost:8080/tyk/reload/group").Result;
            }
            return NoContent();
        }
    }
}
