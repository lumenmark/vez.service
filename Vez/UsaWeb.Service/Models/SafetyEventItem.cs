﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models
{
    public partial class SafetyEventItem
    {
        [Key]
        public int safetyEventItemId { get; set; }
        public int? safetyEventId { get; set; }
        public int itemId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string itemType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime createTs { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? deleteTs { get; set; }
    }
}