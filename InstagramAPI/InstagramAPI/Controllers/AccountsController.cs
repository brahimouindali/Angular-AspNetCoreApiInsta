using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ApplicationSettings _options;

        public AccountsController(ApplicationDbContext context,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IOptions<ApplicationSettings> options,
            IEmailService emailService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _options = options.Value;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers() =>
            await _context.AppUsers.Include(u => u.Gender).OrderByDescending(u => u.RegisteredAt).ToListAsync();

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (user == null) return BadRequest();
            if (await _userManager.FindByEmailAsync(user.Email) != null) return BadRequest("email already exist!");
            var newUser = new AppUser
            {
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                WebSite = user.Website,
                Biography = user.Biography,
                GenderId = user.GenderId,
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

            await _emailService.SendAsync(mailTo, subject, $"by clicking the <a href={link}>link</a> you verify your email adress", true);
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
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
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.jwtSecret)),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            return BadRequest(new { message = "username or password is incorrect." });
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
        public async Task<ActionResult<AppUser>> GetUserAsync()
        {
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(user);
        }

        [HttpPatch]
        [Route("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser(UserModel model)
        {
            var sameEmail = true;
            var userToUpdate = await _userManager.FindByIdAsync(model.Id);
            if (!model.Email.Equals(userToUpdate.Email))
            {
                sameEmail = false;
            }
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
            //userToUpdate.RegisteredAt = userToUpdate.RegisteredAt;

            _context.AppUsers.Update(userToUpdate);
            await _context.SaveChangesAsync();
            if (!sameEmail)
            {
                userToUpdate.EmailConfirmed = false;
                await SendEmail(userToUpdate);
            }
            return Ok("user has been updated successfully");
        }
    }
}
