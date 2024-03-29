﻿using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.UnitTests.Mocks
{
    public class ApiRepositoryMocks
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
            mockApiRepository.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Api>())).ReturnsAsync(
                (Domain.Entities.Api api) =>
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
                   apis[0].Id = api.Id;
                   apis[0].Name = api.Name;
                   apis[0].TargetUrl = api.TargetUrl;
                   apis[0].Version = api.Version;
                   apis[0].IsActive = api.IsActive;
               });
            mockApiRepository.Setup(repo => repo.GetPagedReponseAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(apis);

            return mockApiRepository;

        }
      }

    }
