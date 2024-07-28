using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpGet("/department/list")]
        public IActionResult Get(string status, string applicationShortName)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(status) && string.IsNullOrEmpty(applicationShortName))
                query = "Select * from department where status='ACTIVE' order by name FOR JSON AUTO";
            else
            {
                d.Add(new KeyValuePair<string, string>("@applicationShortName", applicationShortName));
                if (!string.IsNullOrEmpty(status))
                    query = "Select * from department " +
                            "where (@applicationShortName IS NULL OR applicationShortName = @applicationShortName) " + 
                            "order by name FOR JSON AUTO";
                else
                    query = "Select * from department " +
                            "where status='ACTIVE' AND (@applicationShortName IS NULL OR applicationShortName = @applicationShortName) " + 
                            "Order by name FOR JSON AUTO";
            }
            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }
    }
}
