using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class StkRepl2
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public decimal OnHandQty { get; set; }
        public string DimCode { get; set; }
        public decimal MinimumQty { get; set; }
        public decimal SalesOrderQty { get; set; }
        public decimal SuggestQty { get; set; }
        public string Agency { get; set; }

        public string PartDescription { get; set; }
    }
}