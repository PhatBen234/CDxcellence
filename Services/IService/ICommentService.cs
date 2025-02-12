using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment?> UpdateCommentAsync(int id, Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
