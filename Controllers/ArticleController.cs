﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Article;
using WikYRepositories.IRepositories;

namespace WikY.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly UserManager<AppUser> _userManager;
        public ArticleController(
            IArticleRepository articleRepository,
            IAuthorRepository authorRepository,
            UserManager<AppUser> userManager
            )
        {
            _articleRepository = articleRepository;
            _authorRepository = authorRepository;
            this._userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetListArticleDTO>>> GetLatestArticles()
        {
            return await _articleRepository.GetLatestArticles();
        }

        // GET: api/Articles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetListArticleDTO>>> GetArticles(int skip)
        {
            return await _articleRepository.GetAll(skip);
        }

        [HttpGet(template: "{AuthorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetListArticleDTO>>> GetArticlesFromAuthorId(int AuthorId)
        {
            return await _articleRepository.GetFromAuthor(AuthorId);
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetArticleDTO>> GetArticle(int id)
        {
            var article = await _articleRepository.GetFromId(id);

            if (article == null)
            {
                return NotFound();
            }
            return article;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateArticle(int id, UpdateArticleDTO articleDTO)
        {
            AppUser appUser = await GetConnectedUser();
            if (id != articleDTO.Id)
            {
                return BadRequest();
            }
            await _articleRepository.UpdateArticle(appUser.Author.Id, articleDTO);

            return NoContent();
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateArticleAdmin(int id, UpdateArticleAdminDTO articleDTO)
        {
            AppUser appUser = await GetConnectedUser();
            if (id != articleDTO.Id)
            {
                return BadRequest();
            }
            await _articleRepository.UpdateArticleAdmin(articleDTO);

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Article>> PostArticle(AddArticleDTO articleDTO)
        {
            AppUser appUser = await GetConnectedUser();
            Article article = await _articleRepository.AddArticle(appUser, articleDTO); ;

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("Delete/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            AppUser appUser = await GetConnectedUser();

            await _articleRepository.DeleteArticleFromId(appUser, id);

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
    }
}
