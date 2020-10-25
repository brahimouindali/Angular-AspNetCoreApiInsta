using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InstagramAPI.Data;
using InstagramAPI.ModelLogic;
using InstagramAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstagramAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHostingEnvironment _hosting;

        public MediasController(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            IHostingEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
        }

        //get all medias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaManage>>> GetMedias()
        {
            var user = await UserLoggedInAsync();
            var userFollowsId = _context.UserFollows
                                        .Where(uf => uf.AppUserFollowId == user.Id).Select(uf => uf.AppUserFollowedId);

            var medias = _context.Medias.Where(m => userFollowsId.Contains(m.AppUserId))
                .Include(m => m.AppUser)
                                    .OrderByDescending(m => m.PublishedAt);

            var mediaManages = new List<MediaManage>();
            foreach (var media in medias)
            {
                var mediaIsLiked = _context.UserLikeMedias.Any(um => um.AppUserId == user.Id && um.MediaId == media.Id);
                var comments = new List<Comment>();
                var com = _context.Comments.Where(c => c.MediaId == media.Id).FirstOrDefault();
                var countLikeMedia = _context.UserLikeMedias.Where(cm => cm.MediaId == media.Id).Count();
                comments.Add(com);
                var mediaManage = new MediaManage
                {
                    Media = media,
                    IsLiked = mediaIsLiked,
                    Comments = comments,
                    CountComments = comments.Count,
                    CountLikes = countLikeMedia
                };
                mediaManages.Add(mediaManage);
            }

            return Ok(mediaManages);
        }

        //get user logged in
        private async Task<AppUser> UserLoggedInAsync()
        {
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        //get media of the user logged in
        [Route("media")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Media>>> GetMedia()
        {
            var user = await UserLoggedInAsync();

            var media = _context.Medias.Where(m => user.Id.Contains(m.AppUserId))
                .OrderByDescending(m => m.PublishedAt);
            return Ok(media);
        }

        // add media
        [Route("media")]
        [HttpPost]
        public async Task<ActionResult<Media>> PostMedia([FromForm] MediaLogic media)
        {
            var user = await UserLoggedInAsync();

            if (media.File != null)
            {
                var directory = _hosting.WebRootPath + "/img";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(media.File.FileName);

                string fullPath = Path.Combine(directory, fileName);

                media.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                var newMedia = new Media
                {
                    AppUserId = user.Id,
                    PublishedAt = DateTime.Now,
                    MediaUrl = fileName,
                    Description = media.Description
                };

                _context.Medias.Add(newMedia);
                await _context.SaveChangesAsync();
                return Ok(newMedia);
            }
            return BadRequest();
        }

        //delete media
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            if (id != null)
            {
                var media = _context.Medias.Where(m => m.Id == id).FirstOrDefault();
                if (media != null)
                {
                    _context.Medias.Remove(media);
                    await _context.SaveChangesAsync();
                    return Ok(media);
                }
                return NotFound();
            }
            return BadRequest();
        }

        // update media
        [HttpPatch]
        [Route("media")]
        public async Task<IActionResult> UpdateMedia(MediaLogic media)
        {
            var mediaToUpdate = _context.Medias.Where(m => m.Id == media.Id).FirstOrDefault();
            if (mediaToUpdate != null)
            {
                _context.Entry(mediaToUpdate).State = EntityState.Modified;
                mediaToUpdate.Description = media.Description;
                _context.Medias.Update(mediaToUpdate);
                await _context.SaveChangesAsync();

                return Ok("media has been updated successfully");
            }
            return NotFound("media not found");
        }

        [HttpPost]
        [Route("likemedia")]
        public async Task<IActionResult> LikeMedia(MediaLogic media)
        {
            var user = await UserLoggedInAsync();
            var userLikeMedia = new UserLikeMedia
            {
                AppUserId = user.Id,
                MediaId = media.Id
            };
            var isAlreadyLiked = _context.UserLikeMedias.Any(um => um.AppUserId == user.Id && um.MediaId == media.Id);
            if (!isAlreadyLiked)
            {
                _context.UserLikeMedias.Add(userLikeMedia);
                await _context.SaveChangesAsync();
                return Ok(userLikeMedia);
            }
            return BadRequest("media already liked");
        }

        [HttpDelete]
        [Route("deslikemedia/{id}")]
        public async Task<IActionResult> DesLikeMedia(int id)
        {
            var user = await UserLoggedInAsync();
            var userLikeMediaToDelete = _context.UserLikeMedias
                    .Where(um => um.AppUserId == user.Id && um.MediaId == id).FirstOrDefault();

            if (userLikeMediaToDelete != null)
            {
                _context.UserLikeMedias.Remove(userLikeMediaToDelete);
                await _context.SaveChangesAsync();
                return Ok(userLikeMediaToDelete);
            }
            return NotFound();
        }

    }
}