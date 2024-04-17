using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikYModels.DbContexts;
using WikYModels.Models;
using WikYRepositories.DTOs.Article;
using WikYRepositories.IRepositories;

namespace WikY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly UserManager<AppUser> _userManager;
        public ArticleController(
            IArticleRepository articleRepository,
            UserManager<AppUser> userManager
            )
        {
            _articleRepository = articleRepository;
            this._userManager = userManager;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles(int skip)
        {
            return await _articleRepository.GetAll(skip);
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
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
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Article>> PostArticle(AddArticleDTO articleDTO)
        {

            var appUser = await _userManager.GetUserAsync(HttpContext.User);
            if (appUser == null)
            {
                return Unauthorized();
            }
            
            if (appUser.Author.Id != articleDTO.AuthorID)
            {
                articleDTO.AuthorID = appUser.Author.Id;
            }

            Article article = await _articleRepository.AddArticle(articleDTO);

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

       /* // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

    }
}
