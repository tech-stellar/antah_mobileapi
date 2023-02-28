using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ReturnAssemblyToStock
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string FromJobNum { get; set; }
        public int FromAssemblySeq { get; set; }


        public string FromWarehouseCode { get; set; }
        public string FromBinNum { get; set; }

        public string ToPartNum { get; set; }
        public string ToPartIUM { get; set; }
        public string ToWarehouseCode { get; set; }
        public string ToBinNum { get; set; }
        public string ToLotNum { get; set; }
        public decimal TranQty { get; set; }
        public int LabelCount { get; set; }

        public string Reference { get; set; }
        public string TranNum { get; set; }
        public string CurrentPlant { get; set; }

    }
}