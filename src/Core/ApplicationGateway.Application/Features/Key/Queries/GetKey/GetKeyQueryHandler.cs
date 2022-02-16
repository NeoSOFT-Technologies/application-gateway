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

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQueryHandler : IRequestHandler<GetKeyQuery, Response<Domain.TykData.Key>>
    {
        readonly ILogger<GetKeyQueryHandler> _logger;
        readonly IMapper _mapper;
        readonly IKeyService _keyService;
        readonly TykConfiguration _tykConfiguration;
        readonly RestClient<string> _restClient;
        readonly Dictionary<string, string> _headers;

        public GetKeyQueryHandler(ILogger<GetKeyQueryHandler> logger, IMapper mapper, IKeyService keyService,        IOptions<TykConfiguration> tykConfiguration)
            
        {
            _logger = logger;
            _mapper = mapper;
            _keyService = keyService;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization",_tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "tyk/reload/group", _headers);
        }

        public async Task<Response<Domain.TykData.Key>> Handle(GetKeyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetKeyQueryHandler initiated for {request}");
            Domain.TykData.Key key = await _keyService.GetKeyAsync(request.keyId);
            Response<Domain.TykData.Key> response = new Response<Domain.TykData.Key> {Succeeded=true, Data = key, Message = "Success" };
            return response;
        }
    }
}
