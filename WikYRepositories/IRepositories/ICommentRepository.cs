using WikYModels.Models;
using WikYRepositories.DTOs.Comment;

namespace WikYRepositories.IRepositories
{
    public interface ICommentRepository
    {
        Task<List<GetCommentDTO>> GetAll();
        Task<GetCommentDTO?> GetComment(int CommentId);
        Task<List<GetCommentDTO>> GetCommentsFromUser(AppUser AppUser);
        Task<List<GetCommentDTO>> GetCommentsFromAuthorId(int AuthorId);
        Task<List<GetCommentDTO>> GetCommentsFromArticle(int ArticleId);
        Task<Comment> CreateComment(AppUser AppUser, AddCommentDTO CommentDTO);
        Task<Comment> UpdateComment(AppUser AppUser, UpdateCommentDTO commentDTO);
        Task DeleteComment(AppUser AppUser, int CommentId);
    }
}
