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
            _reminderService.Delete(id);

            return NoContent();
        }
        [HttpGet("{id}")]
        public ActionResult<ReminderDto> Get([FromRoute] int id)
        {
            ReminderDto reminder = _reminderService.GetById(id);

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
            var updatedReminder = _reminderService.Update(id, dto);

            return Ok(updatedReminder);
        }
    }
}
