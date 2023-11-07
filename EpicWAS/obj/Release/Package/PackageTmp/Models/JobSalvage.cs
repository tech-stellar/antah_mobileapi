using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class JobSalvage
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public string JobPartNum { get; set; }
        public Int32 AssemblySeq { get; set; }
        public Int32 MtlSeq { get; set; }
        public string PartNum { get; set; }
        public string IUM { get; set; }
        public decimal dTranQty { get; set; }
        public string LotNum { get; set; }
        public string Reference { get; set; }

        public string ToWarehouseCode { get; set; }
        public string ToBinNum { get; set; }
        public int LabelCount { get; set; }
    }
}