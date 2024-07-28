using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AssetLocationController : ControllerBase
    {
        [HttpPost("/track/asset_location")]
        public IActionResult Post(AssetLocationVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.AssetLocation.FirstOrDefault(x => x.assetId == model.assetId);
                if (obj == null)
                {
                    db.AssetLocation.Add(new AssetLocation
                    {
                        assetId = model.assetId,
                        lattitude = model.lattitude,
                        longitude = model.longitude,
                        createTs = DateTime.Now,
                    });
                }
                else 
                {
                    obj.lattitude = model.lattitude;
                    obj.longitude = model.longitude;
                }

                db.SaveChanges();
            }
            return Ok();
        }
    }
}
