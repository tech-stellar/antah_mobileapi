using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class JobHead
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public string JobType { get; set; }
        public string PartNum { get; set; }
        public string PartDescription { get; set; }
        public string RevisionNum { get; set; }
        public decimal ProdQty { get; set; }
    }
}