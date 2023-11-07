using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class Quote
    {
        public string Company { get; set; }
        //public string ExtQuoteNum { get; set; }
        public int QuoteNum { get; set; }
        //public DateTime EntryDate { get; set; }
        //public DateTime DueDate { get; set; }
        public string PONum { get; set; }

        //public string CustID { get; set; }
        public int CustNum { get; set; }
        //public string SalesPerson { get; set; }
        public string ShipViaCode { get; set; }
        public string QuoteComment { get; set; }
        public string ShipToNum { get; set; }
        public string TermsCode { get; set; }

        public string CustomerCustID { get; set; }
        public string ShipToCustID { get; set; }
        public int ShipToCustNum { get; set; }

        public QuoteDtls[] QuoteDtls;

    }
}