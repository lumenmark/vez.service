using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UsaWeb.Service.Data;
using UsaWeb.Service.Helper;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels.Staff;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UsaWeb.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrsController : ControllerBase
    {
        private readonly ILogger<PrsController> _logger;

        public PrsController(ILogger<PrsController> logger)
        {
            _logger = logger;
        }

        #region "PrTeamMember"

        [HttpGet("/prs/staff_worklist/{memberIdManager}&year={year}")]
        public IActionResult Get(int memberIdManager, string year, string status)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdManager", memberIdManager.ToString()));
                d.Add(new KeyValuePair<string, string>("@year", year.ToString()));

                query = getCommonMemberStaffQuery("GET", status, null);

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }

        private string getCommonMemberStaffQuery(string getType,string status, string insertedIds)
        {
            string query = string.Empty;
            if (getType == "GET")
            {
                string statusQry = string.Empty;
                if (status != null && status.ToLower() == "active") statusQry = " and tm.deleteTs is null ";

                query = "select * from PrTeamMember tm " +
                           "left join PrAnnualMemberSession ams " +
                           "on tm.memberIdStaff=ams.memberIdReviewed " +
                           "inner join Member as member " +
                           "on member.memberId=tm.memberIdStaff " +
                           "Where memberIdManager=@memberIdManager " +
                           "AND ams.year in (select splitdata from dbo.fnSplitString(@year, ',')) " + statusQry +
                           "FOR JSON AUTO";
            }
            else if (getType == "INSERTED")
            {
                query = "select * from PrTeamMember tm " +
                              "left join PrAnnualMemberSession ams " +
                              "on tm.memberIdStaff=ams.memberIdReviewed " +
                              "inner join Member as member " +
                              "on member.memberId=tm.memberIdStaff " +
                              "Where tm.prTeamMemberId in (" + insertedIds + ") " +
                              "AND ams.year in (select splitdata from dbo.fnSplitString(@year, ',')) " +
                              "FOR JSON AUTO";
            }
            else
            {
                query = "select * from PrTeamMember tm " +
                           "left join PrAnnualMemberSession ams " +
                           "on tm.memberIdStaff = ams.memberIdReviewed " +
                           "inner join Member as member " +
                           "on member.memberId = tm.memberIdStaff " +
                           "Where tm.memberIdManager = @memberIdManager " +
                           "AND tm.memberIdStaff = @memberIdStaff " +
                           "AND ams.year in (select splitdata from dbo.fnSplitString(@year, ',')) " +
                           "FOR JSON AUTO";
            }
            return query;
        }

        [HttpGet("/prs/team")]
        public IActionResult GetTeamList(string memberIdManager, string memberIdManagerExclude, 
            string memberIdStaff, string status)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdManager", memberIdManager));
                d.Add(new KeyValuePair<string, string>("@memberIdManagerExclude", memberIdManagerExclude));
                d.Add(new KeyValuePair<string, string>("@memberIdStaff", memberIdStaff));

                string statusQry = string.Empty;
                if (status != null && status.ToLower() == "active") statusQry = " and deleteTs is null ";

                query = "select * from PrTeamMember tm inner join member as member on member.memberid=tm.memberidStaff " +
                        "where (@memberIdManager IS NULL OR memberIdManager in (select splitdata from dbo.fnSplitString(@memberIdManager, ','))) " +
                        "AND (@memberIdManagerExclude IS NULL OR memberIdManager not in (select splitdata from dbo.fnSplitString(@memberIdManagerExclude, ',')) ) " +
                        "AND (@memberIdStaff IS NULL OR memberIdStaff in (select splitdata from dbo.fnSplitString(@memberIdStaff, ',')) ) " +
                        statusQry +
                        "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }

        [HttpPost("/prs/prteammember")]
        public IActionResult Post(PrTeamMemberModel model)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            d.Add(new KeyValuePair<string, string>("@memberIdManager", model.memberIdManager));
            d.Add(new KeyValuePair<string, string>("@memberIdStaff", model.memberIdStaff));

            query = "UPDATE PrTeamMember set deleteTs=CURRENT_TIMESTAMP " +
                    "where memberIdStaff in (select splitdata from dbo.fnSplitString(@memberIdStaff, ',')) " +
                    "and memberIdManager not in (select splitdata from dbo.fnSplitString(@memberIdManager, ',')) " +
                    "and deleteTs is null";
            DBHelper.RawSqlQuery(query, d);

            var staffArr = model.memberIdStaff.Split(',');
            var memArr = model.memberIdManager.Split(",");
            //allow only 1 memberIdManager for now
            //loop through memberIdStaff
            List<string> listInsertedIds = new List<string>();
            foreach (var staff in staffArr)
            {
                d.Clear();
                d.Add(new KeyValuePair<string, string>("@memberIdManager", memArr[0]));
                d.Add(new KeyValuePair<string, string>("@memberIdStaff", staff));
                d.Add(new KeyValuePair<string, string>("@year", model.year));

                query = "INSERT INTO PrTeamMember (memberIdManager, memberIdStaff) " +
                        "output inserted.prTeamMemberId " +
                        "SELECT @memberIdManager,@memberIdStaff " +
                        "WHERE NOT EXISTS (SELECT * FROM PrTeamMember where memberIdManager=@memberIdManager and memberIdStaff=@memberIdStaff and deleteTS is null);";
                query = query +
                        "INSERT INTO PrAnnualMemberSession (memberIdReviewed, prStatusId, year) " +
                        "SELECT @memberIdStaff,1,@year " +
                        "WHERE NOT EXISTS (SELECT * FROM PrAnnualMemberSession where memberIdReviewed=@memberIdStaff and year=@year)";
                string result = DBHelper.RawSqlQuery(query, d);

                if (int.TryParse(result, out _))
                    listInsertedIds.Add(result);
            }

            if (listInsertedIds.Count > 0)
                query = getCommonMemberStaffQuery("INSERTED", null, string.Join(",", listInsertedIds.Select(n => n.ToString()).ToArray()));
            else
            {
                d.Clear();
                d.Add(new KeyValuePair<string, string>("@memberIdManager", model.memberIdManager));
                d.Add(new KeyValuePair<string, string>("@memberIdStaff", model.memberIdStaff));
                d.Add(new KeyValuePair<string, string>("@year", model.year));
                query = getCommonMemberStaffQuery("DEFAULT", null, null);
            }

            var result2 = DBHelper.RawSqlQuery(query, d);

            return Ok(result2);
        }

        //private void Insert_PrTeamMember(Usaweb_DevContext db, PrTeamMemberModel model)
        //{
        //    db.PrTeamMember.Add(new PrTeamMember
        //    {
        //        memberIdManager = model.memberIdManager,
        //        memberIdStaff = model.memberIdStaff,
        //        referredTs = model.referredTs,
        //        //deleteTs = model.deleteTs,
        //        createTs = DateTime.Now,
        //    });
        //}

        //[HttpPut("/prs/prteammember/{prTeamMemberId}")]
        //public IActionResult Put(long prTeamMemberId, PrTeamMemberModel model)
        //{
        //    using (Usaweb_DevContext db = new Usaweb_DevContext())
        //    {
        //        var obj = db.PrTeamMember.FirstOrDefault(x => x.prTeamMemberId == prTeamMemberId);

        //        if (obj == null)
        //        {
        //            Insert_PrTeamMember(db, model);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            obj.memberIdManager = model.memberIdManager;
        //            obj.memberIdStaff = model.memberIdStaff;
        //            obj.referredTs = model.referredTs;
        //            db.SaveChanges();
        //        }
        //    }
        //    return Accepted();
        //}

        [HttpPatch("/prs/prteammember/{prTeamMemberId}")]
        public IActionResult Update(int prTeamMemberId,
                          [FromBody] JsonPatchDocument<PrTeamMember> patch)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.PrTeamMember.FirstOrDefault(x => x.prTeamMemberId == prTeamMemberId);
                if (entity != null)
                {
                    patch.ApplyTo(entity);
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }


        [HttpDelete("/prs/prteammember/{prTeamMemberId}")]
        public IActionResult Delete(long prTeamMemberId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PrTeamMember.FirstOrDefault(x => x.prTeamMemberId == prTeamMemberId);
                if (obj != null)
                {
                    obj.deleteTs = DateTime.Now;
                    //db.Remove(obj);
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }

            //add code here to delete
            return NoContent();
        }

        #endregion

        #region "PrAnnualMemberSession"

        [HttpGet("/prs/prannualmembersession")]
        public IActionResult Get()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
              
                query = "select * from PrAnnualMemberSession " +
                        "where (@prAnnualMemberSessionId IS NULL OR prAnnualMemberSessionId=@prAnnualMemberSessionId) " +
                        "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, null);
                return Ok(result);
            }
        }

        [HttpGet("/prs/prannualmembersession/{prAnnualMemberSessionId}")]
        public IActionResult Get(string prAnnualMemberSessionId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@prAnnualMemberSessionId", prAnnualMemberSessionId));

                query = "select * from PrAnnualMemberSession " +
                        "where prAnnualMemberSessionId = @prAnnualMemberSessionId " +
                        "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }


        [HttpPost("/prs/prannualmembersession")]
        public IActionResult Post(PrAnnualMemberSessionModel model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                Insert_PrAnnualMemberSession(db, model);
                db.SaveChanges();
            }

            return Ok();
        }

        [HttpPut("/prs/prannualmembersession/{prAnnualMemberSessionId}")]
        public IActionResult Put(long prAnnualMemberSessionId, PrAnnualMemberSessionModel model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PrAnnualMemberSession.FirstOrDefault(x => x.prAnnualMemberSessionId == prAnnualMemberSessionId);

                if (obj == null)
                {
                    Insert_PrAnnualMemberSession(db, model);
                    db.SaveChanges();
                }
                else
                {
                    obj.memberIdReviewed = model.memberIdReviewed;
                    obj.prStatusId = model.prStatusId;
                    obj.year = model.year;
                    obj.signature = model.signature;
                    obj.staffSignedTs = model.staffSignedTs;
                    obj.managerNote = model.managerNote;
                    obj.staffNote = model.staffNote;
                    db.SaveChanges();
                }
            }
            return Accepted();
        }

        [HttpPatch("/prs/prannualmembersession/{prAnnualMemberSessionId}")]
        public IActionResult Update(int prAnnualMemberSessionId,
                    [FromBody] JsonPatchDocument<PrAnnualMemberSession> patch)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.PrAnnualMemberSession.FirstOrDefault(x => x.prAnnualMemberSessionId == prAnnualMemberSessionId);
                if (entity != null)
                {
                    patch.ApplyTo(entity);
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [HttpDelete("/prs/prannualmembersession/{prAnnualMemberSessionId}")]
        public IActionResult DeleteMS(long prAnnualMemberSessionId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PrAnnualMemberSession.FirstOrDefault(x => x.prAnnualMemberSessionId == prAnnualMemberSessionId);
                if (obj != null)
                {
                    db.Remove(obj);
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return NoContent();
        }


        private void Insert_PrAnnualMemberSession(Usaweb_DevContext db, PrAnnualMemberSessionModel model)
        {
            db.PrAnnualMemberSession.Add(new PrAnnualMemberSession
            {
                memberIdReviewed = model.memberIdReviewed,
                prStatusId = model.prStatusId,
                year = model.year,
                signature = model.signature,
                staffSignedTs = model.staffSignedTs,
                managerNote = model.managerNote,
                staffNote = model.staffNote,
                createTs = DateTime.Now,
            });
        }

        #endregion

        #region "PrTeamReferral"
        [HttpGet("/prs/teamreferral/{memberIdReviewer}")]
        public IActionResult GetTeamReferral(int memberIdReviewer, string status)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdReviewer", memberIdReviewer.ToString()));

                string statusQry = string.Empty;
                if (status != null && status.ToLower() == "active") statusQry = " and deleteTs is null ";

                query = "select * from PrTeamReferral where memberIdReviewer=@memberIdReviewer " +
                        statusQry +
                        "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }

        [HttpPost("/prs/teamreferral")]
        public IActionResult Post(PrTeamReferralModel model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                //string memberIdReviewer = model.memberIdReviewerOld.ToString();
                //if (model.memberIdReviewerOld == 0)
                //    memberIdReviewer = model.memberIdReviewerNew.ToString();
                //model.memberIdReviewerOld =====   loggedin = user
                //model.memberIdReviewerNew =====   selected user

                d.Add(new KeyValuePair<string, string>("@memberIdReviewer", model.memberIdReviewerOld.ToString()));
                d.Add(new KeyValuePair<string, string>("@memberIdReviewed", model.memberIdReviewed.ToString()));

                query = "Update PrTeamMember set PrTeamMember.referredTs= GETDATE() " +
                        "where memberIdManager=@memberIdReviewer and memberIdStaff = @memberIdReviewed and deleteTs is null";
                DBHelper.RawSqlQuery(query, d);

                db.PrTeamReferral.Add(new PrTeamReferral {
                    memberIdReviewer = model.memberIdReviewerNew,
                    memberIdReviewed = model.memberIdReviewed,
                    createTs = DateTime.Now,
                });

                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPost("/prs/teamreferral/unrefer/{memberIdReviewed}")]
        public IActionResult PostUnRefer(int memberIdReviewed, int memberIdReviewer)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdReviewer", memberIdReviewer.ToString()));
                d.Add(new KeyValuePair<string, string>("@memberIdReviewed", memberIdReviewed.ToString()));

                query = "update PrTeamMember set referredTs = null where memberIdStaff = @memberIdReviewed " +
                        " AND referredTs is not null";
                DBHelper.RawSqlQuery(query, d);

                query = "update PrTeamReferral set deleteTs = GETDATE() where memberIdReviewed = @memberIdReviewed " +
                      " AND deleteTs is null";
                
                DBHelper.RawSqlQuery(query, d);
            }

            return Ok();
        }

        [HttpDelete("/prs/teamreferral")]
        public IActionResult DeleteRef(long prTeamReferralId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PrTeamReferral.FirstOrDefault(x => x.prTeamReferralId == prTeamReferralId);
                if (obj != null)
                {
                    obj.deleteTs = DateTime.Now;
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return NoContent();
        }

        [HttpGet("/prs/referral_worklist/{memberIdReviewer}&year={year}")]
        public IActionResult GetReferralWorklist(int memberIdReviewer, string year, string status)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdReviewer", memberIdReviewer.ToString()));
                d.Add(new KeyValuePair<string, string>("@year", year.ToString()));

                string statusQry = string.Empty;
                if (status != null && status.ToLower() == "active") statusQry = " and tr.deleteTs is null ";

                query = "select * from PrTeamReferral tr " +
                           "left join PrAnnualMemberSession ams " +
                           "on tr.memberIdReviewed = ams.memberIdReviewed " +
                           "inner join Member as member " +
                           "on member.memberId = tr.memberIdReviewed " +
                           "Where tr.memberIdReviewer = @memberIdReviewer " +
                           "AND ams.year in (select splitdata from dbo.fnSplitString(@year, ',')) " + statusQry +
                           "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }


        #endregion

        #region "PrMemberGoal"

        [HttpGet("/prs/goals/{memberIdReviewed}")]
        public IActionResult Get(int memberIdReviewed, string prAnnualMemberSessionId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdReviewed", memberIdReviewed.ToString()));
                d.Add(new KeyValuePair<string, string>("@prAnnualMemberSessionId", prAnnualMemberSessionId));

                query = "select * from PrMemberGoal mg where memberIdReviewed=@memberIdReviewed " +
                        "AND (@prAnnualMemberSessionId IS NULL OR prAnnualMemberSessionId=@prAnnualMemberSessionId) " + 
                        "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }

        [HttpPost("/prs/goals")]
        public IActionResult Post(PrMemberGoalModel model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {

                db.PrMemberGoal.Add(new PrMemberGoal
                {
                    prAnnualMemberSessionId = model.prAnnualMemberSessionId,
                    year = model.year,
                    memberIdReviewer = model.memberIdReviewer,
                    memberIdReviewed = model.memberIdReviewed,
                    goalText = model.goalText,
                    targetDt = HelperService.ParseDate(model.targetDt),
                    createTs = DateTime.Now,
                });
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut("/prs/goals/{prMemberGoalId}")]
        public IActionResult Put(long prMemberGoalId, PrMemberGoalModel model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PrMemberGoal.FirstOrDefault(x => x.prMemberGoalId == prMemberGoalId);
         
                if (obj == null)
                {
                    db.PrMemberGoal.Add(new PrMemberGoal
                    {
                        prAnnualMemberSessionId = model.prAnnualMemberSessionId,
                        year = model.year,
                        memberIdReviewer = model.memberIdReviewer,
                        memberIdReviewed = model.memberIdReviewed,
                        goalText = model.goalText,
                        targetDt = HelperService.ParseDate(model.targetDt),
                        createTs = DateTime.Now,
                    });
                    db.SaveChanges();
                }
                else
                {
                    obj.prAnnualMemberSessionId = model.prAnnualMemberSessionId;
                    obj.year = model.year;
                    obj.memberIdReviewer = model.memberIdReviewer;
                    obj.memberIdReviewed = model.memberIdReviewed;
                    obj.goalText = model.goalText;
                    obj.targetDt = HelperService.ParseDate(model.targetDt);
                    db.SaveChanges();
                }
            }
            return Accepted();
        }


        [HttpPatch("/prs/goals/{prMemberGoalId}")]
        public IActionResult Update(int prMemberGoalId,
                   [FromBody] JsonPatchDocument<PrMemberGoal> patch)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.PrMemberGoal.FirstOrDefault(x => x.prMemberGoalId == prMemberGoalId);
                if (entity != null)
                {
                    patch.ApplyTo(entity);
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [HttpPost("/prs/goals/update/{prMemberGoalId}")]
        public IActionResult Post(long prMemberGoalId, PrMemberGoalModel model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.PrMemberGoal.FirstOrDefault(x => x.prMemberGoalId == prMemberGoalId);
                if (entity != null)
                {
                    db.PrMemberGoalHistory.Add(new PrMemberGoalHistory
                    {
                        prMemberGoalId = entity.prMemberGoalId,
                        prAnnualMemberSessionId = entity.prAnnualMemberSessionId,
                        year = entity.year,
                        memberIdReviewer = entity.memberIdReviewer,
                        memberIdReviewed = entity.memberIdReviewed,
                        goalText = entity.goalText,
                        targetDt = entity.targetDt,
                        createTs = entity.createTs,
                    });
                    db.SaveChanges();
                    
                
                    entity.prAnnualMemberSessionId = model.prAnnualMemberSessionId;
                    entity.year = model.year;
                    entity.memberIdReviewer = model.memberIdReviewer;
                    entity.memberIdReviewed = model.memberIdReviewed;
                    entity.goalText = model.goalText;
                    entity.targetDt = HelperService.ParseDate(model.targetDt);
                    entity.createTs = DateTime.Now;

                    db.SaveChanges();


                }
                else
                    return NotFound();
            }
            return Ok();
        }



        [HttpDelete("/prs/goals")]
        public IActionResult DeleteGoal(long prMemberGoalId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.PrMemberGoal.FirstOrDefault(x => x.prMemberGoalId == prMemberGoalId);
                if (obj != null)
                {
                    db.Remove(obj);
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/prs/goals/delete_all/{memberIdReviewed}/{prAnnualMemberSessionId}")]
        public IActionResult DeleteGoalAll(long memberIdReviewed, long prAnnualMemberSessionId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@memberIdReviewed", memberIdReviewed.ToString()));
                d.Add(new KeyValuePair<string, string>("@prAnnualMemberSessionId", prAnnualMemberSessionId.ToString()));

                query = "Delete from prmembergoal " + 
                        " where memberIdReviewed = @memberIdReviewed " + 
                        " And prAnnualMemberSessionId = @prAnnualMemberSessionId";
                DBHelper.RawSqlQuery(query, d);
            }
            return Ok();
        }

        #endregion

        [HttpGet("/gen/note_list")]
        public IActionResult Get(string fktable, string fkid, string type)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@fktable", fktable));
                d.Add(new KeyValuePair<string, string>("@fkid", fkid));
                d.Add(new KeyValuePair<string, string>("@type", type));

                query = "select * from Note " +
                        "where (@fktable IS NULL OR FkTable=@fktable) " +
                        "AND (@fkid IS NULL OR fkid=@fkid) " +
                        "AND (@type IS NULL OR type=@type) " +
                        "FOR JSON AUTO";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }

        [HttpGet("/prs/admin/member_list")]
        public IActionResult Get(string firstName, string lastName, string status, string year,
            int maxRecs = 200, string orderBy = " firstName, lastName asc")
        {
            //status =  "0,2,3"
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();

                d.Add(new KeyValuePair<string, string>("@firstName", firstName));
                d.Add(new KeyValuePair<string, string>("@lastName", lastName));
                d.Add(new KeyValuePair<string, string>("@status", status));
                d.Add(new KeyValuePair<string, string>("@year", year));

                string statusFilter = "AND (@status IS NULL OR prStatusId in (select splitdata from dbo.fnSplitString(@status, ','))) ";
                if (status == "-1")
                    statusFilter = "AND prStatusId IS NULL ";

                string statusQry = "CASE WHEN prStatusId=0 THEN 'COMPLETE' " +
                                   "WHEN prStatusId=1 THEN 'NEEDS GOALS' " +
                                   "WHEN prStatusId=2 THEN 'NEEDS CHECKIN' " +
                                   "WHEN prStatusId=3 THEN 'NEEDS YE REVIEW' " +
                                   "ELSE 'UNASSIGNED TO TEAM' END AS MainStatus";
                query = "select top " + maxRecs + " *, " + statusQry + " " +
                        "From member m " +
                        "Left join PrAnnualMemberSession ams " +
                        "ON m.MemberId = ams.memberIdReviewed " +
                        "Where (@firstName IS NULL OR FirstName like '%' + @firstName + '%') " +
                        "AND (@lastName IS NULL OR LastName like '%' + @lastName + '%') " +
                        "AND (@year IS NULL OR ams.year = @year ) " +
                        statusFilter + 
                        "Order by " + orderBy + " " +
                        "FOR JSON PATH";

                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }


    }
}
 