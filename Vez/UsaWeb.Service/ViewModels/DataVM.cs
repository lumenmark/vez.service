using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsaWeb.Service.ViewModels
{
    public class DataVM
    {
        public bool Flag { get; set; }
        public bool isError { get; set; }
        public string Error { get; set; }
        public Array Data { get; set; }
    }
}
