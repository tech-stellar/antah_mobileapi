using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class StkRepl2Sub
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public decimal OnHandQty { get; set; }
        public string DimCode { get; set; }
        public string ExpirationDate { get; set; }
        public string ZoneID { get; set; }
    }
}