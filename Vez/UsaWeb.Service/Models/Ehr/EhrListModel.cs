using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsaWeb.Service.Models.Ehr
{
    public class EhrListModel
    {
        public string id { get; set; }
        public bool active { get; set; }
        public string birthDate { get; set; }
        public string name { get; set; }
        public string mrn { get; set; }
        public string cernId { get; set; }

    }
}
