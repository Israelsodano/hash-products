using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Entities;
using Hash.Products.Domain.Models;
using Hash.Products.Domain.Result;
using Hash.Products.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Hash.Products.Application.Processors.BuyProducts
{
    public class BuyProductsProcessorWithCheckExistingProducts : IBuyProductsProcessor
    {
        private readonly IBuyProductsProcessor _buyProductsProcessor;
        private readonly IProductsService _productsService;
        private readonly BuyProductsDataWorkFlow _buyProductsDataWorkFlow;
        private readonly ILogger _logger;

        public BuyProductsProcessorWithCheckExistingProducts(IBuyProductsProcessor buyProductsProcessor,
                                                             IProductsService productsService, 
                                                             BuyProductsDataWorkFlow buyProductsDataWorkFlow,
                                                             ILogger<BuyProductsProcessorWithCheckExistingProducts> logger)
        {
            _buyProductsProcessor = buyProductsProcessor ?? throw new ArgumentNullException(nameof(buyProductsProcessor));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));        
            _buyProductsDataWorkFlow = buyProductsDataWorkFlow ?? throw new ArgumentNullException(nameof(buyProductsDataWorkFlow));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult> ProcessAsync(BuyProductsCommand command)
        {
            _logger.LogInformation($"receiving persisted products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
            var result = await _productsService.BuildPurchaseProductsAsync(command.Products);

            if(!result.IsSuccess)
                return result;

            _logger.LogInformation($"successfully received products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
            _buyProductsDataWorkFlow.PurchasedProducts = (IEnumerable<PurchasedProduct>)result.Value;

            return await _buyProductsProcessor.ProcessAsync(command);
        }
    }
}