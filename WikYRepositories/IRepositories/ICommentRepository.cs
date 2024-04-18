using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.DTOs.Comment;

namespace WikYRepositories.IRepositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAll();
        Task<List<Comment>> GetCommentsFromUser(AppUser AppUser);
        Task<List<Comment>> GetCommentsFromArticle(int ArticleId);
        Task<Comment> CreateComment(AppUser AppUser, AddCommentDTO CommentDTO);
        Task<Comment> UpdateComment(AppUser AppUser, UpdateCommentDTO commentDTO);
        Task DeleteComment(AppUser AppUser, int CommentId);
    }
}
