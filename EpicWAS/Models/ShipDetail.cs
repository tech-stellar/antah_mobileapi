using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class ShipDetail
    {
        public string Company { get; set; }
        public int PackNum { get; set; }
        public string Customer { get; set; }
        public string ShipDate { get; set; }
        public string ShipViaCode { get; set; }
        public string ShipPerson { get; set; }
        public string EntryPerson { get; set; }
        public string TrackingNumber { get; set; }
        public decimal Weight { get; set; }
        public string WeightUOM { get; set; }
        public string PickListNum { get; set; }
        public decimal CartonNum { get; set; }
        public string StationID { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
        public string RePrintBy { get; set; }
        public string RePrintDT { get; set; }
        public string PalletNumber { get; set; }
        public bool IsPicked { get; set; }
        public string Picker { get; set; }
        public string StartPicked { get; set; }
        public string EndPicked { get; set; }
        public bool IsPacked { get; set; }
        public string Packer { get; set; }
        public string StartPacked { get; set; }
        public string EndPacked { get; set; }
        public int PackLine { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRel { get; set; }
        public decimal SellingInventoryShipQty { get; set; }
        public string PartNum { get; set; }
        public string LineDesc { get; set; }
        public string IUM { get; set; }
        public string SalesUm { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public string LotNum { get; set; }
        public bool IsComplete { get; set; }
		public bool LineIsPacked { get; set; }
		public string LinePackedBy { get; set; }
		public string LinePackedDate { get; set; }
		public bool LineIsPicked { get; set; }
		public string LinePickedBy { get; set; }
		public string LinePickedDate { get; set; }
		public string TagNum { get; set; }
		public string ExpirationDate { get; set; }

    }
}