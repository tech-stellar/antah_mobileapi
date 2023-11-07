using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class StkRepl
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WarehouseCode { get; set; }
        public string BinNum { get; set; }
        public decimal OnHandQty { get; set; }
        public string LotNum { get; set; }
        public string DimCode { get; set; }
        public decimal AllocatedQty { get; set; }
        public decimal SalesAllocatedQty { get; set; }
        public decimal SalesPickingQty { get; set; }
        public decimal SalesPickedQty { get; set; }
        public decimal JobAllocatedQty { get; set; }
        public decimal JobPickingQty { get; set; }
        public decimal JobPickedQty { get; set; }
        public decimal TFOrdAllocatedQty { get; set; }
        public decimal TFOrdPickingQty { get; set; }
        public decimal TFOrdPickedQty { get; set; }
        public decimal ShippingQty { get; set; }
        public decimal PackedQty { get; set; }
        public string PartLotDescription { get; set; }
        public bool OnHand { get; set; }
        public string FirstRefDate { get; set; }
        public string LastRefDate { get; set; }
        public decimal LotLaborCost { get; set; }
        public decimal LotBurdenCost { get; set; }
        public decimal LotMaterialCost { get; set; }
        public decimal LotSubContCost { get; set; }
        public decimal LotMtlBurCost { get; set; }
        public string ExpirationDate { get; set; }
        public bool ShipDocAvail { get; set; }
        public string Batch { get; set; }
        public string MfgBatch { get; set; }
        public string MfgLot { get; set; }
        public string HeatNum { get; set; }
        public string FirmWare { get; set; }
        public string BestBeforeDt { get; set; }
        public string MfgDt { get; set; }
        public string CureDt { get; set; }
        public string ExpireDt { get; set; }
        public decimal FIFOLotLaborCost { get; set; }
        public decimal FIFOLotBurdenCost { get; set; }
        public decimal FIFOLotMaterialCost { get; set; }
        public decimal FIFOLotSubContCost { get; set; }
        public decimal FIFOLotMtlBurCost { get; set; }
        public string CSFLMWComOath { get; set; }
        public string CSFCJ5FormNbr { get; set; }
        public string CSFCJ5Vessel { get; set; }
        public string CSFCJ5ApvlStart { get; set; }
        public string CSFCJ5ApvlEnd { get; set; }
        public string ImportNum { get; set; }
        public int ImportedFrom { get; set; }
        public string ImportedFromDesc { get; set; }
        public string PartDescription { get; set; }
         
    }
}