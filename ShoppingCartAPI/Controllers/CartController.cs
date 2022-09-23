using Microsoft.AspNetCore.Mvc;
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
            throw new NotImplementedException();
        }

        [HttpGet("item/{id}")]
        public ActionResult GetProduct([FromRoute] string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("item/{id}")]
        public ActionResult AddProduct([FromRoute] string id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("item/{id}")]
        public ActionResult RemoveProduct([FromRoute] string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("checkout")]
        public ActionResult Checkout()
        {
            throw new NotImplementedException();
        }
    }
}
