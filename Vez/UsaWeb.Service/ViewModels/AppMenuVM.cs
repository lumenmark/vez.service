using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsaWeb.Service.ViewModels
{
    public class AppMenuVM
    {
        public Array apps { get; set; }
        public Array memberApps { get; set; }
        public string errorMessage { get; set; }
    }
}
