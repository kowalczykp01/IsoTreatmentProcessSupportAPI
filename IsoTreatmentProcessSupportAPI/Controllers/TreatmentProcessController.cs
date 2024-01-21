using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IsoTreatmentProcessSupportAPI.Controllers
{
    [Route("api/treatment-process")]
    [ApiController]
    [Authorize]
    public class TreatmentProcessController : ControllerBase
    {
        private readonly ITreatmentProcessService _treaamentProcessService;
        public TreatmentProcessController(ITreatmentProcessService treatmentProcessService)
        {
            _treaamentProcessService = treatmentProcessService;
        }

        [HttpGet("{userId}")]
        public ActionResult<TreatmentProcessInfoDto> Get([FromRoute] int userId)
        {
            var treatmentProcessInfo = _treaamentProcessService.GetRemainingTreatmentDays(userId);

            return Ok(treatmentProcessInfo);
        }
    }
}
