using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsaWeb.Service.ViewModels
{
    public class ViewMember
    {
        public int MemberId { get; set; }
        public string JNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Dept { get; set; }

        public string Status { get; set; }
        public string[] SelectedApplication { get; set; }
    }
}
