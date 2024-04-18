using WikYModels.Models;
using WikYRepositories.DTOs.Author;

namespace WikYRepositories.IRepositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAll();
        Task<Author?> GetFromAuthorId(int AuthorId);
        Task<AppUser?> GetAppUserWithAuthor(string appUserId);
        Task<AppUser> SignIn(AddAuthorDTO authorDTO);
        Task<AppUser> LogIn(LoginAuthorDTO authorDTO);
        Task LogOut();
        Task DeleteUserFromId(int AuthorId);
    }
}
