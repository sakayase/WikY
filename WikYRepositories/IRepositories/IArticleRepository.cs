using WikYModels.Models;
using WikYRepositories.DTOs.Article;

namespace WikYRepositories.IRepositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAll();
        Task<List<Article>>? GetFromAuthor(int AuthorId);
        Task<List<Article>> GetFromId(int ArticleId);
        Task<Article> AddArticle(AddArticleDTO article);
        Task<Article> UpdateArticle(UpdateArticleDTO article);
        Task DeleteArticleFromId(int ArticleId);
    }
}
