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
            IEnumerable<Domain.Entities.Key> listOfKey;
            if (!string.IsNullOrWhiteSpace(request.sortParam.param) && !string.IsNullOrWhiteSpace(request.searchParam.name) && !string.IsNullOrWhiteSpace(request.searchParam.value))
            {
                listOfKey = await _keyRepository.GetSearchedResponseAsync(page: request.pageNum, size: request.pageSize, col: request.searchParam.name, value: request.searchParam.value, sortParam: request.sortParam.param, isDesc: request.sortParam.isDesc);
            }
            else if (!string.IsNullOrWhiteSpace(request.sortParam.param))
            {
                listOfKey = await _keyRepository.GetPagedListAsync(page: request.pageNum, size: request.pageSize, sortParam: request.sortParam.param, isDesc: request.sortParam.isDesc);
            }
            else if (!string.IsNullOrWhiteSpace(request.searchParam.name))
            {
                if (string.IsNullOrWhiteSpace(request.searchParam.value))
                    throw new NotFoundException("value param", request.searchParam);
                listOfKey = await _keyRepository.GetSearchedResponseAsync(page: request.pageNum, size: request.pageSize, col: request.searchParam.name, value: request.searchParam.value);
            }
            else
                listOfKey = await _keyRepository.GetPagedListAsync(request.pageNum, request.pageSize);

            int totCount = listOfKey.Count();

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
