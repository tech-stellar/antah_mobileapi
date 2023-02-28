using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PurchaseOrder
    {
        public string Company { get; set; }
        public int PONum { get; set; }
        public DateTime PODate { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string Buyer { get; set; }
        public string ShipViaCode { get; set; }

        public string TermsCode { get; set; }
        public string PurPoint { get; set; }
        public string CurrencyCode { get; set;}
        public bool Approve { get; set; }
        public decimal DocTotalOrder { get; set; }

        /* 2021-04-30 yew mun add in legal number parameter  */
        public string LegalNumber { get; set; }

        public bool Confirmed { get; set; }
    }
}