﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models
{
    public partial class PrTeamReferral
    {
        [Key]
        public int prTeamReferralId { get; set; }
        public int? memberIdReviewer { get; set; }
        public int? memberIdReviewed { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime createTs { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? deleteTs { get; set; }
    }
}