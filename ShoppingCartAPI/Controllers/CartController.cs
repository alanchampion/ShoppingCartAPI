using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ShoppingCartAPI.Data.Interface;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Controllers
{
    // Endpoints for the cart.
    // TODO add middleware to catch errors instead
    // TODO add middleware to add the "success" item into every call
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICatalog _catalog;

        public CartController(ILogger<CartController> logger, ICatalog catalog)
        {
            _logger = logger;
            _catalog = catalog;
        }

        // Get all products in the cart and the total cost of the items. 
        [HttpGet]
        public ActionResult GetCart()
        {
            var products = _catalog.GetCart();
            int cost = GetCost(products);

            return Ok(new
            {
                products = products,
                success = true,
                totalCost = cost
            });
        }

        // Get one item from the cart using id. 
        [HttpGet("item/{id}")]
        public ActionResult GetProduct([FromRoute] string id)
        {
            Product? product = _catalog.GetProductFromCart(id);

            if (product == null)
            {
                return NotFound(new { success = false });
            }

            return Ok(new
            {
                products = product,
                success = true
            });
        }

        // Add one item to the cart using the id. 
        [HttpPost("item/{id}")]
        public ActionResult AddProduct([FromRoute] string id)
        {
            try
            {
                bool success = _catalog.AddProductToCart(id);
                if (success)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return NotFound(new { success = false });
                }
            } catch (NotImplementedException)
            {
                return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status501NotImplemented };
            }
        }

        // Remove one item from the cart using id. 
        [HttpDelete("item/{id}")]
        public ActionResult RemoveProduct([FromRoute] string id)
        {
            bool success = _catalog.RemoveProductFromCart(id);
            if (success)
            {
                return Ok(new { success = true });
            }
            else
            {
                return NotFound(new { success = false });
            }
        }

        // Returns the current cart and cost of all the items. Clears the cart from all items. 
        [HttpPost("checkout")]
        public ActionResult Checkout()
        {
            try
            {
                // Need to deep copy the cart list since a List =! Database
                var products = _catalog.GetCart().Select(p => new Product() { Id = p.Id, Name = p.Name, Price = p.Price, Quantity = p.Quantity }).ToList();
                int cost = GetCost(products);

                // Clear the cart. 
                _catalog.Checkout();

                return Ok(new
                {
                    products = products,
                    success = true,
                    totalCost = cost
                });
            } catch (NotImplementedException)
            {
                return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status501NotImplemented };
            }
        }

        // Get the total cost from a list of products. 
        private static int GetCost(IEnumerable<Product> products)
        {
            int cost = 0;

            foreach (Product product in products)
            {
                cost += product.Price;
            }

            return cost;
        }
    }
}
