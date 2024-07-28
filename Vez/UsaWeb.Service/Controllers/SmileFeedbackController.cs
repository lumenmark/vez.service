using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;

namespace UsaWeb.Service.Controllers
{
    [Route("sm/[controller]")]
    [ApiController]
    public class SmilefeedbackController : ControllerBase
    {
        [HttpGet("/sm/feedback_list")]
        public IActionResult Get()
        { 

            string query = string.Empty;
            query = "select * from SmileFeedback fb " +
                    "inner join asset a on fb.SourceAssetId=a.AssetId " + 
                    "order by 1 desc FOR JSON AUTO;";
            var result = DBHelper.RawSqlQuery(query, null);
            return Ok(result);
        }

        [HttpPost("/sm/smilefeedback")]
        public IActionResult Post(SmileFeedbackCreate model)
        {
            if (model.SourceAssetId == 0)
                return BadRequest();

            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                db.SmileFeedback.Add(new SmileFeedback {
                    SourceAssetId = model.SourceAssetId,
                    FeedbackValue = model.FeedbackValue,
                    FeedbackOneWord = model.FeedbackOneWord,
                    FeedbackComment = model.FeedbackComment,
                    ReviewerEmail = model.ReviewerEmail,
                    ReviewerPhone = model.ReviewerPhone,
                    CreateTs = DateTime.Now
                });
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut("/sm/smilefeedback/{id}")]
        public IActionResult Put(long id, SmileFeedbackCreate model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SmileFeedback.FirstOrDefault(x => x.SmileFeedbackId == id);
                if (obj != null)
                {
                    obj.SourceAssetId = model.SourceAssetId;
                    obj.FeedbackValue = model.FeedbackValue;
                    obj.FeedbackOneWord = model.FeedbackOneWord;
                    obj.FeedbackComment = model.FeedbackComment;
                    obj.ReviewerEmail = model.ReviewerEmail;
                    obj.ReviewerPhone = model.ReviewerPhone;

                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Accepted();
        }

        [HttpDelete("/sm/smilefeedback/{id}")]
        public IActionResult Delete(long id)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SmileFeedback.FirstOrDefault(x => x.SmileFeedbackId == id);
                if (obj != null)
                {
                    db.Remove(obj);
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }

            //add code here to delete
            return NoContent();
        }



    }
}
