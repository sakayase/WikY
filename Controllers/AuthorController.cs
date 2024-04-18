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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorController(
            IAuthorRepository authorRepository,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _authorRepository = authorRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAppUser()
        {
            var appUser = await GetConnectedUser();
            return Ok($"{appUser.UserName}");
        }


        [HttpPost()]
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

        [HttpGet()]
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
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteSelf()
        {
            AppUser appUser = await GetConnectedUser();
            try
            {
                await _authorRepository.DeleteUserFromId(appUser.Author.Id);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
            return NoContent();
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuthorAdmin(int id)
        {
            AppUser appUser = await GetConnectedUser();
            if (!await _userManager.IsInRoleAsync(appUser, "ADMIN"))
            {
                    return Forbid();
            }
            try
            {
                await _authorRepository.DeleteUserFromId(id);
            } catch (NotFoundException e)
            {
                return NotFound(e.Message);
            } catch (Exception e)
            {
                return Problem(e.Message);
            }
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<AppUser> GetConnectedUser()
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                throw new GetConnectedUserException();
            }
            var appUser = await _authorRepository.GetAppUserWithAuthor(userId);
            if (appUser == null)
            {
                throw new GetConnectedUserException();
            }
            return appUser;
        }

        /*[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateRoleAdmin()
        {
            if (!await _roleManager.RoleExistsAsync("ADMIN"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" });

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                    return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));

            }

            return Ok();
        }*/

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddUserToRoleAdmin()
        {
            var appUser = await GetConnectedUser();

            await _userManager.AddToRoleAsync(appUser, "ADMIN");

            return Ok();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IsUserInRole()
        {
            var appUser = await GetConnectedUser();

            return Ok($"{await _userManager.IsInRoleAsync(appUser, "ADMIN")}");
        }

    }
}
