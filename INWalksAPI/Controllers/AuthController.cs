using INWalksAPI.Models.DTO;
using INWalksAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace INWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Role != null && registerRequestDto.Role.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Role);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was Register! Please Login");
                    }
                }
            }
            return BadRequest("SomeThing went wrong");
        }


        //Post : /api/auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if(user!=null)
            {
                var checkResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(checkResult)
                {
                    //Get user roles
                    var roles = await userManager.GetRolesAsync(user);

                    //Create Token
                    var response = new LoginResponseDto
                    {
                        JwtToken = tokenRepository.CreateJwtToken(user, roles.ToList())
                    };

                    return Ok(response);
                }
            }

            return BadRequest("Invalied UserName or Password");
        }
    }
}
