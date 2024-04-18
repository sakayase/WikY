using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WikYModels.DbContexts;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.IRepositories;

namespace WikYRepositories.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly WikYDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthorRepository(
            WikYDbContext dbContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<AppUser> SignIn(AddAuthorDTO authorDTO)
        {
            if (authorDTO.BirthDate > DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
            {
                throw new SignInException(message: "You need to be 18 or older to register.");

            }

            var User = new AppUser { UserName = authorDTO.UserName }; // Add other properties if needed
            User.Author = new() { UserName = authorDTO.UserName, BirthDate = authorDTO.BirthDate };
            var result = await _userManager.CreateAsync(User, authorDTO.Pwd);

            if (result.Succeeded && User != null)
            {
                return User;
            }
            else
                throw new SignInException(message: string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        public async Task<AppUser> LogIn(LoginAuthorDTO authorDTO)
        {
            AppUser? AppUser = await _userManager.FindByNameAsync(authorDTO.UserName);
            if (AppUser == null)
            {
                throw new LoginException(message: "The user doesnt exist, or the password doesnst match.");
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(AppUser, authorDTO.Password, false, false);

            if (result.Succeeded)
            {
                return AppUser;
            }
            else
            {
                throw new LoginException(message: "The user doesnt exist, or the password doesnst match.");
            }
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<List<Author>> GetAll()
        {
            List<Author>? authorsList = await _dbContext.Authors.ToListAsync();
            return authorsList;
        }

        public async Task<Author?> GetFromAuthorId(int AuthorId)
        {
            return await _dbContext.Authors.FindAsync(AuthorId);
        }

        public async Task<AppUser?> GetAppUserWithAuthor(string appUserId)
        {
            return await _dbContext.AppUser.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == appUserId);
        }

        public async Task<Author> GetFromId(int AuthorId)
        {
            return await _dbContext.Authors.ElementAtAsync(AuthorId);
        }

        public async Task DeleteUserFromId(int AuthorId)
        {
            Author author = await _dbContext.Authors
                .FirstOrDefaultAsync(a => a.Id == AuthorId)
                ?? throw new NotFoundException("Author not found");
            AppUser appUser = await _dbContext.AppUser
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Author.Id == author.Id)
                ?? throw new NotFoundException("User not found");

            await _userManager.DeleteAsync(appUser);
            _dbContext.Remove(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}
