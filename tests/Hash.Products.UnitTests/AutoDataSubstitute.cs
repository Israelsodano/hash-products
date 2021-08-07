using AutoFixture;
using AutoFixture.Xunit2;
using Discount;
using Hash.Products.Application.Extensions;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Threading.Tasks;
using static Discount.Discount;

namespace Hash.Products.UnitTests
{
    public class AutoDataSubstitute : AutoDataAttribute
    {
        public AutoDataSubstitute() : base(GetFixture)
        {

        }

        public static IFixture GetFixture()
        {
            var fixture = new Fixture();
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", 
                            optional: false, 
                            reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging();
            var discountService = Substitute.For<DiscountClient>();
            discountService.GetDiscountAsync(Arg.Any<GetDiscountRequest>())
                                .ReturnsForAnyArgs(
                                    new Grpc.Core.AsyncUnaryCall<GetDiscountResponse>(Task.FromResult(new GetDiscountResponse { Percentage = new Random().Next(0, 1) }), 
                                                                                      Task.FromResult(new Grpc.Core.Metadata { }),
                                                                                      () => Grpc.Core.Status.DefaultSuccess,
                                                                                      () => new Grpc.Core.Metadata { },
                                                                                      () => { }));

            services.ConfigureApplicationServices(configuration);
            services.AddSingleton(discountService);
            var provider = services.BuildServiceProvider();

            fixture.Register(() => provider.GetService<IMediator>());
            fixture.Register(() => provider.GetService<DiscountClient>());
            fixture.Register(() => provider.GetService<BuyProductsDataWorkFlow>());
            fixture.Register(() => new BuyProductsCommand
            {
                Products = new ProductCommand[]
                {
                    new ProductCommand
                    {
                        Id = 1,
                        Quantity = 1
                    },
                    new ProductCommand
                    {
                        Id = 2,
                        Quantity = 1
                    },
                    new ProductCommand
                    {
                        Id = 3,
                        Quantity = 1
                    },
                    new ProductCommand
                    {
                        Id = 4,
                        Quantity = 1
                    }
                }
            });

            return fixture;
        }
    }
}
