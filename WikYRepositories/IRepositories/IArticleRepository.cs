using WikYModels.Models;
using WikYRepositories.DTOs.Article;

namespace WikYRepositories.IRepositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAll(int skip = 0);
        Task<List<Article>> GetLatestArticles();
        List<Article> GetFromAuthor(int AuthorId);
        Task<Article?> GetFromId(int ArticleId);
        Task<Article> AddArticle(AddArticleDTO article);
        Task<Article> UpdateArticle(UpdateArticleDTO article);
        Task DeleteArticleFromId(int ArticleId);
    }
}
