using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand
{
    public class CreateKeyCommandHandler:IRequestHandler<CreateKeyCommand,Response<Domain.TykData.Key>>
    {
        readonly IKeyService _keyService;
        readonly IMapper _mapper;
        readonly ILogger<CreateKeyCommandHandler> _logger;
        readonly TykConfiguration _tykConfiguration;
        readonly RestClient<string> _restClient;
        readonly Dictionary<string, string> _headers;

        public CreateKeyCommandHandler(IKeyService keyService, IMapper mapper, ILogger<CreateKeyCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
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

        public async Task<Response<Domain.TykData.Key>> Handle(CreateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateKeyCommandHandler initiated with {request}");
            var keyObj = _mapper.Map<Domain.TykData.Key>(request);
            var key = await _keyService.CreateKeyAsync(keyObj);

            await _restClient.GetAsync(null);

            Response<Domain.TykData.Key> response =new Response<Domain.TykData.Key>(key, "success");
            return response;
        }
    }
}
