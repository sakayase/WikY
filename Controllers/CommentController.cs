﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikYModels.DbContexts;
using WikYModels.Exceptions;
using WikYModels.Models;
using WikYRepositories.DTOs.Comment;
using WikYRepositories.IRepositories;
using WikYRepositories.Repositories;

namespace WikY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(
            IAuthorRepository authorRepository,
            ICommentRepository commentRepository,
            UserManager<AppUser> userManager
            )
        {
            this._commentRepository = commentRepository;
            this._userManager = userManager;
            this._authorRepository = authorRepository;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _commentRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComment(int id)
        {
            return await _commentRepository.GetAll();
        }

        // GET: api/Comments/5
        [HttpGet("GetCommentFromUser")]
        [Authorize]
        public async Task<ActionResult<List<Comment>>> GetCommentFromUser()
        {
            AppUser AppUser = await GetConnectedUser();
            return await _commentRepository.GetCommentsFromUser(AppUser);
        }

        
        [HttpGet("GetCommentFromArticle/{ArticleId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentFromArticle(int ArticleId)
        {
            return await _commentRepository.GetCommentsFromArticle(ArticleId);
        }



        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateComment/{id}")]
        [Authorize]
        public async Task<IActionResult> PutComment(int id, UpdateCommentDTO commentDTO)
        {
            AppUser appUser = await GetConnectedUser();
            if (id != commentDTO.Id)
            {
                return BadRequest();
            }
            await _commentRepository.UpdateComment(appUser, commentDTO);

            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment(AddCommentDTO commentDTO)
        {
            AppUser appUser = await GetConnectedUser();

            Comment Comment = await _commentRepository.CreateComment(appUser, commentDTO);

            return CreatedAtAction("GetComment", new { id = Comment.Id }, Comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            AppUser appUser = await GetConnectedUser();

            await _commentRepository.DeleteComment(appUser, id);

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
