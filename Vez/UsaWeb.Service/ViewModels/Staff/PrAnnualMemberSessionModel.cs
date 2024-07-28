namespace UsaWeb.Service.ViewModels.Staff
{
    public class PrAnnualMemberSessionModel
    {
        public int memberIdReviewed { get; set; }
        public int prStatusId { get; set; }
        public int year { get; set; }
        public string signature { get; set; }
        public DateTime? staffSignedTs { get; set; }
        public string managerNote { get; set; }
        public string staffNote { get; set; }
    }
}
