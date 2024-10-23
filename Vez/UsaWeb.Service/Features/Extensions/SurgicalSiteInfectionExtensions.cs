using UsaWeb.Service.Features.Responses;
using UsaWeb.Service.ViewModels;
using static Amazon.S3.Util.S3EventNotification;


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
                SkinPrep = entity.SkinPrep,
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
                Status = entity.Status,
                NpiSurgeon1 = entity.NpiSurgeon1,
                NpiSurgeon2 = entity.NpiSurgeon2,
                PreOpAntibioticAdminNote = entity.PreOpAntibioticAdminNote,
                preOpAntibioticAdminYN = entity.PreOpAntibioticAdminYn,
                HairRemovalYn = entity.HairRemovalYn,
                SurgicalProcedureRaw = entity.SurgicalProcedureRaw
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
            entity.NpiSurgeon1 = model.NpiSurgeon1;
            entity.NpiSurgeon2 = model.NpiSurgeon2;
            entity.PreOpAntibioticAdminYn = model.preOpAntibioticAdminYN;
            entity.PreOpAntibioticAdminNote = model.PreOpAntibioticAdminNote;
            entity.HairRemovalYn = model.HairRemovalYn;
            entity.SurgicalProcedureRaw = model.SurgicalProcedureRaw;
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
                Status = model.Status,
                NpiSurgeon1 = model.NpiSurgeon1,
                NpiSurgeon2 = model.NpiSurgeon2,
                PreOpAntibioticAdminYn = model.preOpAntibioticAdminYN,
                PreOpAntibioticAdminNote = model.PreOpAntibioticAdminNote,
                HairRemovalYn = model.HairRemovalYn,
                SurgicalProcedureRaw = model.SurgicalProcedureRaw
            };
        }

        public static SurgicalSiteInfectionResponse ToResponse(this Models.SurgicalSiteInfection entity)
        {
            return new SurgicalSiteInfectionResponse
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
                PreOpAntibioticAdminNote = entity.PreOpAntibioticAdminNote,
                preOpAntibioticAdminYN = entity.PreOpAntibioticAdminYn,
                SkinPrep = entity.SkinPrep,
                NpiSurgeon1 = entity.NpiSurgeon1,
                NpiSurgeon2 = entity.NpiSurgeon2,
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
                Status = entity.Status,
                HairRemovalYn = entity.HairRemovalYn,
                SurgicalProcedureRaw = entity.SurgicalProcedureRaw
            };
        }
    }
}
