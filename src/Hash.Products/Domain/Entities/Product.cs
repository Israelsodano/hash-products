using Newtonsoft.Json;

namespace Hash.Products.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; } 

        [JsonProperty("is_gift")]
        public bool IsGift { get; set; }
    }
}