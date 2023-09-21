using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IsoTreatmentProcessSupportAPI.Controllers
{
    [Route("api/reminder")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        [HttpPost("user/{userId}")]
        public ActionResult Add([FromRoute] int userId, [FromBody] CreateReminderDto dto)
        {
            _reminderService.Add(userId, dto);

            return Ok();
        }
        [HttpPost("{id}")]
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
        public ActionResult Update([FromRoute] int id, [FromBody] ReminderDto dto)
        {
            _reminderService.Update(id, dto);

            return Ok();
        }
    }
}
