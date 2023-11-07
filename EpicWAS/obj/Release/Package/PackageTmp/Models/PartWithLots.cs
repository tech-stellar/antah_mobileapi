using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class PartWithLots
    {
        public Part Parts { get; set; }

        public IList<PartLot> PartLots { get; set; }
    }
}