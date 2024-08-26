﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsaWeb.Service.ViewModels
{
    public class SurgicalSiteInfectionViewModel
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

        public string IsPreOpAntibioticAdmin { get; set; }

        public string SkinPrep { get; set; }

        public int? SurgeonNpi1 { get; set; }

        public int? SurgeonNpi2 { get; set; }

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
    }
}
