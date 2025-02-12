using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null) return NotFound();

            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] Article article)
        {
            if (article == null || string.IsNullOrWhiteSpace(article.Title) || string.IsNullOrWhiteSpace(article.Content))
                return BadRequest("Invalid article data.");

            var createdArticle = await _articleService.CreateArticleAsync(article);
            return CreatedAtAction(nameof(GetArticleById), new { id = createdArticle.Id }, createdArticle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] Article article)
        {
            if (article == null || string.IsNullOrWhiteSpace(article.Title) || string.IsNullOrWhiteSpace(article.Content))
                return BadRequest("Invalid article data.");

            var updatedArticle = await _articleService.UpdateArticleAsync(id, article);
            if (updatedArticle == null) return NotFound();

            return Ok(updatedArticle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var result = await _articleService.DeleteArticleAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/publish")]
        public async Task<IActionResult> PublishArticle(int id)
        {
            var result = await _articleService.PublishArticleAsync(id);
            if (!result) return NotFound();

            return Ok(new { message = "Article published successfully." });
        }

        [HttpPut("{id}/unpublish")]
        public async Task<IActionResult> UnpublishArticle(int id)
        {
            var result = await _articleService.UnpublishArticleAsync(id);
            if (!result) return NotFound();

            return Ok(new { message = "Article unpublished successfully." });
        }
    }
}
