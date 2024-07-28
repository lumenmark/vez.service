using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpGet]
        public AppMenuVM Get(int memberId)
        {
            AppMenuVM model = new AppMenuVM();
            model.errorMessage = "No Error";
            try
            {
                using (Usaweb_DevContext db = new Usaweb_DevContext())
                {
                    var allApps = db.Application.Where(x => x.Status == "ACTIVE").ToList();
                    var allmemApps = db.Application.Where(x => x.Status == "ACTIVE" && x.ApplicationMember.Any(z => z.MemberId == memberId && z.Status == "ACTIVE")).ToList();

                    List<Application> sortApps = new List<Application>(); //= allApps.OrderBy(x => allmemApps).ToList();
                    foreach (var item in allmemApps)
                        sortApps.Add(item);

                    foreach (var item in allApps)
                    {
                        var obj = allmemApps.FirstOrDefault(x => x.ApplicationShortName == item.ApplicationShortName);
                        if (obj == null)
                            sortApps.Insert(sortApps.Count, item);
                    }
                    model.memberApps = allmemApps.ToArray();
                    model.apps = sortApps.ToArray();
                }
            }
            catch (Exception ex)
            {
                model.errorMessage = ex.Message;
            }
            
            return model;
        }

        [HttpGet]
        [Route("/Application/all")]
        public Array Get()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var list = db.Application.Where(x => x.Status == "ACTIVE").OrderBy(x=>x.FullName);
                return list.ToArray();
            }
        }
    }
}
