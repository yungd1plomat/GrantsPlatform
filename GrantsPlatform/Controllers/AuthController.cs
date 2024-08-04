using GrantsPlatform.Abstractions;
using GrantsPlatform.Models.Viewmodels;
using GrantsPlatform.Models.Viewmodels.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrantsPlatform.Controllers
{
    [Route("/admin/api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userRepository.AuthenticateAsync(loginRequest.Login, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _userRepository.GenerateJwtTokenAsync(user);
            return Ok(new AuthCredentials()
            {
                Token = token,
            });
        }

        [HttpPost("check")]
        public IActionResult Check()
        {
            return NoContent();
        }
    }
}
