using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IsoTreatmentProcessSupportAPI.Controllers
{
    [Route("api/entry")]
    [ApiController]
    [Authorize]
    public class EntryController : ControllerBase
    {
        private readonly IEntryService _entryService;
        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }
        [HttpPost("user/{userId}")]
        public ActionResult Add([FromRoute] int userId, [FromBody] CreateEntryDto dto)
        {
            int id = _entryService.Add(userId, dto);

            return Ok(id);
        }
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<EntryDto>> GetAll([FromRoute] int userId)
        {
            var entries = _entryService.GetAll(userId);
            
            return Ok(entries);
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateEntryDto dto)
        {
            _entryService.Update(id, dto);

            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _entryService.Delete(id);

            return NoContent();
        }
    }
}
