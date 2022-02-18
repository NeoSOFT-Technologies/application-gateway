using Microsoft.AspNetCore.Mvc;
using ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand;
using MediatR;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Commands.DeletePolicyCommand;
using ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery;
using ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery;

namespace ApplicationGateway.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PolicyController> _logger;

        public PolicyController(IMediator mediator, ILogger<PolicyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllPolicies()
        {
            _logger.LogInformation("GetAllPolicies Initiated");
            Response<List<GetAllPoliciesDto>> response = await _mediator.Send(new GetAllPoliciesQuery());
            _logger.LogInformation("GetAllPolicies Completed");
            return Ok(response);
        }

        [HttpGet("{policyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPolicyByid(Guid policyId)
        {
            _logger.LogInformation("GetPolicyByid Initiated with {@Guid}", policyId);
            Response<GetPolicyByIdDto> response = await _mediator.Send(new GetPolicyByIdQuery() { PolicyId = policyId });
            _logger.LogInformation("GetPolicyByid Completed");
            return Ok(response);
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
        public async Task<ActionResult> UpdatePolicy(UpdatePolicyCommand updatePolicyCommand)
        {
            _logger.LogInformation("UpdatePolicy Initiated with {@UpdatePolicyCommand}", updatePolicyCommand);
            Response<UpdatePolicyDto> response = await _mediator.Send(updatePolicyCommand);
            _logger.LogInformation("UpdatePolicy Completed");
            return Ok(response);
        }

        [HttpDelete("{policyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePolicy(Guid policyId)
        {
            _logger.LogInformation("DeletePolicy Initiated with {@Guid}", policyId);
            await _mediator.Send(new DeletePolicyCommand() { PolicyId = policyId });
            _logger.LogInformation("DeletePolicy Completed");
            return NoContent();
        }
    }
}