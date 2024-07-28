using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UsaWeb.Service.ViewModels
{
    public class ApplicationMemberAssignedVM
    {
        public string applicationShortName { get; set; }
        public string applicationShortName2 { get; set; }
        public int memberId { get; set; }
        public string status { get; set; }
    }
}
