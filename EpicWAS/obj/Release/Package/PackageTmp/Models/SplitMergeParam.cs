using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class SplitMergeParam
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public string ProcessType { get; set; }
        public decimal Qty { get; set; }
        public string UOM { get; set; }
        public string CurrentPlant { get; set; }
    }
}