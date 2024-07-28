namespace UsaWeb.Service.ViewModels
{
    public class WordRequestModel
    {
        public string siteName { get; set; }
        public string provider { get; set; }
        public string patientAge { get; set; }
        public string patientSex { get; set; }
        public string specialty { get; set; }
        public string minuteWaitExamRoom { get; set; }
        public string minuteWaitProvider { get; set; }
        public string dateStart { get; set; }
        public string dateEnd { get; set; }
    }
}
