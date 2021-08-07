using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Models;
using MediatR;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Discount.Discount;

namespace Hash.Products.UnitTests
{
    public class ProductRulesTests
    {
        [Theory]
        [AutoDataSubstitute]
        public async Task Should_Be_Success_When_Quantity_Greater_Then_Zero(IMediator mediator, 
                                                                            BuyProductsCommand command)
        {
            foreach (var product in command.Products)
                product.Quantity = 1;

            var result = await mediator.Send(command);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [AutoDataSubstitute]
        public async Task Should_Be_Error_When_Quantity_Less_Then_Zero(IMediator mediator,
                                                               BuyProductsCommand command)
        {
            foreach (var product in command.Products)
                product.Quantity = 0;

            var result = await mediator.Send(command);
            Assert.True(!result.IsSuccess);
        }

        [Theory]
        [AutoDataSubstitute]
        public async Task Should_Be_Success_When_Id_Exists_In_Database(IMediator mediator,
                                                                       BuyProductsCommand command)
        {
            var result = await mediator.Send(command);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [AutoDataSubstitute]
        public async Task Should_Be_Error_When_Id_Not_Exists_In_Database(IMediator mediator,
                                                                         BuyProductsCommand command)
        {
            foreach (var product in command.Products)
                product.Id += 100;

            var result = await mediator.Send(command);
            Assert.True(!result.IsSuccess);
        }

        [Theory]
        [AutoDataSubstitute]
        public async Task Should_Be_Success_When_Service_Discount_Requested(IMediator mediator,
                                                                            DiscountClient discountClient,
                                                                            BuyProductsCommand command)
        {
            await mediator.Send(command);
            var calls = discountClient.ReceivedCalls();
            Assert.True(calls.Any());
        }

        [Theory]
        [AutoDataSubstitute]
        public async Task Should_Be_Success_When_Only_One_Gift_Is_Applied(IMediator mediator,
                                                                          BuyProductsCommand command,
                                                                          BuyProductsDataWorkFlow buyProductsDataWorkFlow)
        {
            buyProductsDataWorkFlow.BlackFridayDate = DateTime.Now;
            var result = await mediator.Send(command);
            Assert.True(result.IsSuccess);
            Assert.True(((PurchaseSummary)result.Value).Products.Count() -1 == command.Products.Count());
        }
    }
}
