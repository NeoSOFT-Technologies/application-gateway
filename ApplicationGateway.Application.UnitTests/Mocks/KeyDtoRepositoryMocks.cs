using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.UnitTests.Mocks
{
    public class KeyDtoRepositoryMocks
    {
        public static Mock<IKeyDtoRepository> GetKeyRepository()
        {
            var policies = new List<KeyDto>()
            {
                new KeyDto()
                {
                    Id = "keyId1",
                    KeyName =  "Key1",
                    IsActive = true,
                    Policies = new List<string> { "policy1","policy2"}
                },
                new KeyDto()
                {
                    Id = "keyId2",
                    KeyName =  "Key2",
                    IsActive = true,
                    Policies = new List<string> { "policy1","policy2"}
                },

            };

            var mockKeyRepository = new Mock<IKeyDtoRepository>();

            mockKeyRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(policies);

            return mockKeyRepository;
        }
    }
}
