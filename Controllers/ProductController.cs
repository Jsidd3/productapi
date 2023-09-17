using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using mycartnow.productapi.Repository;
using mycartnow.productapi.Providers;

namespace ProductController.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductProvider _productProvider;

        public ProductController(IProductProvider productProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductRequest product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var productId = await _productProvider.InsertProductAsync(product);
            return Ok(productId);
        }

        [HttpGet]
        [Route("ListOfProducts")]
        public async Task<IActionResult> ListOfProducts()
        {
            var products = await _productProvider.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
