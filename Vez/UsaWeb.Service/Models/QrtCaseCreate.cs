namespace UsaWeb.Service.Models
{
    public class QrtCaseCreate
    {
        public string CaseType { get; set; }

        public int? FIN { get; set; }

        public int? MRN { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientMiddleName { get; set; }

        public string PatientLastName { get; set;}

        public string Dob { get; set; }

        public string Sex { get; set; }

        public string AdmitDt { get; set; }

        public string AdmitNote { get; set; }

        public string EventDt { get; set; }

        public string DischargeDt { get; set; }

        public int? DischargeDiagId { get; set; }

        public string DischargeDiagText { get; set; }

        public long PrimaryDiagId { get; set; }

        public string PrimaryDiagText { get; set;}

        public long? FacilityId { get; set; }

        public long? FacilityNurseUnitId { get; set; }

        public int? AttendingProviderId { get; set; }

        public int? CreatedByMemberId { get; set; }
    }
}
