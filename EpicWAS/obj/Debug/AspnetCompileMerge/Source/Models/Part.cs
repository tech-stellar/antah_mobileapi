using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class Part
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string PartDescription { get; set; }
        public string IUM { get; set; }
        public string TypeCode { get; set; }
        public bool NonStock { get; set; }
        public string ProdCode { get; set; }
        public string CostMethod { get; set; }
        public bool QtyBearing { get; set; }
        public bool TrackLots { get; set; }
        public bool OnHold { get; set; }
        public bool InActive { get; set; }
    }
}