using System;
using System.Threading.Tasks;
using Hash.Products.Application.Factories;
using Hash.Products.Application.Processors.Data;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Result;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hash.Products.Application.Processors.BuyProducts
{
    public class BuyProductsProcessorWithError : IBuyProductsProcessor
    {
        private readonly IBuyProductsProcessor _buyProductsProcessor;
        private readonly BuyProductsDataWorkFlow _buyProductsDataWorkFlow;
        private readonly ILogger _logger;

        public BuyProductsProcessorWithError(IBuyProductsProcessor buyProductsProcessor,
                                             BuyProductsDataWorkFlow buyProductsDataWorkFlow,
                                             ILogger<BuyProductsProcessorWithError> logger)
        {
            _buyProductsProcessor = buyProductsProcessor ?? throw new ArgumentNullException(nameof(buyProductsProcessor));        
            _buyProductsDataWorkFlow = buyProductsDataWorkFlow ?? throw new ArgumentNullException(nameof(buyProductsDataWorkFlow));        
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult> ProcessAsync(BuyProductsCommand command)
        {
            try
            {
                _buyProductsDataWorkFlow.StringJsonProductsCommand = JsonConvert.SerializeObject(command.Products);

                _logger.LogInformation($"starting buy-products-flow, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");
                var result = await _buyProductsProcessor.ProcessAsync(command);
                _logger.LogInformation($"buy-products-flow ends successfully, products: {_buyProductsDataWorkFlow.StringJsonProductsCommand}");

                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "buy-products-flow ends with errors.");
                return ResultFactory.WithError((ex.Message, ex.GetType().Name));
            }
        }
    }
}