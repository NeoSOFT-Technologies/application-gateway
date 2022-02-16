using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand
{
    public class UpdateKeyCommandHandler:IRequestHandler<UpdateKeyCommand,Response<UpdateKeyCommandDto>>
    {
        readonly IKeyService _keyService;
        readonly IMapper _mapper;
        readonly ILogger<UpdateKeyCommandHandler> _logger;
        readonly TykConfiguration _tykConfiguration;
        readonly RestClient<string> _restClient;
        readonly Dictionary<string, string> _headers;

        public UpdateKeyCommandHandler(IKeyService keyService, IMapper mapper, ILogger<UpdateKeyCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization",_tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "tyk/reload/group", _headers);
        }

        public async Task<Response<UpdateKeyCommandDto>> Handle(UpdateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateKeyHandler initiated for {request}");
            RestClient<string> restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/keys", _headers);
            string responseKey = await restClient.GetAsync(request.KeyId.ToString());
            JObject responseObject = JObject.Parse(responseKey);
            if (responseObject["message"] is not null && responseObject["message"].ToString() == "Key not found")
                throw new NotFoundException("Key", request.KeyId);

            Domain.TykData.Key key = await _keyService.UpdateKeyAsync(_mapper.Map<Domain.TykData.Key>(request));

            await _restClient.GetAsync(null);

            UpdateKeyCommandDto updateKeyCommandDto = _mapper.Map<UpdateKeyCommandDto>(key);
            Response<UpdateKeyCommandDto> response = new Response<UpdateKeyCommandDto>() {Succeeded=true,Data=updateKeyCommandDto,Message="success" };
            _logger.LogInformation($"UpdateKeyHandler completed for {request}");
            return response;
        }
    }
}
