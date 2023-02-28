using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ReprintInvoice
    {
        public string Company { get; set; }
        public string InvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public string LegalNumber { get; set; }
        public string InvoiceComment { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRel { get; set; }
        public int PackNum { get; set; }
        public int PackLine { get; set; }
        public decimal Quantity { get; set; }
        public string Warehouse { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public string PartNum { get; set; }
        public string PartDescription { get; set; }
        public string UOM { get; set; }
        public decimal Weight { get; set; }
        public string ShipVia { get; set; }
        public int Carton { get; set; }
        public string TrackingNum { get; set; }
        public string PickListNum { get; set; }
        public string Picker { get; set; }
        public string Packer { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddr1 { get; set; }
        public string ShipToAddr2 { get; set; }
        public string ShipToAddr3 { get; set; }
        public string ShipToState { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountry { get; set; }
        public string ExpiryDate { get; set; }
    }
}