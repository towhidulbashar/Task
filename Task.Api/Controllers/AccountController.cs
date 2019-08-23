using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Task.Api.Core;
using Task.Api.Core.Domain;

namespace Task.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AppSettings appSettings;

        public AccountController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            this.unitOfWork = unitOfWork;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]LoginModel loginModel)
        {
            try
            {
                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(loginModel.UserName);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });
                await unitOfWork.SignInManager.SignInAsync(user, true);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                unitOfWork.Complete();

                return Ok(new
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = tokenString
                });
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]ApplicationUser user)
        {
            try
            {
                var createresult = await unitOfWork.UserManager.CreateAsync(user, user.PasswordHash);
                unitOfWork.Complete();
                if (createresult.Succeeded)
                    return Ok();
                else
                    return BadRequest(createresult.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var applicationUsers = await unitOfWork.UserManager.FindByEmailAsync("tbashar@ael-bd.com");
                unitOfWork.Complete();
                return Ok(applicationUsers);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}