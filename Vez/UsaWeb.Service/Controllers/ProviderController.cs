using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsaWeb.Service.Data;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        [HttpGet("/providers")]
        public IActionResult Get()
        {
            string query = string.Empty;
            query = "Select * from provider where organizationid=1 for json path;";

            var result = DBHelper.RawSqlQuery(query, null);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            string query = string.Empty;
            query = "Select * from provider where organizationid=1 and providerid=@providerid for json path;";

            IDictionary<string, string> d = new Dictionary<string, string>();
            d.Add(new KeyValuePair<string, string>("@providerid", id.ToString()));
            var result = DBHelper.RawSqlQuery(query, d);

            return Ok(result);
        }


        [HttpGet("/providerInsight/providerInsightSummary")]
        public IActionResult Get(string year, string providerName, string hasCaseReviewMortality,
            string hasCaseReviewMorbidity, string hasSafetyEvent, string hasProfConduct, 
            string pgPercentile, string totalColor, string providerInsightSummaryId, string npi, string hasCaseReview, bool? hasSurgicalSiteInfection)
        {
            string hasCaseReviewMortality_sql = string.Empty;
            string hasCaseReviewMorbidity_sql = string.Empty;
            string hasCaseReview_sql = string.Empty;
            string hasSafetyEvent_sql = string.Empty;
            string hasProfConduct_sql = string.Empty;
            string hasSurgicalSiteInfection_sql = string.Empty;

            if (hasCaseReviewMortality == "true")
                hasCaseReviewMortality_sql = "And mortalityReviewCount > 0 ";
            else if (hasCaseReviewMortality == "false")
                hasCaseReviewMortality_sql = "And mortalityReviewCount = 0 ";


            if (hasCaseReviewMorbidity == "true")
                hasCaseReviewMorbidity_sql = "And morbidityReviewCount > 0 ";
            else if (hasCaseReviewMorbidity == "false")
                hasCaseReviewMorbidity_sql = "And morbidityReviewCount = 0 ";

            if (hasCaseReview == "true")
                hasCaseReview_sql = "And (mortalityReviewCount > 0 or  morbidityReviewCount > 0) ";

            if (hasSafetyEvent == "true")
                hasSafetyEvent_sql = "And safetyEventCount > 0 ";
            else if (hasSafetyEvent == "false")
                hasSafetyEvent_sql = "And safetyEventCount = 0 ";

            if (hasProfConduct == "true")
                hasProfConduct_sql = "And profConductCount > 0 ";
            else if (hasProfConduct == "false")
                hasProfConduct_sql = "And profConductCount = 0 ";

            if (hasSurgicalSiteInfection == true)
                hasSurgicalSiteInfection_sql = "And surgicalSiteInfectionCount > 0 ";
            else if (hasSurgicalSiteInfection == false)
                hasSurgicalSiteInfection_sql = "And surgicalSiteInfectionCount = 0 ";

            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            d.Add(new KeyValuePair<string, string>("@year", year));
            d.Add(new KeyValuePair<string, string>("@providerName", providerName));
            //d.Add(new KeyValuePair<string, string>("@hasCaseReviewMortality", hasCaseReviewMortality));
            //d.Add(new KeyValuePair<string, string>("@hasCaseReviewMorbidity", hasCaseReviewMorbidity));
            //d.Add(new KeyValuePair<string, string>("@hasSafetyEvent", hasSafetyEvent));
            //d.Add(new KeyValuePair<string, string>("@hasProfConduct", hasProfConduct));
            d.Add(new KeyValuePair<string, string>("@pgPercentile", pgPercentile));
            d.Add(new KeyValuePair<string, string>("@totalColor", totalColor));
            d.Add(new KeyValuePair<string, string>("@providerInsightSummaryId", providerInsightSummaryId));
            d.Add(new KeyValuePair<string, string>("@npi", npi));
           


            query = "Select * from ProviderInsightSummary " +
                    "Where (@year IS NULL OR year in (select splitdata from dbo.fnSplitString(@year, ','))) " +
                    "And (@providerName IS NULL OR providerName like '%' + @providerName + '%' ) " +
                    hasCaseReviewMortality_sql + hasCaseReviewMorbidity_sql + hasCaseReview_sql + 
                    hasSafetyEvent_sql + hasProfConduct_sql + hasSurgicalSiteInfection_sql +
                    //"And (@hasCaseReviewMortality IS NULL OR mortalityReviewCount > 0 ) " +
                    //"And (@hasCaseReviewMorbidity IS NULL OR morbidityReviewCount > 0 ) " +
                    //"And (@hasSafetyEvent IS NULL OR safetyEventCount > 0 ) " +
                    //"And (@hasProfConduct IS NULL OR profConductCount > 0 ) " +
                    "AND (@pgPercentile IS NULL OR pgPercentile in (select splitdata from dbo.fnSplitString(@pgPercentile, ','))) " +
                    "AND (@totalColor IS NULL OR totalColor in (select splitdata from dbo.fnSplitString(@totalColor, ','))) " +
                    "And (@providerInsightSummaryId IS NULL OR providerInsightSummaryId = @providerInsightSummaryId ) " +
                    "And (@npi IS NULL OR npi = @npi ) " +
                    "Order by totalScore desc, year " +
                    "FOR JSON PATH";

            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }




        [HttpGet("/professional_conduct")]
        public IActionResult Get(string npi, string startDt,
           string endDt, string profConductId)
        {
            string query = string.Empty;
            IDictionary<string, string> d = new Dictionary<string, string>();

            d.Add(new KeyValuePair<string, string>("@npi", npi));
            d.Add(new KeyValuePair<string, string>("@startDt", startDt));
            d.Add(new KeyValuePair<string, string>("@endDt", endDt));
            d.Add(new KeyValuePair<string, string>("@profConductId", profConductId));

            query = "select top 500 * from profConduct pc " +
                    "Where (@npi IS NULL OR npi = @npi ) " +

                    "And (@startDt IS NULL OR eventDt between @startDt and @endDt) " +
                    "And (@profConductId IS NULL OR profConductId=@profConductId) " +

                    "FOR JSON PATH";

            var result = DBHelper.RawSqlQuery(query, d);
            return Ok(result);
        }








    }
}
