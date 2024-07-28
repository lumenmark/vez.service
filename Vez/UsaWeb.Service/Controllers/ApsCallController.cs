using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;

namespace UsaWeb.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApsCallController : ControllerBase
    {
        [HttpGet]
        [Route("/aps/get")]
        public IActionResult Get(string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(startDate))
                startDate = DateTime.Now.AddDays(-10).ToShortDateString();
            if (string.IsNullOrEmpty(endDate))
                endDate = DateTime.Now.AddDays(1).ToShortDateString();
            else
                endDate = DateTime.Parse(endDate).AddDays(1).ToShortDateString();

            string query = "select * from ApsCall where CreateTs between '" +
                            startDate + "' and '" + endDate + "' for json path;";
            var result = DBHelper.RawSqlQuery(query, null);
            return Ok(result);
        }
    }
}
