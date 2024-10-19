using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models;

[Table("SurgicalSiteInfectionSkinPrep")]
public partial class SurgicalSiteInfectionSkinPrep
{
    [Key]
    [Column("surgicalSiteInfectionSkinPrepId")]
    public int SurgicalSiteInfectionSkinPrepId { get; set; }

    [Column("surgicalSiteInfectionId")]
    public int? SurgicalSiteInfectionId { get; set; }

    /// <summary>
    /// Betadine, CHG, Chloraprep, Alcohol
    /// </summary>
    [Column("skinPrep")]
    [StringLength(50)]
    [Unicode(false)]
    public string SkinPrep { get; set; }

    [Column("createTs", TypeName = "datetime")]
    public DateTime CreateTs { get; set; }
}
