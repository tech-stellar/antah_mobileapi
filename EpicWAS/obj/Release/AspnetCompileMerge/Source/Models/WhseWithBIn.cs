using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class WhseWithBIn
    {
        public PartWhse PartWhse { get; set; }

        public IList< WhseBin> WhseBinList { get; set; }
    }
}