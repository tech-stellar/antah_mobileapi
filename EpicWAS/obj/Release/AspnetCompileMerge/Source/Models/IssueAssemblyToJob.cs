using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class IssueAssemblyToJob
    {
        public string Company { get; set; }

        public string FromPartNum { get; set; }
        public string FromPartIUM { get; set; }
        public decimal FromQuantity { get; set; }
        public string FromWhseCode { get; set; }
        public string FromBinNum { get; set; }
        public string FromLotNum { get; set; }


        public string ToJobNum { get; set; }
        public string ToJobPartNum { get; set; }
        public int ToJobAssemblySeq { get; set; }

        public string ToWhseCode { get; set; }
        public string ToBinNum { get; set; }
        public int LabelCount { get; set; }
        public string Reference { get; set; }
        public string TranNum { get; set; }

        public string CurPlant { get; set; }
    }
}