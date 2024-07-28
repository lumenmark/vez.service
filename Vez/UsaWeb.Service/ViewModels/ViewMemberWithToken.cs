namespace UsaWeb.Service.ViewModels
{
    public class ViewMemberWithToken
    {
        public int memberId { get; set; }
        public string jNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string dept { get; set; }
        public string status { get; set; }
        public string tagColor { get; set; }
        public string jwtToken { get; set; }
        public Array memberApps { get; set; }
    }
}
