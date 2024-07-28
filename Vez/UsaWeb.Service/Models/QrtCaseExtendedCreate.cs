namespace UsaWeb.Service.Models
{
    public class QrtCaseExtendedCreate
    {
        public int CaseId { get; set; }

        public string Status { get; set; }

        public int? MemberIdAssigned { get; set; }

        public string Note { get; set; }

        public string NoteInline { get; set; }

        public bool? Screen_IsDeathExpectedAtAdmission { get; set; }

        public bool? Screen_IsDeathExpectedAtTimeOfDeath { get; set; }

        public bool? Screen_IsSepsis { get; set; }

        public bool? Screen_IsReadmittedWithin30Days { get; set; }

        public string Screen_SummaryOfDeath { get; set; }

        public bool? Pr_IsNeeded { get; set; }

        public string Pr_DtInitiated { get; set; }

        public int? Pr_ProviderId1 { get; set; }

        public int? Pr_ProviderId2 { get; set; }
        
        public int? Pr_ProviderId3 { get; set; }

        public bool? PrSource_IsRl6 { get; set; }

        public bool? Pr_Source_IsMM { get; set; }

        public int? Pr_Source_MM_DeptId { get; set; }

        public bool? Pr_Source_IsScreening { get; set; }

        public bool? Pr_Source_IsOther { get; set; }

        public string Pr_Source_Other_Text { get; set; }

        public bool? Pr_Refer_IsToDept { get; set; }

        public int? Pr_Refer_DeptId { get; set; }

        public bool? Pr_Refer_IsToMulti { get; set; }

        public bool? Pr_Refer_IsToSse { get; set; }

        public bool? Outcome_IsOpportunityForImprovement { get; set; }

        public bool? Outcome_IsProviderConcern { get; set; }

        public int? Outcome_ProviderConcern_ProviderId1 { get; set; }

        public int? Outcome_ProviderConcern_ProviderId2 { get; set; }

        public bool? Outcome_IsSystemConcern { get; set; }

        public string Outcome_SystemOfConcernNote { get; set; }

        public bool? Outcome_IsNursingConcern { get; set; }

        public string Outcome_NursingConcernNote { get; set; }

        public bool? Outcome_IsLeadershipCouncilReview { get; set; }

        public string Outcome_Note { get; set; }

    }
}
