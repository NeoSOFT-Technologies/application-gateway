﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand;
using ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand;
using ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand;
using ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery;
using ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery;

namespace ApplicationGateway.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApplicationGatewayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationGatewayController> _logger;

        public ApplicationGatewayController(IMediator mediator, ILogger<ApplicationGatewayController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllApis(int pageNum, int pageSize)
        {
            _logger.LogInformation("GetAllApis Initiated");
            PagedResponse<GetAllApisDto> response = await _mediator.Send(new GetAllApisQuery() {pageNum = pageNum, pageSize = pageSize });
            _logger.LogInformation("GetAllApis Completed");
            return Ok(response);
        }

        [HttpGet("{apiId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetApiById(Guid apiId)
        {
            _logger.LogInformation("GetApiById Initiated with {@Guid}", apiId);
            Response<GetApiByIdDto> response = await _mediator.Send(new GetApiByIdQuery() { ApiId = apiId });
            _logger.LogInformation("GetApiById Completed: {@Response<GetApiByIdDto>}", response);
            return Ok(response);
        } 

        [HttpPost("CreateApi")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateApi(CreateApiCommand createApiCommand)
        {
            _logger.LogInformation("CreateApi Initiated with {@CreateApiCommand}", createApiCommand);
            Response<CreateApiDto> response = await _mediator.Send(createApiCommand);
            _logger.LogInformation("CreateApi CompletedGetApiByIdDto: {@Response<CreateApiDto>}", response);
            return Ok(response);

        }

        [HttpPost("CreateMultipleApis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateMultipleApis(CreateMultipleApisCommand createMultipleApisCommand)
        {
            _logger.LogInformation("CreateMultipleApis Initiated with {@CreateMultipleApisCommand}", createMultipleApisCommand);
            Response<CreateMultipleApisDto> response = await _mediator.Send(createMultipleApisCommand);
            _logger.LogInformation("CreateMultipleApis Completed: {@Response<CreateMultipleApisDto>}", response);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateApi(UpdateApiCommand updateApiCommand)
        {
            _logger.LogInformation("UpdateApi Initiated with {@updateApiCommand}", updateApiCommand);
            Response<UpdateApiDto> response = await _mediator.Send(updateApiCommand);
            _logger.LogInformation("UpdateApi Completed: {@Response<UpdateApiDto>}", response);
            return Ok(response);
        }

        [HttpDelete("{apiId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteApi(Guid apiId)
        {
            _logger.LogInformation("DeleteApi Initiated with {@Guid}", apiId);
            await _mediator.Send(new DeleteApiCommand() { ApiId = apiId });
            _logger.LogInformation("DeleteApi Completed: {@Guid}", apiId);
            return NoContent();
        }
    }
}