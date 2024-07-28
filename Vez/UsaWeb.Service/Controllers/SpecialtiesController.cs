using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;

namespace UsaWeb.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialtiesController : Controller
    {
        private readonly ILogger<SpecialtiesController> _logger;

        public SpecialtiesController(ILogger<SpecialtiesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Array Get()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var result = db.SurgicalScheduleRaw.Where(x=>x.surg_case_spec != "")
                             .Select(x => x.surg_case_spec).Distinct().OrderBy(x => x);
                return result.ToArray();
            }
        }
    }
}
