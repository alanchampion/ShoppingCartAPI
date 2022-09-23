using Newtonsoft.Json;

namespace ShoppingCartAPI.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
