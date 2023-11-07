using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class SplitMergeDetail
    {
        public int RowNo { get; set; }
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public decimal OnHandQty { get; set; }
        public decimal Qty { get; set; }
        public string UOM { get; set; }
        public decimal ConvFact { get; set; }
        public string ConvFactUOM { get; set; }
        public decimal AllocatedQty { get; set; }
          
    }
}