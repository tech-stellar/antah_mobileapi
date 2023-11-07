using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class EmpClock
    {
        public string Company { get; set; }
        public string Employee { get; set; }
        public int Shift { get; set; }
        public string CurrentPlant { get; set; }
    }
}