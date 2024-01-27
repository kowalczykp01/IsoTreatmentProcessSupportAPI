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
        [HttpPost()]
        public ActionResult<EntryDto> Add([FromBody] CreateEntryDto dto)
        {
            var token = HttpContext.Request.Cookies["token"];

            var addedEntry = _entryService.Add(token, dto);

            return Ok(addedEntry);
        }
        [HttpGet()]
        public ActionResult<IEnumerable<EntryDto>> GetAll()
        {
            var token = HttpContext.Request.Cookies["token"];

            var entries = _entryService.GetAll(token);
            
            return Ok(entries);
        }
        [HttpPut("{id}")]
        public ActionResult<EntryDto> Update([FromRoute] int id, [FromBody] UpdateEntryDto dto)
        {
            var token = HttpContext.Request.Cookies["token"];

            var updatedEntry = _entryService.Update(token, id, dto);

            return Ok(updatedEntry);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var token = HttpContext.Request.Cookies["token"];

            _entryService.Delete(token, id);

            return NoContent();
        }
    }
}
