using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ShipHeader
    {
        public string Company { get; set; }
        public int PackNum { get; set; }
        public string PickListNum { get; set; }
    }
}