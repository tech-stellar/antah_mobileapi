using System;

namespace EpicWAS.Models
{
	public class Summary
	{
		public string PickListDate { get; set; }
		public string PickListNum { get; set; }
		public string OrderNum { get; set; }
		public string Status { get; set; }
		public string PickListLine { get; set; }
		public string PartNum { get; set; }
		public string PartDescription { get; set; }
		public string UOM { get; set; }
		public decimal AllocatedQty { get; set; }
		public string Warehouse { get; set; }
		public string BinNum { get; set; }
		public string LotNum { get; set; }
		public string ExpirationDate { get; set; }
		public bool ManualAlloc { get; set; }
		public decimal OnhandQty { get; set; }
		public decimal AvailableQty { get; set; }
		public bool BackOrder { get; set; }
	}
}