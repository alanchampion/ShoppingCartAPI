using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Data.Interface
{
    public interface ICatalog
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetStockedProducts();
        Product? GetProduct(string id);
        int GetSize();
        int GetStockedSize();

        IEnumerable<Product> GetCart();
        Product? GetProductFromCart(string id);
        bool AddProductToCart(string id);
        bool RemoveProductFromCart(string id);
        bool Checkout();
    }
}
