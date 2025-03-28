using Azure;
using Microsoft.AspNetCore.Mvc;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Shared.DTOs.Auth;

namespace MSClubInsights.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService , IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var response = await _authService.Login(model);

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            var response = await _userService.Register(model);
            
            return Ok(response);
        }
    }
}
