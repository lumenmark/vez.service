namespace UsaWeb.Service.ViewModels.Staff
{
    public class PrMemberGoalModel
    {
        public int? prAnnualMemberSessionId { get; set; }

        public int? year { get; set; }

        public int? memberIdReviewer { get; set; }

        public int? memberIdReviewed { get; set; }

        public string goalText { get; set; }

        public string targetDt { get; set; }
    }
}
