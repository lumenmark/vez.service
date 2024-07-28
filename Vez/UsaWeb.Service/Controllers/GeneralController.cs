using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UsaWeb.Service.Data;
using UsaWeb.Service.Helper;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        public enum MessageType
        {
            GOALS_INITIAL_SET,
            GOALS_QUARTERLY_FEEDBACK
        }

        #region "Asset"

        [HttpPost("/asset")]
        public IActionResult Post(AssetVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                db.Asset.Add(new Asset
                {
                    fkOwner = model.fkOwner,
                    fkOwnerId = model.fkOwnerId,
                    mediaType = model.mediaType,
                    url = model.url,
                    note = model.note,
                    memberIdCreatedBy = model.memberIdCreatedBy,
                    createTs = DateTime.Now,
                });
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpPut("/asset/{assetId}")]
        public IActionResult Put(long assetId, AssetVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Asset.FirstOrDefault(x => x.assetId == assetId);
                if (obj != null)
                {

                    obj.fkOwner = model.fkOwner;
                    obj.fkOwnerId = model.fkOwnerId;
                    obj.mediaType = model.mediaType;
                    obj.url = model.url;
                    obj.note = model.note;

                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Accepted();
        }

        [HttpPatch("/asset/{assetId}")]
        public IActionResult Update(int assetId,
                         [FromBody] JsonPatchDocument<Asset> patch)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.Asset.FirstOrDefault(x => x.assetId == assetId);
                if (entity != null)
                {
                    patch.ApplyTo(entity);
                    db.SaveChanges();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpDelete("/asset/{assetId}")]
        public IActionResult Delete(long assetId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Asset.FirstOrDefault(x => x.assetId == assetId);
                if (obj != null)
                {
                    obj.deleteTs = DateTime.Now;
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            //add code here to delete
            return NoContent();
        }

        #endregion


        #region "SafetyEvent"

        [HttpGet("/qrt/se/case_list")]
        public IActionResult Get(string startDate, string endDate, string facility, string classification, 
            string status, string assigned, string mrn, string fin, string safetyEventId, string grouping, string rl6CaseId)
        {
            string query = string.Empty;

            int i = 0;
            if (!string.IsNullOrEmpty(safetyEventId))
                i += 1;
            if (!string.IsNullOrEmpty(mrn))
                i += 1;
            if (!string.IsNullOrEmpty(fin))
                i += 1;

            if (i > 1)
                return BadRequest("SafetyEventId, MRN, and FIN cannot be selected at the same time. Please enter only one option.");
       
            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@startDate", startDate));
            d.Add(new KeyValuePair<string, string>("@endDate", endDate));
            d.Add(new KeyValuePair<string, string>("@facility", facility));
            d.Add(new KeyValuePair<string, string>("@classification", classification));
            d.Add(new KeyValuePair<string, string>("@status", status));
            d.Add(new KeyValuePair<string, string>("@assigned", assigned));
            d.Add(new KeyValuePair<string, string>("@mrn", mrn));
            d.Add(new KeyValuePair<string, string>("@fin", fin));
            d.Add(new KeyValuePair<string, string>("@safetyEventId", safetyEventId));
            d.Add(new KeyValuePair<string, string>("@grouping", grouping));
            d.Add(new KeyValuePair<string, string>("@rl6CaseId", rl6CaseId));

            if (!string.IsNullOrEmpty(safetyEventId))
            {
                query = "Select * from SafetyEvent " +
                        "Where " +
                        "safetyEventId = @safetyEventId " +
                        "AND deleteTs is null " +
                        "FOR JSON AUTO";
            }
            else if (!string.IsNullOrEmpty(mrn))
            {
                query = "Select * from SafetyEvent " +
                        "Where " +
                        "mrn = @mrn " +
                        "AND deleteTs is null " +
                        "FOR JSON AUTO";
            }
            else
            {
                if (!string.IsNullOrEmpty(assigned) && assigned.Contains("-1"))
                {
                    query = "Select * from SafetyEvent " +
                        "Where " +
                        "(@startDate IS NULL OR eventDt between @startDate and @endDate ) " +
                        "AND (@facility IS NULL OR facilityId in (select splitdata from dbo.fnSplitString(@facility, ','))) " +
                        "AND (@classification IS NULL OR classification in (select splitdata from dbo.fnSplitString(@classification, ','))) " +
                        "AND (@status IS NULL OR status in (select splitdata from dbo.fnSplitString(@status, ','))) " +

                        "AND (memberIdAssigned in (select splitdata from dbo.fnSplitString(@assigned, ',')) Or memberIdAssigned IS NULL) " +

                        "AND (@mrn IS NULL OR mrn = @mrn) " +
                        "AND (@fin IS NULL OR fin = @fin) " +                        
                        "And (@rl6CaseId IS NULL OR rl6CaseIdDelim like '%' + @rl6CaseId + '%' ) " +

                        "AND (@grouping IS NULL OR grouping in (select splitdata from dbo.fnSplitString(@grouping, ','))) " +
                        "AND deleteTs is null " +
                        "order by createTs desc " +
                        "FOR JSON AUTO";
                }
                else
                {
                    query = "Select * from SafetyEvent " +
                        "Where " +
                        "(@startDate IS NULL OR eventDt between @startDate and @endDate ) " +
                        "AND (@facility IS NULL OR facilityId in (select splitdata from dbo.fnSplitString(@facility, ','))) " +
                        "AND (@classification IS NULL OR classification in (select splitdata from dbo.fnSplitString(@classification, ','))) " +
                        "AND (@status IS NULL OR status in (select splitdata from dbo.fnSplitString(@status, ','))) " +

                        "AND (@assigned IS NULL OR memberIdAssigned in (select splitdata from dbo.fnSplitString(@assigned, ','))) " +

                        "AND (@mrn IS NULL OR mrn = @mrn) " +
                        "AND (@fin IS NULL OR fin = @fin) " +
                        "And (@rl6CaseId IS NULL OR rl6CaseIdDelim like '%' + @rl6CaseId + '%' ) " +
                        "AND (@grouping IS NULL OR grouping in (select splitdata from dbo.fnSplitString(@grouping, ','))) " +
                        "AND deleteTs is null " +
                        "order by createTs desc " +
                        "FOR JSON AUTO";
                }
            }


            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }


        [HttpGet("/qrt/se/{safetyEventId}")]
        public IActionResult Get(int safetyEventId)
        {
            string query = string.Empty;

            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@safetyEventId", safetyEventId.ToString()));

            query = "select * from SafetyEvent where safetyEventId = @safetyEventId " +
                    " FOR JSON AUTO";

            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }

        [HttpPost("/qrt/se")]
        public IActionResult Post(SafetyEventVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
              var obj = db.SafetyEvent.Add(new SafetyEvent
                {
                    eventDt = string.IsNullOrEmpty(model.eventDt) ? null : DateTime.Parse(model.eventDt),
                    facilityId = model.facilityId,
                    typeGeneral = model.typeGeneral,
                    typeSpecific = model.typeSpecific,
                    personAffectedType = model.personAffectedType,
                    briefDescription = model.briefDescription,
                    dept = model.dept,
                    classification = model.classification,
                    comment = model.comment,
                    inlineNote = model.inlineNote,
                    status = model.status,
                    memberIdAssigned = model.memberIdAssigned,
                    assetURL = model.assetURL,
                    mrn = model.mrn,
                    fin = model.fin,
                    personAffected = model.personAffected,
                    rl6CaseIdDelim = model.rl6CaseIdDelim,
                    qrtCaseIdDelim = model.qrtCaseIdDelim,
                    createTs = DateTime.Now,
                    memberIdCreatedBy = model.memberIdCreatedBy,
                    grouping = model.grouping,
                });
                db.SaveChanges();
                return Ok(obj.Entity.safetyEventId);
            }
        }

        [HttpPut("/qrt/se/{safetyEventId}")]
        public IActionResult Put(long safetyEventId, SafetyEventVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SafetyEvent.FirstOrDefault(x => x.safetyEventId == safetyEventId);
                if (obj != null)
                {

                    obj.eventDt = string.IsNullOrEmpty(model.eventDt) ? null : DateTime.Parse(model.eventDt);
                    obj.facilityId = model.facilityId;
                    obj.typeGeneral = model.typeGeneral;
                    obj.typeSpecific = model.typeSpecific;
                    obj.personAffectedType = model.personAffectedType;
                    obj.briefDescription = model.briefDescription;
                    obj.dept = model.dept;
                    obj.classification = model.classification;
                    obj.comment = model.comment;
                    obj.inlineNote = model.inlineNote;
                    obj.status = model.status;
                    obj.memberIdAssigned = model.memberIdAssigned;
                    obj.assetURL = model.assetURL;
                    obj.mrn = model.mrn;
                    obj.fin = model.fin;
                    obj.personAffected = model.personAffected;
                    obj.rl6CaseIdDelim = model.rl6CaseIdDelim;
                    obj.qrtCaseIdDelim = model.qrtCaseIdDelim;
                    obj.memberIdCreatedBy = model.memberIdCreatedBy;
                    obj.grouping = model.grouping;

                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Accepted();
        }

        [HttpPatch("/qrt/se/{safetyEventId}")]
        public IActionResult UpdatePSE(int safetyEventId,
                         [FromBody] JsonPatchDocument<SafetyEvent> patch)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.SafetyEvent.FirstOrDefault(x => x.safetyEventId == safetyEventId);
                if (entity != null)
                {
                    patch.ApplyTo(entity);
                    db.SaveChanges();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpDelete("/qrt/se/{safetyEventId}")]
        public IActionResult DeletePSE(long safetyEventId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SafetyEvent.FirstOrDefault(x => x.safetyEventId == safetyEventId);
                if (obj != null)
                {
                    var listItems = db.SafetyEventItem.Where(x => x.safetyEventId == safetyEventId).ToList();
                    foreach (var item in listItems)
                    {
                        var itemToDelete = db.SafetyEventItem.FirstOrDefault(x => x.safetyEventItemId == item.safetyEventItemId);
                        itemToDelete.deleteTs = DateTime.Now;
                        db.SaveChanges();
                    }

                    obj.deleteTs = DateTime.Now;
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            //add code here to delete
            return NoContent();
        }

        [HttpGet("/qrt/se/rl6_case_list/{safetyEventId}")]
        public IActionResult GetSE(int safetyEventId)
        {
            string query;
            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@safetyEventId", safetyEventId.ToString()));

            query = "select sei.safetyEventItemId, sei.createTs, rl6.* " +
                    "From SafetyEventItem  sei " +
                    "left join Rl6Case rl6 on sei.itemId=rl6.rl6CaseId " +
                    "Where itemType='RL6' " +
                    "And sei.safetyEventId = @safetyEventId " +
                    "And deleteTs is null " +
                    "FOR JSON AUTO";
            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }

        [HttpGet("/qrt/se/qrt_case_list/{safetyEventId}")]
        public IActionResult GetQrtSE(int safetyEventId)
        {
            string query;
            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@safetyEventId", safetyEventId.ToString()));

            query = "Select sei.safetyEventItemId, sei.createTs, qrt.* " +
                    "From SafetyEventItem sei " +
                    "Left join QrtCase qrt on sei.itemId = qrt.QrtCaseId " +
                    "Where itemType = 'QRT' " +
                    "And sei.safetyEventId = @safetyEventId " +
                    "And sei.deleteTs is null " +
                    "And qrt.deleteTs is null " +
                    "FOR JSON AUTO";
            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }

        [HttpPost("/qrt/safety_event_item")]
        public IActionResult Post(SafetyEventItemVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SafetyEvent.FirstOrDefault(x => x.safetyEventId == model.safetyEventId);
                if (obj != null && !string.IsNullOrEmpty(model.itemType))
                {

                   var objReturn = db.SafetyEventItem.Add(new SafetyEventItem
                    {
                        safetyEventId = model.safetyEventId,
                        itemId = model.itemId,
                        itemType = model.itemType,
                        createTs = DateTime.Now,
                    });
                    db.SaveChanges();


                    var itemIdsList = db.SafetyEventItem
                        .Where(x => x.itemType == model.itemType && x.safetyEventId == model.safetyEventId)
                        .Select(x => x.itemId)
                        .ToList();
                    string itemIds = string.Join(",", itemIdsList);

                    if (model.itemType == "RL6")
                        obj.rl6CaseIdDelim = itemIds;
                    else if (model.itemType == "QRT")
                        obj.qrtCaseIdDelim = itemIds;

                    db.SaveChanges();

                    return Ok(objReturn.Entity.safetyEventItemId);
                }
                return NotFound();
            }
        }

        [HttpDelete("/qrt/safety_event_item/{safetyEventItemId}")]
        public IActionResult DeleteSEI(int safetyEventItemId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var objList = db.SafetyEventItem.Where(x => x.safetyEventItemId == safetyEventItemId).ToList();
                if (objList.Count > 0)
                {
                    var item = objList.FirstOrDefault();

                    db.SafetyEventItem.Remove(objList.FirstOrDefault());
                    db.SaveChanges();


                    var itemIdsList = db.SafetyEventItem
                                        .Where(x => x.itemType == item.itemType && x.safetyEventId == item.safetyEventId)
                                        .Select(x => x.itemId)
                                        .ToList();
                    string itemIds = string.Join(",", itemIdsList);

                    var objSE = db.SafetyEvent.FirstOrDefault(x => x.safetyEventId == item.safetyEventId);
                    if (item.itemType == "RL6")
                        objSE.rl6CaseIdDelim = itemIds;
                    else if (item.itemType == "QRT")
                        objSE.qrtCaseIdDelim = itemIds;

                    db.SaveChanges();

                }
            }
            return NoContent();
        }

        #endregion


        #region "ApplicationMemberAssigned"

        [HttpGet("/application_member_assign_list")]
        public IActionResult GetAMAList(string applicationShortName, string applicationShortName2, string status)
        {
            string query = string.Empty;

            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@applicationShortName", applicationShortName));
            d.Add(new KeyValuePair<string, string>("@applicationShortName2", applicationShortName2));
            d.Add(new KeyValuePair<string, string>("@status", status));

            query = "Select * from applicationmemberassigned ama " +
                    "inner join member m on ama.memberid = m.MemberId " +
                    "Where @applicationshortname IS NULL OR applicationshortname = @applicationShortName " +
                    "And (@applicationShortName2 IS NULL OR applicationShortName2 = @applicationShortName2) " +
                    "And (@status IS NULL OR ama.status = @status) " +
                    "FOR JSON AUTO";

            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }

        [HttpPost("/application_member_assign")]
        public IActionResult Post(ApplicationMemberAssignedVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.ApplicationMemberAssigned.Add(new ApplicationMemberAssigned
                {
                  applicationShortName = model.applicationShortName,
                  applicationShortName2 = model.applicationShortName2,
                  memberId = model.memberId,
                  status = model.status,
                  createTs = DateTime.Now,
                });
                db.SaveChanges();
                return Ok(obj.Entity.applicationMemberAssignId);
            }
        }

        [HttpPut("/application_member_assign/{applicationMemberAssignId}")]
        public IActionResult Put(long applicationMemberAssignId, ApplicationMemberAssignedVM model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.ApplicationMemberAssigned.FirstOrDefault(x => x.applicationMemberAssignId == applicationMemberAssignId);
                if (obj != null)
                {
                    obj.applicationShortName = model.applicationShortName;
                    obj.applicationShortName2 = model.applicationShortName2;
                    obj.memberId = model.memberId;
                    obj.status = model.status;

                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Accepted();
        }

        #endregion

        [HttpGet("/facility_list")]
        public IActionResult GetFacilityList(string facilityId, string status)
        {
            string query = string.Empty;

            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@facilityId", facilityId));
            d.Add(new KeyValuePair<string, string>("@status", status));

            query = "Select * from facility " +
                    "Where @facilityId IS NULL OR facilityId = @facilityId " +
                    "And (@status IS NULL OR status = @status) " +
                    "FOR JSON AUTO";

            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }




        [HttpGet("/email")]
        public async Task<IActionResult> Email(string type, string prannualmembersessionId)
        {
            string query = string.Empty;

            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@prannualmembersessionId", prannualmembersessionId));

            if (type == MessageType.GOALS_INITIAL_SET.ToString())
            {
                query = "select mg.goalText, mg.targetDt, mg.year, m.email, m.FullName " + 
                        "from PrAnnualMemberSession ams " +
                        "inner join member m on ams.memberIdReviewed=m.MemberId " +
                        "inner join PrMemberGoal mg on mg.prAnnualMemberSessionId=ams.prAnnualMemberSessionId " +
                        "where ams.prAnnualMemberSessionId = @prannualmembersessionId " +
                        "FOR JSON Path";
                var result = DBHelper.RawSqlQuery(query, d);
               
                List<EmailVM> emailVM = JsonConvert.DeserializeObject<List<EmailVM>>(result);


                if (emailVM.Count > 0)
                {
                    string email = emailVM[0].Email;
                    if (!string.IsNullOrEmpty(email))
                    {
                        string body = string.Empty;
                        int i = 1;
                        body += "Congratulations! During our recent goal-setting process, your manager assigned you the following goals. " +
                            "You will be meeting with your manager quarterly to discuss your progress. " +
                            "You will receive quarterly notifications to provide instructions on completing the feedback sessions. <br /><br />";

                        foreach (var item in emailVM)
                        {
                            string dueText = string.Empty;
                            if (!string.IsNullOrEmpty(item.TargetDt))
                                dueText = " Due: " + item.TargetDt;
                            body += i + ". " + item.GoalText + dueText + "<br />";
                            i++;
                        }
                        var emlResult = await HelperService.SendEmail(
                            email,
                            emailVM[0].FullName,
                            "Your " + emailVM[0].Year + " Goals!",
                            body
                        );
                    }
                }
            }
            return Ok();
        }

        [HttpGet("/applicationMemberAssigned")]
        public IActionResult GetApplicationMemberAssigned(string applicationShortName, string applicationShortName2)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@applicationShortName", applicationShortName));
            d.Add(new KeyValuePair<string, string>("@applicationShortName2", applicationShortName2));

            query = "select ama.memberId, ama.applicationMemberAssignId, ama.applicationShortName, " + 
                    "ama.applicationShortName2, ama.status as amaStatus, m.FirstName, m.LastName, " + 
                    "m.FullName, m.Email, m.title, m.TagColor, m.status as memberStatus " +
                    "from ApplicationMemberAssigned ama " +
                    "inner join member m on m.MemberId=ama.memberId " +
                    "where (@applicationShortName IS NULL OR applicationShortName = @applicationShortName) " +
                    "And (@applicationShortName2 IS NULL OR applicationShortName2 = @applicationShortName2) " + 
                    "Order by fullname " +
                    "FOR JSON AUTO";
            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }


        [HttpGet("/reappointments")]
        public IActionResult Get(string npi, string name, string nextApptStartDt,
           string nextApptEndDt, string status, string facilityName, string dept)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            d.Add(new KeyValuePair<string, string>("@npi", npi));
            d.Add(new KeyValuePair<string, string>("@name", name));
            d.Add(new KeyValuePair<string, string>("@nextApptStartDt", nextApptStartDt));
            d.Add(new KeyValuePair<string, string>("@nextApptEndDt", nextApptEndDt));
            d.Add(new KeyValuePair<string, string>("@status", status));
            d.Add(new KeyValuePair<string, string>("@facilityName", facilityName));
            d.Add(new KeyValuePair<string, string>("@dept", dept));

            query = "Select * from reappointment " +
                    "Where (@npi IS NULL OR npi = @npi ) " +
                    "And (@name IS NULL OR nameWithDegree like '%' + @name + '%' ) " +
                    "And (@nextApptStartDt IS NULL OR nextAppointmentDt between @nextApptStartDt and @nextApptEndDt) " +
                    "AND (@status IS NULL OR status in (select splitdata from dbo.fnSplitString(@status, ','))) " +
                    "AND (@facilityName IS NULL OR facilityName in (select splitdata from dbo.fnSplitString(@facilityName, ','))) " +
                    "AND (@dept IS NULL OR department in (select splitdata from dbo.fnSplitString(@dept, ','))) " +
                    "FOR JSON PATH";

            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }
    }
}
