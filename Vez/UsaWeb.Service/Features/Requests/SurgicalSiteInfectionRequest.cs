using Microsoft.AspNetCore.Http;

namespace UsaWeb.Service.Features.Requests
{
    public class SurgicalSiteInfectionRequest
    {
        public int? SurgicalSiteInfectionId { get; set; }
        public List<string> StatusList { get; set; }
        public DateTime? EventDtStart { get; set; }
        public DateTime? EventDtEnd { get; set; }
        public string ProviderName { get; set; }
        public List<string> SurgeryList { get; set; }
        public List<string> WoundClassificationList { get; set; }
    }
}
