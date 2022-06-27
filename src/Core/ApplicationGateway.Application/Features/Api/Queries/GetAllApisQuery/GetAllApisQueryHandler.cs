using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisQueryHandler : IRequestHandler<GetAllApisQuery, PagedResponse<GetAllApisDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllApisQueryHandler> _logger;
        private readonly IApiRepository _apiRepository;

        public GetAllApisQueryHandler(IApiRepository apiDtoRepository, IMapper mapper, ILogger<GetAllApisQueryHandler> logger)
        {
            _apiRepository = apiDtoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<GetAllApisDto>> Handle(GetAllApisQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            IEnumerable<Domain.Entities.Api> apiList;
            if(!string.IsNullOrWhiteSpace(request.sortParam.param) && !string.IsNullOrWhiteSpace(request.searchParam.name) && !string.IsNullOrWhiteSpace(request.searchParam.value))
            {
                apiList = await _apiRepository.GetSearchedResponseAsync(page: request.pageNum, size: request.pageSize, col: request.searchParam.name, value: request.searchParam.value, sortParam: request.sortParam.param, isDesc: request.sortParam.isDesc);
            }
            else if (!string.IsNullOrWhiteSpace(request.sortParam.param))
            {
                apiList = await _apiRepository.GetPagedListAsync(page: request.pageNum, size: request.pageSize, sortParam: request.sortParam.param, isDesc: request.sortParam.isDesc);
            }
            else if (!string.IsNullOrWhiteSpace(request.searchParam.name))
            {
                if (string.IsNullOrWhiteSpace(request.searchParam.value))
                    throw new NotFoundException("value param", request.searchParam);
                apiList = await _apiRepository.GetSearchedResponseAsync(page: request.pageNum, size: request.pageSize, col: request.searchParam.name, value: request.searchParam.value);
            }
            else
                apiList = await _apiRepository.GetPagedListAsync( request.pageNum, request.pageSize);


                int totCount = await _apiRepository.GetTotalCount();
            GetAllApisDto getAllApisDto = new GetAllApisDto()
            {
                Apis = _mapper.Map<List<GetAllApiModel>>(apiList)
            };

            PagedResponse<GetAllApisDto> response = new PagedResponse<GetAllApisDto>(getAllApisDto,totCount,request.pageNum,request.pageSize);
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}