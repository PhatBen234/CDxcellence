using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Lấy danh sách bình luận của một bài viết
        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetCommentsByArticleId(int articleId)
        {
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId);
            return Ok(comments);
        }

        // Tạo bình luận mới
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (comment == null)
                return BadRequest("Invalid comment data");

            var createdComment = await _commentService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentsByArticleId), new { articleId = createdComment.ArticleId }, createdComment);
        }

        // Cập nhật bình luận
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            var updatedComment = await _commentService.UpdateCommentAsync(id, comment);
            if (updatedComment == null)
                return NotFound();

            return Ok(updatedComment);
        }

        // Xóa bình luận
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
