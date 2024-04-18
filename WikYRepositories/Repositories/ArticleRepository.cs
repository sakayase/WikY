

using Microsoft.EntityFrameworkCore;
using WikYModels.DbContexts;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Article;
using WikYRepositories.DTOs.Author;
using WikYRepositories.DTOs.Theme;
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

        public async Task<Article> AddArticle(AppUser appUser, AddArticleDTO articleDTO)
        {
            Author author = appUser.Author;
            Theme theme = await _dbContext.Themes
                .FirstAsync(t => t.Id == articleDTO.ThemeId);
            Article article = new()
            {
                Author = author,
                Content = articleDTO.Content,
                Priority = articleDTO.Priority,
                Theme = theme
            };

            var test = _dbContext.Add(article);
            await _dbContext.SaveChangesAsync();
            return article;
        }
        public async Task<GetArticleDTO?> GetFromId(int ArticleId)
        {
            return await _dbContext.Articles
                .Include(a => a.Comments)
                .Include(a => a.Author)
                .Include(a => a.Theme)
                .Select(a => new GetArticleDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = a.Author.Id,
                        UserName = a.Author.UserName,
                    },
                    Theme = a.Theme != null ? new GetThemeDTO()
                    {
                        id = a.Theme.Id,
                        Name = a.Theme.Name,
                    } : null,
                    Comments = a.Comments!.Select(c => new GetArticleCommentDTO()
                    {
                        id = c.Id,
                        Content = c.Content,
                        ArticleAuthor = new GetAuthorDTO()
                        {
                            id = c.Author.Id,
                            UserName = c.Author.UserName
                        }
                    }).ToList(),
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Id = a.Id,
                    Priority = a.Priority,
                })
                .FirstOrDefaultAsync(a => a.Id == ArticleId);
        }
        public Task<List<GetListArticleDTO>> GetAll(int skip = 0)
        {
            return _dbContext.Articles
                .Include(a => a.Theme)
                .Include(a => a.Author)
                .Skip(skip)
                .OrderByDescending(a => a.Priority)
                .Select(a => new GetListArticleDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = a.Author.Id,
                        UserName = a.Author.UserName
                    },
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    Id = a.Id,
                    Priority = a.Priority,
                    UpdatedAt = a.UpdatedAt,
                    Theme = new GetThemeDTO()
                    {
                        id = a.Theme.Id,
                        Name = a.Theme.Name
                    }
                }).ToListAsync();

        }
        public Task<List<GetListArticleDTO>> GetLatestArticles()
        {
            return _dbContext.Articles
                .OrderByDescending(a => a.UpdatedAt)
                .OrderByDescending(a => a.CreatedAt)
                .OrderByDescending(a => a.Priority)
                .Take(3)
                .Select(a => new GetListArticleDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = a.Author.Id,
                        UserName = a.Author.UserName
                    },
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    Id = a.Id,
                    Priority = a.Priority,
                    UpdatedAt = a.UpdatedAt,
                    Theme = new GetThemeDTO()
                    {
                        id = a.Theme.Id,
                        Name = a.Theme.Name
                    }
                })
                .ToListAsync();
        }

        public async Task<List<GetListArticleDTO>> GetFromAuthor(int AuthorId)
        {
            return await _dbContext.Articles
                .Where(a => a.Author.Id == AuthorId)
                .Include(a => a.Theme)
                .Include(a => a.Author)
                .OrderByDescending(a => a.Priority)
                .Select(a => new GetListArticleDTO()
                {
                    Author = new GetAuthorDTO()
                    {
                        id = a.Author.Id,
                        UserName = a.Author.UserName
                    },
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    Id = a.Id,
                    Priority = a.Priority,
                    UpdatedAt = a.UpdatedAt,
                    Theme = new GetThemeDTO()
                    {
                        id = a.Theme.Id,
                        Name = a.Theme.Name
                    }
                }).ToListAsync();
        }

        public async Task<Article?> UpdateArticle(int CurrAuthorId, UpdateArticleDTO articleDTO)
        {
            Article? article = await _dbContext.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == articleDTO.Id);
            if (article == null)
            {
                throw new UpdateEntryException(message: "The article does not exist.");
            }
            if (article.Author.Id != CurrAuthorId)
            {
                throw new UpdateEntryException(message: $"The article does not belong to you.");
            }
            if (articleDTO.Content != null)
            {
                article.Content = articleDTO.Content;
            }
            if (articleDTO.ThemeId != null)
            {
                Theme? theme = await _dbContext.Themes.FirstOrDefaultAsync(t => t.Id == articleDTO.ThemeId);
                if (theme == null)
                {
                    throw new UpdateEntryException(message: "Theme not found.");
                }
                article.Theme = theme;

            }
            _dbContext.Entry(article).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return article;
        }

        public async Task<Article?> UpdateArticleAdmin(UpdateArticleAdminDTO articleDTO)
        {
            Article? article = await _dbContext.Articles.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == articleDTO.Id);
            if (article == null)
            {
                throw new UpdateEntryException(message: "The article does not exist.");
            }
            if (articleDTO.Content != null)
            {
                article.Content = articleDTO.Content;
            }
            if (articleDTO.ThemeId != null)
            {
                Theme? theme = await _dbContext.Themes.FirstOrDefaultAsync(t => t.Id == articleDTO.ThemeId);
                if (theme == null)
                {
                    throw new UpdateEntryException(message: "Theme not found.");
                }
                article.Theme = theme;

            }
            if (articleDTO.Priority != null)
            {
                article.Priority = articleDTO.Priority ?? article.Priority;
            }
            _dbContext.Entry(article).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return article;
        }

        public async Task DeleteArticleFromId(AppUser AppUser, int ArticleId)
        {
            Article? Article = await _dbContext.Articles
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => ArticleId == c.Id);
            if (Article == null) { throw new UpdateEntryException(message: "The article does not exist."); }
            if (AppUser.Author.Id != Article.Author.Id) { throw new UpdateEntryException(message: "The article does not belong to you."); }

            _dbContext.Articles.Remove(Article);
            await _dbContext.SaveChangesAsync();
        }
    }
}
