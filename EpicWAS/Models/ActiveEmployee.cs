using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ActiveEmployee
    {
        public string Company { get; set; }
        public string EmployeeNum { get; set; }
        public int Shift { get; set; }
        public string PayrollDate { get; set; }
        public int LaborHedSeq { get; set; }

        public string ClockInDate { get; set; }
        public string ClockInTime { get; set; }
        public string EmployeeName { get; set; }

    }
}