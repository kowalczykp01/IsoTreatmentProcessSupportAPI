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
        public ActionResult Get([FromRoute] int userId)
        {
            var remainingTreatmentDays = _treaamentProcessService.GetRemainingTreatmentDays(userId);

            return Ok(remainingTreatmentDays);
        }
    }
}
