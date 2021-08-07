using System;
using System.Threading.Tasks;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Result;
using Hash.Products.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Hash.Products.Application.Processors.BuyProducts
{
    public class BuyProductsProcessorWithReceivePercentageDiscount : IBuyProductsProcessor
    {
        private readonly IBuyProductsProcessor _buyProductsProcessor;
        private readonly IProductsService _productsService;
        private readonly BuyProductsDataWorkFlow _buyProductsDataWorkFlow;
        private readonly ILogger _logger;

        public BuyProductsProcessorWithReceivePercentageDiscount(IBuyProductsProcessor buyProductsProcessor,
                                                                IProductsService productsService, 
                                                                BuyProductsDataWorkFlow buyProductsDataWorkFlow,
                                                                ILogger<BuyProductsProcessorWithReceivePercentageDiscount> logger)
        {
            _buyProductsProcessor = buyProductsProcessor ?? throw new ArgumentNullException(nameof(buyProductsProcessor));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));        
            _buyProductsDataWorkFlow = buyProductsDataWorkFlow ?? throw new ArgumentNullException(nameof(buyProductsDataWorkFlow));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult> ProcessAsync(BuyProductsCommand command)
        {
            _logger.LogInformation($"receiving discounts from discount-service, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
            
            foreach (var product in _buyProductsDataWorkFlow.PurchasedProducts)
                await _productsService.ReceiveAndApplyDiscountAsync(product);

            _logger.LogInformation($"successfully received discounts from discount-service, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");

            return await _buyProductsProcessor.ProcessAsync(command);
        }
    }
}