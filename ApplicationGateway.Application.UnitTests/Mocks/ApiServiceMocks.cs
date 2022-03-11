using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Domain.GatewayCommon;
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
            var apis = new List<Domain.GatewayCommon.Api>()
            {
                new Domain.GatewayCommon.Api()
                {
                    ApiId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                    Name =  "Api1",
                    ListenPath = "/testpath/",
                    TargetUrl = "http://localhost:5000",
                    RateLimit = new RateLimit(){Rate=5,Per=20},
                    Blacklist = new List<string> { "192.168.4.5", "125.365.547"},
                    Whitelist = new List<string> { "192.168.4.5", "125.365.547"},
                    VersioningInfo = new VersioningInfo() {Location=VersioningLocation.header,Key="mykey" },
                    DefaultVersion = "Default1",
                    Versions = new List<VersionModel>{ new VersionModel() { Name="versionName",OverrideTarget="overridetarget1"} },
                    AuthType ="open",
                    OpenidOptions = new OpenIdOptions(){Providers = new List<Provider> { new Provider() { Issuer="issuer1",Client_ids=new List<ClientPolicy> { new ClientPolicy() { ClientId="clientid",Policy="policy"} } } }},
                    LoadBalancingTargets = new List<string>{"target1","target2","target3"}

                },
                  new Domain.GatewayCommon.Api()
                {
                    ApiId = Guid.Parse("{d29cd198-03a1-46bc-965e-56d5c6748429}"),
                    Name =  "Api2",
                    ListenPath = "/testpaths/",
                    TargetUrl = "http://localhost:5001",
                    RateLimit = new RateLimit(){Rate=5,Per=20},
                    Blacklist = new List<string> { "192.168.4.6", "125.365.548"},
                    Whitelist = new List<string> { "192.168.4.6", "125.365.548"},
                    VersioningInfo = new VersioningInfo() {Location=VersioningLocation.header,Key="mykey2" },
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

            mockApiService.Setup(repo => repo.CreateApiAsync(It.IsAny<Domain.GatewayCommon.Api>())).ReturnsAsync(
                (Domain.GatewayCommon.Api api) =>
                {
                    api.ApiId = Guid.NewGuid();
                    apis.Add(api);
                    return api;

                });

            mockApiService.Setup(repo => repo.UpdateApiAsync(It.IsAny<Domain.GatewayCommon.Api>())).ReturnsAsync(
                (Domain.GatewayCommon.Api api) =>
                {
                    //api.ApiId = Guid.NewGuid();
                    apis.Add(api);
                    return api;
                });

            mockApiService.Setup(repo => repo.DeleteApiAsync(It.IsAny<Guid>())).Callback(
                (Guid id) =>
                {
                    apis.RemoveAll(a=>a.ApiId==id);

                });

            mockApiService.Setup(repo => repo.CheckUniqueListenPathAsync(It.IsAny<Domain.GatewayCommon.Api>())).ReturnsAsync(
                (Domain.GatewayCommon.Api api)=>
                {
                    var matches = apis.Any(e=>e.ListenPath != api.ListenPath);
                    return matches;
                });

            return mockApiService;



        }
    }
}
