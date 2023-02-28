using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class QuoteDtls
    {
        public string Company { get; set; }
        public int QuoteNum { get; set; }
        public int QuoteLine { get; set; }
        public string PartNum { get; set; }
        public string LineDesc { get; set; }
        public string ProdCode { get; set; }
        public string QuoteComment { get; set;}
        public decimal OrderQty { get; set; }
        public decimal SellingExpectedQty { get; set; }
        public string SellingExpectedUM { get; set; }
    }

}