using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IsoTreatmentProcessSupportAPI.Controllers
{
    [Route("api/reminder")]
    [ApiController]
    [Authorize]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        [HttpPost()]
        public ActionResult<ReminderDto> Add([FromBody] CreateAndUpdateReminderDto dto)
        {
            var token = HttpContext.Request.Cookies["token"];

            var addedReminder = _reminderService.Add(token, dto);

            return Ok(addedReminder);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var token = HttpContext.Request.Cookies["token"];

            _reminderService.Delete(token, id);

            return NoContent();
        }
        [HttpGet("{id}")]
        public ActionResult<ReminderDto> Get([FromRoute] int id)
        {
            var token = HttpContext.Request.Cookies["token"];

            ReminderDto reminder = _reminderService.GetById(token, id);

            return Ok(reminder);
        }
        [HttpGet()]
        public ActionResult<IEnumerable<ReminderDto>> GetAll()
        {
            var token = HttpContext.Request.Cookies["token"];

            var reminders = _reminderService.GetAll(token);

            return Ok(reminders);
        }
        [HttpPut("{id}")]
        public ActionResult<ReminderDto> Update([FromRoute] int id, [FromBody] CreateAndUpdateReminderDto dto)
        {
            var token = HttpContext.Request.Cookies["token"];

            var updatedReminder = _reminderService.Update(token, id, dto);

            return Ok(updatedReminder);
        }
    }
}
