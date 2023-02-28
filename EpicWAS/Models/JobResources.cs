using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class JobResources
    {
        public string Company { get; set; }
        public string JobNum { get; set; }
        public Int32 AssemblySeq { get; set; }
        public Int32 OperationSeq { get; set; }
        public string Opcode { get; set; }
        public string ResourceGroupId { get; set; }
        public string ResourceGroupDescription { get; set; }
        public string ResourceId { get; set; }
        public string ResourceDescription { get; set; }
        public string Plant { get; set; }
    }
}