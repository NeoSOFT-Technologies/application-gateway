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
    public class KeyRepositoryMocks
    {
        public static Mock<IKeyRepository> GetKeyRepository()
        {
            var keys = new List<Domain.Entities.Key>()
            {
                new Domain.Entities.Key()
                {
                    Id = "EE272F8B-6096-4CB6-8625-BB4BB2D89E8B",
                    KeyName =  "Key1",
                    IsActive = true,
                    Policies = new List<string> { "policy1","policy2"}
                },
                new Domain.Entities.Key()
                {
                    Id = "d917933a-becc-4beb-83c5-3364dd19fd44",
                    KeyName =  "Key2",
                    IsActive = true,
                    Policies = new List<string> { "policy1","policy2"}
                },

            };

            var mockKeyRepository = new Mock<IKeyRepository>();

            mockKeyRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(keys);
            mockKeyRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
               (Guid keyId) =>
               {
                   return keys.SingleOrDefault(x => x.Id == keyId.ToString());
               });

            mockKeyRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Key>())).ReturnsAsync(
                (Domain.Entities.Key key) =>
                {
                    key.Id = "keyId";
                    keys.Add(key);
                    return key;

                }
               );
            mockKeyRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Key>())).Callback(

                (Domain.Entities.Key key) =>
                {
                    keys.Remove(key);
                }
                );

            return mockKeyRepository;
        }
    }
}
