using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class Customer
    {
        public string Company { get; set; }
        public string CustID { get; set; }
        public int CustNum { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int CountryNum { get; set; }
        public string PhoneNum { get; set; }
        public string FaxNum { get; set; }
        public string EMailAddress { get; set; }
        public string CustURL { get; set; }
        public string TerritoryID { get; set; }
        public string SalesRepCode { get; set; }
        public string TermsCode { get; set; }
        public string GroupCode { get; set; }
        public string BusinessCatList { get; set; }
        public bool FS_APCLicense_c { get; set; }
        public bool FS_MMAMembership_c { get; set; }
        public bool FS_GVOTForm24_c { get; set; }
        public bool FS_GVOTForm49_c { get; set; }
        public bool FS_BussReg_c { get; set; }
        public bool FS_TradingLicense_c { get; set; }
        public bool FS_IC_c { get; set; }

    }
}