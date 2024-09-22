using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models;

[Table("NhsnProcedureCategory")]
public partial class NhsnProcedureCategory
{
    [Key]
    [Column("nhsnProcedureCategoryCode")]
    [StringLength(20)]
    [Unicode(false)]
    public string NhsnProcedureCategoryCode { get; set; }

    [Column("procedureCategoryName")]
    [StringLength(50)]
    [Unicode(false)]
    public string ProcedureCategoryName { get; set; }
}
