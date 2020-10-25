using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstagramAPI.Data;
using InstagramAPI.ModelLogic;
using InstagramAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstagramAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<AppUser> UserLoggedInAsync()
        {
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAll(int id)
        {
            var comments = await _context.Comments.Where(c => c.MediaId == id)
                .OrderByDescending(c => c.CommentAt).ToListAsync();
            return comments;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(CommentModel model)
        {
            var user = await UserLoggedInAsync();
            var comment = new Comment
            {
                CommentAt = DateTime.Now,
                Likes = 0,
                MediaId = model.MediaId,
                Content = model.Content,
                AppUserId = user.Id
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

    }
}
