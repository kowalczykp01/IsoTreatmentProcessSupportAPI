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
        private readonly ITreatmentProcessService _treatmentProcessService;
        public TreatmentProcessController(ITreatmentProcessService treatmentProcessService)
        {
            _treatmentProcessService = treatmentProcessService;
        }

        [HttpGet()]
        public ActionResult<TreatmentProcessInfoDto> Get()
        {
            var token = HttpContext.Request.Cookies["token"];

            var treatmentProcessInfo = _treatmentProcessService.GetRemainingTreatmentDays(token);

            return Ok(treatmentProcessInfo);
        }
    }
}
