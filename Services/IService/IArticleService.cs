using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models.Entities;

public interface IArticleService
{
    Task<IEnumerable<ArticleDto>> GetAllArticlesAsync();
    Task<ArticleDto?> GetArticleByIdAsync(int id);
    Task<Article> CreateArticleAsync(Article article);
    Task<Article?> UpdateArticleAsync(int id, Article article);
    Task<bool> DeleteArticleAsync(int id);
    Task<bool> PublishArticleAsync(int id);
    Task<bool> UnpublishArticleAsync(int id);
}
