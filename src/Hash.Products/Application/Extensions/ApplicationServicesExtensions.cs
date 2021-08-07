using System.IO;
using Grpc.Core;
using Hash.Products.Application.Processors;
using Hash.Products.Application.Processors.BuyProducts;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Application.Services;
using Hash.Products.Domain.Entities;
using Hash.Products.Domain.Repository;
using Hash.Products.Domain.Services;
using Hash.Products.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Discount.Discount;

namespace Hash.Products.Application.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, 
                                                                      IConfiguration configuration) =>
            services
                .AddMediatR(typeof(ApplicationServicesExtensions).Assembly)
                .AddSingleton<ChannelBase>(x=> 
                new Channel(configuration.GetValue<string>("Target-Discount-Service"), ChannelCredentials.Insecure))
                    .AddScoped<DiscountClient>()
                .AddScoped<BuyProductsDataWorkFlow>()
            .AddSingleton(new FileInfo(configuration.GetValue<string>("Directory-Data-Products")))
                .AddScoped<IRepository<Product>, Repository<Product>>()
                .AddScoped<IProductsService, ProductsService>()
                .AddScoped<IBuyProductsProcessor, BuyProductsProcessorWithGenerateTotals>()
                .Decorate<IBuyProductsProcessor, BuyProductsProcessorWithApplyGift>()
                .Decorate<IBuyProductsProcessor, BuyProductsProcessorWithReceivePercentageDiscount>()
                .Decorate<IBuyProductsProcessor, BuyProductsProcessorWithCheckExistingProducts>()
                .Decorate<IBuyProductsProcessor, BuyProductsProcessorWithError>();
    }
}