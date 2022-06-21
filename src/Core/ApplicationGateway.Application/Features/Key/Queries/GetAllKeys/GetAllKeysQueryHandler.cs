using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
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
    public class GetAllKeysQueryHandler : IRequestHandler<GetAllKeysQuery, PagedResponse<GetAllKeysDto>>
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

        public async Task<PagedResponse<GetAllKeysDto>> Handle(GetAllKeysQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllKeysQueryHandler initiated");
            IReadOnlyList<Domain.Entities.Key> listOfKey;
            if (request.sort)
            {
                if (string.IsNullOrWhiteSpace(request.sortParam.param))
                    throw new NotFoundException("param", request.sortParam);
                listOfKey = await _keyRepository.GetSortedPagedResponseAsync(request.pageNum, request.pageSize, request.sortParam.param, request.sortParam.isDesc);
            }
            else
                listOfKey = await _keyRepository.GetPagedReponseAsync(request.pageNum, request.pageSize);

            int totCount = await _keyRepository.GetTotalCount();

            GetAllKeysDto allKeysDto = new GetAllKeysDto()
            { 
                Keys = _mapper.Map<List<GetAllKeyModel>>(listOfKey)
            };

            PagedResponse<GetAllKeysDto> response = new PagedResponse<GetAllKeysDto>(allKeysDto, totCount, request.pageNum, request.pageSize);
            _logger.LogInformation("GetAllKeysQueryHandler completed");
            return response;
        }
    }
}
