using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IsoTreatmentProcessSupportAPI.Controllers
{
    [Route("api/user/{userId}/reminder")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        [HttpPost]
        //[Authorize]
        public ActionResult Add([FromRoute] int userId, [FromBody] CreateReminderDto dto)
        {
            _reminderService.Add(userId, dto);

            return Ok();
        }
    }
}
