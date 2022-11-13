using L1.Auth;
using L1.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace L1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<HotelRestUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private List<string> blackList = new List<string>();

        public AuthController(UserManager<HotelRestUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
                return BadRequest("User with specified username already exists.");

            var newUser = new HotelRestUser
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!createUserResult.Succeeded)
                return BadRequest("Could not create user");

            await _userManager.AddToRoleAsync(newUser, HotelRoles.HotelUser);

            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginUserDto)
        {
            var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
            if (user == null)
                return BadRequest("User name or password is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (!isPasswordValid)
                return BadRequest("User name or password is invalid.");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var jti = _jwtTokenService.ReturnGuid();
            await _userManager.SetAuthenticationTokenAsync(user, "JWT", "JWT Token", jti);

            return Ok(new SuccessfulLoginDto(accessToken));
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(LogoutDto logoutDto)
        {
            HotelRestUser hotelsRestUser = await _userManager.FindByIdAsync(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
            var jti = await _userManager.GetAuthenticationTokenAsync(hotelsRestUser, "JWT", "JWT Token");
            if (jti != User.FindFirstValue(JwtRegisteredClaimNames.Jti))
                return Unauthorized();

            await _userManager.RemoveAuthenticationTokenAsync(hotelsRestUser, "JWT", "JWT Token");

            return Ok();
        }

    }
}
