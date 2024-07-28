//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsaWeb.Service.Data;
using UsaWeb.Service.Helper;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        static TokenController()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        [HttpGet()]
        public IActionResult Get(TokenCred param)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == param.email && x.Password == param.password).FirstOrDefault();
                if (obj != null)
                {
                    string stringToken = HelperService.GetToken(param.email);
                    return Ok(stringToken);
                }
            }
            return BadRequest();
        }
    }
}
