using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.DbContexts;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.DTOs.Comment;
using WikYRepositories.IRepositories;

namespace WikYRepositories.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly WikYDbContext _dbContext;

        public CommentRepository(
            WikYDbContext dbContext
            )
        {
            this._dbContext = dbContext;
        }

        public async Task<Comment> CreateComment(AppUser AppUser, AddCommentDTO CommentDTO)
        {
            Article? Article = await _dbContext.Articles
                .FirstOrDefaultAsync(a => CommentDTO.articleId == a.Id);
            if (Article == null) { throw new CreateCommentException(message: "The article does not exist."); }
            Author? Author = await _dbContext.Authors
                .FirstOrDefaultAsync(a => AppUser.Author.Id == a.Id);
            if (Author == null) { throw new CreateCommentException(message: "The author does not exist."); }

            Comment Comment = new() { Article = Article, Author = Author, Content = CommentDTO.content };
            _dbContext.Add(Comment);
            await _dbContext.SaveChangesAsync();
            return Comment;
        }

        public async Task DeleteComment(AppUser AppUser, int CommentId)
        {
            Comment? Comment = await _dbContext.Comments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => CommentId == c.Id);
            if (Comment == null) { throw new UpdateEntryException(message: "The comment does not exist."); }
            if (AppUser.Author.Id != Comment.Author.Id) { throw new UpdateEntryException(message: "The comment does not belong to you."); }

            _dbContext.Comments.Remove(Comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GetCommentDTO>> GetCommentsFromArticle(int ArticleId)
        {
            List<GetCommentDTO> comments = await _dbContext.Comments
                .Include(c => c.Article)
                .Where(c => c.Article.Id == ArticleId)
                .Select(c => new GetCommentDTO() 
                { 
                    Author = new GetAuthorDTO() 
                    { 
                        id = c.Author.Id, 
                        UserName = c.Author.UserName 
                    }, 
                    Content = c.Content, 
                    Id= c.Id, 
                    CreatedAt = c.CreatedAt, 
                    UpdatedAt = c.UpdatedAt 
                })
                .ToListAsync();
            return comments;
        }

        public async Task<List<GetCommentDTO>> GetCommentsFromUser(AppUser AppUser)
        {
            List<GetCommentDTO> comments = await _dbContext.Comments
                .Include(c => c.Author)
                .Where(c => c.Author.Id == AppUser.Author.Id)
                .Select(c => new GetCommentDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = c.Author.Id,
                        UserName = c.Author.UserName
                    },
                    Content = c.Content,
                    Id = c.Id,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
            return comments;
        }

        public async Task<List<GetCommentDTO>> GetCommentsFromAuthorId(int AuthorId)
        {
            List<GetCommentDTO> comments = await _dbContext.Comments
                .Include(c => c.Author)
                .Where(c => c.Author.Id == AuthorId)
                .Select(c => new GetCommentDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = c.Author.Id,
                        UserName = c.Author.UserName
                    },
                    Content = c.Content,
                    Id = c.Id,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
            return comments;
        }
        public async Task<List<GetCommentDTO>> GetAll()
        {
            List<GetCommentDTO> comments = await _dbContext.Comments
                .Include(c => c.Author)
                .Select(c => new GetCommentDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = c.Author.Id,
                        UserName = c.Author.UserName
                    },
                    Content = c.Content,
                    Id = c.Id,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
            return comments;
        }

        public async Task<Comment> UpdateComment(AppUser AppUser, UpdateCommentDTO commentDTO)
        {
            Comment? comment = await _dbContext.Comments
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == commentDTO.Id);
            if (comment == null)
            {
                throw new UpdateEntryException(message: "The comment does not exist.");
            }
            if (comment.Author.Id != AppUser.Author.Id)
            {
                throw new UpdateEntryException(message: $"The comment does not belong to you.");
            }
            if (commentDTO.Content != null)
            {
                comment.Content = commentDTO.Content;
            }
            _dbContext.Entry(comment).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<GetCommentDTO?> GetComment(int CommentId)
        {
            return await _dbContext.Comments.Select(c => new GetCommentDTO()
            {
                Author = new GetAuthorDTO()
                {
                    id = c.Author.Id,
                    UserName = c.Author.UserName
                },
                Content = c.Content,
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).FirstOrDefaultAsync(a => a.Id == CommentId);
        }
    }
}
