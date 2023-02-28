using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class EpicUser
    {
        public string Epic_UserId { get; set; }
        public string Epic_PassKey { get; set; }
        public string Epic_UserName { get; set; }
        public string Epic_Company { get; set; }
        public string Epic_CurCompany { get; set; }
        public string Epic_Plant { get; set; }

    }
}