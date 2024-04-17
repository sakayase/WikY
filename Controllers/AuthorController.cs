using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WikYModels.DbContexts;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.IRepositories;
using WikYRepositories.Repositories;

namespace WikY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly WikYDbContext _context;
        private readonly IAuthorRepository _authorRepository;
        private readonly UserManager<AppUser> _userManager;

        public AuthorController(
            WikYDbContext context, 
            IAuthorRepository authorRepository,
            UserManager<AppUser> userManager
            )
        {
            _context = context;
            _authorRepository = authorRepository;
            _userManager = userManager;
        }

        [HttpGet("GetLoggedUser")]
        [Authorize]
        public async Task<IActionResult> GetAppUser()
        {
            var appUser = await _userManager.GetUserAsync(HttpContext.User);
            return Ok($"{appUser.UserName}");
        }


        [HttpPost(template: "signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(AddAuthorDTO userDTO)
        {
            try
            {
                AppUser createdUser = await _authorRepository.SignIn(userDTO);
                return Ok($"User created at ID:{createdUser.Id} !");
            }
            catch (SignInException e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginAuthorDTO userDTO)
        {
            try
            {
                AppUser loggedUser = await _authorRepository.LogIn(userDTO);
                return Ok($"User logged ({loggedUser.UserName})");
            } catch (LoginException e) 
            {
                return Problem(e.Message);
            }
        }

        [HttpGet(template: "logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _authorRepository.LogOut();
                return Ok("User Disconnected");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _authorRepository.GetAll();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _authorRepository.GetFromAuthorId(id);

            if (author == null)
            {
                return NotFound();
            } 
            return author;
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
