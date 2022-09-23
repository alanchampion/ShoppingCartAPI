using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ShoppingCartAPI.Data.Interface;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Controllers
{
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

        // TODO add middleware to catch errors instead
        // TODO add middleware to add the "success" item into every call
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

        [HttpPost("checkout")]
        public ActionResult Checkout()
        {
            try
            {
                // Need to deep copy the cart list since a List =! Database
                var products = _catalog.GetCart().Select(p => new Product() { Id = p.Id, Name = p.Name, Price = p.Price, Quantity = p.Quantity }).ToList();
                int cost = GetCost(products);

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
