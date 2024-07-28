using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrtController : ControllerBase
    {
        [HttpGet("/Drt/DefaultList")]
        public async Task<Array> Get()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                //var sp_call = new Usaweb_DevContextProcedures(db);
                //var callresult = await sp_call.sp_GetRoundingListAsync("", "", "");
                //return callresult.ToArray();
                return null;
            }
        }

        [Route("/Drt/Update")]
        [HttpPost]
        public string Update(UpdatePatientRound model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PatientRound.FirstOrDefault(x => x.Mrn == model.Mrn);
                if (obj != null)
                {
                    obj.Q1ConcernsGenYesNo = model.Q1YesNo;
                    obj.Q1ConcernsGenComments = model.Q1Comments;

                    obj.Q2ConcernsMealsYesNo = model.Q2YesNo;
                    obj.Q2ConcernsMealsComments = model.Q2Comments;

                    obj.Q3FallRiskYesNo = model.Q3YesNo;
                    obj.Q4PressureInjuryRiskYesNo = model.Q4YesNo;

                    obj.Q5AdequateInfoYesNo = model.Q5YesNo;
                    obj.Q5AdequateInfoComment = model.Q5Comments;

                    obj.UpdateDate = DateTime.Now;

                    if ((model.Q1YesNo.HasValue && model.Q1YesNo.Value) ||
                        (model.Q2YesNo.HasValue && model.Q2YesNo.Value) ||
                        (model.Q3YesNo.HasValue && model.Q3YesNo.Value))
                    {
                        obj.Status = "COMPLETE";
                    }
                    else
                    {
                        obj.Status = "INCOMPLETE";
                    }

                    db.SaveChanges();

                }
            }
            return "";
        }
    }

    public class DrtList 
    {
        public string Hospital { get; set; }
        public string Unit { get; set; }
        public string Room { get; set; }
        public string PatientName { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
