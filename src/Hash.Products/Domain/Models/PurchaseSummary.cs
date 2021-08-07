using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Hash.Products.Domain.Models
{
    public class PurchaseSummary
    {
        [JsonProperty("total_amount")]
        public long TotalAmount { get; set; }

        [JsonProperty("total_amount_with_discount")]
        public long TotalAmountWithDiscount { get; set; }

        [JsonProperty("total_discount")]
        public long TotalDiscount { get; set; }
        
        public IEnumerable<PurchasedProduct> Products { get; set; }

        public static PurchaseSummary Build(IEnumerable<PurchasedProduct> purchasedProducts)
        {
            var totalAmount = purchasedProducts.Sum(x=> x.TotalAmount);
            var totalDiscount = purchasedProducts.Sum(x=> x.Discount);
            
            return purchasedProducts is null ? null : new PurchaseSummary
            {
                TotalAmount = totalAmount,
                TotalAmountWithDiscount = totalAmount - totalDiscount,
                TotalDiscount =  totalDiscount,
                Products = purchasedProducts
            };
        }
    }
}