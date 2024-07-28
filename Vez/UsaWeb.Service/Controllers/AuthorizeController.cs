using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Helper;

namespace UsaWeb.Service.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        [HttpGet("/secure")]
        public Array Get()
        {
            List<AutoComplete> list = new List<AutoComplete>();
            list.Add(new AutoComplete
            {
                value = "javascript",
                label = "javascript"
            });
            list.Add(new AutoComplete
            {
                value = "chakra",
                label = "chakra"
            });
            list.Add(new AutoComplete
            {
                value = "react",
                label = "react"
            });
            list.Add(new AutoComplete
            {
                value = "css",
                label = "css"
            });
            return list.ToArray();
        }

    }
}
