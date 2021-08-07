using System;
using System.Threading.Tasks;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Result;
using Hash.Products.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Hash.Products.Application.Processors.BuyProducts
{
    public class BuyProductsProcessorWithApplyGift : IBuyProductsProcessor
    {

        private readonly IBuyProductsProcessor _buyProductsProcessor;
        private readonly IProductsService _productsService;
        private readonly BuyProductsDataWorkFlow _buyProductsDataWorkFlow;
        private readonly ILogger _logger;

        public BuyProductsProcessorWithApplyGift(IBuyProductsProcessor buyProductsProcessor,
                                                IProductsService productsService, 
                                                BuyProductsDataWorkFlow buyProductsDataWorkFlow,
                                                ILogger<BuyProductsProcessorWithApplyGift> logger)
        {
            _buyProductsProcessor = buyProductsProcessor ?? throw new ArgumentNullException(nameof(buyProductsProcessor));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));        
            _buyProductsDataWorkFlow = buyProductsDataWorkFlow ?? throw new ArgumentNullException(nameof(buyProductsDataWorkFlow));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult> ProcessAsync(BuyProductsCommand command)
        {
            _logger.LogInformation($"checking if it's black friday, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
            
            if(_buyProductsDataWorkFlow.BlackFridayDate is not null && 
               _buyProductsDataWorkFlow.BlackFridayDate.Value.Date == DateTime.Now.Date)
            {
                _logger.LogInformation($"the date is black friday, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
                _buyProductsDataWorkFlow.PurchasedProducts = await _productsService.ApplyGiftAsync(_buyProductsDataWorkFlow.PurchasedProducts);
            }

            return await _buyProductsProcessor.ProcessAsync(command);
        }
    }
}