namespace UsaWeb.Service.ViewModels
{
    public class ViewMembers
    {
        public int memberId { get; set; }
        public string jNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string dept { get; set; }

        public string status { get; set; }
        public string[] applicationShortNames { get; set; }
    }
}
