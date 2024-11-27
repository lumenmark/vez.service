using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Helper;

namespace UsaWeb.Service.Models;

[Table("QrtCaseMeeting")]
public partial class QrtCaseMeeting
{
    [Key]
    [Column("qrtCaseMeetingId")]
    public int QrtCaseMeetingId { get; set; }

    [Column("qrtCaseId")]
    public int QrtCaseId { get; set; }

    [Column("department")]
    [StringLength(50)]
    [Unicode(false)]
    public string Department { get; set; }

    [Column("meetingDt")]
    public DateOnly? MeetingDt { get; set; }

    [Column("reasonForReview")]
    [Unicode(false)]
    public string ReasonForReview { get; set; }

    [Column("attendingNpi1")]
    public int? AttendingNpi1 { get; set; }

    [Column("isOfi")]
    public bool? IsOfi { get; set; }

    [Column("isDeviationFromSoC")]
    public bool? IsDeviationFromSoC { get; set; }

    [Column("notes")]
    [Unicode(false)]
    public string Notes { get; set; }

    [Column("isReferToProcessImprove")]
    public bool? IsReferToProcessImprove { get; set; }

    [Column("referToProcessImproveNotes")]
    [Unicode(false)]
    public string ReferToProcessImproveNotes { get; set; }

    [Column("createTs", TypeName = "datetime")]
    public DateTime CreateTs { get; set; }

    [Column("deleteTs", TypeName = "datetime")]
    public DateTime? DeleteTs { get; set; }
}
