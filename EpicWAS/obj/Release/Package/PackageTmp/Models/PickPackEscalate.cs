using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PickPackEscalate
    {
        public string Company { get; set; }
        public string MQ_SysRowID { get; set; }
        public string U14_SysRowID { get; set; }
        public string PickPackRemarks { get; set; }
        public string CurrentPlant { get; set; }

        public string Reason { get; set; }
        public string Picker { get; set; }

        public string EscalateDateTime { get; set; }
        public string PickingNo { get; set; }
        public string EntryPerson { get; set; }
        public string CurrentStage { get; set; }
        public string Customer { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRel { get; set; }
        public string Key1 { get; set; }

        public string AssignResolvedBy { get; set; }
        public string AssignedDateTime { get; set; }

        public string PartNum { get; set; }
        public string PartDesc { get; set; }

        public string AllocatedWarehouse { get; set; }

        public string AllocatedBin { get; set; }
        public string AllocatedLot { get; set; }

        public decimal AllocatedQuantity { get; set; }
        public string ExpiryDate { get; set; }
      
        public string Station { get; set; }
        public string PickListNo { get; set; }


    }
}