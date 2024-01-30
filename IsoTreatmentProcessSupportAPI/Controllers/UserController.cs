using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IsoTreatmentProcessSupportAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _userService.RegisterUser(dto);
            return Ok();
        }
        [HttpPost("confirmEmail")]
        public ActionResult ConfirmEmail([FromQuery] string emailConfirmationToken)
        {
            _userService.ConfirmEmail(emailConfirmationToken);
            return Ok("Email confirmed, thank you!");
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _userService.GenerateLoginToken(dto);

            HttpContext.Response.Cookies.Append("token", token,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

            return Ok();
        }
        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            HttpContext.Response.Cookies.Append("token", "", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            return Ok("User logged out successfully");
        }

        [Authorize]
        [HttpGet("info")]
        public ActionResult<UserDto> GetUserInfo()
        {
            var token = HttpContext.Request.Cookies["token"];

            var userInfo = _userService.GetUserInfo(token);

            return userInfo;
        }

        [Authorize]
        [HttpPost("info/update")]
        public ActionResult<UserDto> UpdateUserInfo([FromBody] UserDto dto)
        {
            var token = HttpContext.Request.Cookies["token"];

            var userInfo = _userService.UpdateUserInfo(token, dto);

            return userInfo;
        }
    }
}
