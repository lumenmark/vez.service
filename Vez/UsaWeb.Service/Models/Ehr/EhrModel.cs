using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsaWeb.Service.Models.Ehr;

namespace UsaWeb.Service.Models
{
    public class EhrModel
    {
        public string fin { get; set; }
        public string mrn { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string birth_date { get; set; }
        public Contact contact { get; set; }
        public Observation observation { get; set; }
        public Daily_Schedule daily_schedule { get; set; }
        public string lab_work_scheduled { get; set; }


    }
}
