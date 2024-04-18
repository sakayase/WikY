using WikYModels.Models;
using WikYRepositories.DTOs.Article;

namespace WikYRepositories.IRepositories
{
    public interface IArticleRepository
    {
        Task<List<GetListArticleDTO>> GetAll(int skip = 0);
        Task<List<GetListArticleDTO>> GetLatestArticles();
        Task<List<GetListArticleDTO>> GetFromAuthor(int AuthorId);
        Task<GetArticleDTO?> GetFromId(int ArticleId);
        Task<Article> AddArticle(AppUser appUser, AddArticleDTO article);
        Task<Article?> UpdateArticle(int CurrAuthorId, UpdateArticleDTO article);
        Task<Article?> UpdateArticleAdmin(UpdateArticleAdminDTO article);
        Task DeleteArticleFromId(AppUser appUser, int ArticleId);
    }
}
