using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PickPack2
    {

        public string Company { get; set; }
        public string Customer { get; set; }
        public string PickListNum { get; set; }
        public string TagNum { get; set; }
        public string Picker { get; set; }
        public string StationId { get; set; }
        public string Remark { get; set;  }
        public string OrderNum { get; set; }
        public string PartNum { get; set; }
        public string PartDesc { get; set;}
        public string UOM { get; set; }
        public string LotNum { get; set; }
        public string ExpiryDate { get; set; }
        public decimal PickQty { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal CartonNum { get; set; }
        public string Transporter { get; set; }
        public string PalletNum { get; set; }

        public string BumiAgDONum { get; set; }
        public bool UrgentOrder { get; set; }
        public string SerialNum { get; set; }
        public string ShipVia { get; set; }
        public string ShipViaDesc { get; set; }

        public string MQ_SysRowId { get; set; }
        public string U14_SysRowId { get; set; }
        public string Consignment { get; set; }

    }
}