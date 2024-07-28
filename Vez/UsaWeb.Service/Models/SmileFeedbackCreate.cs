namespace UsaWeb.Service.Models
{
    public class SmileFeedbackCreate
    {
        public int SourceAssetId { get; set; }

        public string FeedbackValue { get; set;}

        public string FeedbackOneWord { get; set;}

        public string FeedbackComment { get;}

        public string ReviewerEmail { get; set;}

        public string ReviewerPhone { get; set;}


    }
}
