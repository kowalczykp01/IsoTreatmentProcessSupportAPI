using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(token);
        }
    }
}
