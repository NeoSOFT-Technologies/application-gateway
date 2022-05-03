using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQueryHandler : IRequestHandler<GetKeyQuery, Response<GetKeyDto>>
    {
        readonly ILogger<GetKeyQueryHandler> _logger;
        readonly IKeyService _keyService;
        readonly IMapper _mapper;   
        readonly IApiService _apiService;

        public GetKeyQueryHandler(ILogger<GetKeyQueryHandler> logger, IKeyService keyService, IMapper mapper, IApiService apiService)
            
        {
            _logger = logger;
            _keyService = keyService;
            _mapper = mapper;
            _apiService = apiService;
        }

        public async Task<Response<GetKeyDto>> Handle(GetKeyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetKeyQueryHandler initiated for {request}", request);
            Domain.GatewayCommon.Key key = await _keyService.GetKeyAsync(request.keyId);
            GetKeyDto getKeyDto = _mapper.Map<GetKeyDto>(key);
            foreach(var api in getKeyDto.AccessRights)
            {
                List<string> allApiVersions = new();
                Domain.GatewayCommon.Api apiObj = await _apiService.GetApiByIdAsync(api.ApiId);
                apiObj.Versions.ForEach(v => allApiVersions.Add(v.Name)); 
                api.AllApiVersions = allApiVersions;
            }
            Response<GetKeyDto> response = new Response<GetKeyDto> {Succeeded=true, Data = getKeyDto, Message = "Success" };
            _logger.LogInformation("GetKeyQueryHandler completed for {request}", request);
            return response;
        }
    }
}
