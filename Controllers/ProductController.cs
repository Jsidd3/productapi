using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using mycartnow.productapi.Repository;

namespace ProductController.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequest product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var productId = await _productRepository.CreateProductAsync(product);
            product.Id = productId;

            return Ok(product); // Return the product with a 200 OK status.
        }

        [HttpGet]
        public async Task<IActionResult> ListProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
