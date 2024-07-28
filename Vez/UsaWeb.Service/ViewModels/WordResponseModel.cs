using System.Collections.Generic;

namespace UsaWeb.Service.ViewModels
{
    public class WordResponseModel
    {
        public List<WordModel> words { get; set; }
        public List<WordPieModel> pie { get; set; }
        public int pieTotal { get; set; }
    }
}
