﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsaWeb.Service.Models
{
    public partial class Asset
    {
        [Key]
        public int assetId { get; set; }
        /// <summary>
        /// PatientSafetyEvent, QrtCase
        /// </summary>
        [StringLength(500)]
        [Unicode(false)]
        public string fkOwner { get; set; }
        public int? fkOwnerId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string mediaType { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string url { get; set; }
        [StringLength(2000)]
        [Unicode(false)]
        public string note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime createTs { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? deleteTs { get; set; }
        public int? memberIdCreatedBy { get; set; }
    }
}