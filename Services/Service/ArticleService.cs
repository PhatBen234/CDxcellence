using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Services
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;

        public ArticleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesAsync()
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = a.Category.Name
                })
                .ToListAsync();
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Where(a => a.Id == id)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    CategoryId = a.CategoryId,
                    CategoryName = a.Category.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Article> CreateArticleAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<Article?> UpdateArticleAsync(int id, Article article)
        {
            var existingArticle = await _context.Articles.FindAsync(id);
            if (existingArticle == null) return null;

            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            existingArticle.ImageUrl = article.ImageUrl;
            existingArticle.IsPublished = article.IsPublished;
            existingArticle.CategoryId = article.CategoryId;
            existingArticle.AuthorId = article.AuthorId;
            existingArticle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingArticle;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PublishArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return false;

            article.IsPublished = true;
            article.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnpublishArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return false;

            article.IsPublished = false;
            article.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
