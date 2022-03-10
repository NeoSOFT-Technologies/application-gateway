using ApplicationGateway.Application.Contracts.Persistence;
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
        public static Mock<IKeyRepository> GetKeyRepository()
        {
            var policies = new List<Domain.Entities.Key>()
            {
                new Domain.Entities.Key()
                {
                    Id = "keyId1",
                    KeyName =  "Key1",
                    IsActive = true,
                    Policies = new List<string> { "policy1","policy2"}
                },
                new Domain.Entities.Key()
                {
                    Id = "keyId2",
                    KeyName =  "Key2",
                    IsActive = true,
                    Policies = new List<string> { "policy1","policy2"}
                },

            };

            var mockKeyRepository = new Mock<IKeyRepository>();

            mockKeyRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(policies);

            return mockKeyRepository;
        }
    }
}
