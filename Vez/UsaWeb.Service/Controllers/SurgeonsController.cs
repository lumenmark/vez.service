using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;

namespace UsaWeb.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurgeonsController : Controller
    {
        private readonly ILogger<SurgeonsController> _logger;
        public SurgeonsController(ILogger<SurgeonsController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public Array Get()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var result = db.SurgicalScheduleRaw.Where(x => x.surgeon != "")
                             .Select(x => x.surgeon).Distinct().OrderBy(x => x);
                return result.ToArray();
            }
        }
    }
}
