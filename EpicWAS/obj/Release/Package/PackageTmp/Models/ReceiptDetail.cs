using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ReceiptDetail
    {
        public string Company { get; set; }
        public int PONum { get; set; }
        public int POLine { get; set; }
        public int POLineRel { get; set; }
        public string VendorID { get; set; }
        public int VendorNum { get; set; }
        public string PackSlip { get; set; }
        public string PartNum { get; set; }
        public string PartDesc { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public decimal TranQty { get; set; }
        public string RecvUOM { get; set; }
        public decimal PORelQty { get; set; }
        public string EntryPerson { get; set; }
        public bool OpenOrder { get; set; }
        public int LabelCount { get; set; }

        public string LegalNumber { get; set; }
        public string PORelPlant { get; set; }
        public string PORelWarehouse { get; set; }

        /* 2021-07-12 yew mun add in the balance qty   */
        public decimal ReceivedQty { get; set; }
        public decimal ArrivedQty { get; set; }
        public decimal BalancedQty { get; set; }
        public decimal POQty { get; set; }

        public bool Approve { get; set; }
        public bool Confirmed { get; set; }

        public DateTime PODate { get; set; }

        public int PackLine { get; set; }
        public string PurPoint { get; set; }


    }
}