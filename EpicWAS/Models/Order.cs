using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class Order
    {
        public string Company { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRel { get; set; }
        public string PartNum { get; set; }
        public string PartDescription { get; set; }
        public decimal OrderQty { get; set; }
        public string Warehouse { get; set; }
        public string Bin { get; set; }
        public string Lot { get; set; }
        public string IUM { get; set; }
        public string SalesUM { get; set; }
        public string NetWeightUOM { get; set; }

        public string CustNum { get; set; }
        public string ShipToNum { get; set; }

    }
}