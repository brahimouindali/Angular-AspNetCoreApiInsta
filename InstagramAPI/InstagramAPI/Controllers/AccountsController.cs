﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InstagramAPI.Data;
using InstagramAPI.ModelLogic;
using InstagramAPI.Models;
using InstagramAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;

namespace InstagramAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHostingEnvironment _hosting;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationSettings _options;

        public AccountsController(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            IOptions<ApplicationSettings> options,
            IHostingEnvironment hosting,
            IEmailSender emailSender
            )
        {
            _context = context;
            _userManager = userManager;
            _hosting = hosting;
            _emailSender = emailSender;
            _options = options.Value;
        }


        [HttpGet]
        [Route("ur")]
        public async Task<IEnumerable<AppUser>> GetUsersAsync() =>
           await _context.AppUsers.OrderByDescending(u => u.RegisteredAt).ToListAsync();


        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<IEnumerable<UsersAndUserLoggedIn>>> GetUsers()
        {
            var user = await GetUserAsync();

            var users = await _context.AppUsers.OrderByDescending(u => u.RegisteredAt).ToListAsync();

            var usersAndUserLoggedIn = new UsersAndUserLoggedIn
            {
                AppUser = user,
                AppUsers = users
            };
            return Ok(usersAndUserLoggedIn);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (user == null) return BadRequest();
            var usernameIsExistInDb = _context.AppUsers.Any(u => u.UserName == user.UserName);
            var emailIsExistInDb = _context.AppUsers.Any(u => u.Email == user.Email);

            if (usernameIsExistInDb || emailIsExistInDb) return BadRequest("username or email already exist!");

            var newUser = new AppUser
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RegisteredAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                await SendEmail(newUser);
                return Ok(newUser);
            }

            return BadRequest();
        }

        private async Task SendEmail(AppUser user)
        {
            var userToVerifyEmail = await _userManager.FindByEmailAsync(user.Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userToVerifyEmail);
            var link = Url.Action("VerifyEmail", "Accounts", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

            var mailTo = user.Email;
            string subject = "Verify email adress";

            var message = new Message(new string[] { mailTo }, subject, link);
            _emailSender.SendEmail(message);

            //  await _emailService.SendAsync(mailTo, subject, $"by clicking the <a href={link}>link</a> you verify your email adress", true);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail([FromQuery] string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) return Ok();
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogInUser(UserModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && checkPassword)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(10),
                    SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.jwtSecret)),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            return BadRequest(new { message = "email or password is incorrect." });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                return Ok(user);
            return BadRequest();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return Ok(user);
            }
            return BadRequest("user is not exist");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("user")]
        public async Task<AppUser> GetUserAsync()
        {
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        [HttpPatch]
        [Route("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser(UserModel model)
        {
            var sameEmail = true;
            var userToUpdate = await _userManager.FindByIdAsync(model.Id);
            if (_context.AppUsers.Any(u => u.UserName == model.UserName)) return BadRequest("username already exist!");
            if (!model.Email.Equals(userToUpdate.Email))
            {
                sameEmail = false;
            }

            var fileName = SaveImage(model.File);

            _context.Entry(userToUpdate).State = EntityState.Modified;
            userToUpdate.Biography = model.Biography == null ? userToUpdate.Biography : model.Biography;
            userToUpdate.Email = model.Email == null ? userToUpdate.Email : model.Email;
            userToUpdate.NormalizedEmail = userToUpdate.Email.ToUpper();
            userToUpdate.UserName = model.UserName == null ? userToUpdate.UserName : model.UserName;
            userToUpdate.NormalizedUserName = userToUpdate.UserName.ToUpper();
            userToUpdate.FirstName = model.FirstName == null ? userToUpdate.FirstName : model.FirstName;
            userToUpdate.LastName = model.LastName == null ? userToUpdate.LastName : model.LastName;
            userToUpdate.WebSite = model.Website == null ? userToUpdate.WebSite : model.Website;
            userToUpdate.PhoneNumber = model.PhoneNumber == null ? userToUpdate.PhoneNumber : model.PhoneNumber;
            userToUpdate.ImagePath = fileName == null ? userToUpdate.ImagePath : fileName;

            _context.AppUsers.Update(userToUpdate);
            await _context.SaveChangesAsync();
            if (!sameEmail)
            {
                userToUpdate.EmailConfirmed = false;
                await SendEmail(userToUpdate);
            }
            return Ok("user has been updated successfully");
        }

        [HttpPost]
        [Route("followOrUnfollow")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> FollowOrUnfollow(UserModel model)
        {
            var user = await GetUserAsync();

            var userToFollowOrDelete = _context.UserFollows
                .Where(uf => uf.AppUserFollowedId == model.Id && uf.AppUserFollowId == user.Id).FirstOrDefault();

            if (userToFollowOrDelete != null)
            {
                _context.UserFollows.Remove(userToFollowOrDelete);
            }
            else
            {
                var followUser = new UserFollow
                {
                    AppUserFollowedId = model.Id,
                    AppUserFollowId = user.Id
                };
                _context.UserFollows.Add(followUser);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private string SaveImage(IFormFile File)
        {
            string fileName = null;
            if (File != null)
            {
                var directory = _hosting.WebRootPath + "/profile";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);

                string fullPath = Path.Combine(directory, fileName);

                File.CopyTo(new FileStream(fullPath, FileMode.Create));
            }

            return fileName;
        }
    }
}
