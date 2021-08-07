using System.Collections.Generic;
using System.Threading.Tasks;
using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Entities;
using Hash.Products.Domain.Models;
using Hash.Products.Domain.Result;

namespace Hash.Products.Domain.Services
{
    public interface IProductsService
    {
        Task<IResult> BuildPurchaseProductsAsync(IEnumerable<ProductCommand> products);
        Task<IEnumerable<PurchasedProduct>> ApplyGiftAsync(IEnumerable<PurchasedProduct> purchasedProducts);
        Task<long> ReceiveAndApplyDiscountAsync(PurchasedProduct product);
    }
}