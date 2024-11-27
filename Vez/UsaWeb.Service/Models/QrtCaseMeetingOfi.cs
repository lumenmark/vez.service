using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models;

[Table("QrtCaseMeetingOfi")]
public partial class QrtCaseMeetingOfi
{
    [Key]
    [Column("qrtCaseMeetingOfiId")]
    public int QrtCaseMeetingOfiId { get; set; }

    [Column("qrtCaseMeetingId")]
    public int QrtCaseMeetingId { get; set; }

    /// <summary>
    /// comm, doc, system, nursing, provider, dept, other
    /// </summary>
    [Required]
    [Column("ofiType")]
    [StringLength(50)]
    [Unicode(false)]
    public string OfiType { get; set; }

    [Column("notes")]
    [StringLength(500)]
    [Unicode(false)]
    public string Notes { get; set; }

    [Column("providerNpi1")]
    public int? ProviderNpi1 { get; set; }

    [Column("providerNpi2")]
    public int? ProviderNpi2 { get; set; }

    [Column("createTs", TypeName = "datetime")]
    public DateTime CreateTs { get; set; }
}
