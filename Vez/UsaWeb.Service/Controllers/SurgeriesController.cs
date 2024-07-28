using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurgeriesController : ControllerBase
    {   
        private readonly ILogger<SurgeriesController> _logger;

        public SurgeriesController(ILogger<SurgeriesController> logger)
        {
            _logger = logger;
        }


        [HttpGet("/surgeries/list")]
        public async Task<Array> Get(string aptDtStart, string aptDtEnd, string specialty, string surgeon,
           string dept, string statusfilter, string userAssignedFilter, string search, string sortList,
           string includeExlude, bool? isArrival, bool? isNeedClearance, int maxrows = 1000)
        { 
            string searchValue = search; //= (search != "null" ? search : null);
            string search_first = null;
            string search_last = null;
            string status = null;

            //specialty = (specialty != "null" ? specialty : null);
            //surgeon = (surgeon != "null" ? surgeon : null);
            if (!string.IsNullOrEmpty(statusfilter))
                statusfilter = statusfilter.Replace("IN PROGRESS", "IN_PROGRESS");
            if (!isArrival.HasValue)
                isArrival = null;
            if (!isNeedClearance.HasValue)
                isNeedClearance = null;

            if (searchValue != null)
            {
                var arr = searchValue.Split(' ');
                if (arr.Length == 2 && arr.FirstOrDefault(x => x.Contains("first:")) != null &&
                                       arr.FirstOrDefault(x => x.Contains("last:")) != null)
                {
                    search_first = arr.FirstOrDefault(x => x.Contains("first:")).Replace("first:", "");
                    search_last = arr.FirstOrDefault(x => x.Contains("last:")).Replace("last:", "");
                    searchValue = null;
                }
                else if (searchValue.Contains("first:"))
                {
                    search_first = searchValue.Replace("first:", "");
                    searchValue = null;
                }

                else if (searchValue.Contains("last:"))
                {
                    search_last = searchValue.Replace("last:", "");
                    searchValue = null;
                }
            }
            try
            {
                using (Usaweb_DevContext db = new Usaweb_DevContext())
                {
                    var sp_call = new Usaweb_DevContextProcedures(db);

                    //"2:User, Surgical|36:user, Ehr"
                    List<string> list = new List<string>();
                    if (userAssignedFilter != null)
                    {
                        var arr1 = userAssignedFilter.Split('|');
                        foreach (var item in arr1)
                        {
                            var arr2 = item.Split(':');
                            list.Add(arr2[0]);
                        }
                    }
                    string _userAssignedFilter = string.Join(",", list.ToArray());
                    var result = await sp_call.sp_GetSS_RawResultAsync(aptDtStart, aptDtEnd, status, sortList, specialty,
                            surgeon, dept, statusfilter, _userAssignedFilter, searchValue, search_first, search_last,
                            includeExlude, isArrival, isNeedClearance);
                    return result.Take(maxrows).ToArray();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        [HttpGet]
        public async Task<DataVM> Get(string dateStart, string dateEnd, string specialty, string surgeon, 
            string dept,string statusfilter, string userAssignedFilter, string search, string sortList, 
            int maxrows, string includeExlude, bool? isArrival, bool? isNeedClearance)
        {
            DataVM vm = new DataVM();
            vm.Flag = true;

            string searchValue =(search != "null" ? search : null);
            string search_first = null;
            string search_last = null;
            string status = null;
          
            specialty = (specialty != "null" ? specialty : null);
            surgeon = (surgeon != "null" ? surgeon : null);
            if (!string.IsNullOrEmpty(statusfilter))
                statusfilter = statusfilter.Replace("IN PROGRESS", "IN_PROGRESS");
            if (!isArrival.Value)
                isArrival = null;
            if (!isNeedClearance.Value)
                isNeedClearance = null;

            if (searchValue != null)
            {
                var arr = searchValue.Split(' ');
                if (arr.Length == 2 && arr.FirstOrDefault(x => x.Contains("first:")) != null &&
                                       arr.FirstOrDefault(x => x.Contains("last:")) != null)
                {
                    search_first = arr.FirstOrDefault(x => x.Contains("first:")).Replace("first:", "");
                    search_last = arr.FirstOrDefault(x => x.Contains("last:")).Replace("last:", "");
                    searchValue = null;
                }
                else if (searchValue.Contains("first:"))
                {
                    search_first = searchValue.Replace("first:", "");
                    searchValue = null;
                }

                else if (searchValue.Contains("last:"))
                {
                    search_last = searchValue.Replace("last:", "");
                    searchValue = null;
                }
            }
            try
            {
                using (Usaweb_DevContext db = new Usaweb_DevContext())
                {
                    var sp_call = new Usaweb_DevContextProcedures(db);

                    //"2:User, Surgical|36:user, Ehr"
                    List<string> list = new List<string>();
                    if (userAssignedFilter != null)
                    {
                       var arr1 = userAssignedFilter.Split('|');
                        foreach (var item in arr1)
                        {
                            var arr2 = item.Split(':');
                            list.Add(arr2[0]);
                        }
                    }
                    string _userAssignedFilter = string.Join(",", list.ToArray());
                    var result = await sp_call.sp_GetSS_RawResultAsync(dateStart, dateEnd, status, sortList, specialty, 
                            surgeon, dept, statusfilter, _userAssignedFilter, searchValue, search_first, search_last,
                            includeExlude, isArrival, isNeedClearance);
                    vm.Data = result.Take(maxrows).ToArray();
                    return vm;
                }
            }
            catch (Exception ex)
            {
                vm.isError = true;
                vm.Error = ex.Message;
                return vm;
            }
        }

        [HttpGet("{surg_case_nbr}")]
        public Array Get(string surg_case_nbr)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var result = db.SurgicalScheduleRaw.Where(x=>x.surg_case_nbr == surg_case_nbr);
                return result.ToArray();
            }
        }

        [HttpGet("/Surgeries/Dept")]
        public Array GetDept()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var result = db.SurgicalScheduleRaw.Select(x => x.dept).Distinct().OrderBy(x => x);
                return result.ToArray();
            }
        }

        [HttpGet("/Surgeries/GetSurgicalListByMrn")]
        public async Task<SurgeriesNoteVM> GetSurgicalListByMrn(int mrn, int memberId)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                List<SurgicalNote> noteList = new List<SurgicalNote>();
                SurgeriesNoteVM vm = new SurgeriesNoteVM();

                var sp_call = new Usaweb_DevContextProcedures(db);
                var result = await sp_call.sp_GetSS_RawResultByMrnAsync(mrn);
                vm.SurgicalScheduleRaw = result.ToArray();
                //////////////////////////////////////////////////////////////////////////////////////////
                var nlist1 = db.Note.Where(x => x.FkTable == "Patient" && x.FkId == mrn.ToString()
                                                   && x.MemberIdCreatedBy == memberId)
                                       .OrderByDescending(x => x.CreateTs).ToList();
                var member = db.Member.FirstOrDefault(x => x.MemberId == memberId);
                foreach (var sItem in nlist1)
                {
                    noteList.Add(new SurgicalNote
                    {
                        note = sItem.NoteText,
                        displayDate = sItem.CreateTs.ToString(),
                        displayName = member.FirstName + " " + member.LastName,
                        isMain = true
                    });
                }
                vm.NoteList = noteList.ToArray();
                //////////////////////////////////////////////////////////////////////////////////////////
                List<MemberTextValue> mtv = new List<MemberTextValue>();
   
                var mList = db.Member.Where(x => x.Dept == "SURGERY" && x.Status == "ACTIVE");
                foreach (var item in mList)
                {
                    mtv.Add(new MemberTextValue { 
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        FullName = item.FullName,
                        MemberId = item.MemberId,
                        TagColor = item.TagColor
                    });
                }
                vm.MemberList = mtv.ToArray();
                //////////////////////////////////////////////////////////////////////////////////////////
                var callresult = await sp_call.sp_GetSS_CallsByMrnAsync(mrn);
                
                vm.InCompleteCallList = callresult.ToArray();
                return vm;
            }
        }

        [HttpGet("/Surgeries/GetSurgeryUserList")]
        public Array GetSurgeryUserList()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                SurgeriesNoteVM vm = new SurgeriesNoteVM();

                List<MemberTextValue> mtv = new List<MemberTextValue>();

                mtv.Add(new MemberTextValue { 
                    FullName = "Unassigned",
                    MemberId = -1
                });
                var mList = db.Member.Where(x => x.Dept == "SURGERY" && x.Status == "ACTIVE").OrderBy(o => o.FullName);
                foreach (var item in mList)
                {
                    mtv.Add(new MemberTextValue
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        FullName = item.FullName,
                        MemberId = item.MemberId,
                        TagColor = item.TagColor
                    });
                }
                return mtv.ToArray();
            }
        }

        [HttpPost]
        [Route("/Surgeries/Save")]
        public ResultMessage Post(SurgicalNote item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                string FkTable = "SurgicalScheduleRaw";
                string FkId = item.surgCaseNbr;
                if (item.isMain) 
                {
                    FkTable = "Patient";
                    FkId = item.mrn;
                }

                db.Note.Add(new Models.Note
                {
                    FkTable = FkTable,
                    FkId = FkId,
                    NoteText = item.note,
                    CreateTs = DateTime.Now,
                    MemberIdCreatedBy = item.memberId
                });
                db.SaveChanges();
                
                return new ResultMessage { Status = "ok" };
            }
        }

        [HttpPost]
        [Route("/Surgeries/SaveLog")]
        public ResultMessage Post(LogVm item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {  
                db.Log.Add(new Models.Log
                {
                    Type = item.type,
                    ApplicationId = item.applicationId,
                    Value1 = item.value1,
                    Value2 = item.value2,
                    MemberId = item.memberId,
                    CreateTs = DateTime.Now
                });
                db.SaveChanges();

                return new ResultMessage { Status = "ok" };
            }
        }

        [HttpPost]
        [Route("/Surgeries/ChangeStatus")]
        public ResultMessage Post(StatusVM item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                if (obj == null)
                {
                    db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                    {
                        surg_case_nbr = item.surgCaseNbr,
                        Status = item.value
                    });
                }
                else 
                {
                    obj.Status = item.value;
                }
                db.SaveChanges();

                return new ResultMessage { Status = item.token };
            }
        }

        [HttpPost]
        [Route("/Surgeries/ChangeUserAssign")]
        public ResultMessage Post(UserAssignVM item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                if (obj == null)
                {
                    db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                    {
                        surg_case_nbr = item.surgCaseNbr,
                        MemberIdAssigned = item.memberIdAssigned
                    });
                }
                else
                {
                    if (item.memberIdAssigned == 0 || item.memberIdAssigned == -1)
                        obj.MemberIdAssigned = null;
                    else
                        obj.MemberIdAssigned = item.memberIdAssigned;
                }
                db.SaveChanges();

                return new ResultMessage { Status = item.token };
            }
        }


        [HttpPost]
        [Route("/Surgeries/ChangeSSDetailCall")]
        public ResultMessage Post(SSDetailCall item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {  
                var obj = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                if (obj == null)
                {
                    DateTime? Call1CompletedTs = null;
                    int? Call1CompletedMemberId = null;
                    DateTime? Call2CompletedTs = null;
                    int? Call2CompletedMemberId = null;
                    if (item.callType == 1)
                    {
                        Call1CompletedTs = DateTime.Now;
                        Call1CompletedMemberId = item.memberId;
                    }
                    else if (item.callType == 2)
                    {
                        Call2CompletedTs = DateTime.Now;
                        Call2CompletedMemberId = item.memberId;
                    }
                    db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                    {
                        surg_case_nbr = item.surgCaseNbr,
                        Call1CompletedTs = Call1CompletedTs,
                        Call1CompletedMemberId = Call1CompletedMemberId,
                        Call2CompletedTs = Call2CompletedTs,
                        Call2CompletedMemberId = Call2CompletedMemberId
                    });
                }
                else
                {
                    if (item.callType == 1)
                    {
                        if (item.callValue == 1)
                        {
                            obj.Call1CompletedTs = DateTime.Now;
                            obj.Call1CompletedMemberId = item.memberId;
                        }
                        else
                        {
                            obj.Call1CompletedTs = null;
                            obj.Call1CompletedMemberId = null;
                        }
                    }
                    else if (item.callType == 2)
                    {
                        if (item.callValue == 1)
                        {
                            obj.Call2CompletedTs = DateTime.Now;
                            obj.Call2CompletedMemberId = item.memberId;
                        }
                        else
                        {
                            obj.Call2CompletedTs = null;
                            obj.Call2CompletedMemberId = null;
                        }
                    }
                }
                db.SaveChanges();

                if ((item.callType == 1 || item.callType == 2) && item.callValue == 1) 
                {
                    db.SurgicalScheduleCall.Add(new SurgicalScheduleCall
                    {
                        sug_case_nbr = item.surgCaseNbr,
                        CreateTs = DateTime.Now,
                        MemberId = item.memberId,
                        CallResult = "COMPLETE"
                    });
                    db.SaveChanges();
                }

                return new ResultMessage { Status = item.token };
            }
        }

        [HttpPost]
        [Route("/Surgeries/SaveSSDetailNotes")]
        public ResultMessage Post(SSDetailNote item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                if (obj == null)
                {
                    if (item.isMedical)
                        db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                        {
                            surg_case_nbr = item.surgCaseNbr,
                            PaperText = item.detailNote
                        });
                    else
                        db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                        {
                            surg_case_nbr = item.surgCaseNbr,
                            NoteLatestText = item.detailNote,
                            NoteCreatedByTs = DateTime.Now,
                            NoteCreatedByMemberId = item.memberId
                        });
                }
                else
                {
                    if (item.isMedical)
                        obj.PaperText = item.detailNote;
                    else
                    {
                        obj.NoteLatestText = item.detailNote;
                        obj.NoteCreatedByTs = DateTime.Now;
                        obj.NoteCreatedByMemberId = item.memberId;
                    }
                }
                db.SaveChanges();

                return new ResultMessage { Status = item.token };
            }
        }

        [HttpPost]
        [Route("/Surgeries/SaveSSDetailCallInComplete")]
        public SSCallLogView Post(SSDetailCallIncomplete item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                SSCallLogView retObj = null;
                var obj = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                if (obj == null)
                {
                    if (item.callResult == "VOICEMAIL")
                        db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                        {
                            surg_case_nbr = item.surgCaseNbr,
                            CallVoicemailCount = 1
                        });
                    else if (item.callResult == "NO_ANSWER")
                        db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                        {
                            surg_case_nbr = item.surgCaseNbr,
                            CallNoAnswerCount = 1
                        });
                    db.SaveChanges();
                    var nObj = db.SurgicalScheduleCall.Add(new SurgicalScheduleCall
                    {
                        sug_case_nbr = item.surgCaseNbr,
                        CreateTs = DateTime.Now,
                        MemberId = item.memberId,
                        CallResult = item.callResult
                    }).Entity;
                    db.SaveChanges();

                    retObj = new SSCallLogView { 
                        SurgicalScheduleCallId = nObj.SurgicalScheduleCallId,
                        surgCaseNbr = nObj.sug_case_nbr,
                        CreateTs = nObj.CreateTs,
                        memberId = nObj.MemberId,
                        callResult = nObj.CallResult,
                        fullName = db.Member.FirstOrDefault(x=>x.MemberId == nObj.MemberId).FullName
                    };
                }
                else
                {
                    if (item.callResult == "VOICEMAIL")
                        obj.CallVoicemailCount = obj.CallVoicemailCount == null ? 1 : obj.CallVoicemailCount + 1;
                    else if (item.callResult == "NO_ANSWER")
                        obj.CallNoAnswerCount = obj.CallNoAnswerCount == null ? 1 : obj.CallNoAnswerCount + 1;
                    db.SaveChanges();
                    var nObj = db.SurgicalScheduleCall.Add(new SurgicalScheduleCall
                    {
                        sug_case_nbr = item.surgCaseNbr,
                        CreateTs = DateTime.Now,
                        MemberId = item.memberId,
                        CallResult = item.callResult
                    }).Entity;
                    db.SaveChanges();

                    retObj = new SSCallLogView
                    {
                        SurgicalScheduleCallId = nObj.SurgicalScheduleCallId,
                        surgCaseNbr = nObj.sug_case_nbr,
                        CreateTs = nObj.CreateTs,
                        memberId = nObj.MemberId,
                        callResult = nObj.CallResult,
                        fullName = db.Member.FirstOrDefault(x => x.MemberId == nObj.MemberId).FullName
                    };
                }
              

                return retObj;
            }
        }


        [HttpPost]
        [Route("/Surgeries/RemoveSSDetailCallLog")]
        public ResultMessage Post(SSDetailRemoveCallVM item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.SurgicalScheduleCall.FirstOrDefault(x => x.SurgicalScheduleCallId == item.surgicalScheduleCallId);
                if (obj != null)
                {
                    var objMain = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == obj.sug_case_nbr);
                    if (objMain != null)
                    {
                        if (obj.CallResult == "VOICEMAIL")
                            objMain.CallVoicemailCount = objMain.CallVoicemailCount == null ? 0 : objMain.CallVoicemailCount - 1;
                        else if (obj.CallResult == "NO_ANSWER")
                            objMain.CallNoAnswerCount = objMain.CallNoAnswerCount == null ? 0 : objMain.CallNoAnswerCount - 1;
                        db.SaveChanges();
                    }
                    db.SurgicalScheduleCall.Remove(obj);
                    db.SaveChanges();
                }
                return new ResultMessage { Status = item.token };
            }
        }

        [HttpPost]
        [Route("/Surgeries/UpdateSSDetailArrival")]
        public ResultMessage Post(SSDetailUpdateArrival item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var main = db.SurgicalScheduleRaw.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                var objMain = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                DateTime? arrDate = null;
                
                if (string.IsNullOrEmpty(item.arrivalTs) || item.arrivalTs == "Invalid date")
                    arrDate = main.apt_dt_tm.Value.AddHours(-2);
                else
                    arrDate = DateTime.Parse(item.arrivalTs);

                if (objMain != null)
                {
                    objMain.ArrivalTs = arrDate;
                    db.SaveChanges();
                }
                else
                {
                    db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                    {
                        surg_case_nbr = item.surgCaseNbr,
                        ArrivalTs = arrDate
                    });
                    db.SaveChanges();
                }
                return new ResultMessage { Status = item.token };
            }
        }

        [HttpPost]
        [Route("/Surgeries/UpdateSSDetailClearance")]
        public ResultMessage Post(SSDetailClearance item)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var objMain = db.SurgicalScheduleExtended.FirstOrDefault(x => x.surg_case_nbr == item.surgCaseNbr);
                DateTime? PulmonaryTs = null;
                DateTime? CardioTs = null;
                if (item.cType == "pulm")
                    PulmonaryTs = DateTime.Now;
                else
                    CardioTs = DateTime.Now;

                if (objMain != null)
                {
                    if (item.cType == "pulm")
                    {
                        if (objMain.ClearanceNeededPulmonaryTs == null && item.isComplete == 0)
                            objMain.ClearanceNeededPulmonaryTs = DateTime.Now;
                        else if (objMain.ClearanceNeededPulmonaryTs != null && objMain.ClearanceNeededPulmonaryCompleteTs != null && item.isComplete == 1)
                            objMain.ClearanceNeededPulmonaryCompleteTs = null;
                        else if (objMain.ClearanceNeededPulmonaryTs != null && item.isComplete == 1)
                            objMain.ClearanceNeededPulmonaryCompleteTs = DateTime.Now;
                        else
                        {
                            objMain.ClearanceNeededPulmonaryTs = null;
                            objMain.ClearanceNeededPulmonaryCompleteTs = null;
                        }
                    }
                    else
                    {
                        if (objMain.ClearanceNeededCardioTs == null && item.isComplete == 0)
                            objMain.ClearanceNeededCardioTs = DateTime.Now;
                        else if (objMain.ClearanceNeededCardioTs != null && objMain.ClearanceNeededCardioCompleteTs != null && item.isComplete == 1)
                            objMain.ClearanceNeededCardioCompleteTs = DateTime.Now;
                        else if (objMain.ClearanceNeededCardioTs != null && item.isComplete == 1)
                            objMain.ClearanceNeededCardioCompleteTs = DateTime.Now;
                        else
                        {
                            objMain.ClearanceNeededCardioTs =  null;
                            objMain.ClearanceNeededCardioCompleteTs = null;
                        }
                    }
                    db.SaveChanges();
                }
                else
                {
                    db.SurgicalScheduleExtended.Add(new SurgicalScheduleExtended
                    {
                        surg_case_nbr = item.surgCaseNbr,
                        ClearanceNeededPulmonaryTs = PulmonaryTs,
                        ClearanceNeededCardioTs = CardioTs
                    });
                    db.SaveChanges();
                }
                return new ResultMessage { Status = item.token };
            }
        }

        [HttpGet("search/{value}")]
        public Array Search(string value)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var result = db.SurgicalScheduleRaw.Where(x => x.patient_first_name.Contains(value) || x.patient_last_name.Contains(value))
                                .OrderByDescending(x => x.surg_create_dt_tm).Take(5);
                return result.ToArray();
            }
        }

        [HttpGet("batchcreatets")]
        public Array BatchCreateTs()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Batch.OrderByDescending(x => x.BatchId).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.Value1 == null)
                    {
                        string[] array = new string[] { obj.CreateTs.ToString() };
                        return array;
                    }
                    else 
                    {
                        string[] array = new string[] { obj.Value1 };
                        return array;
                    }
                    
                }
                return null;
            }
        }

        private bool isInt(string value)
        {
            int Value;
            return int.TryParse(value, out Value);
        }

        private IEnumerable<char> CharsToTitleCase(string s)
        {
            bool newWord = true;
            foreach (char c in s)
            {
                if (newWord) { yield return Char.ToUpper(c); newWord = false; }
                else yield return Char.ToLower(c);
                if (c == ' ') newWord = true;
            }
        }





        [HttpGet("/Surgeries/getextended")]
        public string GetExtended()
        {
            string query = "select * from SurgicalScheduleExtended for json path;";
            var result = DBHelper.RawSqlQuery(query,null);
            return result.ToString();
        }
    }
}
