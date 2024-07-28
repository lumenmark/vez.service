
namespace UsaWeb.Service.ViewModels
{
    public class MemberSave
    {
        public int? memberid { get; set; }
        public string jnumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string dept { get; set; }

        public string status { get; set; }
        public string[] selectedApplication { get; set; }
        public string editOption { get; set; }
    }
}
