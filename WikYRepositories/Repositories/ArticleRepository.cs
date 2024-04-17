

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WikYModels.DbContexts;
using WikYModels.Models;
using WikYRepositories.DTOs.Article;
using WikYRepositories.IRepositories;

namespace WikYRepositories.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        WikYDbContext _dbContext;

        public ArticleRepository(
            WikYDbContext dbContext 
            )
        {
            this._dbContext = dbContext;
        }

        public async Task<Article> AddArticle(AddArticleDTO articleDTO)
        {
            Author author = await _dbContext.Authors
                .FirstAsync(a => a.Id == articleDTO.AuthorID);
            Theme theme = await _dbContext.Themes
                .FirstAsync(t => t.Id == articleDTO.ThemeId);
            Article article = new () { 
                Author = author, 
                Content = articleDTO.Content, 
                Priority = articleDTO.Priority, 
                Theme = theme
            };

            var test = _dbContext.Add(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }

        public Task DeleteArticleFromId(int ArticleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetAll(int skip = 0)
        {
            return _dbContext.Articles
                .Skip(skip)
                .OrderByDescending(a => a.Priority)
                .ToListAsync();
        }

        public Task<List<Article>> GetLatestArticles()
        {
            return _dbContext.Articles
                .OrderByDescending(a => a.UpdatedAt)
                .OrderByDescending(a => a.CreatedAt)
                .OrderByDescending(a => a.Priority)
                .Take(3)
                .ToListAsync();
        }

        public List<Article> GetFromAuthor(int AuthorId)
        {
            return _dbContext.Articles.Where(a => a.Id == AuthorId).ToList();
        }

        public async Task<Article?> GetFromId(int ArticleId)
        {
            return await _dbContext.Articles
                .Include(a => a.Comments)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == ArticleId);
        }

        public Task<Article> UpdateArticle(UpdateArticleDTO article)
        {
            throw new NotImplementedException();
        }
    }
}
