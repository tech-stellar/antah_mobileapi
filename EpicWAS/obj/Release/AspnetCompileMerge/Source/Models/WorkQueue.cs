using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class WorkQueue
    {
        public string Company { get; set; }
        public string JobNum { get; set; }
        public Int32 AssemblySeq { get; set; }
        public int OprSeq { get; set; }
        public string OpCode { get; set; }
        public string ResourceGrpID { get; set; }
        public string ResourceID { get; set; }
        public string LaborType { get; set; }
        public string ActiveTrans { get; set; }
        public string EmployeeNum { get; set; }

        public string ClockInDate { get; set; }
        public string ClockIntime { get; set; }

        public int LaborHedSeq { get; set; }
        public int LaborDtlSeq { get; set; }

        public string CurPlant { get; set; }

        public decimal TranQty { get; set; }
        public string EmployeeName { get; set; }
    }
}