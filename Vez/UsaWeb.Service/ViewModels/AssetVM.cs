namespace UsaWeb.Service.ViewModels
{
    public class AssetVM
    {
        public string fkOwner { get; set; }

        public int? fkOwnerId { get; set;}

        public string mediaType { get; set;}

        public string url { get; set;}

        public string note { get; set;}  

        public int? memberIdCreatedBy { get; set; }

    }
}
