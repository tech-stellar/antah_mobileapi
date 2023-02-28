using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class MoveInventory
    {
        public string Company { get; set; }
        public string ReqNum { get; set;  }
        public string PartNum { get; set; }
        public string IUM { get; set; }
        public decimal TranQty { get; set; }
        public string FromWarehouseCode { get; set; }
        public string FromBinNum { get; set; }
        public string FromLotNum { get; set; }
        public string ToWarehouseCode { get; set; }
        public string ToBinNum { get; set; }
        public string ToLotNum { get; set; }
        public string Reference { get; set;  }
        public int ReqStatus { get; set;  }
        public string ReqBy { get; set; }
        public DateTime ReqDate { get; set; }
        public string UpdBy { get; set;  }
        public DateTime UpdDate { get; set; }

        public int LabelCount { get; set; }

        //added by yew mun 2020/11/13
        public string Description { get; set; }

        public string CurrentPlant { get; set; }
        public string TranNum { get; set; }
    }
}