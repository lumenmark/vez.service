namespace UsaWeb.Service.ViewModels
{
    public class UpdatePatientRound
    {
        public int Mrn { get; set; }
        public bool? Q1YesNo { get; set; }
        public bool? Q2YesNo { get; set; }
        public bool? Q3YesNo { get; set; }
        public bool? Q4YesNo { get; set; }
        public bool? Q5YesNo { get; set; }
        public string Q1Comments { get; set; }
        public string Q2Comments { get; set; }
        public string Q3Comments { get; set; }
        public string Q4Comments { get; set; }
        public string Q5Comments { get; set; }
    }
}
