using System;
using System.Threading;
using System.Threading.Tasks;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Result;
using MediatR;
using Hash.Products.Application.Processors;

namespace Hash.Products.Application.Handlers
{
    public class ProductsHandler : IRequestHandler<BuyProductsCommand, IResult>
    {
        private readonly IBuyProductsProcessor _buyProductsProcessor;
        public ProductsHandler(IBuyProductsProcessor buyProductsProcessor) => 
            _buyProductsProcessor = buyProductsProcessor ?? throw new ArgumentNullException(nameof(buyProductsProcessor));

        public Task<IResult> Handle(BuyProductsCommand request, 
                                    CancellationToken cancellationToken) => 
            _buyProductsProcessor.ProcessAsync(request);
    }
}