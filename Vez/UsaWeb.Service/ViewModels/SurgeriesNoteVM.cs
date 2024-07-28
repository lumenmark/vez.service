using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsaWeb.Service.Models;

namespace UsaWeb.Service.ViewModels
{
    public class SurgeriesNoteVM
    {
        public Array SurgicalScheduleRaw { get; set; }     //1 object
        public Array NoteList { get; set; } //multiple objects
        public Array MemberList { get; set; }
        public Array InCompleteCallList { get; set; }
    }
}
