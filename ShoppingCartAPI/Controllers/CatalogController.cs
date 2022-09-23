using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Data.Interface;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalog _catalog;

        public CatalogController(ILogger<CatalogController> logger, ICatalog catalog)
        {
            _logger = logger;
            _catalog = catalog;
        }

        // TODO add middleware to catch errors instead
        // TODO add middleware to add the "success" item into every call
        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _catalog.GetStockedProducts();
                return Ok(new
                {
                    products = products,
                    success = true
                });
            } catch (NotImplementedException)
            {
                return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status501NotImplemented};
            }
        }

        [HttpGet("size")]
        public IActionResult GetSize()
        {
            try
            {
                var size = _catalog.GetStockedSize();

                return Ok(new
                {
                    size = size,
                    success = true
                });
            } catch (NotImplementedException)
            {
                return new ObjectResult(new { success = false }) { StatusCode = StatusCodes.Status501NotImplemented };
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] string id)
        {
            Product? product = _catalog.GetProduct(id);

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
    }
}
