using System.Collections.Generic;
using System;
using Hash.Products.Domain.Entities;
using Hash.Products.Domain.Repository;
using static Discount.Discount;
using System.Threading.Tasks;
using Hash.Products.Domain.Result;
using System.Linq;
using Hash.Products.Application.Factories;
using Hash.Products.Domain.Services;
using Discount;
using Hash.Products.Domain.Models;
using Hash.Products.Domain.Commands;
using Microsoft.Extensions.Logging;

namespace Hash.Products.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly DiscountClient _discountClient;
        private readonly IRepository<Product> _repository;
        private readonly ILogger _logger;
        
        public ProductsService(DiscountClient discountClient,
                               IRepository<Product> repository,
                               ILogger<ProductsService> logger)
        {
            _discountClient = discountClient ?? throw new ArgumentNullException(nameof(discountClient));    
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult> BuildPurchaseProductsAsync(IEnumerable<ProductCommand> products)
        {
            if(products.Any(x=> x.Quantity <= 0))
               return ResultFactory.WithError(("zero or negative quantities are not accepted", "ZERO_OR_NEGATIVE_QUANTITIES"));

            var persistedProducts = await _repository.GetAllAsync();

            var nonExistingProductIds = from npp in products
                                      join pp in persistedProducts on npp.Id equals pp.Id into gp
                                      from subpp in gp.DefaultIfEmpty()
                                      where subpp is null
                                      select npp.Id;

            if(nonExistingProductIds.Any())
                return ResultFactory.WithError(($"the current ids: {string.Join(" | ", nonExistingProductIds)}, do not exist in the base", "NON_EXISTENT_PRODUCTS"));

            var purchasedProducts = from pp in persistedProducts
                                    join npp in products on pp.Id equals npp.Id
                                    select PurchasedProduct.Build(pp, npp); 

            return ResultFactory.WithSuccess(purchasedProducts);
        }

        public async Task<IEnumerable<PurchasedProduct>> ApplyGiftAsync(IEnumerable<PurchasedProduct> purchasedProducts)
        {
            var random = new Random();

            var gifts = (await _repository.GetAllAsync()).Where(x=> x.IsGift);
            var gift = gifts.ElementAt(random.Next(0, gifts.Count()));
            var result = new List<PurchasedProduct>();
            result.AddRange(purchasedProducts);
            result.Add(PurchasedProduct.Build(gift, new ProductCommand{ Quantity = 1 }, true));

            return result;
        }

        public async Task<long> ReceiveAndApplyDiscountAsync(PurchasedProduct product)
        {
            GetDiscountResponse response = null; 
            
            try
            {
                response = await _discountClient.GetDiscountAsync(new GetDiscountRequest
                {
                    ProductID = product.Id
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "error when making request for discount-service");
            }
            
            response ??= new GetDiscountResponse
            {
                Percentage = 0
            };

            return product.Discount = (long)(response.Percentage * product.TotalAmount);
        }
    }
}