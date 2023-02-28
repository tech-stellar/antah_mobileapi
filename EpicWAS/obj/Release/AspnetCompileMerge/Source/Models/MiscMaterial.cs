using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class MiscMaterial
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public string IUM { get; set; }
        public decimal TranQty { get; set; }
        public string ReasonCode { get; set; }
        public string Reference { get; set; }
        public string CurrentPlant { get; set; }
        public string TranNum { get; set; }
    }
}