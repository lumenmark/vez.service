namespace UsaWeb.Service.ViewModels
{
    public class SafetyEventVM
    {
        public string eventDt { get; set; }

        public int? facilityId { get; set; }

        public string typeGeneral { get; set; }

        public string typeSpecific { get; set; }

        public string personAffectedType { get; set; }

        public string briefDescription { get; set; }

        public string dept { get; set; }

        public string classification { get; set; }

        public string comment { get; set; }

        public string inlineNote { get; set; }

        public string status { get; set; }

        public int? memberIdAssigned { get; set; }

        public string assetURL { get; set; }

        public int? mrn { get; set; }

        public int? fin { get; set; }

        public string personAffected { get; set; }  

        public string rl6CaseIdDelim { get; set; }

        public string qrtCaseIdDelim { get; set; }

        public int? memberIdCreatedBy { get; set; }

        public string grouping { get; set; }

    }
}
