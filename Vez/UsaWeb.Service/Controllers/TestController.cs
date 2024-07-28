using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using UsaWeb.Service.Data;
using UsaWeb.Service.Helper;

namespace UsaWeb.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/test")]
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

     
        [HttpGet("/test/testemail")]
        public async Task<IActionResult> TestEmail(string email)
        {
            var emlResult = await HelperService.SendEmail2(email, "Test", "subject", "body");

            return Ok(emlResult);
        }

        [HttpGet("/test/testemail2")]
        public async Task<IActionResult> TestEmail2(string email)
        {
            var emlResult = await HelperService.SendEmail(email, "Test", "subject", "body");

            return Ok(emlResult);
        }

        //[HttpPost("/test/sendasync")]
        //public string TestJson(AutoComplete model)
        //{

        //    return "ok";
        //}

        [HttpGet("/testupdatepassword")]
        public IActionResult Get2()
        {
            //using (Usaweb_DevContext db = new Usaweb_DevContext())
            //{
            //   var list = db.Member.ToList();
            //    foreach (var item in list)
            //    {
            //        if (!string.IsNullOrEmpty(item.Password) && !item.Password.Contains("==")) 
            //        {
            //            var pass = HelperService.GetEncryptedPassword(item.Password).Result;
                        
            //            var obj = db.Member.FirstOrDefault(x => x.MemberId == item.MemberId);
            //            obj.Password = pass;
                        
            //            db.SaveChanges();

            //        }
            //    }
            //}
            return Ok();
        }

    }

    public class AutoComplete
    {
        public string value { get; set; }
        public string label { get; set; }
    }


}
