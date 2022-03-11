using ApplicationGateway.Application.Contracts.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.UnitTests.Mocks
{
    public class TransformerRepositoryMocks
    {
        public static Mock<ITransformerRepository> GetTransformerRepository()
        {
            var transformers = new List<Domain.Entities.Transformer>()
            {
                new Domain.Entities.Transformer()
                {
                    TransformerId = Guid.NewGuid(),
                    TemplateName =  "TemplateName1",
                    TransformerTemplate = "TransformerTemplate2",
                    Gateway = new Domain.Entities.Gateway(){},
                },
                new Domain.Entities.Transformer()
                {
                    TransformerId = Guid.NewGuid(),
                    TemplateName =  "TemplateName2",
                    TransformerTemplate = "TransformerTemplate2",
                    Gateway = new Domain.Entities.Gateway(){},
                }
            };
            var mockTransformerRepository = new Mock<ITransformerRepository>();
            mockTransformerRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(transformers);
            mockTransformerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                (Guid id) =>
                {
                    return transformers.SingleOrDefault(x => x.TransformerId == id);
                }
                );
            return mockTransformerRepository;
        }
    }
}
