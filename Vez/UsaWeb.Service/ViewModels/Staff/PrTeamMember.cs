using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsaWeb.Service.ViewModels.Staff
{
    public partial class PrTeamMemberModel
    {
        public string memberIdManager { get; set; }
        public string memberIdStaff { get; set; }
        public string year { get; set; }
    }
}
