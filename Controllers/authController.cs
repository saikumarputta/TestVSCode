using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestVSCode.viewModels;
using TodoApi.Models;

namespace TestVSCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration; 
        public authController(TodoContext context,UserManager<IdentityUser> userManager,IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> InsertUser([FromBody]RegisterViewModel model)
        {
            var user = new IdentityUser{
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
           var result = await _userManager.CreateAsync(user,model.Password);
           if(result.Succeeded)
           {
                await _userManager.AddToRoleAsync(user,"Customer");
           }
            return Ok(new {username = user.UserName,user.Email});
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
           var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null && await _userManager.CheckPasswordAsync(user,model.Password))
            {
               var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
               var credentials = new SigningCredentials(signinKey,SecurityAlgorithms.HmacSha256);
               int expiryinminutes = Convert.ToInt32(_configuration["JWT:ExpiryInTime"]);
               var token = new JwtSecurityToken(
                   issuer : _configuration["JWT:Site"],
                   audience : _configuration["JWT:Site"],
                   expires : DateTime.UtcNow.AddMinutes(expiryinminutes),
                   signingCredentials : credentials
               );
                return Ok(new{
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
                });
            }
         return Unauthorized();
        }
    }
}