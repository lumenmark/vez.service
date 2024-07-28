using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsaWeb.Service.Models.Ehr
{
    public class EhrListModel_Message
    {
        public string Message { get; set; }
        public List<EhrListModel> List { get; set; }
    }
}
