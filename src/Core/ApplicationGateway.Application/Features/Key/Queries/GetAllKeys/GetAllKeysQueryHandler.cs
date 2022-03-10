using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
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
        readonly IKeyRepository _keyRepository;

        public GetAllKeysQueryHandler(IKeyRepository keyDtoRepository, ILogger<GetAllKeysQueryHandler> logger, IMapper mapper)
        {
            _keyRepository = keyDtoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response<GetAllKeysDto>> Handle(GetAllKeysQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllKeysQueryHandler initiated");
            IReadOnlyList<Domain.Entities.Key> listOfKey = await _keyRepository.ListAllAsync();

            GetAllKeysDto allKeysDto = new GetAllKeysDto()
            { 
                Keys = _mapper.Map<List<GetAllKeyModel>>(listOfKey)
            };

            _logger.LogInformation("GetAllKeysQueryHandler initiated");
            return new Response<GetAllKeysDto>() { Succeeded = true, Data = allKeysDto };
        }
    }
}
