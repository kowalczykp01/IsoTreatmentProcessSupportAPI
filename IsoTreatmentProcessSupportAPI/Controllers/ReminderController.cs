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
        [HttpPost("user/{userId}")]
        public ActionResult<ReminderDto> Add([FromRoute] int userId, [FromBody] CreateAndUpdateReminderDto dto)
        {
            var addedReminder = _reminderService.Add(userId, dto);

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
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<ReminderDto>> GetAll([FromRoute] int userId)
        {
            var reminders = _reminderService.GetAll(userId);

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
