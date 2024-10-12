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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategoryResponseModel>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var response = categories.Select(category => new CategoryResponseModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Products = category.Products
            });

            return Ok(response);
        }

        // GET: api/category/{id}
        [HttpGet("category/{id}")]
        public async Task<ActionResult<CategoryResponseModel>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var response = new CategoryResponseModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Products = category.Products
            };

            return Ok(response);
        }

        // POST: api/category
        [HttpPost("category")]
        public async Task<ActionResult<int>> InsertCategory([FromBody] CategoryRequestModel request)
        {
            var categoryModel = new CategoryModel
            {
                CategoryName = request.CategoryName
            };

            var categoryId = await _categoryService.InsertCategoryAsync(categoryModel);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryId }, categoryId);
        }

        // PUT: api/category/{id}
        [HttpPut("category/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryRequestModel request)
        {
            var exists = await _categoryService.CategoryExistAsync(id);
            if (!exists) return NotFound();

            var categoryModel = new CategoryModel
            {
                CategoryName = request.CategoryName
            };

            var result = await _categoryService.UpdateCategoryAsync(id, categoryModel);
            if (!result) return BadRequest();

            return NoContent(); // Return 204 No Content
        }

        // DELETE: api/category/{id}
        [HttpDelete("category/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var exists = await _categoryService.CategoryExistAsync(id);
            if (!exists) return NotFound();

            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result) return BadRequest();

            return NoContent(); // Return 204 No Content
        }
    }
}
