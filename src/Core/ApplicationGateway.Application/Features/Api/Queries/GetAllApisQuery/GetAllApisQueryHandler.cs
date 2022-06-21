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
            IReadOnlyList<Domain.Entities.Api> apiList;
            if (request.sort)
            {
                if (string.IsNullOrWhiteSpace(request.sortParam.param))
                    throw new NotFoundException("param", request.sortParam);
                apiList = await _apiRepository.GetSortedPagedResponseAsync(request.pageNum, request.pageSize, request.sortParam.param, request.sortParam.isDesc);
            }
            else
                apiList = await _apiRepository.GetPagedReponseAsync( request.pageNum, request.pageSize);
            

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