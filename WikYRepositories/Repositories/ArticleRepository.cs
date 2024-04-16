

using WikYModels.DbContexts;
using WikYModels.Models;
using WikYRepositories.DTOs.Article;
using WikYRepositories.IRepositories;

namespace WikYRepositories.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        WikYDbContext dbContext;

        public ArticleRepository(WikYDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Article> AddArticle(AddArticleDTO article)
        {
            throw new NotImplementedException();
        }

        public Task DeleteArticleFromId(int ArticleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>>? GetFromAuthor(int AuthorId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetFromId(int ArticleId)
        {
            throw new NotImplementedException();
        }

        public Task<Article> UpdateArticle(UpdateArticleDTO article)
        {
            throw new NotImplementedException();
        }
    }
}
