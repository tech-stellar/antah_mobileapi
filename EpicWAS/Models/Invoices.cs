using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class Invoices
    {
        public string Company { get; set; }
        public string InvoiceDate { get; set; }
        public string LegalNumber { get; set; }
        public bool FS_ChopSignVerify_c { get; set; }
        public string FS_ChopSignDate_c { get; set; }
        public string FS_ChopSignDateTime_c { get; set; }
        public string FS_ChopSignBy_c { get; set; }
        public string ShipToNum { get; set; }
        public string ShipTo_Name { get; set; }
        public string ShipTo_Address1 { get; set;}
        public string ShipTo_Address2 { get; set;}
        public string ShipTo_Address3 { get; set;}
        public string ShipTo_City { get; set; }
        public string ShipTo_State { get; set; }
        public string ShipTo_ZIP { get; set; }
        public string ShipTo_Country { get; set; }
        public bool OrderHed_UseOTS { get; set; }
        public int OrderNum { get; set; }
        public int PrcConNum { get; set; }
        public int ShpConNum { get; set; }
        public int InvoiceNum { get; set; }
        public bool PrimSCon { get; set; }
        public string PhoneNum { get; set; }
        public string InvoiceComment { get; set; }
        public int CustNum { get; set; }
        public string CustID { get; set; }
    }
}