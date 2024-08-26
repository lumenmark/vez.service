using UsaWeb.Service.ViewModels;


namespace UsaWeb.Service.Features.Extensions
{
    public static class SurgicalSiteInfectionExtensions
    {
        public static SurgicalSiteInfectionViewModel ToViewModel(this Models.SurgicalSiteInfection entity)
        {
            if (entity == null) return null;

            return new SurgicalSiteInfectionViewModel
            {
                SurgicalSiteInfectionId = entity.SurgicalSiteInfectionId,
                Fin = entity.Fin,
                Mrn = entity.Mrn,
                PatientFirstName = entity.PatientFirstName,
                PatientLastMiddleName = entity.PatientLastMiddleName,
                PatientLastName = entity.PatientLastName,
                Dob = entity.Dob,
                Sex = entity.Sex,
                AdmitDt = entity.AdmitDt,
                AdmitNote = entity.AdmitNote,
                SurgicalProcedure = entity.SurgicalProcedure,
                OutPatientInpatient = entity.OutPatientInpatient,
                SurgeryDt = entity.SurgeryDt,
                EventDt = entity.EventDt,
                SurgicalSiteInfectionType = entity.SurgicalSiteInfectionType,
                IsPreOpAntibioticAdmin = entity.IsPreOpAntibioticAdmin,
                SkinPrep = entity.SkinPrep,
                SurgeonNpi1 = entity.SurgeonNpi1,
                SurgeonNpi2 = entity.SurgeonNpi2,
                OrRoom = entity.OrRoom,
                WoundClassification = entity.WoundClassification,
                Nhsn = entity.Nhsn,
                NoteInline = entity.NoteInline,
                Note = entity.Note,
                MemberIdCreatedBy = entity.MemberIdCreatedBy,
                MemberIdAssigned = entity.MemberIdAssigned,
                CreateTs = entity.CreateTs,
                Temp1 = entity.Temp1,
                Temp2 = entity.Temp2,
                Status = entity.Status
            };
        }

        public static void UpdateFromViewModel(this Models.SurgicalSiteInfection entity, SurgicalSiteInfectionViewModel model)
        {
            entity.Fin = model.Fin;
            entity.Mrn = model.Mrn;
            entity.PatientFirstName = model.PatientFirstName;
            entity.PatientLastMiddleName = model.PatientLastMiddleName;
            entity.PatientLastName = model.PatientLastName;
            entity.Dob = model.Dob;
            entity.Sex = model.Sex;
            entity.AdmitDt = model.AdmitDt;
            entity.AdmitNote = model.AdmitNote;
            entity.SurgicalProcedure = model.SurgicalProcedure;
            entity.OutPatientInpatient = model.OutPatientInpatient;
            entity.SurgeryDt = model.SurgeryDt;
            entity.EventDt = model.EventDt;
            entity.SurgicalSiteInfectionType = model.SurgicalSiteInfectionType;
            entity.IsPreOpAntibioticAdmin = model.IsPreOpAntibioticAdmin;
            entity.SkinPrep = model.SkinPrep;
            entity.SurgeonNpi1 = model.SurgeonNpi1;
            entity.SurgeonNpi2 = model.SurgeonNpi2;
            entity.OrRoom = model.OrRoom;
            entity.WoundClassification = model.WoundClassification;
            entity.Nhsn = model.Nhsn;
            entity.NoteInline = model.NoteInline;
            entity.Note = model.Note;
            entity.MemberIdCreatedBy = model.MemberIdCreatedBy;
            entity.MemberIdAssigned = model.MemberIdAssigned;
            entity.CreateTs = model.CreateTs;
            entity.Temp1 = model.Temp1;
            entity.Temp2 = model.Temp2;
            entity.Status = model.Status;
        }

        public static Models.SurgicalSiteInfection ToEntity(this SurgicalSiteInfectionViewModel model)
        {
            if (model == null) return null;

            return new Models.SurgicalSiteInfection
            {
                Fin = model.Fin,
                Mrn = model.Mrn,
                PatientFirstName = model.PatientFirstName,
                PatientLastMiddleName = model.PatientLastMiddleName,
                PatientLastName = model.PatientLastName,
                Dob = model.Dob,
                Sex = model.Sex,
                AdmitDt = model.AdmitDt,
                AdmitNote = model.AdmitNote,
                SurgicalProcedure = model.SurgicalProcedure,
                OutPatientInpatient = model.OutPatientInpatient,
                SurgeryDt = model.SurgeryDt,
                EventDt = model.EventDt,
                SurgicalSiteInfectionType = model.SurgicalSiteInfectionType,
                IsPreOpAntibioticAdmin = model.IsPreOpAntibioticAdmin,
                SkinPrep = model.SkinPrep,
                SurgeonNpi1 = model.SurgeonNpi1,
                SurgeonNpi2 = model.SurgeonNpi2,
                OrRoom = model.OrRoom,
                WoundClassification = model.WoundClassification,
                Nhsn = model.Nhsn,
                NoteInline = model.NoteInline,
                Note = model.Note,
                MemberIdCreatedBy = model.MemberIdCreatedBy,
                MemberIdAssigned = model.MemberIdAssigned,
                CreateTs = model.CreateTs,
                Temp1 = model.Temp1,
                Temp2 = model.Temp2,
                Status = model.Status
            };
        }
    }
}
