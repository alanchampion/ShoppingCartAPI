using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoppingCartAPI.Data.Interface;
using ShoppingCartAPI.Models;
//using System.Text.Json;
//using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShoppingCartAPI.Data
{
    // In a true environment, this would probably be a database context.
    // Instead, we'll just simplify it here. 
    // I have included sql statements that would be actually doing this. 
    public class Catalog : ICatalog
    {
        // I'm having this be a dictionary just to make it easier to access based on id. 
        private List<Product>? Products{ get; set; }

        public Catalog()
        {
            if (File.Exists("products.json"))
            {
                using (StreamReader r = new StreamReader("products.json"))
                {
                    string json = r.ReadToEnd();
                    JToken? products = JObject.Parse(json)["products"];
                    if (products != null)
                    {
                        Products = JsonConvert.DeserializeObject<List<Product>>(products.ToString()) ?? new List<Product>();
                    }
                    else
                    {
                        Products = new List<Product>();
                    }
                }
            }
            // Values just to test with if wanted. 
            /*Products = new List<Product>
            {
                new Product
                {
                    Id = "001",
                    Name = "item1",
                    Price = 100,
                    Quantity = 1
                },
                new Product
                {
                    Id = "002",
                    Name = "item2",
                    Price = 60,
                    Quantity = 1
                },
                new Product
                {
                    Id = "003",
                    Name = "item3",
                    Price = 210,
                    Quantity = 1
                },
                new Product
                {
                    Id = "004",
                    Name = "item4",
                    Price = 170,
                    Quantity = 0
                }
            };*/
        }

        // SELECT id, name, price, quantity FROM catalog;
        public IEnumerable<Product> GetAllProducts()
        {
            if (Products == null)
            {
                throw new NotImplementedException();
            }
            return Products;
        }

        // SELECT id, name, price, quantity FROM catalog WHERE NOT quantity = 0;
        public IEnumerable<Product> GetStockedProducts()
        {
            if (Products == null)
            {
                throw new NotImplementedException();
            }
            return Products.FindAll(p => { return p.Quantity > 0; });
        }

        // SELECT id, name, price, quantity FROM catalog WHERE id = {id};
        public Product? GetProduct(string id)
        {
            if (Products == null)
            {
                return null;
            }
            return Products.Find(p => { return p.Id == id; });
        }

        // SELECT COUNT(*) FROM catalog WHERE NOT quantity = 0;
        public int GetSize()
        {
            if (Products == null)
            {
                return -1;
            }
            return Products.Count;
        }

        // SELECT COUNT(*) FROM catalog;
        public int GetStockedSize()
        {
            var stockedProducts = GetStockedProducts();

            return stockedProducts.Count();
        }
    }
}
