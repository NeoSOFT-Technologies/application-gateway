using AutoMapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Categories.Queries.GetCategoriesList;
using ApplicationGateway.Application.Profiles;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.Application.UnitTests.Categories.Queries
{
    public class GetCategoriesListQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<GetCategoriesListQueryHandler>> _logger;
         
        public GetCategoriesListQueryHandlerTests()
        {
            _mockCategoryRepository = CategoryRepositoryMocks.GetCategoryRepository();
            _logger = new Mock<ILogger<GetCategoriesListQueryHandler>>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GetCategoriesListTest()
        {
            var handler = new GetCategoriesListQueryHandler(_mapper, _mockCategoryRepository.Object, _logger.Object);

            var result = await handler.Handle(new GetCategoriesListQuery(), CancellationToken.None);

            result.ShouldBeOfType<Response<IEnumerable<CategoryListVm>>>();
            result.Data.ShouldNotBeEmpty();
        }
    }
}
