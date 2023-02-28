using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PartWhse
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseDescription { get; set; }
        public decimal OnHandQty { get; set; }
    }
}