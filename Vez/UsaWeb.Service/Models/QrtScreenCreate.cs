namespace UsaWeb.Service.Models
{
    public class QrtScreenCreate
    {
        public int QrtCaseId { get; set; }

        public int? ScreenedByMemberId { get; set; }

        public bool? IsPalliativeCare { get; set; }

        public bool? IsSepsisDiag { get; set; }

        public bool? Is30DayReadmit { get; set; }

        public string CauseOfDeathSummary { get; set; }

        public string FinalDisp { get; set; }

        public bool? IsPeerReviewNeeded { get; set; }

        public bool? IsPeerReviewByDept { get; set; }

        public bool? IsPeerReviewByInterdisc { get; set; }

        public bool? IsPeerReviewBySse { get; set; }

        public string PeerReviewStartdate { get; set; }

    }
}
