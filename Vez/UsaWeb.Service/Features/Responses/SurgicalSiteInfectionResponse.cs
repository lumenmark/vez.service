using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsaWeb.Service.Features.Responses
{
    public class SurgicalSiteInfectionResponse
    {
        public int SurgicalSiteInfectionId { get; set; }

        public int? Fin { get; set; }

        public int? Mrn { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastMiddleName { get; set; }

        public string PatientLastName { get; set; }

        public DateOnly? Dob { get; set; }

        public string Sex { get; set; }

        public DateOnly? AdmitDt { get; set; }

        public string AdmitNote { get; set; }
        public string SurgicalProcedure { get; set; }

        public string OutPatientInpatient { get; set; }

        public DateOnly? SurgeryDt { get; set; }

        public DateOnly? EventDt { get; set; }

        public string SurgicalSiteInfectionType { get; set; }

        public string PreOpAntibioticAdminNote { get; set; }

        public bool? preOpAntibioticAdminYN { get; set; }

        public string SkinPrep { get; set; }

        public string SurgicalSiteInfectionSkinPrep { get; set; }

        public string NpiSurgeon1 { get; set; }

        public string NpiSurgeon2 { get; set; }

        public string OrRoom { get; set; }

        public string WoundClassification { get; set; }

        public string Nhsn { get; set; }

        public string NoteInline { get; set; }

        public string Note { get; set; }

        public int? MemberIdCreatedBy { get; set; }

        public int? MemberIdAssigned { get; set; }

        public DateTime? CreateTs { get; set; }

        public string Temp1 { get; set; }

        public string Temp2 { get; set; }

        public string Status { get; set; }

        public bool? HairRemovalYn { get; set; }
        public string SurgicalProcedureRaw { get; set; }

        public string SurgicalSiteInfectionTypeRaw { get; set; }

        // Additional fields from joins
        public string FullName { get; set; }
        public string Npi { get; set; }
        public string NameWithDegree { get; set; }
        public DateTime? ResignedDt { get; set; }
    }

}
