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

        public string Epic_CurCompanyName { get; set; }
        public string Epic_PlantName { get; set; }

        // add on 20190612
        public bool IsEnable_MiscIssue { get; set; }
        public bool IsEnable_MoveInventory { get; set; }
        public bool IsEnable_JobReceipts { get; set; }
        public bool IsEnable_ReturnMaterial { get; set; }
        public bool IsEnable_MoveInventoryRequest { get; set; }
        public bool IsEnable_MoveInventoryApproval { get; set; }
        public bool IsEnable_SalvageReceipts { get; set; }
        public bool IsEnable_POReceipts { get; set; }
        public bool IsEnable_IssueAssembly { get; set; }
        public bool IsEnable_ReturnAssembly { get; set; }
        public bool IsEnable_DeliveryTrack { get; set; }
        public bool IsEnable_Reprint { get; set;}
        public bool IsEnable_SplitMergeUOM { get; set; }

        public bool IsEnable_IssueMiscMaterial { get; set; }
        public bool IsEnable_ReturnMiscMaterial { get; set; }
        public bool IsEnable_QtyAdjustment { get; set; }

        public bool IsEnable_StartEndOp { get; set; }
        public bool IsEnable_ClockInOut { get; set; }
        public bool IsEnable_WorkQueue { get; set; }

        public bool IsEnable_UseDefaultLabelQty { get; set; }
        public int DefaultLabelQty { get; set;}


        public string IMEI { get; set; }
        public string Epic_EmpId { get; set; }


        public bool IsEnable_UserDefine01 { get; set; }
        public bool IsEnable_UserDefine02 { get; set; }
        public bool IsEnable_UserDefine03 { get; set; }
        public bool IsEnable_UserDefine04 { get; set; }
        public bool IsEnable_UserDefine05 { get; set; }
        public bool IsEnable_UserDefine06 { get; set; }
        public bool IsEnable_UserDefine07 { get; set; }
        public bool IsEnable_UserDefine08 { get; set; }
        public bool IsEnable_UserDefine09 { get; set; }
        public bool IsEnable_UserDefine10 { get; set; }

        public string OrderType { get; set; }


        public bool IsEnable_NewMenu { get; set; }
        public bool IsEnable_NewPick { get; set; }
        public bool IsEnable_NewPack { get; set; }
        public bool IsEnable_NewEsc { get; set; }
        public bool IsEnable_NewReprReass { get; set; }
        public bool IsEnable_NewScanSign { get; set; }
		public bool IsEnable_NewConfirmDel { get; set; }
		public bool IsEnable_NewReprintLabel { get; set; }
		public bool IsEnable_NewSummDash { get; set; }
	}
}