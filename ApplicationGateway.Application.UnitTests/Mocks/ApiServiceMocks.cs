using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.UnitTests.Mocks
{
    public class ApiServiceMocks
    {
        public static Mock<IApiService> GetApiService()
        {
            var apis = new List<Api>()
            {
                new Api()
                {
                    ApiId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name =  "Api1",
                    ListenPath = "/testpath1/",
                    TargetUrl = "http://localhost:5000",
                    RateLimit = new RateLimit(){Rate=5,Per=20},
                    Blacklist = new List<string> { "192.168.4.5", "125.365.547"},
                    Whitelist = new List<string> { "192.168.4.5", "125.365.547"},
                    VersioningInfo = new VersioningInfo() {Location="mylocation1",Key="mykey" },
                    DefaultVersion = "Default1",
                    Versions = new List<VersionModel>{ new VersionModel() { Name="versionName",OverrideTarget="overridetarget1"} },
                    AuthType ="open",
                    OpenidOptions = new OpenIdOptions(){Providers = new List<Provider> { new Provider() { Issuer="issuer1",Client_ids=new List<ClientPolicy> { new ClientPolicy() { ClientId="clientid",Policy="policy"} } } }},
                    LoadBalancingTargets = new List<string>{"target1","target2","target3"}

                },
                  new Api()
                {
                    ApiId = Guid.Parse("{EE272F8B-6096-49B6-8625-BB4BB2F83E8B}"),
                    Name =  "Api2",
                    ListenPath = "/testpath2/",
                    TargetUrl = "http://localhost:5000",
                    RateLimit = new RateLimit(){Rate=5,Per=20},
                    Blacklist = new List<string> { "192.168.4.6", "125.365.548"},
                    Whitelist = new List<string> { "192.168.4.6", "125.365.548"},
                    VersioningInfo = new VersioningInfo() {Location="mylocation2",Key="mykey2" },
                    DefaultVersion = "Default2",
                    Versions = new List<VersionModel>{ new VersionModel() { Name="versionName2",OverrideTarget="overridetarget2"} },
                    AuthType ="open",
                    OpenidOptions = new OpenIdOptions(){Providers = new List<Provider> { new Provider() { Issuer="issuer2",Client_ids=new List<ClientPolicy> { new ClientPolicy() { ClientId="clientid2",Policy="policy2"} } } }},
                    LoadBalancingTargets = new List<string>{"target4","target5","target6"}

                }
            };

            var mockApiService = new Mock<IApiService>();

            mockApiService.Setup(repo => repo.GetAllApisAsync()).ReturnsAsync(apis);

            mockApiService.Setup(repo => repo.GetApiByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                (Guid ApiId) =>
                {
                    return apis.SingleOrDefault(x => x.ApiId == ApiId);
                });

            mockApiService.Setup(repo => repo.CreateApiAsync(It.IsAny<Api>())).ReturnsAsync(
                (Api api) =>
                {
                    api.ApiId = Guid.NewGuid();
                    apis.Add(api);
                    return api;

                });

            mockApiService.Setup(repo => repo.UpdateApiAsync(It.IsAny<Api>())).ReturnsAsync(
                (Api api) =>
                {
                    api.ApiId = Guid.NewGuid();
                    apis.Add(api);
                    return api;
                });

            mockApiService.Setup(repo => repo.DeleteApiAsync(It.IsAny<Guid>()));

            mockApiService.Setup(repo => repo.CheckUniqueListenPathAsync(It.IsAny<Api>())).ReturnsAsync(
                (Api api)=>
                {
                    var matches = apis.Any(e=>e.ListenPath == api.ListenPath);
                    return matches;
                });

            return mockApiService;



        }
    }
}
