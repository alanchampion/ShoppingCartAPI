using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Data.Interface
{
    public interface ICatalog
    {
        // Get all products from the catalog, including those not in stock. 
        IEnumerable<Product> GetAllProducts();
        // Get products from the catalog. Only includes those that are in stock. 
        IEnumerable<Product> GetStockedProducts();
        // Get a product from the catalog by id. 
        Product? GetProduct(string id);
        // Get the size of the entire catalog. 
        int GetSize();
        // Get the size of the stocked catalog. 
        int GetStockedSize();

        // Get all the products from the cart.
        IEnumerable<Product> GetCart();
        // Get a product from the cart by id. 
        Product? GetProductFromCart(string id);
        // Add a product to the cart by id. 
        // Returns true if successful and false if not. 
        bool AddProductToCart(string id);
        // Remove a product from the cart by id. 
        // Returns true if successful and false if not. 
        bool RemoveProductFromCart(string id);
        // Clears the cart and does any sort of cleanup needed when user checks out. 
        // Returns true if successful and false if the cart is empty. 
        bool Checkout();
    }
}
