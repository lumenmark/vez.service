namespace UsaWeb.Service.ViewModels
{
    public class EmailJsonApi
    {

        public string fromAddress { get; set; }

        public string fromName { get; set; }

        public string toAddress { get; set; }

        public string toName { get; set; }

        public string subject { get; set; }

        public string body { get; set; }
    }
}
