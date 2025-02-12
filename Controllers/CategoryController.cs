using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Invalid category data.");

            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Invalid category data.");

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
            if (updatedCategory == null) return NotFound();

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
