using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsaWeb.Service.ViewModels
{
    public class SurgicalNote
    {
        public string surgCaseNbr { get; set; }
        public string note { get; set; }
        public int memberId { get; set; }
        public string displayDate { get; set; }
        public string displayName { get; set; }
        public bool isMain { get; set; }
        public string mrn { get; set; }
        
    }
}
