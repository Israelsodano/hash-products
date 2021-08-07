using Hash.Products.Domain.Commands;
using Hash.Products.Domain.Entities;
using Newtonsoft.Json;

namespace Hash.Products.Domain.Models
{
    public class PurchasedProduct
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [JsonProperty("unit_amount")]
        public long UnitAmount { get; set; }

        [JsonProperty("total_amount")]
        public long TotalAmount => Quantity * UnitAmount;
        public long Discount { get; set; }

        [JsonProperty("is_gift")]
        public bool IsGift { get; set; }

        public static PurchasedProduct Build(Product product, ProductCommand command, bool isGift = false) =>
        product is null || command is null ? null : new PurchasedProduct
        {
            Id = product.Id,
            UnitAmount = isGift ? 0 : product.Amount,
            Quantity = command.Quantity,
            IsGift = isGift
        };
    }
}