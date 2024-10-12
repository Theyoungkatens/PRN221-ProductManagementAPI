using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP.ProductManagement.Service.BusinessModels;
using SWP.ProductManagement.Service.Service;
using SWP.ProductManagent.API.RequestModel;
using SWP.ProductManagent.API.ResponseModel;

namespace SWP.ProductManagent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<ProductResponseModel>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();

            // Map the ProductModel to ProductResponseModel
            var response = products.Select(product => new ProductResponseModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitsInStock = product.UnitsInStock,
                UnitPrice = product.UnitPrice,
                CategoryId = product.CategoryId // Add this line if you need to return CategoryId as well
            });

            return Ok(response);
        }
        [HttpGet("product/{id}")]
        public async Task<ActionResult<ProductResponseModel>> GetProductById(int id)
        {
            // Retrieve the product by ID from the service
            var product = await _productService.getProductByIdAsync(id);

            // Check if the product exists
            if (product == null)
            {
                return NotFound(); // Return 404 if the product is not found
            }

            // Map the ProductModel to ProductResponseModel
            var response = new ProductResponseModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitsInStock = product.UnitsInStock,
                UnitPrice = product.UnitPrice,
                CategoryId = product.CategoryId// Ensure this is included if needed
            };

            return Ok(response); // Return the product response
        }
        [HttpPost("product")]
        public async Task<ActionResult<int>> InsertProduct([FromBody] ProductRequestModel request)
        {
            var productModel = new ProductModel
            {
                ProductName = request.ProductName,
                UnitsInStock = request.UnitsInStock,
                UnitPrice = request.UnitPrice,
                CategoryId = request.CategoryId
            };

            var productId = await _productService.InsertProductAsync(productModel);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }
        [HttpPut("product/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductRequestModel request)
        {
            var exists = await _productService.ProductExistAsync(id);
            if (!exists) return NotFound();

            var productModel = new ProductModel
            {
                ProductName = request.ProductName,
                UnitsInStock = request.UnitsInStock,
                UnitPrice = request.UnitPrice,
                CategoryId = request.CategoryId
            };

            var result = await _productService.UpdateProductAsync(id, productModel);
            if (!result) return BadRequest();

            return NoContent(); // Return 204 No Content
        }
        [HttpDelete("product/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var exists = await _productService.ProductExistAsync(id);
            if (!exists) return NotFound();

            var result = await _productService.DeleteProductAsync(id);
            if (!result) return BadRequest();

            return NoContent(); // Return 204 No Content
        }
    }
}
