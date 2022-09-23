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
    }
}
