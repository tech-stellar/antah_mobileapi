using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ReceiptHead
    {
        public string Company { get; set; }
        public int PONum { get; set; }
        public string PackSlip { get; set; }

        public string LegalNumber { get; set; }
    }
}