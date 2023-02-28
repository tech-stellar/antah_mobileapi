using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class EpicCompPlant
    {
        public EpicCompany oEpicCompany { get; set; }
        public IList <Plant> oEpicCompPlant { get; set; }
    }
}