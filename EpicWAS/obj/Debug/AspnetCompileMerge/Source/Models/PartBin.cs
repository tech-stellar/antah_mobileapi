using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PartBin
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get;  set; }
        public string BinNum { get; set; }
        public decimal OnHandQty { get; set; }
    }
}