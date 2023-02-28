using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class Vendor
    {
        public string Company { get; set; }
        public string VendorId { get; set; }
        public int VendorNum { get; set; }
        public string Vendorname { get; set; }
        public bool InActive { get; set; }
        public string GroupCode { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

    }
}