using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikYModels.DbContexts;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.IRepositories;

namespace WikYRepositories.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        WikYDbContext dbContext;
        UserManager<User> userManager;

        public AuthorRepository(WikYDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<User> Add(AddAuthorDTO authorDTO)
        {
            var User = new User { UserName = authorDTO.UserName }; // Add other properties if needed
            User.Author = new() { UserName = authorDTO.UserName, BirthDate = authorDTO.BirthDate };
            var result = await userManager.CreateAsync(User, authorDTO.Pwd);

            if (result.Succeeded && User != null)
            {
                return User;
            }
            else
                throw new SignInException(message: string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        public async Task<List<Author>> GetAll()
        {
            List<Author>? authorsList = await dbContext.Authors.ToListAsync();
            return authorsList;
        }

        public Task<Author> GetFromAuthorId(int AuthorId)
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetFromId(int AuthorId)
        {
            throw new NotImplementedException();
        }
    }
}
