using System;
using System.Threading.Tasks;
using Hash.Products.Application.Factories;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Models;
using Hash.Products.Domain.Result;
using Microsoft.Extensions.Logging;

namespace Hash.Products.Application.Processors.BuyProducts
{
    public class BuyProductsProcessorWithGenerateTotals : IBuyProductsProcessor
    {
        private readonly BuyProductsDataWorkFlow _buyProductsDataWorkFlow;
        private readonly ILogger _logger;
        
        public BuyProductsProcessorWithGenerateTotals(BuyProductsDataWorkFlow buyProductsDataWorkFlow,
                                                     ILogger<BuyProductsProcessorWithGenerateTotals> logger)
        {
            _buyProductsDataWorkFlow = buyProductsDataWorkFlow ?? throw new ArgumentNullException(nameof(buyProductsDataWorkFlow));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IResult> ProcessAsync(BuyProductsCommand command)
        {
            _logger.LogInformation($"generating purchase summary, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
            var result = ResultFactory.WithSuccess(PurchaseSummary.Build(_buyProductsDataWorkFlow.PurchasedProducts));
            _logger.LogInformation($"purchase summary generated successfully, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
            
            return Task.FromResult(result);
        }
    }
}