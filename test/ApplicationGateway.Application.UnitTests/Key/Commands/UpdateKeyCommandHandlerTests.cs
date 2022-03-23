﻿using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand;
using ApplicationGateway.Application.Profiles;
using ApplicationGateway.Application.UnitTests.Mocks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationGateway.Application.UnitTests.Key.Commands
{
    public class UpdateKeyCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ISnapshotService> _snapshotService;
        private readonly Mock<ILogger<UpdateKeyCommandHandler>> _mockLogger;
        private readonly Mock<IKeyRepository> _mockKeyRepository;
        private readonly Mock<IKeyService> _mockKeyService;

        public UpdateKeyCommandHandlerTests()
        {
            _mockKeyRepository = KeyRepositoryMocks.GetKeyRepository();
            _mockKeyService = KeyServiceMocks.GetKeyService();
            _snapshotService = new Mock<ISnapshotService>();
            _mockLogger = new Mock<ILogger<UpdateKeyCommandHandler>>();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_Updated_Key()
        {
            var handler = new UpdateKeyCommandHandler(_mockKeyRepository.Object, _mockKeyService.Object, _mapper, _mockLogger.Object, _snapshotService.Object);
            var KeyId = _mockKeyRepository.Object.ListAllAsync().Result.FirstOrDefault().Id;
            await handler.Handle(new UpdateKeyCommand()
            {
                KeyId = KeyId,
                KeyName = "newUpdatedKeyName",
                Rate = 20,
                Per = 20,
                Quota = 20,
                QuotaRenewalRate = 20,
                ThrottleInterval = 20,
                ThrottleRetries = 20,
                Expires = 20,
                IsInActive = false,
                AccessRights = new List<UpdateKeyAccessRightsModel>
                            {
                                new UpdateKeyAccessRightsModel()
                                    { ApiId= Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                                      ApiName="UpdatedKeyapiName",
                                      Versions = new List<string> { "version1","version2"},
                                      AllowedUrls = new List<UpdateKeyAllowedUrl>{new UpdateKeyAllowedUrl() { Url = "url", Methods = new List<string> { "method1", "method2" } } },
                                      Limit = new UpdateKeyLimit(){Rate=10,Per=10,Throttle_interval=10,Throttle_retry_limit=10,Max_query_depth=10,Quota_max=10,Quota_renews = 10,Quota_remaining =10,Quota_renewal_rate=10}
                                     }
                            },
                Policies = new List<string> { "policy4", "policy10" }
            }, CancellationToken.None);
            var allKeys = await _mockKeyRepository.Object.ListAllAsync();
            allKeys[0].Policies[0].ShouldBeEquivalentTo("policy4");
            allKeys[0].Policies[1].ShouldBeEquivalentTo("policy10");
            allKeys.Count.ShouldBe(2);
        }
    }
}