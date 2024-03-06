using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.DTO;
using Microsoft.AspNetCore.Identity;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var identityuser = new IdentityUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.UserName
            };

            var userdata=await userManager.CreateAsync(identityuser, registerRequest.Password);
            if(userdata.Succeeded)
            {
                await userManager.AddToRoleAsync(identityuser, registerRequest.roles);
                if(userdata.Succeeded)
                {
                    return Ok("User is success. Please login");
                }
            }

            return BadRequest("Something went wrong");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var usercheck = await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if(usercheck != null)
            {
                var passwordCheck=await userManager.CheckPasswordAsync(usercheck, loginRequestDto.Password);
                if(passwordCheck)
                {
                    var roles=await userManager.GetRolesAsync(usercheck);

                    //Create a Token
                    if(roles !=null)
                    {
                       var JwtToken= tokenRepository.CreateToken(usercheck, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = JwtToken
                        };
                        return Ok(response);
                    }                    
                }
            }
            return BadRequest("USerName or password is incorrect");
        }
    }
}
