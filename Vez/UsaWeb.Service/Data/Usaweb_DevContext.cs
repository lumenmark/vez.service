﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UsaWeb.Service.Models;

namespace UsaWeb.Service.Data
{
    public partial class Usaweb_DevContext : DbContext
    {
        public Usaweb_DevContext()
        {
        }

        public Usaweb_DevContext(DbContextOptions<Usaweb_DevContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;  // Disable lazy loading
        }

        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationMember> ApplicationMember { get; set; }
        public virtual DbSet<ApplicationMemberAssigned> ApplicationMemberAssigned { get; set; }
        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<AssetLocation> AssetLocation { get; set; }
        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<PatientRound> PatientRound { get; set; }
        public virtual DbSet<PrAnnualMemberSession> PrAnnualMemberSession { get; set; }
        public virtual DbSet<PrMemberGoal> PrMemberGoal { get; set; }
        public virtual DbSet<PrMemberGoalHistory> PrMemberGoalHistory { get; set; }
        public virtual DbSet<PrTeamMember> PrTeamMember { get; set; }
        public virtual DbSet<PrTeamReferral> PrTeamReferral { get; set; }
        public virtual DbSet<QrtCase> QrtCase { get; set; }
        public virtual DbSet<QrtCaseExtended> QrtCaseExtended { get; set; }
        public virtual DbSet<SafetyEvent> SafetyEvent { get; set; }
        public virtual DbSet<SafetyEventItem> SafetyEventItem { get; set; }
        public virtual DbSet<SmileFeedback> SmileFeedback { get; set; }
        public virtual DbSet<SurgicalScheduleCall> SurgicalScheduleCall { get; set; }
        public virtual DbSet<SurgicalScheduleExtended> SurgicalScheduleExtended { get; set; }
        public virtual DbSet<SurgicalScheduleRaw> SurgicalScheduleRaw { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<WordResponse> WordResponse { get; set; }
        public virtual DbSet<WordResponseRaw> WordResponseRaw { get; set; }
        public virtual DbSet<SurgicalSiteInfection> SurgicalSiteInfections { get; set; }
        public virtual DbSet<NhsnProcedureCategory> NhsnProcedureCategories { get; set; }
        public virtual DbSet<SurgicalSiteInfectionSkinPrep> SurgicalSiteInfectionSkinPreps { get; set; }
        public virtual DbSet<QrtCaseMeeting> QrtCaseMeetings { get; set; }
        public virtual DbSet<QrtCaseMeetingOfi> QrtCaseMeetingOfis { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationMember>(entity =>
            {
                entity.HasOne(d => d.ApplicationShortNameNavigation)
                    .WithMany(p => p.ApplicationMember)
                    .HasForeignKey(d => d.ApplicationShortName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationMember_Application");
            });

            modelBuilder.Entity<ApplicationMemberAssigned>(entity =>
            {
                entity.HasKey(e => e.applicationMemberAssignId)
                    .HasName("PK_ApplicationMemberAssign");

                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.status).HasDefaultValueSql("('ACTIVE')");
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.fkOwner).HasComment("PatientSafetyEvent, QrtCase");

                entity.Property(e => e.mediaType).HasComment("");
            });

            modelBuilder.Entity<AssetLocation>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("('ACTIVE')");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PatientRound>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("('INCOMPLETE')");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.PatientRound)
                    .HasForeignKey(d => d.BatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientRound_Batch");
            });

            modelBuilder.Entity<PrAnnualMemberSession>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.prStatusId).HasComment("0=COMPLETE; 1=NEEDS_GOALS; 2=NEEDS_CHECKIN; 3=NEEDS_YEAR_END_REVIEW;");
            });

            modelBuilder.Entity<PrMemberGoal>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PrMemberGoalHistory>(entity =>
            {
                entity.Property(e => e.prMemberGoalId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PrTeamMember>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PrTeamReferral>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<QrtCase>(entity =>
            {
                entity.Property(e => e.CaseType).HasComment("MORT = mortality; MORB - morbidity");

                entity.Property(e => e.CreatedByMemberId).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<QrtCaseExtended>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Status)
                    .HasDefaultValue("NEW")
                    .HasComment("NEW, IN_SCREEN, ESCALATED, COMPLETE");
            });

            modelBuilder.Entity<SafetyEvent>(entity =>
            {
                entity.Property(e => e.classification).HasDefaultValueSql("('UNCLASSIFIED')");

                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SafetyEventItem>(entity =>
            {
                entity.Property(e => e.createTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SmileFeedback>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SurgicalScheduleCall>(entity =>
            {
                entity.Property(e => e.CallResult).HasComment("COMPLETE, LEFT_VMAIL, NO_ANSWER, CALLBACK_REQ, ");

                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SurgicalScheduleExtended>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(e => e.Word1)
                    .HasName("PK_word");
            });

            modelBuilder.Entity<SurgicalSiteInfection>(entity =>
            {
                entity.HasKey(e => e.SurgicalSiteInfectionId).HasName("PK_SurgicalSiteInfection1");

                entity.Property(e => e.Nhsn).IsFixedLength();
            });
            modelBuilder.Entity<SurgicalSiteInfectionSkinPrep>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.SkinPrep).HasComment("Betadine, CHG, Chloraprep, Alcohol");
            });
            modelBuilder.Entity<QrtCaseMeeting>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<QrtCaseMeetingOfi>(entity =>
            {
                entity.Property(e => e.CreateTs).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.OfiType).HasComment("comm, doc, system, nursing, provider, dept, other");
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}