using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Author;
using WikYRepositories.IRepositories;

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppUser()
        {
            var appUser = await GetConnectedUser();
            return Ok($"{appUser.UserName}");
        }


        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LogIn(LoginAuthorDTO userDTO)
        {
            try
            {
                AppUser loggedUser = await _authorRepository.LogIn(userDTO);
                return Ok($"User logged ({loggedUser.UserName})");
            }
            catch (LoginException e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _authorRepository.GetAll();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRoleAdmin()
        {
            if (!await _roleManager.RoleExistsAsync("ADMIN"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" });

                if (result.Succeeded)
                {
                    return Ok("Created");
                }
                else
                    return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));

            }

            return Ok("Already Created");
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddUserToRoleAdmin()
        {
            var appUser = await GetConnectedUser();

            await _userManager.AddToRoleAsync(appUser, "ADMIN");

            return Ok();
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> IsUserInRole()
        {
            var appUser = await GetConnectedUser();

            return Ok($"{await _userManager.IsInRoleAsync(appUser, "ADMIN")}");
        }

    }
}
