using Discount;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Discount.Discount;

namespace Hash.Products.IntegratedTests
{
    public class ServerFixture
    {
        public HttpClient Client { get; private set; }
        public ServerFixture()
        {
            var server = new TestServer(
                                new WebHostBuilder()
                                    .ConfigureServices(services => {
                                        var configuration = new ConfigurationBuilder()
                                                               .AddJsonFile("appsettings.json",
                                                                           optional: false,
                                                                           reloadOnChange: true)
                                                               .AddEnvironmentVariables()
                                                               .Build();
                                        
                                        services.AddSingleton<IConfiguration>(configuration);
                                    })
                                    .UseStartup<Startup>()
                                    .ConfigureTestServices(ConfigureServices));

            Client = server.CreateClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var discountService = Substitute.For<DiscountClient>();
            discountService.GetDiscountAsync(Arg.Any<GetDiscountRequest>())
                                .ReturnsForAnyArgs(
                                    new Grpc.Core.AsyncUnaryCall<GetDiscountResponse>(Task.FromResult(new GetDiscountResponse { Percentage = new Random().Next(0, 1) }),
                                                                                      Task.FromResult(new Grpc.Core.Metadata { }),
                                                                                      () => Grpc.Core.Status.DefaultSuccess,
                                                                                      () => new Grpc.Core.Metadata { },
                                                                                      () => { }));

            services.AddSingleton(discountService);
        }

    }
}
