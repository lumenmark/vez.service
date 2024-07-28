using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Controllers
{
    public class Ho2WordController : ControllerBase
    {
        [HttpGet("/Ho2Word/DefaultList")]
        public WordDefaultVM GetDefaultList()
        {
            WordDefaultVM model = new WordDefaultVM();
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                model.SiteNameList = db.WordResponse.Select(x => x.SiteName).Distinct().OrderBy(x => x).ToArray();
                model.ProviderList = db.WordResponse.Select(x => x.Provider).Distinct().OrderBy(x => x).ToArray();
                model.PatientAgeRangeList = db.WordResponse.Select(x => x.PatientAgeRange).Distinct().OrderBy(x => x).ToArray();
                model.MinuteWaitExamRoomRangeList = db.WordResponse.Select(x => x.MinuteWaitExamRoomRange).Distinct().OrderBy(x => x).ToArray();
                model.MinuteWaitToSeeCpRangeList = db.WordResponse.Select(x => x.MinuteWaitToSeeCpRange).Distinct().OrderBy(x => x).ToArray();
                model.SpecialtyList = db.WordResponse.Where(x=>x.Specialty != "").Select(x => x.Specialty).Distinct().OrderBy(x => x).ToArray();

                return model;
            }
        }

        [HttpPost("/Ho2Word/GetByPrameters")]
        public async Task<WordResponseModel> Get(WordRequestModel req)
        {
            WordResponseModel model = new WordResponseModel();
            List<WordModel> list = new List<WordModel>();
            try
            {
                using (Usaweb_DevContext db = new Usaweb_DevContext())
                {
                    var sp_call = new Usaweb_DevContextProcedures(db);

                    var result = await sp_call.sp_GetWordsByFilterAsync(req.siteName, req.provider, req.patientAge, req.patientSex,
                               req.specialty, req.minuteWaitExamRoom, req.minuteWaitProvider, req.dateStart, req.dateEnd);
                    //return result.ToArray();
                    if (result !=null)
                    {
                        
                        //int i = 1;
                        foreach (var obj in result)
                        {
                            if (obj.word != "")
                            {
                                string[] words = obj.word.Split(' ');
                                if (words.Length == 1)
                                {
                                    list.Add(new WordModel
                                    {
                                        text = words.First(),
                                        value = obj.tCount.ToString() //i.ToString(),
                                    });
                                    //i = i + 1;
                                }
                            }
                        }
                        var pieResult = await sp_call.sp_GetWordsPieByFilterAsync(req.siteName, req.provider, req.patientAge, req.patientSex,
                             req.specialty, req.minuteWaitExamRoom, req.minuteWaitProvider, req.dateStart, req.dateEnd);
                        model.pie = pieResult.Select(x => new WordPieModel { name = x.reaction, value = x.tcount }).ToList();
                        model.pieTotal = pieResult.Sum(x => x.tcount).Value;
                        //var nList  = result.GroupBy(x => new { x.reaction })
                        //              .Select(x => new WordPieModel{ name = x.Key.reaction, value = x.Count() }).ToList();

                        //  model.pie = nList;
                    }


                    model.words = list;
                }
            }
            catch (Exception ex)
            {
                //return null;
            }
            return model;
        }

    }
}
