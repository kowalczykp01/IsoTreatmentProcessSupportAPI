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
        public ActionResult<EntryDto> Add([FromRoute] int userId, [FromBody] CreateEntryDto dto)
        {
            var addedEntry = _entryService.Add(userId, dto);

            return Ok(addedEntry);
        }
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<EntryDto>> GetAll([FromRoute] int userId)
        {
            var entries = _entryService.GetAll(userId);
            
            return Ok(entries);
        }
        [HttpPut("{id}")]
        public ActionResult<EntryDto> Update([FromRoute] int id, [FromBody] UpdateEntryDto dto)
        {
            var updatedEntry = _entryService.Update(id, dto);

            return Ok(updatedEntry);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _entryService.Delete(id);

            return NoContent();
        }
    }
}
