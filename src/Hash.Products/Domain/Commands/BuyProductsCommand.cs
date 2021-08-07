using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using Hash.Products.Domain.Entities;

namespace Hash.Products.Domain.Commands
{
    public class BuyProductsCommand : BaseCommand
    {
        [JsonIgnore]
        public override HttpStatusCode DefaultSuccesResponse => HttpStatusCode.OK;
        public IEnumerable<ProductCommand> Products { get; set; }
    }

    public class ProductCommand
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public static implicit operator Product(ProductCommand command) =>
            command is null ? null : new Product
            {
                Id = command.Id
            };
    }
}