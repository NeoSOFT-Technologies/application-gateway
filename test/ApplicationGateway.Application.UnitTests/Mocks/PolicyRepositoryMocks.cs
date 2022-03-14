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
    public class PolicyRepositoryMocks
    {
        public static Mock<IPolicyRepository> GetPolicyRepository()
        {
            var policies = new List<Domain.Entities.Policy>()
            {
                new Domain.Entities.Policy()
                {
                    Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name =  "policy1",
                    Apis = new List<string> { "api1","api2","api3"},
                    AuthType = "open",
                    State = "active"
                },
                 new Domain.Entities.Policy()
                {
                    Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name =  "policy2",
                    Apis = new List<string> { "api3","api4","api5"},
                    AuthType = "open",
                    State = "active"
                }

            };

            var mockPolicyRepository = new Mock<IPolicyRepository>();

            mockPolicyRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(policies);
            mockPolicyRepository.Setup(repo => repo.GetPagedReponseAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(policies);
            mockPolicyRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Policy>())).ReturnsAsync(
               (Domain.Entities.Policy policy) =>
               {
                   policy.Id = Guid.NewGuid();
                   policies.Add(policy);
                   return policy;
               }
               );
            mockPolicyRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Policy>())).Callback(

              (Domain.Entities.Policy policy) =>
              {
                  policies.RemoveAll(x => x.Id == policy.Id);
              }
              );

            return mockPolicyRepository;
        }
    }
}
