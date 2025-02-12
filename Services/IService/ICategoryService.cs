using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
