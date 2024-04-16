using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.IRepositories;

namespace WikY.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        IAuthorRepository authorRepository;
        UserManager<User> userManager;
        SignInManager<User> signInManager;
        RoleManager<IdentityRole> roleManager;
        public AuthController(
            IAuthorRepository authorRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.authorRepository = authorRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult GetUserName()
        {
            return Ok($"{User.Identity.Name}");
        }
        [HttpGet]
        public IActionResult GetUserId()
        {
            return Ok($"{userManager.GetUserId(User)}");
        }
        [HttpGet]
        public async Task<IActionResult> GetAppUser()
        {
            var appUser = await userManager.GetUserAsync(User);
            return Ok($"{appUser.Email}");
        }
        [HttpGet]
        public async Task<IActionResult> GetAnAppUserById(string userId)
        {
            var appUser = await userManager.Users
                                    .FirstOrDefaultAsync(u => u.Id == userId);

            return Ok($"{appUser.UserName}");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(AddAuthorDTO userDTO)
        {
            try
            {
                User createdUser = await authorRepository.Add(userDTO);
                return Ok($"User created at ID:{createdUser.Id} !");
            }
            catch (SignInException e)
            {
                return Problem(e.message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO userDTO)
        {
            User? currUser = await userManager.GetUserAsync(User);
            if (currUser != null)
            {
                var result = await userManager.ChangePasswordAsync(currUser, userDTO.OldPwd, userDTO.NewPwd);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                    return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));
            } else
            {
                return Forbid();
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateRoleAdmin()
        {
            if (!await roleManager.RoleExistsAsync("ADMIN"))
            {
                var result = await roleManager.CreateAsync(new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" });

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                    return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));

            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> AddUserToRoleAdmin()
        {
            var appUser = await userManager.GetUserAsync(User);

            await userManager.AddToRoleAsync(appUser, "ADMIN");

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> IsUserInRole()
        {
            var appUser = await userManager.GetUserAsync(User);

            return Ok($"{userManager.IsInRoleAsync(appUser, "ADMIN")}");
        }

        [HttpGet]
        public async Task<IActionResult> AnonymousRoute()
        {
            return Ok($"Anonymous");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SecureRoute()
        {
            return Ok($"Logged");
        }
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SecureRouteForRole()
        {
            return Ok($"Logged with admin role");
        }
    }
}