using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class JobOprDtl
    {
        public string Company { get; set; }
        public string JobNum { get; set; }
        public Int32 AssemblySeq { get; set; }
        public int OprSeq { get; set; }
        public int OpDtlSeq { get; set; }
        public string ResourceGrpID { get; set; }
        public string ResourceID { get; set; }
        public string PrimaryProdOpDtl { get; set; }
        public string InputWhse { get; set;  }
        public string InputBinNum { get; set; }
        public string resource_inputwhse { get; set; }
        public string resource_inputbin { get; set; }

        public string OutputWhse { get; set; }
        public string OutputBinNum { get; set; }
        public string resource_outputwhse { get; set; }
        public string resource_outputbin { get; set; }



    }
}