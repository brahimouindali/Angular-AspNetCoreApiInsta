using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstagramAPI.Data;
using InstagramAPI.ModelLogic;
using InstagramAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            if (medias.Count() == 0) return NotFound();

            foreach (var media in medias)
            {
                var mediaIsLiked = _context.UserLikeMedias.Any(um => um.AppUserId == user.Id && um.MediaId == media.Id);

                var comments = new List<Comment>();

                var com = _context.Comments.Where(c => c.MediaId == media.Id)
                    .Include(c => c.AppUser).FirstOrDefault();

                var userLikeMediaId = _context.UserLikeMedias.Where(cm => cm.MediaId == media.Id).Select(u => u.AppUserId);

                var usersLikeMedia = _context.AppUsers
                    .Where(u => userLikeMediaId.Contains(u.Id));// list of users who liked this media

                if (com != null) comments.Add(com);

                var meFollowUsersList = new List<MeFollowUser>();

                foreach (var us in usersLikeMedia)
                {
                    var iFollowUser = _context.UserFollows
                                            .Any(uf => uf.AppUserFollowId == user.Id && us.Id == uf.AppUserFollowedId);

                    var meFollowUser = new MeFollowUser
                    {
                        AppUser = us,
                        IFollowedUser = iFollowUser,
                        IsMe = user.Id == us.Id
                    };
                    meFollowUsersList.Add(meFollowUser);
                }

                var mediaManage = new MediaManage
                {
                    Media = media,
                    IsLiked = mediaIsLiked,
                    Comments = comments,
                    CountComments = comments.Count,
                    CountLikes = usersLikeMedia.Count(),
                    AppUser = user, // user logged in
                    MeFollowUsersList = meFollowUsersList // list of the followers
                };
                //Thread.Sleep(2000);
                mediaManages.Add(mediaManage);
            }

            if (mediaManages != null)
                return Ok(mediaManages);
            return NotFound();
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


        private FileType SaveImage(IFormFile File)
        {
            var directory = String.Empty;
            var isVideo = false;
            var type = File.ContentType;
            var t = type.Split("/")[0];
            switch (t)
            {
                case "video":
                    directory = _hosting.WebRootPath + "/video";
                    isVideo = true;
                    break;
                case "image":
                    directory = _hosting.WebRootPath + "/img";
                    break;
                default:
                    break;
            }

            string fileName = String.Empty;
            if (File != null && directory != String.Empty)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);

                string fullPath = Path.Combine(directory, fileName);

                File.CopyTo(new FileStream(fullPath, FileMode.Create));
            }
            var fileType = new FileType
            {
                FileName = fileName,
                IsVideo = isVideo
            };

            return fileType.FileName != String.Empty ? fileType : null;
        }

        // add media
        [Route("media")]
        [HttpPost]
        public async Task<ActionResult<Media>> PostMedia([FromForm] MediaLogic media)
        {
            var user = await UserLoggedInAsync();

            var file = SaveImage(media.File);
            if (file == null) return BadRequest();

            var newMedia = new Media
            {
                AppUserId = user.Id,
                PublishedAt = DateTime.Now,
                MediaUrl = file.FileName,
                Description = media.Description,
                IsVideo = file.IsVideo
            };

            _context.Medias.Add(newMedia);
            await _context.SaveChangesAsync();
            return Ok(newMedia);
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

        [HttpGet]
        [Route("usermedias/{username}")]
        public ActionResult<IEnumerable<UserMedia>> UserMedias(string username)
        {
            var user = _context.AppUsers.Where(user => user.UserName == username).FirstOrDefault();
            var medias = _context.Medias.Where(media => media.AppUserId == user.Id);
            var countAbonne = _context.UserFollows.Where(uf => uf.AppUserFollowedId == user.Id).Count();
            var countAbonnement = _context.UserFollows.Where(uf => uf.AppUserFollowId == user.Id).Count();

            var userMedias = new UserMedia
            {
                Medias = medias,
                AppUser = user,
                CountAbonne = countAbonne,
                CountAbonnement = countAbonnement,
                CountMedias = medias.Count()
            };
            return Ok(userMedias); // profile component
        }

    }
}