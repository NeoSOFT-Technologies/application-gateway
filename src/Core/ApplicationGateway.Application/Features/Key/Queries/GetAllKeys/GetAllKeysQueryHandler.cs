using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetAllKeys
{
    public class GetAllKeysQueryHandler : IRequestHandler<GetAllKeysQuery, Response<GetAllKeysDto>>
    {
        readonly ILogger<GetAllKeysQueryHandler> _logger;
        readonly IMapper _mapper;
        readonly IKeyService _keyService;

        public GetAllKeysQueryHandler(ILogger<GetAllKeysQueryHandler> logger, IMapper mapper, IKeyService keyService)
        {
            _logger = logger;
            _mapper = mapper;
            _keyService = keyService;
        }

        public async Task<Response<GetAllKeysDto>> Handle(GetAllKeysQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllKeysQueryHandler initiated");
            List<string> listOfKey =await _keyService.GetAllKeysAsync();

            List<AllKeyDto> allKeysDto = new List<AllKeyDto>();
            foreach(var key in listOfKey)
            {
                var keyDto = new AllKeyDto() { AuthType = "Auth Token", Status = "Active", Created = DateTime.Now, KeyId = key };
                allKeysDto.Add(keyDto);
            }

            _logger.LogInformation("GetAllKeysQueryHandler initiated");
            return new Response<GetAllKeysDto>() {Succeeded=true,Data= new GetAllKeysDto() { KeyDto=allKeysDto} };
        }
    }
}
