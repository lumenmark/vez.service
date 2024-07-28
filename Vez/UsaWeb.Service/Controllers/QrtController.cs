using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;
using System.Data.Common;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using UsaWeb.Service.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Abstractions;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class QrtController : ControllerBase
    {
        [Authorize]
        [HttpGet("/qrt/case_list")]
        public IActionResult Get(string caseId, string caseType, string fin, string dischargeDtStart, 
            string dischargeDtEnd, string facilityId,
            string status, string memberIdAssigned, string outcomeProviderConcernProviderId1, 
            string outcomeProviderConcernProviderId2, string sortField, string sortOrder)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            List<DbParameter> parameters = new List<DbParameter>();

            if (string.IsNullOrEmpty(caseId) && string.IsNullOrEmpty(caseType) && string.IsNullOrEmpty(fin)
                 && string.IsNullOrEmpty(dischargeDtStart) && string.IsNullOrEmpty(dischargeDtEnd)
                 && string.IsNullOrEmpty(facilityId)
                 && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(memberIdAssigned)
                 && string.IsNullOrEmpty(outcomeProviderConcernProviderId1)
                 && string.IsNullOrEmpty(outcomeProviderConcernProviderId2))
            {
                d.Add(new KeyValuePair<string, string>("@sortField", sortField));
                d.Add(new KeyValuePair<string, string>("@sortOrder", sortOrder));

                query = "select top 200 c.*, " +
                        "(select qrtCaseExtended.* from QrtCaseExtended where qrtCaseExtended.QrtCaseId = c.QrtCaseId FOR JSON AUTO) as qrtCaseExtended, " +
                        "(select facilityNurseUnit.* from FacilityNurseUnit where facilityNurseUnit.FacilityNurseUnitId = c.FacilityNurseUnitId FOR JSON AUTO) as facilityNurseUnit " +
                        "from QrtCase c " +
                        "Order BY " +
                        "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'asc' THEN c.PatientLastName END asc, " +
                        "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'desc' THEN c.PatientLastName END desc, " +
                        "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'asc' THEN c.DischargeDt END asc, " +
                        "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'desc' THEN c.DischargeDt END desc " +

                        "FOR JSON AUTO";
            }
            else if (!string.IsNullOrEmpty(caseId))
            {
                d.Add(new KeyValuePair<string, string>("@caseId", caseId));
                d.Add(new KeyValuePair<string, string>("@sortField", sortField));
                d.Add(new KeyValuePair<string, string>("@sortOrder", sortOrder));

                query = "select c.*, " +
                        "(select qrtCaseExtended.* from QrtCaseExtended where qrtCaseExtended.QrtCaseId = c.QrtCaseId FOR JSON AUTO) as qrtCaseExtended, " +
                        "(select facilityNurseUnit.* from FacilityNurseUnit where facilityNurseUnit.FacilityNurseUnitId = c.FacilityNurseUnitId FOR JSON AUTO) as facilityNurseUnit " +
                        "from QrtCase c " +
                        "where c.QrtCaseId = @caseId " +
                        "Order BY " +
                        "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'asc' THEN c.PatientLastName END asc, " +
                        "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'desc' THEN c.PatientLastName END desc, " +
                        "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'asc' THEN c.DischargeDt END asc, " +
                        "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'desc' THEN c.DischargeDt END desc " +

                        "FOR JSON AUTO";
            }
            else if (!string.IsNullOrEmpty(fin))
            {
                d.Add(new KeyValuePair<string, string>("@fin", fin));
                d.Add(new KeyValuePair<string, string>("@sortField", sortField));
                d.Add(new KeyValuePair<string, string>("@sortOrder", sortOrder));
                query = "select c.*, " +
                      "(select qrtCaseExtended.* from QrtCaseExtended where qrtCaseExtended.QrtCaseId = c.QrtCaseId FOR JSON AUTO) as qrtCaseExtended, " +
                      "(select facilityNurseUnit.* from FacilityNurseUnit where facilityNurseUnit.FacilityNurseUnitId = c.FacilityNurseUnitId FOR JSON AUTO) as facilityNurseUnit " +
                      "from QrtCase c " +
                      "where @fin IS NULL OR (c.FIN=@fin or c.mrn = @fin) " +
                      "Order BY " +
                      "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'asc' THEN c.PatientLastName END asc, " +
                      "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'desc' THEN c.PatientLastName END desc, " +
                      "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'asc' THEN c.DischargeDt END asc, " +
                      "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'desc' THEN c.DischargeDt END desc " +

                      "FOR JSON AUTO";
            }
            else
            {
                d.Add(new KeyValuePair<string, string>("@caseType", caseType));
                d.Add(new KeyValuePair<string, string>("@fin", fin));
                d.Add(new KeyValuePair<string, string>("@dischargeDtStart", dischargeDtStart));
                d.Add(new KeyValuePair<string, string>("@dischargeDtEnd", dischargeDtEnd));
                d.Add(new KeyValuePair<string, string>("@facilityId", facilityId));

                d.Add(new KeyValuePair<string, string>("@status", status));
                d.Add(new KeyValuePair<string, string>("@memberIdAssigned", memberIdAssigned));
                d.Add(new KeyValuePair<string, string>("@outcomeProviderConcernProviderId1", outcomeProviderConcernProviderId1));
                d.Add(new KeyValuePair<string, string>("@outcomeProviderConcernProviderId2", outcomeProviderConcernProviderId2));
                d.Add(new KeyValuePair<string, string>("@sortField", sortField));
                d.Add(new KeyValuePair<string, string>("@sortOrder", sortOrder));

                string statusQry = string.Empty;
                string statusQry2 = string.Empty;
                if (status != null && (status.Contains("NEW")))
                {
                    statusQry = " or qrtCaseExtended.status is null";
                    statusQry2 = " or d.status is null ";
                }

                query = "select c.* " +
                        ", ( " +
                        "	select qrtCaseExtended.* from QrtCaseExtended  " +
                        "	where qrtCaseExtended.QrtCaseId = c.QrtCaseId  " +
                        "	And (@status IS NULL OR qrtCaseExtended.status in (select splitdata from dbo.fnSplitString(@status, ',')) " + statusQry + ") " +
                        "	And (@memberIdAssigned IS NULL OR qrtCaseExtended.memberidassigned in ((select splitdata from dbo.fnSplitString(@memberIdAssigned,',')))) " +
                        "   And (@outcomeProviderConcernProviderId1 IS NULL OR qrtCaseExtended.Outcome_ProviderConcern_ProviderId1 in (select splitdata from dbo.fnSplitString(@outcomeProviderConcernProviderId1,','))) " +
                        "   And (@outcomeProviderConcernProviderId2 IS NULL OR qrtCaseExtended.Outcome_ProviderConcern_ProviderId2 in (select splitdata from dbo.fnSplitString(@outcomeProviderConcernProviderId2,','))) " +
                        "	FOR JSON AUTO " +
                        ") as qrtCaseExtended,( " +
                        "	select facilityNurseUnit.* from FacilityNurseUnit  " +
                        "	where facilityNurseUnit.FacilityNurseUnitId = c.FacilityNurseUnitId FOR JSON AUTO " +
                        ") as facilityNurseUnit " +
                        "from QrtCase c left join QrtCaseExtended d on c.QrtCaseId = d.QrtCaseId " +
                        "where (@dischargeDtStart IS NULL OR c.DischargeDt between @dischargeDtStart and @dischargeDtEnd) " +
                        "And (@caseType IS NULL OR c.CaseType in (select splitdata from dbo.fnSplitString(@caseType,',')))  " +
                        "And (@fin IS NULL OR (c.FIN = @fin or c.mrn = @fin)) " +
                        "And (@facilityId IS NULL OR c.FacilityId = @facilityId) " +
                        "And (@status IS NULL OR d.status in (select splitdata from dbo.fnSplitString(@status, ',')) " + statusQry2 + ") " +
                        "And (@memberIdAssigned IS NULL OR d.memberidassigned in (select splitdata from dbo.fnSplitString(@memberIdAssigned,','))) " +
                        "And (@outcomeProviderConcernProviderId1 IS NULL OR d.Outcome_ProviderConcern_ProviderId1 in (select splitdata from dbo.fnSplitString(@outcomeProviderConcernProviderId1,',')))  " +
                        "And (@outcomeProviderConcernProviderId2 IS NULL OR d.Outcome_ProviderConcern_ProviderId2 in (select splitdata from dbo.fnSplitString(@outcomeProviderConcernProviderId2,',')))  " +
                        "Order BY " +
                        "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'asc' THEN c.PatientLastName END asc, " +
                        "CASE WHEN @sortField = 'patientLastName' And @sortOrder = 'desc' THEN c.PatientLastName END desc, " +
                        "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'asc' THEN c.DischargeDt END asc, " +
                        "CASE WHEN @sortField = 'dischargeDt' And @sortOrder = 'desc' THEN c.DischargeDt END desc " +

                        "FOR JSON AUTO ";
            }
            try
            {
                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return Ok(ex.Message);
            }
            
        }
        
        [Authorize]
        [HttpGet("/qrt/case/{caseId}")]
        public IActionResult GetCaseDetail(long caseId)
        {
            string query = string.Empty;
            query = "select c.*, " +
                       "(select qrtCaseExtended.* from QrtCaseExtended where qrtCaseExtended.QrtCaseId = c.QrtCaseId FOR JSON AUTO) as qrtCaseExtended, " +
                       "(select facilityNurseUnit.* from FacilityNurseUnit where facilityNurseUnit.FacilityNurseUnitId = c.FacilityNurseUnitId FOR JSON AUTO) as facilityNurseUnit " +
                       "from QrtCase c " +
                       "where c.QrtCaseId = @caseId FOR JSON AUTO";

            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@caseId", caseId.ToString()));
            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }


        [HttpPost("/qrt/case")]
        public IActionResult Post(QrtCaseCreate model)
        {
            if (model.CaseType != "MORT" && model.CaseType != "MORB")
                return BadRequest();

            if (model.Sex != null && model.Sex != "M" && model.Sex != "F")
                return BadRequest();

            int caseId;

            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
               var objNew = db.QrtCase.Add(new QrtCase {
                    CaseType = model.CaseType,
                    FIN = model.FIN,
                    MRN = model.MRN,
                    PatientFirstName = model.PatientFirstName,
                    PatientMiddleName = model.PatientMiddleName,
                    PatientLastName = model.PatientLastName,
                    Dob = model.Dob == null ? null : DateTime.Parse(model.Dob),
                    Sex = model.Sex,
                    AdmitDt = model.AdmitDt == null ? null : DateTime.Parse(model.AdmitDt),
                    AdmitNote = model.AdmitNote,
                    EventDt = model.EventDt == null ? null : DateTime.Parse(model.EventDt),
                    DischargeDt =  model.DischargeDt == null ? null : DateTime.Parse(model.DischargeDt),
                    DischargeDiagId = model.DischargeDiagId,
                    DischargeDiagText = model.DischargeDiagText,
                    PrimaryDiagId = model.PrimaryDiagId,
                    PrimaryDiagText = model.PrimaryDiagText,
                    FacilityId = model.FacilityId,
                    FacilityNurseUnitId = model.FacilityNurseUnitId,
                    AttendingProviderId = model.AttendingProviderId,
                    CreateTs = DateTime.Now,
                    CreatedByMemberId = model.CreatedByMemberId
                });
                db.SaveChanges();
                caseId = objNew.Entity.QrtCaseId;
            }
            return Ok(caseId);
        }

        [HttpPut("/qrt/case/{caseId}")]
        public IActionResult Put(long caseId, QrtCaseCreate model)
        {
           
            if (model.CaseType != "MORT" && model.CaseType != "MORB")
                return BadRequest();
            if (model.Sex != "M" && model.Sex != "F")
                return BadRequest();

            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.QrtCase.FirstOrDefault(x => x.QrtCaseId == caseId);
                if (obj != null)
                {
   
                    obj.CaseType = model.CaseType;
                    obj.FIN = model.FIN;
                    obj.MRN = model.MRN;
                    obj.PatientFirstName = model.PatientFirstName;
                    obj.PatientMiddleName = model.PatientMiddleName;
                    obj.PatientLastName = model.PatientLastName;
                    obj.Dob = model.Dob == null ? null : DateTime.Parse(model.Dob);
                    obj.Sex = model.Sex;
                    obj.AdmitDt = model.AdmitDt == null ? null : DateTime.Parse(model.AdmitDt);
                    obj.AdmitNote = model.AdmitNote;
                    obj.EventDt = model.EventDt == null ? null : DateTime.Parse(model.EventDt);
                    obj.DischargeDt = model.DischargeDt == null ? null : DateTime.Parse(model.DischargeDt);
                    obj.DischargeDiagId = model.DischargeDiagId;
                    obj.DischargeDiagText = model.DischargeDiagText;
                    obj.PrimaryDiagId = model.PrimaryDiagId;
                    obj.PrimaryDiagText = model.PrimaryDiagText;
                    obj.FacilityId = model.FacilityId;
                    obj.FacilityNurseUnitId = model.FacilityNurseUnitId;
                    obj.AttendingProviderId = model.AttendingProviderId;

                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Accepted();
        }

        [HttpDelete("/qrt/case/{caseId}")]
        public IActionResult Delete(long caseId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.QrtCase.FirstOrDefault(x => x.QrtCaseId == caseId);
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




        [HttpPost("/qrt/caseext")]
        public IActionResult Post(QrtCaseExtendedCreate model)
        {
            if (model.CaseId == 0)
                return BadRequest();

            setConditionalValues(model);

            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var objCase = db.QrtCase.FirstOrDefault(x => x.QrtCaseId == model.CaseId);

                InsertQrtCaseExtended(db, model, model.CaseId, objCase.FIN);
                db.SaveChanges();
            }

            return Ok();
        }

        [HttpPut("/qrt/caseext/{caseId}")]
        public IActionResult Put(long caseId, QrtCaseExtendedCreate model)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var objCase = db.QrtCase.FirstOrDefault(x => x.QrtCaseId == caseId);
                if(objCase == null) return BadRequest();

                var obj = db.QrtCaseExtended.FirstOrDefault(x => x.QrtCaseId == caseId);

                setConditionalValues(model);

                if (obj == null)
                {
                    InsertQrtCaseExtended(db, model, Convert.ToInt32(caseId), objCase.FIN);
                    db.SaveChanges();
                }
                else
                {
                    obj.QrtCaseId = model.CaseId;
                    obj.FIN = objCase.FIN;
                    obj.Status = model.Status;
                    obj.MemberIdAssigned = model.MemberIdAssigned;
                    obj.Note = model.Note;
                    obj.NoteInline = model.NoteInline;
                    obj.Screen_IsDeathExpectedAtAdmission = model.Screen_IsDeathExpectedAtAdmission;
                    obj.Screen_IsDeathExpectedAtTimeOfDeath = model.Screen_IsDeathExpectedAtTimeOfDeath;
                    obj.Screen_IsSepsis = model.Screen_IsSepsis;
                    obj.Screen_IsReadmittedWithin30Days = model.Screen_IsReadmittedWithin30Days;
                    obj.Screen_SummaryOfDeath = model.Screen_SummaryOfDeath;
                    obj.Pr_IsNeeded = model.Pr_IsNeeded;
                    obj.Pr_DtInitiated = model.Pr_DtInitiated == null ? null : DateTime.Parse(model.Pr_DtInitiated);
                    obj.Pr_ProviderId1 = model.Pr_ProviderId1;
                    obj.Pr_ProviderId2 = model.Pr_ProviderId2;
                    obj.Pr_ProviderId3 = model.Pr_ProviderId3;

                    obj.PrSource_IsRl6 = model.PrSource_IsRl6;
                    obj.Pr_Source_IsMM = model.Pr_Source_IsMM;
                    obj.Pr_Source_MM_DeptId = model.Pr_Source_MM_DeptId;
                    obj.Pr_Source_IsScreening = model.Pr_Source_IsScreening;
                    obj.Pr_Source_IsOther = model.Pr_Source_IsOther;
                    obj.Pr_Source_Other_Text = model.Pr_Source_Other_Text;
                    obj.Pr_Refer_IsToDept = model.Pr_Refer_IsToDept;
                    obj.Pr_Refer_DeptId = model.Pr_Refer_DeptId;
                    obj.Pr_Refer_IsToMulti = model.Pr_Refer_IsToMulti;
                    obj.Pr_Refer_IsToSse = model.Pr_Refer_IsToSse;
                    obj.Outcome_IsOpportunityForImprovement = model.Outcome_IsOpportunityForImprovement;
                    obj.Outcome_IsProviderConcern = model.Outcome_IsProviderConcern;
                    obj.Outcome_ProviderConcern_ProviderId1 = model.Outcome_ProviderConcern_ProviderId1;
                    obj.Outcome_ProviderConcern_ProviderId2 = model.Outcome_ProviderConcern_ProviderId2;
                    obj.Outcome_IsSystemConcern = model.Outcome_IsSystemConcern;
                    obj.Outcome_SystemOfConcernNote = model.Outcome_SystemOfConcernNote;
                    obj.Outcome_IsNursingConcern = model.Outcome_IsNursingConcern;
                    obj.Outcome_NursingConcernNote = model.Outcome_NursingConcernNote;
                    obj.Outcome_IsLeadershipCouncilReview = model.Outcome_IsLeadershipCouncilReview;
                    obj.Outcome_Note = model.Outcome_Note;

                    db.SaveChanges();

                }
            }
            return Accepted();
        }
     
        [HttpPatch("/qrt/caseext/{caseId}")]
        public IActionResult UpdateCaseExt(int caseId, 
                                [FromBody] JsonPatchDocument<QrtCaseExtended> patch)
        {
            DBHelper.LogError("UpdateCaseExt.1");
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                //var entity = db.Find<QrtCaseExtended>(id);
                //if (entity == null)
                //    return NoContent();
                //patch.ApplyTo(entity);
                //db.SaveChanges();
                DBHelper.LogError("UpdateCaseExt.2");

                var entity = db.QrtCaseExtended.FirstOrDefault(x => x.QrtCaseId == caseId);
                if (entity != null)
                {
                    DBHelper.LogError("UpdateCaseExt.3");

                    patch.ApplyTo(entity);
                    db.SaveChanges();
                    DBHelper.LogError("UpdateCaseExt.4");
                    
                    setConditionalValuesEntity(entity);
                    db.SaveChanges();
                }
                else
                {
                    var objCase = db.QrtCase.FirstOrDefault(x => x.QrtCaseId == caseId);
                    if (objCase == null) return BadRequest();

                    DBHelper.LogError("UpdateCaseExt.5");
                    db.QrtCaseExtended.Add(new QrtCaseExtended { 
                        QrtCaseId = caseId,
                        FIN = objCase.FIN
                    });
                    db.SaveChanges();

                    entity = db.QrtCaseExtended.FirstOrDefault(x => x.QrtCaseId == caseId);
                    patch.ApplyTo(entity);
                    db.SaveChanges();

                    setConditionalValuesEntity(entity);
                    db.SaveChanges();
                }


            }

            return Ok();
        }

        [HttpPost("/qrt/caseext2/{caseId}")]
        public IActionResult UpdateCaseExt(int caseId, [FromBody] string model)
        {
            var operationStrings = HelperService.GetPatchString(model);
            var ops = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Microsoft.AspNetCore.JsonPatch.Operations.Operation>>(operationStrings);
            var patchDocument = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument(ops, new Newtonsoft.Json.Serialization.DefaultContractResolver());

            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var entity = db.QrtCaseExtended.FirstOrDefault(x => x.QrtCaseId == caseId);
                if (entity != null)
                {
                    patchDocument.ApplyTo(entity);
                    db.SaveChanges();
                }
                else
                {
                    var objCase = db.QrtCase.FirstOrDefault(x => x.QrtCaseId == caseId);

                    db.QrtCaseExtended.Add(new QrtCaseExtended
                    {
                        QrtCaseId = caseId,
                        FIN = objCase.FIN
                    });
                    db.SaveChanges();

                    entity = db.QrtCaseExtended.FirstOrDefault(x => x.QrtCaseId == caseId);
                    patchDocument.ApplyTo(entity);
                    db.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpGet("/qrt/rl6_case_list")]
        public IActionResult Get(string rl6CaseId, string status, string mrn, string encounterNumber, string startDt,
            string endDt, string isSE, string site, int maxRecs = 200, string orderBy = " eventts desc")
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string query = string.Empty;
                IDictionary<string, string> d = new Dictionary<string, string>();


                int i = 0;
                if (!string.IsNullOrEmpty(rl6CaseId))
                    i += 1;
                if (!string.IsNullOrEmpty(mrn))
                    i += 1;
                if (!string.IsNullOrEmpty(encounterNumber))
                    i += 1;

                if (i > 1)
                    return BadRequest("RL6 Id, MRN, and FIN cannot be selected at the same time. Please enter only one option.");


                string statusQry = null;
                string isSEQry = string.Empty;
                if (!string.IsNullOrEmpty(status))
                {
                    if (status.ToLower() == "active")
                        statusQry = " And rc.status in ('NEW', 'IN-PROGRESS') ";
                    else if (status.ToLower() == "closed")
                        statusQry = " And rc.status in ('CLOSED') ";
                }
                if (!string.IsNullOrEmpty(isSE))
                {
                    if (isSE.ToLower() == "true")
                        isSEQry = " And sei.safetyEventId is not null ";
                    else if (isSE.ToLower() == "false")
                        isSEQry = " And sei.safetyEventId is null ";
                }

                d.Add(new KeyValuePair<string, string>("@rl6CaseId", rl6CaseId));
                d.Add(new KeyValuePair<string, string>("@mrn", mrn));
                d.Add(new KeyValuePair<string, string>("@encounterNumber", encounterNumber));
                d.Add(new KeyValuePair<string, string>("@startDt", startDt));
                d.Add(new KeyValuePair<string, string>("@endDt", endDt));
                d.Add(new KeyValuePair<string, string>("@site", site));

                query = "select top " + maxRecs + " * from Rl6Case rc " +
                        "left join SafetyEventItem sei on rc.rl6CaseId = sei.itemId And sei.itemType='RL6' And sei.deleteTs is null " +
                        "left join SafetyEvent se on sei.safetyEventId = se.safetyEventid " +
                        "where (@site IS NULL OR rc.site in (select splitdata from dbo.fnSplitString(@site, ','))) " +
                        "And (@rl6CaseId IS NULL OR rc.rl6CaseId = @rl6CaseId) " +
                        "And (@mrn IS NULL OR rc.mrn like '%' + @mrn + '%') " +
                        "And (@encounterNumber IS NULL OR rc.encounterNumber = @encounterNumber ) " +
                        "And (@startDt IS NULL OR CAST(rc.eventTs as date) between @startDt and @endDt) " +
                        statusQry + isSEQry +
                        "order by " + orderBy + " " +
                        "FOR JSON AUTO";
                var result = DBHelper.RawSqlQuery(query, d);
                return Ok(result);
            }
        }



        private void InsertQrtCaseExtended(Usaweb_DevContext db, QrtCaseExtendedCreate model, int caseId, int? fin)
        {
         
            db.QrtCaseExtended.Add(new QrtCaseExtended
            {
                QrtCaseId = caseId,
                FIN = fin,
                Status = model.Status,
                MemberIdAssigned = model.MemberIdAssigned,
                Note = model.Note,
                NoteInline = model.NoteInline,
                Screen_IsDeathExpectedAtAdmission = model.Screen_IsDeathExpectedAtAdmission,
                Screen_IsDeathExpectedAtTimeOfDeath = model.Screen_IsDeathExpectedAtTimeOfDeath,
                Screen_IsSepsis = model.Screen_IsSepsis,
                Screen_IsReadmittedWithin30Days = model.Screen_IsReadmittedWithin30Days,
                Screen_SummaryOfDeath = model.Screen_SummaryOfDeath,
                Pr_IsNeeded = model.Pr_IsNeeded,
                Pr_DtInitiated = model.Pr_DtInitiated == null ? null : DateTime.Parse(model.Pr_DtInitiated),
                Pr_ProviderId1 = model.Pr_ProviderId1,
                Pr_ProviderId2 = model.Pr_ProviderId2,
                Pr_ProviderId3 = model.Pr_ProviderId3,
                PrSource_IsRl6 = model.PrSource_IsRl6,
                Pr_Source_IsMM = model.Pr_Source_IsMM,
                Pr_Source_MM_DeptId = model.Pr_Source_MM_DeptId,
                Pr_Source_IsScreening = model.Pr_Source_IsScreening,
                Pr_Source_IsOther = model.Pr_Source_IsOther,
                Pr_Source_Other_Text = model.Pr_Source_Other_Text,
                Pr_Refer_IsToDept = model.Pr_Refer_IsToDept,
                Pr_Refer_DeptId = model.Pr_Refer_DeptId,
                Pr_Refer_IsToMulti = model.Pr_Refer_IsToMulti,
                Pr_Refer_IsToSse = model.Pr_Refer_IsToSse,
                Outcome_IsOpportunityForImprovement = model.Outcome_IsOpportunityForImprovement,
                Outcome_IsProviderConcern = model.Outcome_IsProviderConcern,
                Outcome_ProviderConcern_ProviderId1 = model.Outcome_ProviderConcern_ProviderId1,
                Outcome_ProviderConcern_ProviderId2 = model.Outcome_ProviderConcern_ProviderId2,
                Outcome_IsSystemConcern = model.Outcome_IsSystemConcern,
                Outcome_SystemOfConcernNote = model.Outcome_SystemOfConcernNote,
                Outcome_IsNursingConcern = model.Outcome_IsNursingConcern,
                Outcome_NursingConcernNote = model.Outcome_NursingConcernNote,
                Outcome_IsLeadershipCouncilReview = model.Outcome_IsLeadershipCouncilReview,
                Outcome_Note = model.Outcome_Note
            });
        }

        private void setConditionalValues(QrtCaseExtendedCreate model)
        {
            if (model.Pr_IsNeeded.HasValue && !model.Pr_IsNeeded.Value)
            {
                model.Pr_DtInitiated = null;
                model.Pr_ProviderId1 = null;
                model.Pr_ProviderId2 = null;
                model.Pr_ProviderId3 = null;
                model.PrSource_IsRl6 = null;
                model.Pr_Source_IsMM = null;
                model.Pr_Source_MM_DeptId = null;
                model.Pr_Source_IsScreening = null;
                model.Pr_Source_IsOther = null;
                model.Pr_Source_Other_Text = null;
                model.Pr_Refer_IsToDept = null;
                model.Pr_Refer_DeptId = null;
                model.Pr_Refer_IsToMulti = null;
                model.Pr_Refer_IsToSse = null;
                model.Outcome_IsOpportunityForImprovement = null;
                model.Outcome_IsProviderConcern = null;
                model.Outcome_ProviderConcern_ProviderId1 = null;
                model.Outcome_ProviderConcern_ProviderId2 = null;
                model.Outcome_IsSystemConcern = null;
                model.Outcome_SystemOfConcernNote = null;
                model.Outcome_IsNursingConcern = null;
                model.Outcome_NursingConcernNote = null;
                model.Outcome_IsLeadershipCouncilReview = null;
                //model.Outcome_Note = null;
            }
            else if (model.Outcome_IsOpportunityForImprovement.HasValue && !model.Outcome_IsOpportunityForImprovement.Value)
            {
                model.Outcome_IsProviderConcern = null;
                model.Outcome_ProviderConcern_ProviderId1 = null;
                model.Outcome_ProviderConcern_ProviderId2 = null;
                model.Outcome_IsSystemConcern = null;
                model.Outcome_SystemOfConcernNote = null;
                model.Outcome_IsNursingConcern = null;
                model.Outcome_NursingConcernNote = null;
                model.Outcome_IsLeadershipCouncilReview = null;
                //model.Outcome_Note = null;
            }
        }

        private void setConditionalValuesEntity(QrtCaseExtended model)
        {
            if (model.Pr_IsNeeded.HasValue && !model.Pr_IsNeeded.Value)
            {
                model.Pr_DtInitiated = null;
                model.Pr_ProviderId1 = null;
                model.Pr_ProviderId2 = null;
                model.Pr_ProviderId3 = null;
                model.PrSource_IsRl6 = null;
                model.Pr_Source_IsMM = null;
                model.Pr_Source_MM_DeptId = null;
                model.Pr_Source_IsScreening = null;
                model.Pr_Source_IsOther = null;
                model.Pr_Source_Other_Text = null;
                model.Pr_Refer_IsToDept = null;
                model.Pr_Refer_DeptId = null;
                model.Pr_Refer_IsToMulti = null;
                model.Pr_Refer_IsToSse = null;
                model.Outcome_IsOpportunityForImprovement = null;
                model.Outcome_IsProviderConcern = null;
                model.Outcome_ProviderConcern_ProviderId1 = null;
                model.Outcome_ProviderConcern_ProviderId2 = null;
                model.Outcome_IsSystemConcern = null;
                model.Outcome_SystemOfConcernNote = null;
                model.Outcome_IsNursingConcern = null;
                model.Outcome_NursingConcernNote = null;
                model.Outcome_IsLeadershipCouncilReview = null;
                //model.Outcome_Note = null;
            }
            else if (model.Outcome_IsOpportunityForImprovement.HasValue && !model.Outcome_IsOpportunityForImprovement.Value)
            {
                model.Outcome_IsProviderConcern = null;
                model.Outcome_ProviderConcern_ProviderId1 = null;
                model.Outcome_ProviderConcern_ProviderId2 = null;
                model.Outcome_IsSystemConcern = null;
                model.Outcome_SystemOfConcernNote = null;
                model.Outcome_IsNursingConcern = null;
                model.Outcome_NursingConcernNote = null;
                model.Outcome_IsLeadershipCouncilReview = null;
                //model.Outcome_Note = null;
            }
        }

    }
}
     