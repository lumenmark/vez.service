using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        //[HttpGet("/qrt/screens")]
        //public IActionResult GetScreens(long qrtcaseid)
        //{
        //    string query = "Select * from qrtscreen where qrtcaseid=@qrtcaseid for json path;";

        //    IDictionary<string, string> d = new Dictionary<string, string>();
        //    d.Add(new KeyValuePair<string, string>("@qrtcaseid", qrtcaseid.ToString()));
        //    var result = DBHelper.RawSqlQuery(query, d);

        //    return Ok(result);
        //}

        //[HttpGet("/qrt/screen/{id}")]
        //public IActionResult Get(long id)
        //{
        //    string query = "Select * from qrtscreen where qrtscreenid=@qrtscreenid for json path;";

        //    IDictionary<string, string> d = new Dictionary<string, string>();
        //    d.Add(new KeyValuePair<string, string>("@qrtscreenid", id.ToString()));
        //    var result = DBHelper.RawSqlQuery(query, d);

        //    return Ok(result);
        //}

        //[HttpPost("/qrt/screen")]
        //public IActionResult Post(QrtScreenCreate model)
        //{
        //    if (model.QrtCaseId == 0)
        //        return BadRequest(new { Error = "invalid case id" });

        //    using (Usaweb_DevContext db = new Usaweb_DevContext())
        //    {
        //        if (model.ScreenedByMemberId != null)
        //        {
        //            var m = db.Member.FirstOrDefault(x => x.MemberId == model.ScreenedByMemberId.Value);
        //            if (m == null)
        //                return BadRequest(new { Error = "invalid member id" });
        //        }

        //        db.QrtScreen.Add(new QrtScreen
        //        {
        //            QrtCaseId = model.QrtCaseId,
        //            ScreenedByMemberId = model.ScreenedByMemberId,
        //            IsPalliativeCare = model.IsPalliativeCare,
        //            IsSepsisDiag = model.IsSepsisDiag,
        //            Is30DayReadmit = model.Is30DayReadmit,
        //            CauseOfDeathSummary = model.CauseOfDeathSummary,
        //            FinalDisp = model.FinalDisp,
        //            IsPeerReviewNeeded = model.IsPeerReviewNeeded,
        //            IsPeerReviewByDept = model.IsPeerReviewByDept,
        //            IsPeerReviewByInterdisc = model.IsPeerReviewByInterdisc,
        //            IsPeerReviewBySse = model.IsPeerReviewBySse,
        //            PeerReviewStartdate = string.IsNullOrEmpty(model.PeerReviewStartdate) ? null : DateTime.Parse(model.PeerReviewStartdate),
        //            CreateTs = DateTime.Now
        //        });
        //        db.SaveChanges();
        //    }
        //    return Ok();
        //}

        //[HttpPut("/qrt/screen/{id}")]
        //public IActionResult Put(long id, QrtScreenCreate model)
        //{
        //    if (model.QrtCaseId == 0)
        //        return BadRequest(new { Error = "invalid case id" });

        //    using (Usaweb_DevContext db = new Usaweb_DevContext())
        //    {
        //        if (model.ScreenedByMemberId != null)
        //        {
        //            var m = db.Member.FirstOrDefault(x => x.MemberId == model.ScreenedByMemberId.Value);
        //            if (m == null)
        //                return BadRequest(new { Error = "invalid member id" });
        //        }

        //        var obj = db.QrtScreen.FirstOrDefault(x => x.QrtScreenId == id);
        //        if (obj != null)
        //        {
        //            obj.QrtCaseId = model.QrtCaseId;
        //            obj.ScreenedByMemberId = model.ScreenedByMemberId;
        //            obj.IsPalliativeCare = model.IsPalliativeCare;
        //            obj.IsSepsisDiag = model.IsSepsisDiag;
        //            obj.Is30DayReadmit = model.Is30DayReadmit;
        //            obj.CauseOfDeathSummary = model.CauseOfDeathSummary;
        //            obj.FinalDisp = model.FinalDisp;
        //            obj.IsPeerReviewNeeded = model.IsPeerReviewNeeded;
        //            obj.IsPeerReviewByDept = model.IsPeerReviewByDept;
        //            obj.IsPeerReviewByInterdisc = model.IsPeerReviewByInterdisc;
        //            obj.IsPeerReviewBySse = model.IsPeerReviewBySse;
        //            obj.PeerReviewStartdate = string.IsNullOrEmpty(model.PeerReviewStartdate) ? null : DateTime.Parse(model.PeerReviewStartdate);
        //            db.SaveChanges();
        //        }
        //        else
        //            return NotFound();
        //    }
        //    return Accepted();
        //}

        //[HttpDelete("/qrt/screen/{id}")]
        //public IActionResult Delete(long id)
        //{
        //    using (Usaweb_DevContext db = new Usaweb_DevContext())
        //    {
        //        var obj = db.QrtScreen.FirstOrDefault(x => x.QrtScreenId == id);
        //        if (obj != null)
        //        {
        //            db.Remove(obj);
        //            db.SaveChanges();
        //        }
        //        else
        //            return NotFound();
        //    }

        //    //add code here to delete
        //    return NoContent();
        //}

    }
}
