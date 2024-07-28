namespace UsaWeb.Service.ViewModels
{
    public class SSCallLogView
    {
        public int SurgicalScheduleCallId { get; set; }
        public string surgCaseNbr { get; set; }
        public DateTime CreateTs { get; set; }
        public int memberId { get; set; }
        public string callResult { get; set; }
        public string fullName { get; set; }

    }
}
