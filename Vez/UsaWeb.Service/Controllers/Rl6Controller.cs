using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Rl6Controller : ControllerBase
    {
       
        [HttpGet("/rl6_safety_event")]
        public IActionResult Get(string npi, string startDt,
           string endDt, string fileId)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            d.Add(new KeyValuePair<string, string>("@npi", npi));
            d.Add(new KeyValuePair<string, string>("@startDt", startDt));
            d.Add(new KeyValuePair<string, string>("@endDt", endDt));
            d.Add(new KeyValuePair<string, string>("@fileId", fileId));

            query = "select top 500 * from rl6safetyevent rse " +
                    "Where (@npi IS NULL OR npi = @npi ) " +

                    "And (@startDt IS NULL OR eventDt between @startDt and @endDt) " +
                    "And (@fileID IS NULL OR fileId=@fileId) " +


                    "FOR JSON PATH";

            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }




    }
}
