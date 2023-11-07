using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class JobAsmbl
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public Int32 AssemblySeq { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public string RevisionNum { get; set; }
        public decimal RequiredQty { get; set; }
        public string IUM { get; set; }
    }
}