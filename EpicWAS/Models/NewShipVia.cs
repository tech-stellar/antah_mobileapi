using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class NewShipVia
    {
        public string ShipViaCode { get; set; }
        public string ShipViaDescription { get; set; }
        public decimal SD_MWS_MinWeight_c { get; set;}
        public bool SD_MWS_IsMinWeight_c { get; set; }
        public bool SD_MWS_IsSkipWeight_c { get; set; }
    }
}