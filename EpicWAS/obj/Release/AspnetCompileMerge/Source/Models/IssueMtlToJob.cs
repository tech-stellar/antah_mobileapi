using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class IssueMtlToJob
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
        public int ToJobMaterialSeq { get; set; }
        public string ToJobMaterialSeqPartNum { get; set; }
        public string ToWhseCode { get; set; }
        public string ToBinNum { get; set; }

        public int LabelCount { get; set; }
        public string CurrentPlant { get; set; }
        public string TranNum { get; set; }

    }
}