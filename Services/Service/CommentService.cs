using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId)
        {
            return await _context.Comments
                .Where(c => c.ArticleId == articleId)
                .Include(c => c.User) // Load thông tin người bình luận
                .ToListAsync();
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment comment)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null) return null;

            existingComment.Content = comment.Content;
            existingComment.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
