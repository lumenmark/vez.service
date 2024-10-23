using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsaWeb.Service.ViewModels
{
    public class SurgicalSiteInfectionViewModel
    {
        /// <summary>
        /// Gets or sets the surgical site infection identifier.
        /// </summary>
        /// <value>
        /// The surgical site infection identifier.
        /// </value>
        public int SurgicalSiteInfectionId { get; set; }

        public int? Fin { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public int? Mrn { get; set; }

        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        /// <value>
        /// The first name of the patient.
        /// </value>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient last middle.
        /// </summary>
        /// <value>
        /// The name of the patient last middle.
        /// </value>
        public string PatientLastMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        /// <value>
        /// The last name of the patient.
        /// </value>
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateOnly? Dob { get; set; }

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets the admit dt.
        /// </summary>
        /// <value>
        /// The admit dt.
        /// </value>
        public DateOnly? AdmitDt { get; set; }

        /// <summary>
        /// Gets or sets the admit note.
        /// </summary>
        /// <value>
        /// The admit note.
        /// </value>
        public string AdmitNote { get; set; }

        /// <summary>
        /// Gets or sets the surgical procedure.
        /// </summary>
        /// <value>
        /// The surgical procedure.
        /// </value>
        public string SurgicalProcedure { get; set; }

        /// <summary>
        /// Gets or sets the out patient inpatient.
        /// </summary>
        /// <value>
        /// The out patient inpatient.
        /// </value>
        public string OutPatientInpatient { get; set; }

        /// <summary>
        /// Gets or sets the surgery dt.
        /// </summary>
        /// <value>
        /// The surgery dt.
        /// </value>
        public DateOnly? SurgeryDt { get; set; }

        /// <summary>
        /// Gets or sets the event dt.
        /// </summary>
        /// <value>
        /// The event dt.
        /// </value>
        public DateOnly? EventDt { get; set; }

        /// <summary>
        /// Gets or sets the type of the surgical site infection.
        /// </summary>
        /// <value>
        /// The type of the surgical site infection.
        /// </value>
        public string SurgicalSiteInfectionType { get; set; }

        /// <summary>
        /// Gets or sets the pre op antibiotic admin note.
        /// </summary>
        /// <value>
        /// The pre op antibiotic admin note.
        /// </value>
        public string PreOpAntibioticAdminNote { get; set; }

        /// <summary>
        /// Gets or sets the pre op antibiotic admin yn.
        /// </summary>
        /// <value>
        /// The pre op antibiotic admin yn.
        /// </value>
        public bool? preOpAntibioticAdminYN { get; set; }

        /// <summary>
        /// Gets or sets the skin prep.
        /// </summary>
        /// <value>
        /// The skin prep.
        /// </value>
        public string SkinPrep { get; set; }

        /// <summary>
        /// Gets or sets the surgical site infection skin prep.
        /// </summary>
        /// <value>
        /// The surgical site infection skin prep.
        /// </value>
        public string SurgicalSiteInfectionSkinPrep { get; set; }

        /// <summary>
        /// Gets or sets the npi surgeon1.
        /// </summary>
        /// <value>
        /// The npi surgeon1.
        /// </value>
        public string NpiSurgeon1 { get; set; }

        /// <summary>
        /// Gets or sets the npi surgeon2.
        /// </summary>
        /// <value>
        /// The npi surgeon2.
        /// </value>
        public string NpiSurgeon2 { get; set; }

        /// <summary>
        /// Gets or sets the or room.
        /// </summary>
        /// <value>
        /// The or room.
        /// </value>
        public string OrRoom { get; set; }

        /// <summary>
        /// Gets or sets the wound classification.
        /// </summary>
        /// <value>
        /// The wound classification.
        /// </value>
        public string WoundClassification { get; set; }

        /// <summary>
        /// Gets or sets the NHSN.
        /// </summary>
        /// <value>
        /// The NHSN.
        /// </value>
        public string Nhsn { get; set; }

        /// <summary>
        /// Gets or sets the note inline.
        /// </summary>
        /// <value>
        /// The note inline.
        /// </value>
        public string NoteInline { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the member identifier created by.
        /// </summary>
        /// <value>
        /// The member identifier created by.
        /// </value>
        public int? MemberIdCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the member identifier assigned.
        /// </summary>
        /// <value>
        /// The member identifier assigned.
        /// </value>
        public int? MemberIdAssigned { get; set; }

        /// <summary>
        /// Gets or sets the create ts.
        /// </summary>
        /// <value>
        /// The create ts.
        /// </value>
        public DateTime? CreateTs { get; set; }

        /// <summary>
        /// Gets or sets the temp1.
        /// </summary>
        /// <value>
        /// The temp1.
        /// </value>
        public string Temp1 { get; set; }

        /// <summary>
        /// Gets or sets the temp2.
        /// </summary>
        /// <value>
        /// The temp2.
        /// </value>
        public string Temp2 { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the hair removal yn.
        /// </summary>
        /// <value>
        /// The hair removal yn.
        /// </value>
        public bool? HairRemovalYn { get; set; }

        /// <summary>
        /// Gets or sets the surgical procedure raw.
        /// </summary>
        /// <value>
        /// The surgical procedure raw.
        /// </value>
        public string SurgicalProcedureRaw { get; set; }
    }
}
