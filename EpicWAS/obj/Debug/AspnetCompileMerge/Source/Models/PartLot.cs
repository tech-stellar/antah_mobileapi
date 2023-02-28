using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PartLot
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public string PartLotDescription { get; set; }
        public string Batch { get; set; }
        public string MfgBatch { get; set; }
        public string MfgLot { get; set; }
        public string HeatNum { get; set; }
        public string FirmWare { get; set; }
        public DateTime BestBeforeDt { get; set; }
        public DateTime MfgDt { get; set; }
        public DateTime CureDt { get; set; }
        public DateTime ExpireDt { get; set; }
    }
}