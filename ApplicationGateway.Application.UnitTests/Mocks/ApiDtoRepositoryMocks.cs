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
    public class ApiDtoRepositoryMocks
    {
        public static Mock<IApiRepository> GetApiRepository()
        {
            var apis = new List<Domain.Entities.Api>()
            {
                new Domain.Entities.Api()
                {
                    Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name =  "Api1",
                    TargetUrl = "http://localhost:5002",
                    Version = "version1",
                    IsActive = true,
                },
                 new Domain.Entities.Api()
                {
                    Id = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name =  "Api2",
                    TargetUrl = "http://localhost:5003",
                    Version = "version1",
                    IsActive = true,
                }

            };

            var mockApiRepository = new Mock<IApiRepository>();

            mockApiRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(apis);
            mockApiRepository.Setup(repo => repo.AddAsync(It.IsAny<Api>())).ReturnsAsync(
                (Api api) =>
                {
                    api.Id = Guid.NewGuid();
                    apis.Add(api);
                    return api;

                });
            mockApiRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Api>())).Callback(
                (Domain.Entities.Api api) =>
                {
                    apis.Remove(api);

                });

            mockApiRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Api>())).Callback(
               (Domain.Entities.Api api) =>
               {
                   //api.Id = Guid.NewGuid();
                   apis.Add(api);
                   
               });

            return mockApiRepository;

        }
      }

    }
