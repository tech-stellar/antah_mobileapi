using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PartTran
    {
        public string Company { get; set; }
        public string SysDate { get; set; }
        public int TranNo { get; set; }
        public string TranType { get; set; }
        public string PartNum { get; set; }
        public decimal TranQty { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string JobNum { get; set; }
        public int AssemblySeq { get; set; }
        public string LotNum { get; set; }
        public string UOM { get; set; }
        public string EntryPerson { get; set; }
    }
}