using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoppingCartAPI.Data.Interface;
using ShoppingCartAPI.Models;
using System.Collections.Generic;
//using System.Text.Json;
//using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShoppingCartAPI.Data
{
    // In a true environment, this would probably be a database context.
    // Instead, we'll just simplify it here. 

    // I have included sql statements that would be actually doing this. 
    // SQL table would be something like catalog(id, name, price, quantity, in_cart)
    public class Catalog : ICatalog
    {
        private List<Product>? Products { get; set; }
        private List<Product> Cart { get; set; }

        public Catalog()
        {
            if (File.Exists("products.json"))
            {
                // Read in the file. 
                using (StreamReader r = new StreamReader("products.json"))
                {
                    string json = r.ReadToEnd();
                    // Parse and grab the products value from the json. 
                    JToken? products = JObject.Parse(json)["products"];
                    if (products != null)
                    {
                        // Deserialize the products. 
                        Products = JsonConvert.DeserializeObject<List<Product>>(products.ToString()) ?? new List<Product>();
                    }
                    else
                    {
                        Products = new List<Product>();
                    }
                }
            }

            Cart = new List<Product>();
        }

        // SELECT id, name, price, quantity FROM catalog;
        public IEnumerable<Product> GetAllProducts()
        {
            // For now just throw an exception to let the controller know that it needs to return a 501 - Not Implemented. 
            if (Products == null)
            {
                throw new NotImplementedException();
            }
            return Products;
        }

        // SELECT id, name, price, quantity FROM catalog WHERE NOT quantity = 0;
        public IEnumerable<Product> GetStockedProducts()
        {
            // For now just throw an exception to let the controller know that it needs to return a 501 - Not Implemented. 
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
            // Just ensure that Products isn't null. Only null if the products.json file doesn't exist. 
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

        // SELECT id, name, price, quantity FROM catalog WHERE in_cart = true;
        public IEnumerable<Product> GetCart()
        {
            return Cart;
        }

        // SELECT id, name, price, quantity FROM catalog WHERE in_cart = true AND id = {id};
        public Product? GetProductFromCart(string id)
        {
            return Cart.Find(p => { return p.Id == id; });
        }

        // UPDATE catalog SET quantity = 0, in_cart = true WHERE id = {id};
        public bool AddProductToCart(string id)
        {
            // Get the product from the catalog. 
            Product? product = GetProduct(id);
            if (product == null || product.Quantity == 0)
            {
                // Return false if the product doesn't exist or we don't have any quantity. 
                return false;
            }

            // If the product is already in the cart, ensure the controller returns a 501 - Not Implemented. 
            if(GetProductFromCart(id) != null)
            {
                throw new NotImplementedException();
            }

            product.Quantity = 0;
            Cart.Add(product);

            return true;
        }

        // UPDATE catalog SET quantity = 1, in_cart = false WHERE id = {id};
        public bool RemoveProductFromCart(string id)
        {
            Product? product = GetProductFromCart(id);
            if (product == null)
            {
                return false;
            }

            product.Quantity = 1;
            Cart.Remove(product);

            return true;
        }

        // // UPDATE catalog SET in_cart = false WHERE id = {id};
        public bool Checkout()
        {
            if (Cart.Count == 0)
            {
                throw new NotImplementedException();
            }

            Cart.Clear();

            return true;
        }
    }
}
