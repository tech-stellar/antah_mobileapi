using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PartLot
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public string PartLotDescription { get; set; }
        public string Batch { get; set; }
        public string MfgBatch { get; set; }
        public string MfgLot { get; set; }
        public string HeatNum { get; set; }
        public string FirmWare { get; set; }
        public string BestBeforeDt { get; set; }
        public string MfgDt { get; set; }
        public string CureDt { get; set; }
        public string ExpireDt { get; set; }

        public int LabelCount { get; set; }
        public string CurrentPlant { get; set; }

        public string Warehouse { get; set; }
        public string BinNum { get; set; }
        public decimal OnHandQty { get; set; }

        public string PartDescription { get; set; }
        public string ProdGroup { get; set; }
        public string ProdGroupName { get; set; }

        public decimal AllocatedQty { get; set; }
        public decimal AvailableQty { get; set; }

        public string UOM { get; set; }
    }
}