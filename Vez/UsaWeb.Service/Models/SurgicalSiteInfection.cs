using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models;

[Table("SurgicalSiteInfection")]
public partial class SurgicalSiteInfection
{
    [Key]
    [Column("surgicalSiteInfectionId")]
    public int SurgicalSiteInfectionId { get; set; }

    [Column("fin")]
    public int? Fin { get; set; }

    [Column("mrn")]
    public int? Mrn { get; set; }

    [Column("patientFirstName")]
    [StringLength(500)]
    [Unicode(false)]
    public string PatientFirstName { get; set; }

    [Column("patientLastMiddleName")]
    [StringLength(500)]
    [Unicode(false)]
    public string PatientLastMiddleName { get; set; }

    [Column("patientLastName")]
    [StringLength(500)]
    [Unicode(false)]
    public string PatientLastName { get; set; }

    [Column("dob")]
    public DateOnly? Dob { get; set; }

    [Column("sex")]
    [StringLength(20)]
    [Unicode(false)]
    public string Sex { get; set; }

    [Column("admitDt")]
    public DateOnly? AdmitDt { get; set; }

    [Column("admitNote")]
    [StringLength(4000)]
    [Unicode(false)]
    public string AdmitNote { get; set; }

    [Column("surgicalProcedure")]
    [StringLength(50)]
    [Unicode(false)]
    public string SurgicalProcedure { get; set; }

    [Column("outPatientInpatient")]
    [StringLength(50)]
    [Unicode(false)]
    public string OutPatientInpatient { get; set; }

    [Column("surgeryDt")]
    public DateOnly? SurgeryDt { get; set; }

    [Column("eventDt")]
    public DateOnly? EventDt { get; set; }

    [Column("surgicalSiteInfectionType")]
    [StringLength(200)]
    [Unicode(false)]
    public string SurgicalSiteInfectionType { get; set; }

    [Column("preOpAntibioticAdminNote")]
    [StringLength(200)]
    [Unicode(false)]
    public string PreOpAntibioticAdminNote { get; set; }

    [Column("preOpAntibioticAdminYN")]
    public bool? PreOpAntibioticAdminYn { get; set; }

    [Column("skinPrep")]
    [StringLength(50)]
    [Unicode(false)]
    public string SkinPrep { get; set; }

    [Column("npiSurgeon1")]
    [StringLength(200)]
    [Unicode(false)]
    public string NpiSurgeon1 { get; set; }

    [Column("npiSurgeon2")]
    [StringLength(200)]
    [Unicode(false)]
    public string NpiSurgeon2 { get; set; }

    [Column("orRoom")]
    [StringLength(50)]
    [Unicode(false)]
    public string OrRoom { get; set; }

    [Column("woundClassification")]
    [StringLength(50)]
    [Unicode(false)]
    public string WoundClassification { get; set; }

    [Column("nhsn")]
    [StringLength(10)]
    public string Nhsn { get; set; }

    [Column("noteInline")]
    [StringLength(1000)]
    [Unicode(false)]
    public string NoteInline { get; set; }

    [Column("note")]
    [StringLength(4000)]
    [Unicode(false)]
    public string Note { get; set; }

    [Column("memberIdCreatedBy")]
    public int? MemberIdCreatedBy { get; set; }

    [Column("memberIdAssigned")]
    public int? MemberIdAssigned { get; set; }

    [Column("createTs", TypeName = "datetime")]
    public DateTime? CreateTs { get; set; }

    [Column("temp1")]
    [StringLength(4000)]
    [Unicode(false)]
    public string Temp1 { get; set; }

    [Column("temp2")]
    [StringLength(4000)]
    [Unicode(false)]
    public string Temp2 { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; }

    [Column("hairRemovalYN")]
    public bool? HairRemovalYn { get; set; }

    [Column("surgicalProcedureRaw")]
    [StringLength(50)]
    [Unicode(false)]
    public string SurgicalProcedureRaw { get; set; }
}
