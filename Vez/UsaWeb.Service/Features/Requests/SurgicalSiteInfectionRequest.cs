namespace UsaWeb.Service.Features.Requests
{
    public class SurgicalSiteInfectionRequest
    {
        public int? SurgicalSiteInfectionId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string Sex { get; set; }
        public DateTime? CreateTs { get; set; }
    }
}
