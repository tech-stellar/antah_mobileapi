using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Ajax.Utilities;

namespace EpicWAS.Models
{
    public class PickPackBO
    {
		public bool _LoadSummary(ref EpicEnv oEpicEnv, string strCompany, string strUID, string strOrderNum, string strPartNum, string strPickListNum, string strWarehouse, string strStatus, bool backOrder, 
            string strFromPickDate, string strToPickDate, bool showVoid, ref IList<Summary> oSummaryList, out string strMessage)
		{
			bool IsError = false;
            var strPickGrps = "";

			try
			{
				string _strSQL = "select top 1 SD_ProdGrps_c as ProductGroups, SD_BackOrder_c as BackOrder from UD20 where Key1 = '" + strUID + "' and SD_ProdGrps_c != ''";

				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                bool isBackOrder = false;

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						strPickGrps = row["ProductGroups"].ToString().Replace("~", "\',\'");
						strPickGrps = "\'" + strPickGrps + "\'";
                        isBackOrder = (bool)row["BackOrder"];
                    }

				}

                string backOrderStatus = backOrder ? "1" : "1,0";

                if (!isBackOrder)
                {
                    backOrderStatus = "0";
                }

				
                string voidStatus = showVoid ? "1" : "0";
                if (strStatus == "VOIDED")
                {
                    voidStatus = "1";
                }

                _strSQL = "select SD_PickListDate_c as PickListDate, u.Key1 as PickListNum, ua.SD_OrderNum_c as OrderNum, (case when ua.SD_PickedBy_c != '' then 1 else 0 end) as Picked, " +
                    "(case when u.SD_Status_c like 'ESCALATED%' then 1 else 0 end) as Escalated, ChildKey2 as PickListLine, SD_PartNum_c as PartNum, PartDescription, SD_UOM_c as UOM, " +
                    "SD_AllocateQuantity_c as AllocatedQty, SD_Warehouse_c as Warehouse, SD_BinNum_c as BinNum, SD_LotNum_c as LotNum, ExpirationDate, SD_ManualAlloc_c as ManualAlloc, " +
                    "ISNULL(OnhandQty, 0) as OnhandQty, ISNULL(OnhandQty - pb.AllocatedQty_c, 0) as AvailableQty, (CASE WHEN SD_BackOrder_c = 1 THEN 1 ELSE 0 END) as BackOrder, (CASE WHEN u.SD_Voided_c = 1 THEN 1 ELSE 0 END) as VoidOrder " +
                    "from UD103 u join UD103A ua on u.Company = ua.Company and u.Key1 = ua.Key1 " +
                    "join OrderRel orl on orl.Company = u.Company and orl.OrderNum = ua.SD_OrderNum_c and orl.OrderLine = ua.SD_OrderLine_c and orl.OrderRelNum = ua.SD_OrderRel_c " +
                    "join Part p on p.Company = u.Company and p.PartNum = ua.SD_PartNum_c " +
                    "join PartLot pl on pl.Company = u.Company and pl.PartNum = ua.SD_PartNum_c and SD_LotNum_c = pl.LotNum " +
                    "left join PartBin pb on pb.Company = u.Company and pb.WarehouseCode = ua.SD_Warehouse_c and pb.BinNum = ua.SD_BinNum_c and pb.LotNum = ua.SD_LotNum_c and pb.PartNum = ua.SD_PartNum_c ";

                string _strWHERE = "";

                _strWHERE += "and u.SD_Status_c = 'ALLOCATED' and u.SD_Voided_c = 0 ";

                if (strOrderNum != "" || strOrderNum != null)
                {
                    _strWHERE += "and ua.SD_OrderNum_c like '%" + strOrderNum + "%' ";
                }

                if (strPartNum != "" || strPartNum != null)
                {
                    _strWHERE += "and SD_PartNum_c like '%" + strPartNum + "%' ";
                }

                if (strPickListNum != "" || strPickListNum != null)
                {
                    _strWHERE += "and u.Key1 like '%" + strPickListNum + "%' ";
                }

                if (strWarehouse != "" || strWarehouse != null)
                {
                    _strWHERE += "and SD_Warehouse_c like '%" + strWarehouse + "%' ";
                }

                if ((strStatus != "" || strStatus != null) && strStatus != "VOIDED")
                {
                    _strWHERE += "and u.SD_Status_c like '%" + strStatus + "%' ";
                }

                if (backOrderStatus != "" || backOrderStatus != null)
                {
                    _strWHERE += "and u.SD_BackOrder_c in (" + backOrderStatus + ") ";
                }

                //if (voidStatus != "" || voidStatus != null)
                //{
                //    _strWHERE += "and u.SD_Voided_c in (" + voidStatus + ") ";
                //}

                if (strPickGrps != "" || strPickGrps != null)
                {
                    _strWHERE += "and SD_PickListGroup_c in (" + strPickGrps + ") ";
                }

                if (!String.IsNullOrEmpty(strFromPickDate))
                {
                    _strWHERE += "and SD_PickListDate_c >= '" + strFromPickDate + "' "; 
                }
				if (!String.IsNullOrEmpty(strToPickDate))
				{
                    _strWHERE += "and SD_PickListDate_c <= '" + strToPickDate + "' ";
				}

                if (_strWHERE != "")
                {
                    _strSQL = _strSQL  + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); 
                
                }

                _strSQL += " order by PickListNum, PickListLine ";


				// SQLServerBO _MSSQL = new SQLServerBO();
				// string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				_dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Summary oSummary = new Summary();
                        oSummary.PickListDate = DBNull.Value.Equals(row["PickListDate"]) ? "1999-01-01" : Convert.ToDateTime((row["PickListDate"])).ToString("yyyy-MM-dd");
						oSummary.PickListNum = row["PickListNum"].ToString();
						oSummary.OrderNum = row["OrderNum"].ToString();
						oSummary.Picked = row["Picked"].ToString().ToLower() == "1" ? true : false;
                        oSummary.Escalated = row["Escalated"].ToString().ToLower() == "1" ? true : false;
						oSummary.PickListLine = row["PickListLine"].ToString();
						oSummary.PartNum = row["PartNum"].ToString();
						oSummary.PartDescription = row["PartDescription"].ToString();
						oSummary.UOM = row["UOM"].ToString();
						oSummary.AllocatedQty = Decimal.Parse(row["AllocatedQty"].ToString());
						oSummary.Warehouse = row["Warehouse"].ToString();
						oSummary.BinNum = row["BinNum"].ToString();
						oSummary.LotNum = row["LotNum"].ToString();
						oSummary.ExpirationDate = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");
						oSummary.ManualAlloc = row["ManualAlloc"].ToString() == "True" ? true : false;
						oSummary.OnhandQty = Decimal.Parse(row["OnhandQty"].ToString());
						oSummary.AvailableQty = Decimal.Parse(row["AvailableQty"].ToString());
						oSummary.BackOrder = row["BackOrder"].ToString().ToLower() == "1" ? true : false;
                        oSummary.VoidOrder = row["VoidOrder"].ToString().ToLower() == "1" ? true : false;

                        oSummaryList.Add(oSummary);
                    }
					strMessage = "";
					IsError = false;

				}
				else
				{
					strMessage = "No list generated";
					IsError = true;
				}

			}
			catch (Exception ex)
			{
                strMessage = ex.Message.ToString();
				IsError = true;
			}


			return (IsError ? false : true);

		}

		public bool _LoadPickPacks(ref EpicEnv oEpicEnv, string strCompany, string strCustID, string strCustName, ref IList<PickPack> oPickPackList, string strPickNum, string strTagNum, string strUserID, out string strMessage)
        {
            bool IsError = false;
            string strPickingNum = "";

            try
            {

                string _strSQL = "select pp.*, c.CustID, c.Name, o.AP_BumiAgDONum_c, o.AP_UrgentOrder_c, p.AP_SerialNo_c, s.Description, s.ShipViaCode ";
                _strSQL += "from PickPack pp ";
                _strSQL += "left join OrderHed o on pp.MQ_Company = o.Company and pp.MQ_OrderNum = o.OrderNum ";
                _strSQL += "left join Erp.Customer c on o.Company = c.Company and o.CustNum = c.CustNum ";
                _strSQL += "left join Part p on pp.MQ_Company = p.Company and pp.MQ_PartNum = p.PartNum ";
                _strSQL += "left join erp.ShipVia s on o.Company = s.Company and o.ShipViaCode = s.ShipViaCode ";
                _strSQL += "Where pp.MQ_Company = '" + strCompany + "' ";

                if (strCustID != "" && strCustID != null)
                {
                    _strSQL += " and c.CustID = '" + strCustID + "' ";
                }

                if (strCustName != "" && strCustName != null)
                {
                    _strSQL += " and c.Name like  '" + strCustName + "%' ";
                }

                if (strPickNum != "" && strPickNum != null)
                {
                    _strSQL += " and pp.U14_Key5 = '" + strPickNum + "' ";
                }

                if (strTagNum != "" && strTagNum != null)
                {
                    _strSQL += " and pp.U14_Key5 in (select distinct top 1 x.u14_key5 from PickPack x where x.U14_ShortChar17 = '" + strTagNum + "') ";
                    _strSQL += " and 0 = isnull((select top 1 case when s.ShipStatus = 'INVOICED' then 1 else 0 end from ShipHead s where s.Company = pp.MQ_Company and s.FS_PickListNo_c = pp.U14_Key5 ),0) ";

                }

                _strSQL += " and pp.U14_Key5 in (select distinct x.u14_key5 from pickpack x where x.u14_checkbox20 = 1 ) ";
                _strSQL += " order by cast(pp.MQ_OrderNum as int), cast(pp.MQ_OrderLine as int), cast(pp.MQ_OrderRelNum as int) ";


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PickPack oPickPack = new PickPack();
                        oPickPack.Company = row["MQ_Company"].ToString();
                        oPickPack.MQ_AssemblySeq = DBNull.Value.Equals(row["MQ_AssemblySeq"]) ? 0 : int.Parse(row["MQ_AssemblySeq"].ToString());
                        oPickPack.MQ_AssignedToEmpID = (row["MQ_AssignedToEmpID"].ToString());
                        oPickPack.MQ_DistributionType = (row["MQ_DistributionType"].ToString());
                        oPickPack.MQ_EpicorFSA = (row["MQ_EpicorFSA"].ToString());
                        oPickPack.MQ_ForeignSysRowID = (row["MQ_ForeignSysRowID"].ToString());
                        oPickPack.MQ_FromBinNum = (row["MQ_FromBinNum"].ToString());
                        oPickPack.MQ_FromPCID = (row["MQ_FromPCID"].ToString());
                        oPickPack.MQ_FromWhse = (row["MQ_FromWhse"].ToString());
                        oPickPack.MQ_FS_PrintStatus_c = (row["MQ_FS_PrintStatus_c"].ToString());
                        oPickPack.MQ_IUM = (row["MQ_IUM"].ToString());
                        oPickPack.MQ_JobNum = (row["MQ_JobNum"].ToString());
                        oPickPack.MQ_JobSeq = DBNull.Value.Equals(row["MQ_JobSeq"]) ? 0 : int.Parse(row["MQ_JobSeq"].ToString());
                        oPickPack.MQ_JobSeqType = (row["MQ_JobSeqType"].ToString());
                        oPickPack.MQ_LastMgrChangeEmpID = (row["MQ_LastMgrChangeEmpID"].ToString());
                        oPickPack.MQ_LastUsedPCID = (row["MQ_LastUsedPCID"].ToString());
                        oPickPack.MQ_Lock = row["MQ_Lock"].ToString() == "True" ? true : false;
                        oPickPack.MQ_LotNum = (row["MQ_LotNum"].ToString());
                        oPickPack.MQ_MtlQueueSeq = DBNull.Value.Equals(row["MQ_MtlQueueSeq"]) ? 0 : int.Parse(row["MQ_MtlQueueSeq"].ToString());
                        oPickPack.MQ_NCTranID = (row["MQ_NCTranID"].ToString());
                        oPickPack.MQ_NeedByDate = (row["MQ_NeedByDate"].ToString());
                        oPickPack.MQ_NeedByTime = (row["MQ_NeedByTime"].ToString());
                        oPickPack.MQ_NextAssemblySeq = DBNull.Value.Equals(row["MQ_NextAssemblySeq"]) ? 0 : int.Parse(row["MQ_NextAssemblySeq"].ToString());
                        oPickPack.MQ_NextJobSeq = DBNull.Value.Equals(row["MQ_NextJobSeq"]) ? 0 : int.Parse(row["MQ_NextJobSeq"].ToString());
                        oPickPack.MQ_Obs10_PkgLine = (row["MQ_Obs10_PkgLine"].ToString());
                        oPickPack.MQ_Obs10_PkgNum = (row["MQ_Obs10_PkgNum"].ToString());
                        oPickPack.MQ_OrderLine = DBNull.Value.Equals(row["MQ_OrderLine"]) ? 0 : int.Parse(row["MQ_OrderLine"].ToString());
                        oPickPack.MQ_OrderNum = DBNull.Value.Equals(row["MQ_OrderNum"]) ? 0 : int.Parse(row["MQ_OrderNum"].ToString());
                        oPickPack.MQ_OrderRelNum = DBNull.Value.Equals(row["MQ_OrderRelNum"]) ? 0 : int.Parse(row["MQ_OrderRelNum"].ToString());
                        oPickPack.MQ_PackLine = (row["MQ_PackLine"].ToString());
                        oPickPack.MQ_PackSlip = (row["MQ_PackSlip"].ToString());
                        oPickPack.MQ_PackStation = (row["MQ_PackStation"].ToString());
                        oPickPack.MQ_PartDescription = (row["MQ_PartDescription"].ToString());
                        oPickPack.MQ_PartNum = (row["MQ_PartNum"].ToString());
                        oPickPack.MQ_PCID = (row["MQ_PCID"].ToString());
                        
                       oPickPack.MQ_Plant = (row["MQ_Plant"].ToString());
                       oPickPack.MQ_POLine = DBNull.Value.Equals(row["MQ_POLine"]) ? 0 : int.Parse(row["MQ_POLine"].ToString());
                       oPickPack.MQ_PONum = DBNull.Value.Equals(row["MQ_PONum"]) ? 0 :int.Parse(row["MQ_PONum"].ToString());
                       oPickPack.MQ_PORelNum = DBNull.Value.Equals(row["MQ_PORelNum"]) ? 0: int.Parse(row["MQ_PORelNum"].ToString());
                       oPickPack.MQ_Priority = (row["MQ_Priority"].ToString());
                       oPickPack.MQ_PurPoint = (row["MQ_PurPoint"].ToString());
                       oPickPack.MQ_Quantity = DBNull.Value.Equals(row["MQ_Quantity"]) ? 0 : decimal.Parse(row["MQ_Quantity"].ToString());
                       oPickPack.MQ_QueueID = (row["MQ_QueueID"].ToString());
                       oPickPack.MQ_QueuePickSeq = DBNull.Value.Equals(row["MQ_QueuePickSeq"]) ? 0 :int.Parse(row["MQ_QueuePickSeq"].ToString());
                       oPickPack.MQ_Reference = (row["MQ_Reference"].ToString());
                       oPickPack.MQ_ReferencePrefix = (row["MQ_ReferencePrefix"].ToString());
                       oPickPack.MQ_ReleaseForPickingSeq = DBNull.Value.Equals(row["MQ_ReleaseForPickingSeq"]) ? 0 : int.Parse(row["MQ_ReleaseForPickingSeq"].ToString());
                       oPickPack.MQ_RequestedByEmpID = (row["MQ_RequestedByEmpID"].ToString());
                       oPickPack.MQ_RevisionNum = (row["MQ_RevisionNum"].ToString());
                       oPickPack.MQ_RMADisp = (row["MQ_RMADisp"].ToString());
                       oPickPack.MQ_RMALine= DBNull.Value.Equals(row["MQ_RMALine"]) ? 0 :int.Parse(row["MQ_RMALine"].ToString());
                       oPickPack.MQ_RMANum = (row["MQ_RMANum"].ToString());
                       oPickPack.MQ_RMAReceipt = row["MQ_RMAReceipt"].ToString() == "True" ? true : false;

                        
                       oPickPack.MQ_SelectedByEmpID = (row["MQ_SelectedByEmpID"].ToString());
                       oPickPack.MQ_SysDate = (row["MQ_SysDate"].ToString());
                       oPickPack.MQ_SysRevID = (row["MQ_SysRevID"].ToString());
                       oPickPack.MQ_SysRowID = (row["MQ_SysRowID"].ToString());
                       oPickPack.MQ_SysTime = (row["MQ_SysTime"].ToString());
                        
                      oPickPack.MQ_TargetAssemblySeq = DBNull.Value.Equals(row["MQ_TargetAssemblySeq"]) ? 0 : int.Parse(row["MQ_TargetAssemblySeq"].ToString());
                      oPickPack.MQ_TargetJobNum = DBNull.Value.Equals(row["MQ_TargetJobNum"]) ? string.Empty : row["MQ_TargetJobNum"].ToString();
                      oPickPack.MQ_TargetMtlSeq = DBNull.Value.Equals(row["MQ_TargetMtlSeq"]) ? 0 : int.Parse(row["MQ_TargetMtlSeq"].ToString());
                      oPickPack.MQ_TargetTFOrdLine = DBNull.Value.Equals(row["MQ_TargetTFOrdLine"]) ? 0 : int.Parse(row["MQ_TargetTFOrdLine"].ToString());
                      oPickPack.MQ_TargetTFOrdNum = DBNull.Value.Equals(row["MQ_TargetTFOrdNum"]) ? string.Empty : (row["MQ_TargetTFOrdNum"].ToString());
                        
                      oPickPack.MQ_ToBinNum = (row["MQ_ToBinNum"].ToString());
                      oPickPack.MQ_ToPCID = (row["MQ_ToPCID"].ToString());
                      oPickPack.MQ_ToWhse = (row["MQ_ToWhse"].ToString());
                      oPickPack.MQ_TranSource = (row["MQ_TranSource"].ToString());
                      oPickPack.MQ_TranStatus = (row["MQ_TranStatus"].ToString());
                      oPickPack.MQ_TranType = (row["MQ_TranType"].ToString());
                      oPickPack.MQ_UD_SysRevID = (row["MQ_UD_SysRevID"].ToString());


                      oPickPack.MQ_Updated_c = (row["MQ_Updated_c"].ToString());
                      oPickPack.MQ_VendorNum = DBNull.Value.Equals(row["MQ_VendorNum"]) ? 0 : int.Parse(row["MQ_VendorNum"].ToString() );
                      oPickPack.MQ_Visible = (row["MQ_Visible"].ToString() == "True" ? true : false);
                      oPickPack.MQ_WaveNum = DBNull.Value.Equals(row["MQ_WaveNum"]) ? 0 : int.Parse(row["MQ_WaveNum"].ToString() );
                      oPickPack.MQ_WhseGroupCode = (row["MQ_WhseGroupCode"].ToString());

                      oPickPack.U14_Character01 = (row["U14_Character01"].ToString());
                      oPickPack.U14_Character02 = (row["U14_Character02"].ToString());
                      oPickPack.U14_Character03 = (row["U14_Character03"].ToString());
                      oPickPack.U14_Character04 = (row["U14_Character04"].ToString());
                      oPickPack.U14_Character05 = (row["U14_Character05"].ToString());
                      oPickPack.U14_Character06 = (row["U14_Character06"].ToString());
                      oPickPack.U14_Character07 = (row["U14_Character07"].ToString());
                      oPickPack.U14_Character08 = (row["U14_Character08"].ToString());
                      oPickPack.U14_Character09 = (row["U14_Character09"].ToString());
                      oPickPack.U14_Character10 = (row["U14_Character10"].ToString());
                      oPickPack.U14_CheckBox01 = (row["U14_CheckBox01"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox02 = (row["U14_CheckBox02"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox03 = (row["U14_CheckBox03"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox04 = (row["U14_CheckBox04"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox05 = (row["U14_CheckBox05"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox06 = (row["U14_CheckBox06"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox07 = (row["U14_CheckBox07"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox08 = (row["U14_CheckBox08"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox09 = (row["U14_CheckBox09"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox10 = (row["U14_CheckBox10"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox11 = (row["U14_CheckBox11"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox12 = (row["U14_CheckBox12"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox13 = (row["U14_CheckBox13"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox14 = (row["U14_CheckBox14"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox15 = (row["U14_CheckBox15"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox16 = (row["U14_CheckBox16"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox17 = (row["U14_CheckBox17"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox18 = (row["U14_CheckBox18"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox19 = (row["U14_CheckBox19"].ToString() == "True" ? true : false);
                      oPickPack.U14_CheckBox20 = (row["U14_CheckBox20"].ToString() == "True" ? true : false);

                            oPickPack.SerialNum = (row["AP_SerialNo_c"].ToString() == "True" ? true : false);


                            oPickPack.U14_Company = (row["U14_Company"].ToString());

                            oPickPack.U14_Date01 = DBNull.Value.Equals(row["U14_Date01"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date01"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date02 = DBNull.Value.Equals(row["U14_Date02"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date02"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date03 = DBNull.Value.Equals(row["U14_Date03"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date03"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date04 = DBNull.Value.Equals(row["U14_Date04"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date04"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date05 = DBNull.Value.Equals(row["U14_Date05"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date05"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date06 = DBNull.Value.Equals(row["U14_Date06"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date06"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date07 = DBNull.Value.Equals(row["U14_Date07"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date07"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date08 = DBNull.Value.Equals(row["U14_Date08"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date08"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date09 = DBNull.Value.Equals(row["U14_Date09"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date09"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date10 = DBNull.Value.Equals(row["U14_Date10"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date10"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date11 = DBNull.Value.Equals(row["U14_Date11"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date11"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date12 = DBNull.Value.Equals(row["U14_Date12"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date12"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date13 = DBNull.Value.Equals(row["U14_Date13"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date13"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date14 = DBNull.Value.Equals(row["U14_Date14"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date14"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date15 = DBNull.Value.Equals(row["U14_Date15"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date15"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date16 = DBNull.Value.Equals(row["U14_Date16"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date16"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date17 = DBNull.Value.Equals(row["U14_Date17"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date17"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date18 = DBNull.Value.Equals(row["U14_Date18"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date18"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date19 = DBNull.Value.Equals(row["U14_Date19"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date19"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Date20 = DBNull.Value.Equals(row["U14_Date20"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date20"])).ToString("yyyy-MM-dd");


                          oPickPack.U14_GlobalLock = (row["U14_GlobalLock"].ToString());
                          oPickPack.U14_GlobalUD14 = (row["U14_GlobalUD14"].ToString());
                          oPickPack.U14_Key1 = (row["U14_Key1"].ToString());
                          oPickPack.U14_Key2 = (row["U14_Key2"].ToString());
                          oPickPack.U14_Key3 = (row["U14_Key3"].ToString());
                          oPickPack.U14_Key4 = (row["U14_Key4"].ToString());
                          oPickPack.U14_Key5 = (row["U14_Key5"].ToString());
                          strPickingNum = (row["U14_Key5"].ToString());


                          oPickPack.U14_Number01 = DBNull.Value.Equals(row["U14_Number01"]) ? 0M : decimal.Parse(row["U14_Number01"].ToString());
                          oPickPack.U14_Number02 = DBNull.Value.Equals(row["U14_Number02"]) ? 0M : decimal.Parse(row["U14_Number02"].ToString());
                          oPickPack.U14_Number03 = DBNull.Value.Equals(row["U14_Number03"]) ? 0M : decimal.Parse(row["U14_Number03"].ToString());
                          oPickPack.U14_Number04 = DBNull.Value.Equals(row["U14_Number04"]) ? 0M : decimal.Parse(row["U14_Number04"].ToString());

                        
                         oPickPack.U14_Number05 = DBNull.Value.Equals(row["U14_Number05"]) ? 0M : decimal.Parse(row["U14_Number05"].ToString());
                         oPickPack.U14_Number06 = DBNull.Value.Equals(row["U14_Number06"]) ? 0M : decimal.Parse(row["U14_Number06"].ToString());
                         oPickPack.U14_Number07 = DBNull.Value.Equals(row["U14_Number07"]) ? 0M : decimal.Parse(row["U14_Number07"].ToString());
                         oPickPack.U14_Number08 = DBNull.Value.Equals(row["U14_Number08"]) ? 0M : decimal.Parse(row["U14_Number08"].ToString());
                         oPickPack.U14_Number09 = DBNull.Value.Equals(row["U14_Number09"]) ? 0M : decimal.Parse(row["U14_Number09"].ToString());
                         oPickPack.U14_Number10 = DBNull.Value.Equals(row["U14_Number10"]) ? 0M : decimal.Parse(row["U14_Number10"].ToString());
                         oPickPack.U14_Number11 = DBNull.Value.Equals(row["U14_Number11"]) ? 0M : decimal.Parse(row["U14_Number11"].ToString());
                         oPickPack.U14_Number12 = DBNull.Value.Equals(row["U14_Number12"]) ? 0M : decimal.Parse(row["U14_Number12"].ToString());
                         oPickPack.U14_Number13 = DBNull.Value.Equals(row["U14_Number13"]) ? 0M : decimal.Parse(row["U14_Number13"].ToString());
                         oPickPack.U14_Number14 = DBNull.Value.Equals(row["U14_Number14"]) ? 0M : decimal.Parse(row["U14_Number14"].ToString());
                         oPickPack.U14_Number15 = DBNull.Value.Equals(row["U14_Number15"]) ? 0M : decimal.Parse(row["U14_Number15"].ToString());
                         oPickPack.U14_Number16 = DBNull.Value.Equals(row["U14_Number16"]) ? 0M : decimal.Parse(row["U14_Number16"].ToString());
                         oPickPack.U14_Number17 = DBNull.Value.Equals(row["U14_Number17"]) ? 0M : decimal.Parse(row["U14_Number17"].ToString());
                         oPickPack.U14_Number18 = DBNull.Value.Equals(row["U14_Number18"]) ? 0M : decimal.Parse(row["U14_Number18"].ToString());
                         oPickPack.U14_Number19 = DBNull.Value.Equals(row["U14_Number19"]) ? 0M : decimal.Parse(row["U14_Number19"].ToString());
                         oPickPack.U14_Number20 = DBNull.Value.Equals(row["U14_Number20"]) ? 0M : decimal.Parse(row["U14_Number20"].ToString());

                        
                         oPickPack.U14_ShortChar01 = (row["U14_ShortChar01"].ToString());
                         oPickPack.U14_ShortChar02 = (row["U14_ShortChar02"].ToString());
                         oPickPack.U14_ShortChar03 = (row["U14_ShortChar03"].ToString());
                         oPickPack.U14_ShortChar04 = (row["U14_ShortChar04"].ToString());
                         oPickPack.U14_ShortChar05 = (row["U14_ShortChar05"].ToString());
                         oPickPack.U14_ShortChar06 = (row["U14_ShortChar06"].ToString());
                         oPickPack.U14_ShortChar07 = (row["U14_ShortChar07"].ToString());
                         oPickPack.U14_ShortChar08 = (row["U14_ShortChar08"].ToString());
                         oPickPack.U14_ShortChar09 = (row["U14_ShortChar09"].ToString());
                         oPickPack.U14_ShortChar10 = (row["U14_ShortChar10"].ToString());
                         oPickPack.U14_ShortChar11 = (row["U14_ShortChar11"].ToString());
                         oPickPack.U14_ShortChar12 = (row["U14_ShortChar12"].ToString());
                         oPickPack.U14_ShortChar13 = (row["U14_ShortChar13"].ToString());
                         oPickPack.U14_ShortChar14 = (row["U14_ShortChar14"].ToString() == "" ? row["ShipViaCode"].ToString() : row["U14_ShortChar14"].ToString() );
                         oPickPack.U14_ShortChar15 = (row["U14_ShortChar15"].ToString());
                         oPickPack.U14_ShortChar16 = (row["U14_ShortChar16"].ToString());
                         oPickPack.U14_ShortChar17 = (row["U14_ShortChar17"].ToString());
                         oPickPack.U14_ShortChar18 = (row["U14_ShortChar18"].ToString());
                         oPickPack.U14_ShortChar19 = (row["U14_ShortChar19"].ToString());
                         oPickPack.U14_ShortChar20 = (row["U14_ShortChar20"].ToString());
                         oPickPack.U14_SysRevID = (row["U14_SysRevID"].ToString());
                         oPickPack.U14_SysRowID = (row["U14_SysRowID"].ToString());

                         oPickPack.BumiAgDONum = (row["AP_BumiAgDONum_c"].ToString());
                         oPickPack.UrgentOrder = (row["AP_UrgentOrder_c"].ToString() == "True" ? true : false);

                        //oPickPack.Approve = row["Approve"].ToString() == "True" ? true : false;
                        //oPurOrd.DocTotalOrder = decimal.Parse(row["DocTotalOrder"].ToString());
                        //oPurOrd.PODate = Convert.ToDateTime((row["OrderDate"]));


                        oPickPackList.Add(oPickPack);
                    }

                    // update to ud14 table
                    _strSQL = "update ice.ud14 ";
                    _strSQL += "set shortchar11 = '" + strUserID + "' ";
                    _strSQL += "where Company = '" + strCompany + "' ";
                    _strSQL += "and key5 = '" + strPickingNum + "' ";

                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);


                    // update packer complete date
                    _strSQL = "update xd ";
                    _strSQL += "set xd.DateTime03_c = getdate() ";
                    _strSQL += "from ice.ud14_UD xd ";
                    _strSQL += "inner join ice.UD14 x on x.SysRowID = xd.foreignSysrowid ";
                    _strSQL += "where x.Key5 = '" + strPickingNum + "' ";
                    _strSQL += "and x.Company = '" + strCompany + "' ";

                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);




                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "No picking list generated";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }

        public bool _LoadPickPacksV2(ref EpicEnv oEpicEnv, string strCompany, string strCustID, string strCustName, ref IList<PickPack2> oPickPackList, string strPickNum, string strTagNum, string strUserID, out string strMessage)
        {
            bool IsError = false;
            string strPickingNum = "";

            try
            {
                string _strSQL = "select tblX.*, o.AP_BumiAgDONum_c, o.AP_UrgentOrder_c, p.AP_SerialNo_c, s.Description, s.ShipViaCode, ";
                _strSQL += "(select top 1 MQ_SysRowID from dbo.PickPack pp1 where pp1.MQ_Company = tblX.MQ_Company and pp1.MQ_OrderNum = tblX.MQ_OrderNum and pp1.MQ_PartNum = tblX.MQ_PartNum  ) as mq_sysrowid, ";
                _strSQL += "(select top 1 U14_SysRowID from dbo.PickPack pp1 where pp1.MQ_Company = tblX.MQ_Company and pp1.MQ_OrderNum = tblX.MQ_OrderNum and pp1.MQ_PartNum = tblX.MQ_PartNum ) as u14_sysrowid ";
                _strSQL += "from ( ";
                _strSQL += "select pp.MQ_Company, pp.U14_Key5, pp.MQ_OrderNum, pp.MQ_PartNum, pp.MQ_PartDescription, pp.MQ_IUM ";
                _strSQL += ", pp.MQ_LotNum, isnull(SUM(pp.MQ_Quantity),0) as quantity,  pp.U14_Character09 as customer ";
                _strSQL += ", pp.U14_Character10 as OrderComment, pp.U14_ShortChar11 as packer, pp.U14_ShortChar12 as pallet ";
                _strSQL += ", pp.U14_ShortChar13 as consignment, pp.U14_ShortChar14 as transporter, pp.U14_Number18 as total_weight ";
                _strSQL += ", pp.U14_Number19 as total_carton, pp.U14_Date01 as expired_date, '" + strTagNum + "' as tagno ";
                _strSQL += "from PickPack pp ";
                _strSQL += "group by pp.MQ_Company, pp.U14_Key5, pp.MQ_OrderNum, pp.MQ_PartNum, pp.MQ_PartDescription, pp.MQ_IUM, pp.MQ_LotNum ";
                _strSQL += ", pp.U14_Character09, pp.U14_Character10, pp.U14_ShortChar11, pp.U14_ShortChar12, pp.U14_ShortChar13, pp.U14_ShortChar14 ";
                _strSQL += ", pp.U14_Number18, pp.U14_Number19, pp.U14_Date01 ";
                _strSQL += ") tblX ";
                _strSQL += "left join OrderHed o on tblX.MQ_Company = o.Company and tblX.MQ_OrderNum = o.OrderNum ";
                _strSQL += "left join Part p on tblX.MQ_Company = p.Company and tblX.MQ_PartNum = p.PartNum ";
                _strSQL += "left join erp.ShipVia s on o.Company = s.Company and o.ShipViaCode = s.ShipViaCode ";
                _strSQL += "Where tblX.MQ_Company = '" + strCompany + "' ";

                if (strCustID != "" && strCustID != null)
                {
                    _strSQL += " and tblx.customer like '" + strCustID + "%' ";
                }

                if (strCustName != "" && strCustName != null)
                {
                    _strSQL += " and tblx.customer like  '%" + strCustName + "%' ";
                }

                if (strPickNum != "" && strPickNum != null)
                {
                    _strSQL += " and tblx.U14_Key5 = '" + strPickNum + "' ";
                }

                if (strTagNum != "" && strTagNum != null)
                {
                    _strSQL += " and tblX.U14_Key5 in (select distinct top 1 x.u14_key5 from PickPack x where x.U14_ShortChar17 = '" + strTagNum + "') ";
                    _strSQL += " and 0 = isnull((select top 1 case when s.ShipStatus = 'INVOICED' then 1 else 0 end from ShipHead s where s.Company = tblX.MQ_Company and s.FS_PickListNo_c = tblX.U14_Key5 ),0) ";

                }

                _strSQL += " and tblX.U14_Key5 in (select distinct x.u14_key5 from pickpack x where x.u14_checkbox20 = 1 ) ";
                _strSQL += " order by cast(tblX.MQ_OrderNum as int) ";


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PickPack2 oPickPack = new PickPack2();

                        oPickPack.Company = row["MQ_Company"].ToString();
                        oPickPack.CartonNum = DBNull.Value.Equals(row["total_carton"]) ? 0M : decimal.Parse(row["total_carton"].ToString()); 
                        oPickPack.Customer = (row["customer"].ToString());
                        oPickPack.ExpiryDate = DBNull.Value.Equals(row["expired_date"]) ? "1999-01-01" : Convert.ToDateTime((row["expired_date"])).ToString("yyyy-MM-dd");
                        oPickPack.LotNum = (row["MQ_LotNum"].ToString());
                        oPickPack.OrderNum = (row["MQ_OrderNum"].ToString());
                        oPickPack.PalletNum = (row["pallet"].ToString());
                        oPickPack.PartDesc = (row["MQ_PartDescription"].ToString());
                        oPickPack.PartNum = row["MQ_PartNum"].ToString();
                        oPickPack.Picker = row["Packer"].ToString();
                        oPickPack.PickListNum = row["u14_key5"].ToString();
                        strPickingNum = row["u14_key5"].ToString();
                        oPickPack.PickQty = DBNull.Value.Equals(row["quantity"]) ? 0 : decimal.Parse(row["quantity"].ToString()); ;
                        oPickPack.Remark = (row["OrderComment"].ToString());

                        oPickPack.SerialNum = (row["AP_SerialNo_c"].ToString());
                        oPickPack.ShipVia = (row["ShipViaCode"].ToString());
                        oPickPack.ShipViaDesc = (row["Description"].ToString());
                        //oPickPack.StationId = (row[""].ToString()); //
                        oPickPack.TagNum = (row["tagno"].ToString());
                        oPickPack.TotalWeight = DBNull.Value.Equals(row["total_weight"]) ? 0M : decimal.Parse(row["total_weight"].ToString());
                        oPickPack.Transporter = (row["transporter"].ToString());
                        oPickPack.UOM = (row["MQ_IUM"].ToString());

                        oPickPack.BumiAgDONum = (row["AP_BumiAgDONum_c"].ToString());
                        oPickPack.UrgentOrder = (row["AP_UrgentOrder_c"].ToString() == "True" ? true : false);

                        oPickPack.U14_SysRowId = (row["u14_sysrowid"].ToString());
                        oPickPack.MQ_SysRowId = (row["mq_sysrowid"].ToString());

                        oPickPackList.Add(oPickPack);
                    }

                    // update to ud14 table
                    _strSQL = "update ice.ud14 ";
                    _strSQL += "set shortchar11 = '" + strUserID + "' ";
                    _strSQL += "where Company = '" + strCompany + "' ";
                    _strSQL += "and key5 = '" + strPickingNum + "' ";

                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);


                    // update packer complete date
                    _strSQL = "update xd ";
                    _strSQL += "set xd.DateTime03_c = getdate() ";
                    _strSQL += "from ice.ud14_UD xd ";
                    _strSQL += "inner join ice.UD14 x on x.SysRowID = xd.foreignSysrowid ";
                    _strSQL += "where x.Key5 = '" + strPickingNum + "' ";
                    _strSQL += "and x.Company = '" + strCompany + "' ";

                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);




                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "No picking list generated";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }

		public bool _LoadPickPacksUD103(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strCustID, string strCustName, ref IList<PickPack2> oPickPackList, string strPickNum, string strTagNum, string strUserID, out string strMessage)
		{
			bool IsError = false;
			string strPickingNum = "";
			try
			{
				string _strSQL = "select UD103.Company, UD103.Key1 as PickListNum, UD103A.SD_OrderNum_c as OrderNum, OrderDate, SD_PartNum_c as PartNum, PartDescription, ";
                _strSQL += "SD_UOM_c as UOM,SD_LotNum_c as LotNum, sum(SD_AllocateQuantity_c) as AllocateQuantity, SD_CustID_c + ' - ' + c.Name + CASE WHEN ST.Address1 IS NOT NULL THEN + CHAR(10) + CHAR(13) + " +
                    "ST.Address1 + CHAR(10) + CHAR(13) + ST.Address2 + CHAR(10) + CHAR(13) + ST.Address3 + CHAR(10) + CHAR(13) + ST.Zip + ' ' + ST.City + ' ' + ST.State + ' ' + " +
                    "ST.Country ELSE '' END as Customer, oh.OrderComment, ";
				_strSQL += "UD103.SD_PackedBy_c as PackedBy, UD103.SD_PalletNum_c as PalletNum, SD_Consignment_c as Consignment, ";
				_strSQL += "SD_Transporter_c as Transporter, SD_TotalWeight_c as TotalWeight, SD_TotalCarton_c as TotalCarton, pl.ExpirationDate, ";
				_strSQL += "oh.AP_BumiAgDONum_c as BumiAgDONum, SD_Urgent_c as Urgent, UD103.SD_TagNum_c as TagNum, ";
                _strSQL += "oh.ShipViaCode, sv.Description as ShipViaDesc ";
                _strSQL += ", ROW_NUMBER() OVER (PARTITION BY UD103.Company ORDER BY UD103.SD_Urgent_c desc, oh.OrderDate, UD103A.SD_OrderNum_c, UD103.Key1) AS PLRow ";
				_strSQL += "from UD103 join UD103A ";
				_strSQL += "on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 ";
				_strSQL += "join Part p on UD103A.Company = p.Company and UD103A.SD_PartNum_c = p.PartNum ";
				_strSQL += "join OrderHed oh on UD103A.Company = oh.Company and UD103A.SD_OrderNum_c = oh.OrderNum ";
                _strSQL += "join Customer c on UD103.Company = c.Company and UD103.SD_CustID_c = c.CustID ";
				_strSQL += "join PartLot pl on UD103A.Company = pl.Company and pl.LotNum = UD103A.SD_LotNum_c and pl.PartNum = UD103A.SD_PartNum_c ";
                _strSQL += "join ShipVia sv on oh.Company = sv.Company and oh.ShipViaCode = sv.ShipViaCode ";
                _strSQL += "left join Dbo.ShipTo ST on OH.Company = ST.Company AND OH.ShipToCustNum = ST.CustNum AND OH.ShipToNum = ST.ShipToNum ";
                //changed to check UD103 voided status by Gary
                _strSQL += "where UD103.Company = '" + strCompany + "' and UD103.SD_Plant_c = '" + strCurPlant + "' and UD103.SD_Voided_c = 0 ";
                _strSQL += "and UD103.SD_PickedComplete_c = 1 and UD103.SD_Status_c in ('PICKED', 'PACKING') ";
                _strSQL += "and oh.OpenOrder = 1 ";
                //_strSQL += "and UD103.SD_ShipVia_c != 'NA' and UD103.SD_ShipVia_c != '' ";

                if (strCustID != "" && strCustID != null)
				{
					_strSQL += " and UD103.SD_CustID_c like '" + strCustID + "%' ";
				}

				if (strCustName != "" && strCustName != null)
				{
					_strSQL += " and UD103.SD_CustID_c like  '%" + strCustName + "%' ";
				}

				if (strPickNum != "" && strPickNum != null)
				{
					_strSQL += " and UD103.Key1 = '" + strPickNum + "' ";
				}

                if (strTagNum != "" && strTagNum != null)
				//if (strTagNum != null)
				{
                    _strSQL += " and UD103.Key1 in (select distinct top 1 UDA.Key1 from dbo.UD103A UDA where UD103.Company = UDA.Company and UD103.Key1 = UDA.Key1 ";
                    _strSQL += " and UDA.SD_Voided_c = 0 and UDA.SD_PickedBy_c != '' and UDA.SD_TagNum_c = '" + strTagNum + "') ";
                    //_strSQL += " and UD103.Key1 in (select distinct top 1 UDA.Key1  from UD103 UD inner join UD103A UDA ON UD.Company = UDA.Company and UD.Key1 = UDA.Key1 ";
                    //_strSQL += " where UD.SD_PickedComplete_c = 1 and UD.SD_Status_c in ('PICKED', 'PACKING') and UDA.SD_TagNum_c = '" + strTagNum + "') ";
                    _strSQL += " and 0 = isnull((select top 1 case when s.ShipStatus = 'INVOICED' then 1 else 0 end from ShipHead s where s.Company = UD103.Company and s.FS_PickListNo_c = UD103.Key1 ),0) ";
				}

                if (strUserID != "" && strUserID != null)
                {
                    _strSQL += " and UD103.SD_PackedBy_c = CASE WHEN EXISTS (SELECT * FROM UD103 UD WHERE UD103.SysRowID = UD.SysRowID AND UD.SD_PackedBy_c = '" + strUserID + "') ";
                    _strSQL += "THEN '" + strUserID + "' ELSE '' END ";
                }

                // moved the filter query to the code above to prevent multiple picklists in one api result
				//_strSQL += " and UD103.Key1 in (select distinct UD103.Key1 from UD103 where UD103.SD_PickedComplete_c = 1 ";
    //            _strSQL += " and UD103.SD_Status_c = 'PICKED' or UD103.SD_Status_c = 'PACKING') ";

                _strSQL += "group by UD103.Company, UD103.Key1, UD103A.SD_OrderNum_c, OrderDate, SD_PartNum_c, PartDescription, SD_UOM_c, SD_LotNum_c, SD_CustID_c, OrderComment, UD103.SD_PackedBy_c, " +
					"UD103.SD_PalletNum_c, SD_Consignment_c, SD_Transporter_c, SD_TotalWeight_c, " +
                    "SD_TotalCarton_c, ExpirationDate, AP_BumiAgDONum_c, SD_Urgent_c, UD103.SD_TagNum_c, oh.ShipViaCode, sv.Description, c.Name, ST.Address1, ST.Address2, ST.Address3, ST.Zip, ST.City, ST.State, ST.Country ";

                //_strSQL = "select *, (select top 1 SysRowID from UD103A a where x.Company = a.Company and x.OrderNum = a.SD_OrderNum_c and x.PartNum = a.SD_PartNum_c ) as SysRowID from (" + _strSQL + ") x ";
                _strSQL = " WITH InQuery AS ( select *, (select top 1 SysRowID from UD103A a where x.Company = a.Company and x.OrderNum = a.SD_OrderNum_c and x.PartNum = a.SD_PartNum_c ) as SysRowID from (" + _strSQL + ") x ) ";
                _strSQL += " SELECT * FROM InQuery WHERE InQuery.PickListNum = (SELECT PickListNum FROM InQuery WHERE InQuery.PLRow = 1) ";
                //_strSQL += "where x.PLRow = 1 ";

                // _strSQL += "order by UD103.SD_Urgent_c desc, oh.OrderDate, cast(UD103A.SD_OrderNum_c as int) ";
                _strSQL += "order by Urgent desc, OrderDate, cast(OrderNum as int) ";

				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						PickPack2 oPickPack = new PickPack2();

						oPickPack.Company = row["Company"].ToString();
						oPickPack.CartonNum = DBNull.Value.Equals(row["TotalCarton"]) ? 0M : decimal.Parse(row["TotalCarton"].ToString());
						oPickPack.Customer = (row["Customer"].ToString());
                        if (row["ExpirationDate"] == DBNull.Value)
                        {
                            oPickPack.ExpiryDate = "1999-01-01";
                        }
                        else
                        {
                            oPickPack.ExpiryDate = Convert.ToDateTime(row["ExpirationDate"]).ToString("yyyy-MM-dd");
						}
                        // oPickPack.ExpiryDate = (row["ExpiratonDate"] == DBNull.Value) ? "1999-01-01" : Convert.ToDateTime(row["ExpirationDate"]).ToString("yyyy-MM-dd");
						oPickPack.LotNum = (row["LotNum"].ToString());
						oPickPack.OrderNum = (row["OrderNum"].ToString());
						oPickPack.PalletNum = (row["PalletNum"].ToString());
						oPickPack.PartDesc = (row["PartDescription"].ToString());
						oPickPack.PartNum = row["PartNum"].ToString();
						oPickPack.Picker = row["PackedBy"].ToString();
						oPickPack.PickListNum = row["PickListNum"].ToString();
						strPickingNum = row["PickListNum"].ToString();
						oPickPack.PickQty = DBNull.Value.Equals(row["AllocateQuantity"]) ? 0 : decimal.Parse(row["AllocateQuantity"].ToString()); ;
						oPickPack.Remark = (row["OrderComment"].ToString());

						// oPickPack.SerialNum = (row["AP_SerialNo_c"].ToString());
						oPickPack.ShipVia = (row["ShipViaCode"].ToString());
						oPickPack.ShipViaDesc = (row["ShipViaDesc"].ToString());
						//oPickPack.StationId = (row[""].ToString()); //
						oPickPack.TagNum = (row["TagNum"].ToString());
						oPickPack.TotalWeight = DBNull.Value.Equals(row["TotalWeight"]) ? 0M : decimal.Parse(row["TotalWeight"].ToString());
						oPickPack.Transporter = (row["Transporter"].ToString());
						oPickPack.UOM = (row["UOM"].ToString());

						oPickPack.BumiAgDONum = (row["BumiAgDONum"].ToString());
						oPickPack.UrgentOrder = (row["Urgent"].ToString() == "True" ? true : false);

						oPickPack.U14_SysRowId = (row["SysRowID"].ToString());
						//oPickPack.MQ_SysRowId = (row["mq_sysrowid"].ToString());

						oPickPackList.Add(oPickPack);
					}

					// update to ud103 table
					_strSQL = "update UD103 ";
					_strSQL += "set SD_PackedBy_c = '" + strUserID + "', ";
                    _strSQL += "SD_Status_c = 'PACKING' "; 
					_strSQL += "where Company = '" + strCompany + "' ";
					_strSQL += "and Key1 = '" + strPickingNum + "' ";

					_MSSQL._exeSQLCommand(_strSQL, _strSQLCon);


					// update packer complete date
					//_strSQL = "update xd ";
					//_strSQL += "set xd.DateTime03_c = getdate() ";
					//_strSQL += "from ice.ud14_UD xd ";
					//_strSQL += "inner join ice.UD14 x on x.SysRowID = xd.foreignSysrowid ";
					//_strSQL += "where x.Key5 = '" + strPickingNum + "' ";
					//_strSQL += "and x.Company = '" + strCompany + "' ";

     //               _strSQL = "update UD103 ";
     //               _strSQL += "set SD_PackedComplete_c = getdate() ";
     //               _strSQL += "where Key1 = '" + strPickingNum + "' ";
     //               _strSQL += "and Company = '" + strCompany + "' ";

					//_MSSQL._exeSQLCommand(_strSQL, _strSQLCon);


					strMessage = "";
					IsError = false;

				}
				else
				{
					strMessage = "No picking list generated";
					IsError = true;
				}

			}
			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}


			return (IsError ? false : true);

		}

		public bool _LoadPalletNum(ref EpicEnv oEpicEnv, string strCompany, string strOwnership, ref IList<Pallets> PalletsList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT company, key1, Character01, shortchar01 ";
                _strSQL += "FROM ice.UD19  ";

                string _strWHERE = "";


                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND Company = '" + strCompany + "' "; }

                if (strOwnership != null && strOwnership != "")
                { _strWHERE += "AND shortchar01 = '" + strOwnership + "' "; }

                if (_strWHERE != null && _strWHERE != "")
                { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }

                _strSQL += " order by key1 ";


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Pallets oPallet = new Pallets();

                        oPallet.Company = row["Company"].ToString();
                        oPallet.PalletNum = row["key1"].ToString();
                        oPallet.Description = row["Character01"].ToString();
                        oPallet.Ownership = row["shortchar01"].ToString();

                        PalletsList.Add(oPallet);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No pallet number found";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);



        }

		public bool _AssignPickPacks(ref EpicEnv oEpicEnv, string strCompany, string strPicker, ref IList<PickPack> oPickPackList,  out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "SDS_AssignPickPack";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company",strCompany),
                    new SqlParameter("@Picker",strPicker)
                }; 

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResultStoreProcedure(_strSQL, _strSQLCon, parameters);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PickPack oPickPack = new PickPack();
                        oPickPack.Company = row["MQ_Company"].ToString();
                        oPickPack.MQ_AssemblySeq = DBNull.Value.Equals(row["MQ_AssemblySeq"]) ? 0 : int.Parse(row["MQ_AssemblySeq"].ToString());
                        oPickPack.MQ_AssignedToEmpID = (row["MQ_AssignedToEmpID"].ToString());
                        oPickPack.MQ_DistributionType = (row["MQ_DistributionType"].ToString());
                        oPickPack.MQ_EpicorFSA = (row["MQ_EpicorFSA"].ToString());
                        oPickPack.MQ_ForeignSysRowID = (row["MQ_ForeignSysRowID"].ToString());
                        oPickPack.MQ_FromBinNum = (row["MQ_FromBinNum"].ToString());
                        oPickPack.MQ_FromPCID = (row["MQ_FromPCID"].ToString());
                        oPickPack.MQ_FromWhse = (row["MQ_FromWhse"].ToString());
                        oPickPack.MQ_FS_PrintStatus_c = (row["MQ_FS_PrintStatus_c"].ToString());
                        oPickPack.MQ_IUM = (row["MQ_IUM"].ToString());
                        oPickPack.MQ_JobNum = (row["MQ_JobNum"].ToString());
                        oPickPack.MQ_JobSeq = DBNull.Value.Equals(row["MQ_JobSeq"]) ? 0 : int.Parse(row["MQ_JobSeq"].ToString());
                        oPickPack.MQ_JobSeqType = (row["MQ_JobSeqType"].ToString());
                        oPickPack.MQ_LastMgrChangeEmpID = (row["MQ_LastMgrChangeEmpID"].ToString());
                        oPickPack.MQ_LastUsedPCID = (row["MQ_LastUsedPCID"].ToString());
                        oPickPack.MQ_Lock = row["MQ_Lock"].ToString() == "True" ? true : false;
                        oPickPack.MQ_LotNum = (row["MQ_LotNum"].ToString());
                        oPickPack.MQ_MtlQueueSeq = DBNull.Value.Equals(row["MQ_MtlQueueSeq"]) ? 0 : int.Parse(row["MQ_MtlQueueSeq"].ToString());
                        oPickPack.MQ_NCTranID = (row["MQ_NCTranID"].ToString());
                        oPickPack.MQ_NeedByDate = (row["MQ_NeedByDate"].ToString());
                        oPickPack.MQ_NeedByTime = (row["MQ_NeedByTime"].ToString());
                        oPickPack.MQ_NextAssemblySeq = DBNull.Value.Equals(row["MQ_NextAssemblySeq"]) ? 0 : int.Parse(row["MQ_NextAssemblySeq"].ToString());
                        oPickPack.MQ_NextJobSeq = DBNull.Value.Equals(row["MQ_NextJobSeq"]) ? 0 : int.Parse(row["MQ_NextJobSeq"].ToString());
                        oPickPack.MQ_Obs10_PkgLine = (row["MQ_Obs10_PkgLine"].ToString());
                        oPickPack.MQ_Obs10_PkgNum = (row["MQ_Obs10_PkgNum"].ToString());
                        oPickPack.MQ_OrderLine = DBNull.Value.Equals(row["MQ_OrderLine"]) ? 0 : int.Parse(row["MQ_OrderLine"].ToString());
                        oPickPack.MQ_OrderNum = DBNull.Value.Equals(row["MQ_OrderNum"]) ? 0 : int.Parse(row["MQ_OrderNum"].ToString());
                        oPickPack.MQ_OrderRelNum = DBNull.Value.Equals(row["MQ_OrderRelNum"]) ? 0 : int.Parse(row["MQ_OrderRelNum"].ToString());
                        oPickPack.MQ_PackLine = (row["MQ_PackLine"].ToString());
                        oPickPack.MQ_PackSlip = (row["MQ_PackSlip"].ToString());
                        oPickPack.MQ_PackStation = (row["MQ_PackStation"].ToString());
                        oPickPack.MQ_PartDescription = (row["MQ_PartDescription"].ToString());
                        oPickPack.MQ_PartNum = (row["MQ_PartNum"].ToString());
                        oPickPack.MQ_PCID = (row["MQ_PCID"].ToString());

                        oPickPack.MQ_Plant = (row["MQ_Plant"].ToString());
                        oPickPack.MQ_POLine = DBNull.Value.Equals(row["MQ_POLine"]) ? 0 : int.Parse(row["MQ_POLine"].ToString());
                        oPickPack.MQ_PONum = DBNull.Value.Equals(row["MQ_PONum"]) ? 0 : int.Parse(row["MQ_PONum"].ToString());
                        oPickPack.MQ_PORelNum = DBNull.Value.Equals(row["MQ_PORelNum"]) ? 0 : int.Parse(row["MQ_PORelNum"].ToString());
                        oPickPack.MQ_Priority = (row["MQ_Priority"].ToString());
                        oPickPack.MQ_PurPoint = (row["MQ_PurPoint"].ToString());
                        oPickPack.MQ_Quantity = DBNull.Value.Equals(row["MQ_Quantity"]) ? 0 : decimal.Parse(row["MQ_Quantity"].ToString());
                        oPickPack.MQ_QueueID = (row["MQ_QueueID"].ToString());
                        oPickPack.MQ_QueuePickSeq = DBNull.Value.Equals(row["MQ_QueuePickSeq"]) ? 0 : int.Parse(row["MQ_QueuePickSeq"].ToString());
                        oPickPack.MQ_Reference = (row["MQ_Reference"].ToString());
                        oPickPack.MQ_ReferencePrefix = (row["MQ_ReferencePrefix"].ToString());
                        oPickPack.MQ_ReleaseForPickingSeq = DBNull.Value.Equals(row["MQ_ReleaseForPickingSeq"]) ? 0 : int.Parse(row["MQ_ReleaseForPickingSeq"].ToString());
                        oPickPack.MQ_RequestedByEmpID = (row["MQ_RequestedByEmpID"].ToString());
                        oPickPack.MQ_RevisionNum = (row["MQ_RevisionNum"].ToString());
                        oPickPack.MQ_RMADisp = (row["MQ_RMADisp"].ToString());
                        oPickPack.MQ_RMALine = DBNull.Value.Equals(row["MQ_RMALine"]) ? 0 : int.Parse(row["MQ_RMALine"].ToString());
                        oPickPack.MQ_RMANum = (row["MQ_RMANum"].ToString());
                        oPickPack.MQ_RMAReceipt = row["MQ_RMAReceipt"].ToString() == "True" ? true : false;


                        oPickPack.MQ_SelectedByEmpID = (row["MQ_SelectedByEmpID"].ToString());
                        oPickPack.MQ_SysDate = (row["MQ_SysDate"].ToString());
                        oPickPack.MQ_SysRevID = (row["MQ_SysRevID"].ToString());
                        oPickPack.MQ_SysRowID = (row["MQ_SysRowID"].ToString());
                        oPickPack.MQ_SysTime = (row["MQ_SysTime"].ToString());

                        oPickPack.MQ_TargetAssemblySeq = DBNull.Value.Equals(row["MQ_TargetAssemblySeq"]) ? 0 : int.Parse(row["MQ_TargetAssemblySeq"].ToString());
                        oPickPack.MQ_TargetJobNum = DBNull.Value.Equals(row["MQ_TargetJobNum"]) ? string.Empty : row["MQ_TargetJobNum"].ToString();
                        oPickPack.MQ_TargetMtlSeq = DBNull.Value.Equals(row["MQ_TargetMtlSeq"]) ? 0 : int.Parse(row["MQ_TargetMtlSeq"].ToString());
                        oPickPack.MQ_TargetTFOrdLine = DBNull.Value.Equals(row["MQ_TargetTFOrdLine"]) ? 0 : int.Parse(row["MQ_TargetTFOrdLine"].ToString());
                        oPickPack.MQ_TargetTFOrdNum = DBNull.Value.Equals(row["MQ_TargetTFOrdNum"]) ? string.Empty : (row["MQ_TargetTFOrdNum"].ToString());

                        oPickPack.MQ_ToBinNum = (row["MQ_ToBinNum"].ToString());
                        oPickPack.MQ_ToPCID = (row["MQ_ToPCID"].ToString());
                        oPickPack.MQ_ToWhse = (row["MQ_ToWhse"].ToString());
                        oPickPack.MQ_TranSource = (row["MQ_TranSource"].ToString());
                        oPickPack.MQ_TranStatus = (row["MQ_TranStatus"].ToString());
                        oPickPack.MQ_TranType = (row["MQ_TranType"].ToString());
                        oPickPack.MQ_UD_SysRevID = (row["MQ_UD_SysRevID"].ToString());


                        oPickPack.MQ_Updated_c = (row["MQ_Updated_c"].ToString());
                        oPickPack.MQ_VendorNum = DBNull.Value.Equals(row["MQ_VendorNum"]) ? 0 : int.Parse(row["MQ_VendorNum"].ToString());
                        oPickPack.MQ_Visible = (row["MQ_Visible"].ToString() == "True" ? true : false);
                        oPickPack.MQ_WaveNum = DBNull.Value.Equals(row["MQ_WaveNum"]) ? 0 : int.Parse(row["MQ_WaveNum"].ToString());
                        oPickPack.MQ_WhseGroupCode = (row["MQ_WhseGroupCode"].ToString());

                        oPickPack.U14_Character01 = (row["U14_Character01"].ToString());
                        oPickPack.U14_Character02 = (row["U14_Character02"].ToString());
                        oPickPack.U14_Character03 = (row["U14_Character03"].ToString());
                        oPickPack.U14_Character04 = (row["U14_Character04"].ToString());
                        oPickPack.U14_Character05 = (row["U14_Character05"].ToString());
                        oPickPack.U14_Character06 = (row["U14_Character06"].ToString());
                        oPickPack.U14_Character07 = (row["U14_Character07"].ToString());
                        oPickPack.U14_Character08 = (row["U14_Character08"].ToString());
                        oPickPack.U14_Character09 = (row["U14_Character09"].ToString());
                        oPickPack.U14_Character10 = (row["U14_Character10"].ToString());
                        oPickPack.U14_CheckBox01 = (row["U14_CheckBox01"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox02 = (row["U14_CheckBox02"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox03 = (row["U14_CheckBox03"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox04 = (row["U14_CheckBox04"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox05 = (row["U14_CheckBox05"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox06 = (row["U14_CheckBox06"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox07 = (row["U14_CheckBox07"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox08 = (row["U14_CheckBox08"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox09 = (row["U14_CheckBox09"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox10 = (row["U14_CheckBox10"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox11 = (row["U14_CheckBox11"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox12 = (row["U14_CheckBox12"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox13 = (row["U14_CheckBox13"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox14 = (row["U14_CheckBox14"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox15 = (row["U14_CheckBox15"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox16 = (row["U14_CheckBox16"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox17 = (row["U14_CheckBox17"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox18 = (row["U14_CheckBox18"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox19 = (row["U14_CheckBox19"].ToString() == "True" ? true : false);
                        oPickPack.U14_CheckBox20 = (row["U14_CheckBox20"].ToString() == "True" ? true : false);

                        oPickPack.SerialNum = (row["AP_SerialNo_c"].ToString() == "True" ? true : false);

                        oPickPack.U14_Company = (row["U14_Company"].ToString());
                        oPickPack.U14_Date01 = DBNull.Value.Equals(row["U14_Date01"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date01"])).ToString("yyyy-MM-dd"); 
                        oPickPack.U14_Date02 = DBNull.Value.Equals(row["U14_Date02"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date02"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date03 = DBNull.Value.Equals(row["U14_Date03"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date03"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date04 = DBNull.Value.Equals(row["U14_Date04"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date04"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date05 = DBNull.Value.Equals(row["U14_Date05"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date05"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date06 = DBNull.Value.Equals(row["U14_Date06"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date06"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date07 = DBNull.Value.Equals(row["U14_Date07"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date07"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date08 = DBNull.Value.Equals(row["U14_Date08"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date08"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date09 = DBNull.Value.Equals(row["U14_Date09"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date09"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date10 = DBNull.Value.Equals(row["U14_Date10"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date10"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date11 = DBNull.Value.Equals(row["U14_Date11"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date11"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date12 = DBNull.Value.Equals(row["U14_Date12"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date12"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date13 = DBNull.Value.Equals(row["U14_Date13"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date13"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date14 = DBNull.Value.Equals(row["U14_Date14"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date14"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date15 = DBNull.Value.Equals(row["U14_Date15"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date15"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date16 = DBNull.Value.Equals(row["U14_Date16"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date16"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date17 = DBNull.Value.Equals(row["U14_Date17"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date17"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date18 = DBNull.Value.Equals(row["U14_Date18"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date18"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date19 = DBNull.Value.Equals(row["U14_Date19"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date19"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_Date20 = DBNull.Value.Equals(row["U14_Date20"]) ? "1999-01-01" : Convert.ToDateTime((row["U14_Date20"])).ToString("yyyy-MM-dd");
                        oPickPack.U14_GlobalLock = (row["U14_GlobalLock"].ToString());
                        oPickPack.U14_GlobalUD14 = (row["U14_GlobalUD14"].ToString());
                        oPickPack.U14_Key1 = (row["U14_Key1"].ToString());
                        oPickPack.U14_Key2 = (row["U14_Key2"].ToString());
                        oPickPack.U14_Key3 = (row["U14_Key3"].ToString());
                        oPickPack.U14_Key4 = (row["U14_Key4"].ToString());
                        oPickPack.U14_Key5 = (row["U14_Key5"].ToString());

                        oPickPack.U14_Number01 = DBNull.Value.Equals(row["U14_Number01"]) ? 0M : decimal.Parse(row["U14_Number01"].ToString());
                        oPickPack.U14_Number02 = DBNull.Value.Equals(row["U14_Number02"]) ? 0M : decimal.Parse(row["U14_Number02"].ToString());
                        oPickPack.U14_Number03 = DBNull.Value.Equals(row["U14_Number03"]) ? 0M : decimal.Parse(row["U14_Number03"].ToString());
                        oPickPack.U14_Number04 = DBNull.Value.Equals(row["U14_Number04"]) ? 0M : decimal.Parse(row["U14_Number04"].ToString());


                        oPickPack.U14_Number05 = DBNull.Value.Equals(row["U14_Number05"]) ? 0M : decimal.Parse(row["U14_Number05"].ToString());
                        oPickPack.U14_Number06 = DBNull.Value.Equals(row["U14_Number06"]) ? 0M : decimal.Parse(row["U14_Number06"].ToString());
                        oPickPack.U14_Number07 = DBNull.Value.Equals(row["U14_Number07"]) ? 0M : decimal.Parse(row["U14_Number07"].ToString());
                        oPickPack.U14_Number08 = DBNull.Value.Equals(row["U14_Number08"]) ? 0M : decimal.Parse(row["U14_Number08"].ToString());
                        oPickPack.U14_Number09 = DBNull.Value.Equals(row["U14_Number09"]) ? 0M : decimal.Parse(row["U14_Number09"].ToString());
                        oPickPack.U14_Number10 = DBNull.Value.Equals(row["U14_Number10"]) ? 0M : decimal.Parse(row["U14_Number10"].ToString());
                        oPickPack.U14_Number11 = DBNull.Value.Equals(row["U14_Number11"]) ? 0M : decimal.Parse(row["U14_Number11"].ToString());
                        oPickPack.U14_Number12 = DBNull.Value.Equals(row["U14_Number12"]) ? 0M : decimal.Parse(row["U14_Number12"].ToString());
                        oPickPack.U14_Number13 = DBNull.Value.Equals(row["U14_Number13"]) ? 0M : decimal.Parse(row["U14_Number13"].ToString());
                        oPickPack.U14_Number14 = DBNull.Value.Equals(row["U14_Number14"]) ? 0M : decimal.Parse(row["U14_Number14"].ToString());
                        oPickPack.U14_Number15 = DBNull.Value.Equals(row["U14_Number15"]) ? 0M : decimal.Parse(row["U14_Number15"].ToString());
                        oPickPack.U14_Number16 = DBNull.Value.Equals(row["U14_Number16"]) ? 0M : decimal.Parse(row["U14_Number16"].ToString());
                        oPickPack.U14_Number17 = DBNull.Value.Equals(row["U14_Number17"]) ? 0M : decimal.Parse(row["U14_Number17"].ToString());
                        oPickPack.U14_Number18 = DBNull.Value.Equals(row["U14_Number18"]) ? 0M : decimal.Parse(row["U14_Number18"].ToString());
                        oPickPack.U14_Number19 = DBNull.Value.Equals(row["U14_Number19"]) ? 0M : decimal.Parse(row["U14_Number19"].ToString());
                        oPickPack.U14_Number20 = DBNull.Value.Equals(row["U14_Number20"]) ? 0M : decimal.Parse(row["U14_Number20"].ToString());


                        oPickPack.U14_ShortChar01 = (row["U14_ShortChar01"].ToString());
                        oPickPack.U14_ShortChar02 = (row["U14_ShortChar02"].ToString());
                        oPickPack.U14_ShortChar03 = (row["U14_ShortChar03"].ToString());
                        oPickPack.U14_ShortChar04 = (row["U14_ShortChar04"].ToString());
                        oPickPack.U14_ShortChar05 = (row["U14_ShortChar05"].ToString());
                        oPickPack.U14_ShortChar06 = (row["U14_ShortChar06"].ToString());
                        oPickPack.U14_ShortChar07 = (row["U14_ShortChar07"].ToString());
                        oPickPack.U14_ShortChar08 = (row["U14_ShortChar08"].ToString());
                        oPickPack.U14_ShortChar09 = (row["U14_ShortChar09"].ToString());
                        oPickPack.U14_ShortChar10 = (row["U14_ShortChar10"].ToString());
                        oPickPack.U14_ShortChar11 = (row["U14_ShortChar11"].ToString());
                        oPickPack.U14_ShortChar12 = (row["U14_ShortChar12"].ToString());
                        oPickPack.U14_ShortChar13 = (row["U14_ShortChar13"].ToString());
                        oPickPack.U14_ShortChar14 = (row["U14_ShortChar14"].ToString() == "" ? row["ShipViaCode"].ToString() : row["U14_ShortChar14"].ToString());
                        oPickPack.U14_ShortChar15 = (row["U14_ShortChar15"].ToString());
                        oPickPack.U14_ShortChar16 = (row["U14_ShortChar16"].ToString());
                        oPickPack.U14_ShortChar17 = (row["U14_ShortChar17"].ToString());
                        oPickPack.U14_ShortChar18 = (row["U14_ShortChar18"].ToString());
                        oPickPack.U14_ShortChar19 = (row["U14_ShortChar19"].ToString());
                        oPickPack.U14_ShortChar20 = (row["U14_ShortChar20"].ToString());
                        oPickPack.U14_SysRevID = (row["U14_SysRevID"].ToString());
                        oPickPack.U14_SysRowID = (row["U14_SysRowID"].ToString());

                        oPickPack.BumiAgDONum = (row["AP_BumiAgDONum_c"].ToString());
                        oPickPack.UrgentOrder = (row["AP_UrgentOrder_c"].ToString() == "True" ? true : false);


                        //oPickPack.Approve = row["Approve"].ToString() == "True" ? true : false;
                        //oPurOrd.DocTotalOrder = decimal.Parse(row["DocTotalOrder"].ToString());
                        //oPurOrd.PODate = Convert.ToDateTime((row["OrderDate"]));


                        oPickPackList.Add(oPickPack);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "No picking list generated";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }

		public bool _AssignPickPacksUD103(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strPicker, ref IList<PickPack> oPickPackList, out string strMessage)
		{
			bool IsError = false;
            bool IsUpdated = false;
            var pickListNum = "";
            string strPickGrps = "''";
			try
			{
				string _strSQL = "select top 1 SD_ProdGrps_c as ProductGroups from UD20 where Key1 = '" + strPicker + "' and SD_ProdGrps_c != ''";

				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						strPickGrps = row["ProductGroups"].ToString().Replace("~", "\',\'");
						strPickGrps = "\'" + strPickGrps + "\'";
					}

				}

                _strSQL = "select distinct UD103.Key1, SD_Urgent_c, oh.OrderDate, oh.OrderNum, case when UD103.SD_PickedBy_c = '" + strPicker + "' then 1 " +
                    "when UD103.SD_PickedBy_c = '' then 2 end [Assigned] from UD103 join UD103A on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 " +
                    "join OrderHed oh on oh.Company = UD103.Company and oh.OrderNum = UD103A.SD_OrderNum_c where (UD103.SD_PickedBy_c = '' or UD103.SD_PickedBy_c = '" + strPicker + "') " +
                    "and SD_PickedComplete_c = 0 and (SD_Status_c = 'ALLOCATED' or SD_Status_c = 'PICKING') and SD_BackOrder_c = 0 ";
                _strSQL += "and UD103.SD_ShipVia_c != 'NA' and UD103.SD_ShipVia_c != '' ";
				_strSQL += "and SD_PickListGroup_c in (" + strPickGrps + ") ";
				_strSQL += "and UD103.Company = '" + strCompany + "' and SD_Plant_c = '" + strCurPlant + "' and UD103.SD_Voided_c = 0 ";   // add company and plant filter
				_strSQL += "order by Assigned, SD_Urgent_c desc, oh.OrderDate, oh.OrderNum";

				//SQLServerBO _MSSQL = new SQLServerBO();
				//string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				_dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    DataRow row = _dts.Tables[0].Rows[0];
                    _strSQL = "update UD103 set SD_PickedBy_c = '" + strPicker + "', SD_Status_c = 'PICKING' where Key1 = '" + row["Key1"].ToString() + "' ";
                    pickListNum = row["Key1"].ToString();

					IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);
				}

                //bool hasLines = false;
                if (IsUpdated)
                //{
                //    _strSQL = "select * from UD103A where UD103A.SD_Voided_c = 0 and UD103A.Key1 = '" + pickListNum + "'";
				//	_dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);
                //    hasLines = _dts.Tables[0].Rows.Count > 0;
				//}

                // if (IsUpdated && hasLines)
                {
                    _strSQL = "select UD103.SD_OrderNum_c, SD_OrderLine_c, SD_OrderRel_c, SD_PartNum_c, ";
                    _strSQL += "PartDescription, SD_UOM_c, SD_AllocateQuantity_c, ExpirationDate, ";
                    _strSQL += "SD_LotNum_c, SD_Warehouse_c, SD_BinNum_c, SD_Urgent_c, SD_CustID_c, ";
                    _strSQL += "CustID + ' - ' + Name + CHAR(10) + CHAR(13) + Address1 + CHAR(10) + CHAR(13) + Address2 + CHAR(10) + CHAR(13) + ";
                    _strSQL += "Address3 + CHAR(10) + CHAR(13) + Zip + ' ' + City + ' ' + State + ' ' + Country as 'CustDetails', ";
                    _strSQL += "OrderComment, UD103.Key1, UD103A.ChildKey2, SD_Transporter_c, SD_ShipVia_c, UD103A.SD_PickedBy_c, AP_BumiAgDONum_c, UD103A.SysRowID ";
                    _strSQL += "from UD103 join UD103A on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 ";
                    _strSQL += "join Part p on UD103.Company = p.Company and UD103A.SD_PartNum_c = p.PartNum ";
                    _strSQL += "join PartLot pl on UD103.Company = pl.Company and UD103A.SD_PartNum_c = pl.PartNum and SD_LotNum_c = LotNum ";
                    _strSQL += "join Customer c on UD103.Company = c.Company and c.CustNum = UD103.SD_CustNum_c ";
                    _strSQL += "join OrderHed oh on UD103.Company = oh.Company and UD103A.SD_OrderNum_c = oh.OrderNum ";
                    _strSQL += "where UD103.Key1 = '" + pickListNum + "' and UD103A.SD_Voided_c = 0";
                    // and SD_Voided_c = 0

                    _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in _dts.Tables[0].Rows)
                        {
                            PickPack oPickPack = new PickPack();
                            oPickPack.MQ_FromBinNum = (row["SD_BinNum_c"].ToString());
                            oPickPack.MQ_FromWhse = (row["SD_Warehouse_c"].ToString());
                            oPickPack.MQ_IUM = (row["SD_UOM_c"].ToString());
                            oPickPack.MQ_LotNum = (row["SD_LotNum_c"].ToString());
                            oPickPack.MQ_OrderLine = DBNull.Value.Equals(row["SD_OrderLine_c"]) ? 0 : int.Parse(row["SD_OrderLine_C"].ToString());
                            oPickPack.MQ_OrderNum = DBNull.Value.Equals(row["SD_OrderNum_c"]) ? 0 : int.Parse(row["SD_OrderNum_c"].ToString());
                            oPickPack.MQ_OrderRelNum = DBNull.Value.Equals(row["SD_OrderRel_c"]) ? 0 : int.Parse(row["SD_OrderRel_c"].ToString());
                            oPickPack.MQ_PartDescription = (row["PartDescription"].ToString());
                            oPickPack.MQ_PartNum = (row["SD_PartNum_c"].ToString());
                            oPickPack.MQ_Quantity = DBNull.Value.Equals(row["SD_AllocateQuantity_c"]) ? 0 : decimal.Parse(row["SD_AllocateQuantity_c"].ToString());
                            oPickPack.U14_Character09 = (row["CustDetails"].ToString());
                            oPickPack.U14_Character10 = (row["OrderComment"].ToString());
                            oPickPack.U14_CheckBox18 = (row["SD_PickedBy_c"].ToString() == "" ? false : true);
                            oPickPack.U14_Date01 = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");
                            oPickPack.U14_Key4 = (row["ChildKey2"].ToString());     // picklistline
                            oPickPack.U14_Key5 = (row["Key1"].ToString());   // picklistnum
                            oPickPack.U14_ShortChar14 = (row["SD_Transporter_c"].ToString() == "" ? row["SD_ShipVia_c"].ToString() : row["SD_Transporter_c"].ToString());

                            oPickPack.BumiAgDONum = (row["AP_BumiAgDONum_c"].ToString());
                            oPickPack.UrgentOrder = (row["SD_Urgent_c"].ToString() == "True" ? true : false);

                            oPickPack.U14_SysRowID = row["SysRowID"].ToString();


                            oPickPackList.Add(oPickPack);
                        }

                        strMessage = "";
                        IsError = false;

                    }
                    else
                    {
                        strMessage = "No picking list generated";
                        IsError = true;
                    }
     //           }
     //           else if (IsUpdated && !hasLines) 
     //           {
					//_strSQL = "update UD103 set SD_Voided_c = 1 where Key1 = '" + pickListNum + "' ";

					//bool voidUpdate = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

     //               if (voidUpdate)
     //               {
					//	IsError = _AssignPickPacksUD103(ref oEpicEnv, strCompany, strCurPlant, strPicker, ref oPickPackList, out strMessage);
					//}
     //               else
     //               {
     //                   strMessage = "Something went wrong during the retrieval.";
     //                   IsError = true;
     //               }
					
				}
                else
                {
                    strMessage = "No picking list generated";
                    IsError = true;
                }

			}
			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}


			return (IsError ? false : true);

		}

		public bool _AssignBackPickPacks(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strPicker, string strPartNum, ref IList<PickPack> oPickPackList, out string strMessage)
		{
			bool IsError = false;
			bool IsUpdated = false;
			var pickListNum = "";
            var strPickGrps = "";
            bool hasProductGroup = false;

			try
			{
				string _strSQL = "select top 1 SD_ProdGrps_c as ProductGroups from UD20 where Key1 = '" + strPicker + "' and SD_ProdGrps_c != '' and SD_BackOrder_c = 1";

				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						strPickGrps = row["ProductGroups"].ToString().Replace("~", "\',\'");
						strPickGrps = "\'" + strPickGrps + "\'";
					}
                    
                    hasProductGroup = true;

                }

                if (hasProductGroup)
                {
                    _strSQL = "select distinct UD103.Key1, SD_Urgent_c, oh.OrderDate, oh.OrderNum, case when UD103.SD_PickedBy_c = '" + strPicker + "' then 1 " +
                        "when UD103.SD_PickedBy_c = '' then 2 end [Assigned] from UD103 join UD103A on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 " +
                        "join OrderHed oh on oh.Company = UD103.Company and oh.OrderNum = UD103A.SD_OrderNum_c where (UD103.SD_PickedBy_c = '' or UD103.SD_PickedBy_c = '" + strPicker + "') " +
                        "and SD_PickedComplete_c = 0 and (SD_Status_c = 'ALLOCATED' or SD_Status_c = 'PICKING') and SD_BackOrder_c = 1 and UD103.SD_Voided_c = 0 ";
                    _strSQL += "and UD103.SD_ShipVia_c != 'NA' and UD103.SD_ShipVia_c != '' ";
                    _strSQL += "and SD_PickListGroup_c in (" + strPickGrps + ") and UD103.Company = '" + strCompany + "' and SD_Plant_c = '" + strCurPlant + "' ";
                    _strSQL += "and SD_PartNum_c = '" + strPartNum + "' order by Assigned, SD_Urgent_c desc, oh.OrderDate, oh.OrderNum";

                    //SQLServerBO _MSSQL = new SQLServerBO();
                    //string _strSQLCon = _MSSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                    _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = _dts.Tables[0].Rows[0];
                        _strSQL = "update UD103 set SD_PickedBy_c = '" + strPicker + "', SD_Status_c = 'PICKING' where Key1 = '" + row["Key1"].ToString() + "' ";
                        pickListNum = row["Key1"].ToString();

                        IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);
                    }

                    if (IsUpdated)
                    {
                        _strSQL = "select UD103.SD_OrderNum_c, SD_OrderLine_c, SD_OrderRel_c, SD_PartNum_c, ";
                        _strSQL += "PartDescription, SD_UOM_c, SD_AllocateQuantity_c, ExpirationDate, ";
                        _strSQL += "SD_LotNum_c, SD_Warehouse_c, SD_BinNum_c, SD_Urgent_c, SD_CustID_c, ";
                        _strSQL += "CustID + ' - ' + Name + CHAR(10) + CHAR(13) + Address1 + CHAR(10) + CHAR(13) + Address2 + CHAR(10) + CHAR(13) + ";
                        _strSQL += "Address3 + CHAR(10) + CHAR(13) + Zip + ' ' + City + ' ' + State + ' ' + Country as 'CustDetails', ";
                        _strSQL += "OrderComment, UD103.Key1, UD103A.ChildKey2, SD_Transporter_c, SD_ShipVia_c, UD103A.SD_PickedBy_c, AP_BumiAgDONum_c ";
                        _strSQL += "from UD103 join UD103A on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 ";
                        _strSQL += "join Part p on UD103.Company = p.Company and UD103A.SD_PartNum_c = p.PartNum ";
                        _strSQL += "join PartLot pl on UD103.Company = pl.Company and UD103A.SD_PartNum_c = pl.PartNum and SD_LotNum_c = LotNum ";
                        _strSQL += "join Customer c on UD103.Company = c.Company and c.CustNum = UD103.SD_CustNum_c ";
                        _strSQL += "join OrderHed oh on UD103.Company = oh.Company and UD103A.SD_OrderNum_c = oh.OrderNum ";
                        _strSQL += "where UD103.Key1 = '" + pickListNum + "' ";

                        _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in _dts.Tables[0].Rows)
                            {
                                PickPack oPickPack = new PickPack();
                                oPickPack.MQ_FromBinNum = (row["SD_BinNum_c"].ToString());
                                oPickPack.MQ_FromWhse = (row["SD_Warehouse_c"].ToString());
                                oPickPack.MQ_IUM = (row["SD_UOM_c"].ToString());
                                oPickPack.MQ_LotNum = (row["SD_LotNum_c"].ToString());
                                oPickPack.MQ_OrderLine = DBNull.Value.Equals(row["SD_OrderLine_c"]) ? 0 : int.Parse(row["SD_OrderLine_C"].ToString());
                                oPickPack.MQ_OrderNum = DBNull.Value.Equals(row["SD_OrderNum_c"]) ? 0 : int.Parse(row["SD_OrderNum_c"].ToString());
                                oPickPack.MQ_OrderRelNum = DBNull.Value.Equals(row["SD_OrderRel_c"]) ? 0 : int.Parse(row["SD_OrderRel_c"].ToString());
                                oPickPack.MQ_PartDescription = (row["PartDescription"].ToString());
                                oPickPack.MQ_PartNum = (row["SD_PartNum_c"].ToString());
                                oPickPack.MQ_Quantity = DBNull.Value.Equals(row["SD_AllocateQuantity_c"]) ? 0 : decimal.Parse(row["SD_AllocateQuantity_c"].ToString());
                                oPickPack.U14_Character09 = (row["CustDetails"].ToString());
                                oPickPack.U14_Character10 = (row["OrderComment"].ToString());
                                oPickPack.U14_CheckBox18 = (row["SD_PickedBy_c"].ToString() == "" ? false : true);
                                oPickPack.U14_Date01 = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");
                                oPickPack.U14_Key4 = (row["ChildKey2"].ToString());     // picklistline
                                oPickPack.U14_Key5 = (row["Key1"].ToString());   // picklistnum
                                oPickPack.U14_ShortChar14 = (row["SD_Transporter_c"].ToString() == "" ? row["SD_ShipVia_c"].ToString() : row["SD_Transporter_c"].ToString());

                                oPickPack.BumiAgDONum = (row["AP_BumiAgDONum_c"].ToString());
                                oPickPack.UrgentOrder = (row["SD_Urgent_c"].ToString() == "True" ? true : false);


                                oPickPackList.Add(oPickPack);
                            }

                            strMessage = "";
                            IsError = false;

                        }
                        else
                        {
                            strMessage = "No picking list generated";
                            IsError = true;
                        }
                    }
                    else
                    {
                        strMessage = "No picking list generated";
                        IsError = true;
                    }
                }//hasProductGroup
                else
                {
                    strMessage = "You have no access to the backorder part";
                    IsError = true;
                }
			}
			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}


			return (IsError ? false : true);

		}

		public bool _LoadUDReasonCodes(ref EpicEnv oEpicEnv, string strCompany, string strCodeTypeId, ref IList<ReasonCodes> ReasonCodesList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT company, codetypeid, codeid, codedesc ";
                _strSQL += "FROM ice.UDCodes  ";

                string _strWHERE = "";


                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND Company = '" + strCompany + "' "; }

                if (strCodeTypeId != null && strCodeTypeId != "")
                { _strWHERE += "AND codetypeid = '" + strCodeTypeId + "' "; }

                if (_strWHERE != null && _strWHERE != "")
                { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }

                _strSQL += " order by codedesc ";


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        ReasonCodes oReasonCode = new ReasonCodes();

                        oReasonCode.Company = row["Company"].ToString();
                        oReasonCode.ReasonType = row["codetypeid"].ToString();
                        oReasonCode.ReasonCode = row["codeid"].ToString();
                        oReasonCode.Description = row["codedesc"].ToString();

                        ReasonCodesList.Add(oReasonCode);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No User define reason code found";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);



        }

        public bool _LoadInvoices(ref EpicEnv oEpicEnv, string strCompany, string strInvoiceNum, ref IList<Invoices> InvoicesList, out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "select i.Company ,i.InvoiceDate ,i.LegalNumber ,i.FS_ChopSignVerify_c , ";
                _strSQL += "i.FS_ChopSignDate_c ,i.FS_ChopSignDateTime_c , i.FS_ChopSignBy_c , i.CustNum, i.InvoiceNum, ";
                _strSQL += "o.ShipToNum ,o.UseOTS, o.OrderNum, o.PrcConNum, o.ShpConNum, ";
                _strSQL += "s.Name ,s.Address1, s.Address2 , s.Address3 , s.City, s.State , s.ZIP, s.Country ";
                _strSQL += "from InvcHead i ";
                _strSQL += "inner join Erp.OrderHed o on i.Company = o.Company and i.OrderNum = o.OrderNum and (o.OrderNum > 0) ";
                _strSQL += "left outer join Erp.ShipTo s on s.Company = o.Company and s.CustNum = o.CustNum and s.ShipToNum = o.ShipToNum ";

                string _strWHERE = "";


                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND Company = '" + strCompany + "' "; }

                if (strInvoiceNum != null && strInvoiceNum != "")
                { _strWHERE += "AND i.LegalNumber = '" + strInvoiceNum + "' "; }

                if (_strWHERE != null && _strWHERE != "")
                { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Invoices oInvoices = new Invoices();

                        oInvoices.Company = row["Company"].ToString();
                        oInvoices.InvoiceDate = DBNull.Value.Equals(row["InvoiceDate"]) ? "1999-01-01" : Convert.ToDateTime((row["InvoiceDate"])).ToString("yyyy-MM-dd");
                        oInvoices.LegalNumber = row["LegalNumber"].ToString();

                        oInvoices.FS_ChopSignVerify_c = (row["FS_ChopSignVerify_c"].ToString() == "True" ? true : false);
                        oInvoices.FS_ChopSignDate_c = DBNull.Value.Equals(row["FS_ChopSignDate_c"]) ? "1999-01-01" : Convert.ToDateTime((row["FS_ChopSignDate_c"])).ToString("yyyy-MM-dd");
                        oInvoices.FS_ChopSignDateTime_c = DBNull.Value.Equals(row["FS_ChopSignDateTime_c"]) ? "1999-01-01" : Convert.ToDateTime((row["FS_ChopSignDateTime_c"])).ToString("yyyy-MM-dd HH:mm:ss");
                        oInvoices.FS_ChopSignBy_c = row["FS_ChopSignBy_c"].ToString();

                        oInvoices.CustNum = DBNull.Value.Equals(row["CustNum"]) ? 0 : int.Parse(row["CustNum"].ToString());  
                        oInvoices.InvoiceNum = DBNull.Value.Equals(row["InvoiceNum"]) ? 0 : int.Parse(row["InvoiceNum"].ToString());
                        oInvoices.ShipToNum = row["ShipToNum"].ToString();
                        oInvoices.OrderHed_UseOTS = (row["UseOTS"].ToString() == "True" ? true : false);
                        oInvoices.OrderNum = DBNull.Value.Equals(row["OrderNum"]) ? 0 : int.Parse(row["OrderNum"].ToString());
                        oInvoices.PrcConNum = DBNull.Value.Equals(row["PrcConNum"]) ? 0 : int.Parse(row["PrcConNum"].ToString());
                        oInvoices.ShpConNum = DBNull.Value.Equals(row["ShpConNum"]) ? 0 : int.Parse(row["ShpConNum"].ToString());

                        oInvoices.ShipTo_Name = row["Name"].ToString();
                        oInvoices.ShipTo_Address1 = row["Address1"].ToString();
                        oInvoices.ShipTo_Address2 = row["Address2"].ToString();
                        oInvoices.ShipTo_Address3 = row["Address3"].ToString();
                        oInvoices.ShipTo_City = row["City"].ToString();
                        oInvoices.ShipTo_State = row["State"].ToString();
                        oInvoices.ShipTo_Country = row["Country"].ToString();
                        oInvoices.ShipTo_ZIP = row["ZIP"].ToString();


                        InvoicesList.Add(oInvoices);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No invoice found";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _Verify_Tag(ref EpicEnv oEpicEnv, string strCompany, string strPackListNum, string strTag, out string strMessage)
        {
            bool IsError = false;
            strMessage = string.Empty;

            try 
            {
                string _strSQL = "SDS_VerifyTag";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company",strCompany),
                    new SqlParameter("@PickListNum",strPackListNum),
                    new SqlParameter("@TagNum",strTag)
                };

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts =  _MSSQL._MSSQLDataSetResultStoreProcedure(_strSQL, _strSQLCon, parameters);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        if (int.Parse(row["TagCount"].ToString()) == 0)
                        {
                            strMessage = string.Empty;
                            IsError = false;
                        }
                        else
                        {
                            strMessage = "Tag number in use";
                            IsError = true;
                        }

                    }
                }
                else
                {
                    strMessage = "Unable to verify tag.";
                    IsError = true;
                }

            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

		public bool _Verify_TagUD103(ref EpicEnv oEpicEnv, string strCompany, string strPackListNum, string strTag, out string strMessage)
		{
			bool IsError = false;
			strMessage = string.Empty;

			try
			{
                //string _strSQL = "SDS_VerifyTag";

                //IDataParameter[] parameters = new IDataParameter[]
                //{
                //	new SqlParameter("@Company",strCompany),
                //	new SqlParameter("@PickListNum",strPackListNum),
                //	new SqlParameter("@TagNum",strTag)
                //};

                string _strSQL = "select Count(*) as TagCount from ";
                _strSQL += "(select distinct ud.Company, ud.Key1 from UD103 ud ";
                _strSQL += "inner join UD103A uda on ud.company = uda.company and ud.key1 = uda.key1 ";
                _strSQL += "where ud.Company = '" + strCompany + "' ";
                _strSQL += "and uda.SD_TagNum_c = '" + strTag + "' ";
                _strSQL += "and ud.Key1 <> '" + strPackListNum + "' and ud.SD_Status_c != 'Packed' and ud.SD_Voided_c = 0) ud103  ";

				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                //DataSet _dts = _MSSQL._MSSQLDataSetResultStoreProcedure(_strSQL, _strSQLCon, parameters);
                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						if (int.Parse(row["TagCount"].ToString()) == 0)
						{
							strMessage = string.Empty;
							IsError = false;
						}
						else
						{
							strMessage = "Tag number in use";
							IsError = true;
						}

					}
				}
				else
				{
					strMessage = "Unable to verify tag.";
					IsError = true;
				}

			}

			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}

			return (IsError ? false : true);
		}

		public bool _SavePickPack(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strPass, string strLotNum, string strWarehouse, string strBinNum, string strSysRowId, decimal dQuantity, string strTag, string strPallet, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;

            try
            {
                string _strSQL;
                            

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);


                _strSQL = "SDS_SavePickList ";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company",strCompany),
                    new SqlParameter("@Warehouse",strWarehouse),
                    new SqlParameter("@BinNum",strBinNum),
                    new SqlParameter("@LotNum",strLotNum),
                    new SqlParameter("@TagNum",strTag),
                    new SqlParameter("@Quantity",dQuantity),
                    new SqlParameter("@SysRowId",strSysRowId),
                    new SqlParameter("@Pallet",strPallet)
                };

                IsUpdated = _MSSQL._exeStoredProcedureCommand(_strSQL, _strSQLCon, parameters);

                if (IsUpdated)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Can't perform update.";
                    IsError = true;

                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

		public bool _SavePickPackUD103(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strPass, string strLotNum, string strWarehouse, string strBinNum, decimal dQuantity, string strTag, string strPallet, string pickNum, string pickLine, out string strMessage)
		{
			bool IsError = false;
			bool IsUpdated;

			if (checkLineVoid(ref oEpicEnv, pickNum, pickLine))
			{
				strMessage = "Picklist line was already cancelled.";
				IsError = true;
			}
			else
			{
				try
				{
					string _strSQL;


					SQLServerBO _MSSQL = new SQLServerBO();
					string _strSQLCon = _MSSQL._retSQLConnectionString();
					_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

					_strSQL = "update UD103A set SD_ScannedWarehouse_c = '" + strWarehouse + "', ";
					_strSQL += "SD_ScannedBinNum_c = '" + strBinNum + "', ";
					_strSQL += "SD_ScannedLotNUm_c = '" + strLotNum + "', ";
					_strSQL += "SD_ScannedQuantity_c = " + dQuantity.ToString() + ", ";
					_strSQL += "SD_TagNum_c = '" + strTag + "', ";
					_strSQL += "SD_PickedBy_c = '" + strUID + "', ";
					_strSQL += "SD_PickedDate_c = getdate(), ";
					_strSQL += "SD_PickedTime_c = getdate() ";
					_strSQL += "where Key1 = '" + pickNum + "' and ChildKey2 = '" + pickLine + "' ";

					IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

					if (IsUpdated)
					{
						strMessage = "";
						IsError = false;
					}
					else
					{
						strMessage = "Can't perform update.";
						IsError = true;

					}

					// update UD103 (PICKING)

					// get count of picked lines for the picklist, compare with total count for the picklist
					_strSQL = "select count(*) as PickedCount from UD103A where Key1 = '" + pickNum + "' and SD_Voided_c = 0";
                    // check escalated ones as well
					DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);
					var totalCount = 0;
					if (_dts.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow row in _dts.Tables[0].Rows)
						{
							totalCount = Int32.Parse(row["PickedCount"].ToString());
						}
					}

					_strSQL += "and SD_PickedBy_c != '' ";
					_dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);
					var pickedCount = 0;
					if (_dts.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow row in _dts.Tables[0].Rows)
						{
							pickedCount = Int32.Parse(row["PickedCount"].ToString());
						}
					}

					// update UD103 again if counts are equal (PICKED)

					_strSQL = "update UD103 set SD_PickedBy_c = '" + strUID + "' ";

					if (totalCount == pickedCount)
					{
						_strSQL += ", SD_TagNum_c = '" + strTag + "' " +
                            ", SD_PickedComplete_c = 1, " +
							"SD_PickedCompleteDate_c = getdate(), " +
							"SD_PickedCompleteTime_c = getdate(), " +
							"SD_Status_c = 'PICKED' ";
					}

					_strSQL += "where Key1 = '" + pickNum + "' ";

					IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

					if (IsUpdated)
					{
						strMessage = "";
						IsError = false;
					}
					else
					{
						strMessage = "Can't perform update.";
						IsError = true;

					}

				}
				catch (Exception ex)
				{
					strMessage = ex.Message.ToString();
					IsError = true;
				}
			}

			return (IsError ? false : true);
		}

		public bool _SavePackList(ref EpicEnv oEpicEnv, string strCompany, string strRemark, string strTransporter, string strConsignment, string strPallet, decimal dTotalWeight, decimal dTotalBox,  string strPickListNum, string strPacker, string strStation, string strUID, string strPass, string strCurPlant, string strPackPallet, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;
            string _strSQL;
            int lastRunning;
            DataSet _dts;

            try
            {
                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);


                // get the next running number
                if (strConsignment == "" || strConsignment == null)
                {
                    _strSQL = "Select Company, ShipViaCode, Active01_c, PreFix01_c, BeginNum01_c, EndNum01_c, LastRunNum01_c, EndNum01_c - LastRunNum01_c as Balance01, Active02_c, PreFix02_c, BeginNum02_c, EndNum02_c, LastRunNum02_c, EndNum02_c - LastRunNum02_c as Balance02, Suffix01_c, Suffix02_c ";
                    _strSQL += "from ShipVia ";
                    _strSQL += "Where ShipViaCode = '" + strTransporter + "' ";
                    _strSQL += "And company = '" + strCompany + "' ";


                    _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in _dts.Tables[0].Rows)
                        {
                            if (bool.Parse(row["Active01_c"].ToString()) == true)
                            {
                                if (int.Parse(row["Balance01"].ToString()) > 0)
                                {
                                    if (int.Parse(row["LastRunNum01_c"].ToString()) == 0)
                                    {
                                        lastRunning = int.Parse(row["BeginNum01_c"].ToString());
                                    }
                                    else
                                    {
                                        lastRunning = int.Parse(row["LastRunNum01_c"].ToString());
                                        lastRunning++;
                                    }
                                    _strSQL = "update shipvia set LastRunNum01_c = '" + lastRunning.ToString() + "' ";
                                    _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                    strConsignment = row["PreFix01_c"].ToString() + lastRunning.ToString() + row["Suffix01_c"].ToString();
                                }
                                else if (int.Parse(row["Balance02"].ToString()) > 0)
                                {
                                    if (int.Parse(row["LastRunNum02_c"].ToString()) == 0)
                                    {
                                        lastRunning = int.Parse(row["BeginNum02_c"].ToString());
                                    }
                                    else
                                    {
                                        lastRunning = int.Parse(row["LastRunNum02_c"].ToString());
                                        lastRunning++;
                                    }

                                    _strSQL = "update shipvia set LastRunNum02_c = '" + lastRunning.ToString() + "', Active02_c = 1, Active01_c = 0 ";
                                    _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                    strConsignment = row["PreFix02_c"].ToString() + lastRunning.ToString() + row["Suffix02_c"].ToString();

                                }
                                else
                                {
                                    //IsError = true;
                                    strMessage = "Not able to generate consignment number.";
                                    return false;
                                }
                            }
                            else
                            {
                                if (int.Parse(row["Balance02"].ToString()) > 0)
                                {
                                    if (int.Parse(row["LastRunNum02_c"].ToString()) == 0)
                                    {
                                        lastRunning = int.Parse(row["BeginNum02_c"].ToString());
                                    }
                                    else
                                    {
                                        lastRunning = int.Parse(row["LastRunNum02_c"].ToString());
                                        lastRunning++;
                                    }

                                    _strSQL = "update shipvia set LastRunNum02_c = '" + lastRunning.ToString() + "' ";
                                    _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                    strConsignment = row["PreFix02_c"].ToString() + lastRunning.ToString() + row["Suffix02_c"].ToString();

                                }
                                else if (int.Parse(row["Balance01"].ToString()) > 0)
                                {
                                    if (int.Parse(row["LastRunNum01_c"].ToString()) == 0)
                                    {
                                        lastRunning = int.Parse(row["BeginNum01_c"].ToString());
                                    }
                                    else
                                    {
                                        lastRunning = int.Parse(row["LastRunNum01_c"].ToString());
                                        lastRunning++;
                                    }

                                    _strSQL = "update shipvia set LastRunNum01_c = '" + lastRunning.ToString() + "', Active01_c = 1, Active02_c = 0 ";
                                    _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                    _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                    strConsignment = row["PreFix01_c"].ToString() + lastRunning.ToString() + row["Suffix01_c"].ToString();
                                }
                                else
                                {
                                    strMessage = "Not able to generate consignment number.";
                                    return false;

                                }

                            }

                        }

                    }


                }






                // clear the MQ 
                _strSQL = "select pp.mq_sysrowid, pp.mq_partnum, pp.MQ_LotNum, pp.MQ_FromWhse, pp.MQ_FromBinNum, pp.MQ_OrderNum, ";
                _strSQL += "pp.MQ_OrderLine, pp.MQ_OrderRelNum, pp.MQ_IUM, pp.MQ_Quantity, pp.MQ_PartDescription, pp.MQ_ToWhse, pp.MQ_ToBinNum, pp.MQ_IUM, ";
                _strSQL += "p.SalesUM, p.NetWeightUOM, o.CustNum, o.ShipToNum ";
                _strSQL += "from pickpack pp ";
                _strSQL += "left join erp.part p on p.Company = pp.MQ_Company and p.PartNum = pp.mq_partnum ";
                _strSQL += "left join erp.OrderHed o on o.Company = pp.MQ_Company and o.OrderNum = pp.MQ_OrderNum ";
                _strSQL += "where pp.u14_key5 = '" + strPickListNum + "' ";


                _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                EpicorBO oEpicor = new EpicorBO();
                IList<Order> OrderList = new List<Order>();

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        _strSQL = "SDS_Enhance_Fulfillment_Reset";

                        IDataParameter[] parameters = new IDataParameter[]
                        {
                            new SqlParameter("@Company",strCompany),
                            new SqlParameter("@PartNum",row["mq_partnum"].ToString()),
                            new SqlParameter("@MtlQueueRowId",row["mq_sysrowid"].ToString())
                        };

                        _MSSQL._exeStoredProcedureCommand(_strSQL, _strSQLCon, parameters);

                        // clear the mtl queue
                        oEpicor._AMMIssueReturn(ref oEpicEnv, out strMessage, strUID, strPass, strCompany, strCurPlant, row["mq_sysrowid"].ToString());

                        _strSQL = "SDS_Enhance_Fulfillment_Reset2";

                        IDataParameter[] parameters2 = new IDataParameter[]
                        {
                            new SqlParameter("@Company",strCompany),
                            new SqlParameter("@PartNum",row["mq_partnum"].ToString()),
                            new SqlParameter("@LotNum",row["MQ_LotNum"].ToString()),
                            new SqlParameter("@FromWhse",row["MQ_FromWhse"].ToString()),
                            new SqlParameter("@FromBinNum",row["MQ_FromBinNum"].ToString()),
                            new SqlParameter("@OrderNum",row["MQ_OrderNum"].ToString()),
                            new SqlParameter("@OrderLine",row["MQ_OrderLine"].ToString()),
                            new SqlParameter("@OrderRelNum",row["MQ_OrderRelNum"].ToString()),
                            new SqlParameter("@IUM",row["MQ_IUM"].ToString()),
                            new SqlParameter("@Quantity",row["MQ_Quantity"].ToString())
                        };

                        _MSSQL._exeStoredProcedureCommand(_strSQL, _strSQLCon, parameters2);


                        Order ord = new Order();
                        ord.Company = strCompany;
                        ord.OrderNum = int.Parse(row["MQ_OrderNum"].ToString());
                        ord.OrderLine = int.Parse(row["MQ_OrderLine"].ToString());
                        ord.OrderRel = int.Parse(row["MQ_OrderRelNum"].ToString());
                        ord.PartNum = row["mq_partnum"].ToString();
                        ord.PartDescription = row["MQ_PartDescription"].ToString();
                        ord.OrderQty = Decimal.Parse(row["MQ_Quantity"].ToString());
                        ord.Warehouse = row["MQ_ToWhse"].ToString();
                        ord.Bin = row["MQ_ToBinNum"].ToString();
                        ord.Lot = row["MQ_LotNum"].ToString();
                        ord.Warehouse = row["MQ_ToWhse"].ToString();
                        ord.Bin = row["MQ_ToBinNum"].ToString();
                        ord.IUM = DBNull.Value.Equals(row["MQ_IUM"]) ? string.Empty : (row["MQ_IUM"].ToString()); 
                        ord.SalesUM = DBNull.Value.Equals(row["SalesUM"]) ? string.Empty : (row["SalesUM"].ToString());
                        ord.NetWeightUOM = DBNull.Value.Equals(row["NetWeightUOM"]) ? string.Empty : (row["NetWeightUOM"].ToString()); 
                        ord.CustNum = DBNull.Value.Equals(row["CustNum"]) ? string.Empty : (row["CustNum"].ToString()); 
                        ord.ShipToNum = row["ShipToNum"].ToString();

                        OrderList.Add(ord);

                    }
                }

                // update to ud14 table
                _strSQL = "update ice.ud14 ";
                _strSQL += "set shortchar15 = '" + strRemark + "' ";
                _strSQL += ", shortchar14 = '" + strTransporter + "' ";
                _strSQL += ", shortchar13 = '" + strConsignment + "' ";
                _strSQL += ", shortchar12 = '" + strPallet + "' ";
                _strSQL += ", shortchar11 = '" + strPacker + "' ";
                _strSQL += ", shortchar10 = '" + strStation + "' ";
                _strSQL += ", shortchar08 = '" + strPackPallet + "' ";
                _strSQL += ", number18 = " + Math.Round(dTotalWeight,1) + " ";
                _strSQL += ", number19 = " + dTotalBox + " ";
                _strSQL += "where Company = '" + strCompany + "' ";
                _strSQL += "and key5 = '" + strPickListNum + "' ";


                IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                // update packer complete date
                _strSQL = "update xd ";
                _strSQL += "set xd.DateTime04_c = getdate() ";
                _strSQL += "from ice.ud14_UD xd ";
                _strSQL += "inner join ice.UD14 x on x.SysRowID = xd.foreignSysrowid ";
                _strSQL += "where x.Key5 = '" + strPickListNum + "' ";
                _strSQL += "and x.Company = '" + strCompany + "' ";


                IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);


                IsUpdated = oEpicor._PickedOrder(ref oEpicEnv, out strMessage, ref OrderList, strUID, strPass, strCompany, strCurPlant, strPickListNum, strConsignment, Math.Round(dTotalWeight, 1), strTransporter, dTotalBox, strStation);

                if (IsUpdated)
                {
                    _MSSQL._exeSDSCreateLabelSP_Reprint(_strSQLCon, strCompany, strUID, "SHP-LBL", DateTime.Now, "", "", "", 0, Convert.ToInt32(dTotalBox), "", "", "", strPickListNum);
                }

                if (IsUpdated)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Can't perform update.";
                    IsError = true;

                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

		public bool _SavePackListUD103(ref EpicEnv oEpicEnv, string strCompany, string strRemark, string strTransporter, string strConsignment, string strPallet, decimal dTotalWeight, decimal dTotalBox, string strPickListNum, string strPacker, string strStation, string strUID, string strPass, string strCurPlant, string strPackPallet, out string strMessage)
		{
			bool IsError = false;
			bool IsUpdated;
			string _strSQL;
			int lastRunning;
			DataSet _dts;

			//if (checkLineVoid(ref oEpicEnv, strPickListNum, pickLine))
			//{
			//	strMessage = "Picklist line was already cancelled.";
			//	IsError = true;
			//}
			try
			{
				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				// get the next running number
				if (strConsignment == "" || strConsignment == null)
				{
					_strSQL = "Select Company, ShipViaCode, Active01_c, PreFix01_c, BeginNum01_c, EndNum01_c, LastRunNum01_c, EndNum01_c - LastRunNum01_c as Balance01, Active02_c, PreFix02_c, BeginNum02_c, EndNum02_c, LastRunNum02_c, EndNum02_c - LastRunNum02_c as Balance02, Suffix01_c, Suffix02_c ";
					_strSQL += "from ShipVia ";
					_strSQL += "Where ShipViaCode = '" + strTransporter + "' ";
					_strSQL += "And company = '" + strCompany + "' ";


					_dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

					if (_dts.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow row in _dts.Tables[0].Rows)
						{
							if (bool.Parse(row["Active01_c"].ToString()) == true)
							{
								if (int.Parse(row["Balance01"].ToString()) > 0)
								{
									if (int.Parse(row["LastRunNum01_c"].ToString()) == 0)
									{
										lastRunning = int.Parse(row["BeginNum01_c"].ToString());
									}
									else
									{
										lastRunning = int.Parse(row["LastRunNum01_c"].ToString());
										lastRunning++;
									}
									_strSQL = "update shipvia set LastRunNum01_c = '" + lastRunning.ToString() + "' ";
									_strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

									_MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

									strConsignment = row["PreFix01_c"].ToString() + lastRunning.ToString() + row["Suffix01_c"].ToString();
								}
								else if (int.Parse(row["Balance02"].ToString()) > 0)
								{
									if (int.Parse(row["LastRunNum02_c"].ToString()) == 0)
									{
										lastRunning = int.Parse(row["BeginNum02_c"].ToString());
									}
									else
									{
										lastRunning = int.Parse(row["LastRunNum02_c"].ToString());
										lastRunning++;
									}

									_strSQL = "update shipvia set LastRunNum02_c = '" + lastRunning.ToString() + "', Active02_c = 1, Active01_c = 0 ";
									_strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

									_MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

									strConsignment = row["PreFix02_c"].ToString() + lastRunning.ToString() + row["Suffix02_c"].ToString();

								}
								else
								{
									//IsError = true;
									strMessage = "Not able to generate consignment number.";
									return false;
								}
							}
							else
							{
								if (int.Parse(row["Balance02"].ToString()) > 0)
								{
									if (int.Parse(row["LastRunNum02_c"].ToString()) == 0)
									{
										lastRunning = int.Parse(row["BeginNum02_c"].ToString());
									}
									else
									{
										lastRunning = int.Parse(row["LastRunNum02_c"].ToString());
										lastRunning++;
									}

									_strSQL = "update shipvia set LastRunNum02_c = '" + lastRunning.ToString() + "' ";
									_strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

									_MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

									strConsignment = row["PreFix02_c"].ToString() + lastRunning.ToString() + row["Suffix02_c"].ToString();

								}
								else if (int.Parse(row["Balance01"].ToString()) > 0)
								{
									if (int.Parse(row["LastRunNum01_c"].ToString()) == 0)
									{
										lastRunning = int.Parse(row["BeginNum01_c"].ToString());
									}
									else
									{
										lastRunning = int.Parse(row["LastRunNum01_c"].ToString());
										lastRunning++;
									}

									_strSQL = "update shipvia set LastRunNum01_c = '" + lastRunning.ToString() + "', Active01_c = 1, Active02_c = 0 ";
									_strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

									_MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

									strConsignment = row["PreFix01_c"].ToString() + lastRunning.ToString() + row["Suffix01_c"].ToString();
								}
								else
								{
									strMessage = "Not able to generate consignment number.";
									return false;

								}

							}

						}

					}


				}

				_strSQL = "select SD_PartNum_c as PartNum, SD_LotNum_c as LotNum, SD_Warehouse_c as Warehouse, SD_BinNum_c as BinNum, UD103A.SD_OrderNum_c as OrderNum, ";
                _strSQL += "SD_OrderLine_c as OrderLine, SD_OrderRel_c as OrderRel, SD_UOM_c as UOM, SD_AllocateQuantity_c as Quantity, ";
                _strSQL += "p.PartDescription, p.SalesUM, p.NetWeightUOM, oh.CustNum, oh.ShipToNum ";
                _strSQL += "from UD103 join UD103A on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 ";
                _strSQL += "join erp.Part p on UD103.Company = p.Company and UD103A.SD_PartNum_c = p.PartNum ";
                _strSQL += "join erp.OrderHed oh on oh.Company = UD103.Company and oh.OrderNum = UD103A.SD_OrderNum_c ";
                _strSQL += "where UD103.Key1 = '" + strPickListNum + "' ";


				_dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				EpicorBO oEpicor = new EpicorBO();
				IList<Order> OrderList = new List<Order>();

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						// clear the mtl queue
						//oEpicor._AMMIssueReturn(ref oEpicEnv, out strMessage, strUID, strPass, strCompany, strCurPlant, row["mq_sysrowid"].ToString());

						Order ord = new Order();
						ord.Company = strCompany;
						ord.OrderNum = int.Parse(row["OrderNum"].ToString());
						ord.OrderLine = int.Parse(row["OrderLine"].ToString());
						ord.OrderRel = int.Parse(row["OrderRel"].ToString());
						ord.PartNum = row["Partnum"].ToString();
						ord.PartDescription = row["PartDescription"].ToString();
						ord.OrderQty = Decimal.Parse(row["Quantity"].ToString());
						ord.Warehouse = row["Warehouse"].ToString();
						ord.Bin = row["BinNum"].ToString();
						ord.Lot = row["LotNum"].ToString();
						//ord.Warehouse = row["MQ_ToWhse"].ToString();
						//ord.Bin = row["MQ_ToBinNum"].ToString();
						ord.IUM = DBNull.Value.Equals(row["UOM"]) ? string.Empty : (row["UOM"].ToString());
						ord.SalesUM = DBNull.Value.Equals(row["SalesUM"]) ? string.Empty : (row["SalesUM"].ToString());
						ord.NetWeightUOM = DBNull.Value.Equals(row["NetWeightUOM"]) ? string.Empty : (row["NetWeightUOM"].ToString());
						ord.CustNum = DBNull.Value.Equals(row["CustNum"]) ? string.Empty : (row["CustNum"].ToString());
						ord.ShipToNum = row["ShipToNum"].ToString();

						OrderList.Add(ord);

					}
				}

                
                // update UD103 table
                _strSQL = "update UD103 ";
                _strSQL += "set SD_Transporter_c = '" + strTransporter + "' ";
                _strSQL += ", SD_Consignment_c = '" + strConsignment + "' ";
                _strSQL += ", SD_PalletNum_c = '" + strPallet + "' ";
                _strSQL += ", SD_PackedBy_c = '" + strPacker + "' ";
                _strSQL += ", SD_StationId_c = '" + strStation + "' ";
                _strSQL += ", SD_TotalWeight_c = " + Math.Round(dTotalWeight, 1) + " ";
                _strSQL += ", SD_TotalCarton_c = " + dTotalBox + " ";
                _strSQL += ", SD_PackedComplete_c = 1 ";
                _strSQL += ", SD_PackedCompleteDate_c = getdate() ";
				_strSQL += ", SD_PackedCompleteTime_c = getdate() ";
				_strSQL += ", SD_Status_c = 'PACKED' ";
                _strSQL += "where Company = '" + strCompany + "' ";
                _strSQL += "and Key1 = '" + strPickListNum + "' ";

                bool IsUpdatedUD103 = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                // update UD103a table SD_PackedBy_c, SD_PackedDate_c, SD_PackedTime_c,
                _strSQL = "update UD103a ";
                _strSQL += "set SD_PackedBy_c = '" + strPacker + "' ";
                _strSQL += ", SD_PackedDate_c = getdate() ";
                _strSQL += ", SD_PackedTime_c = getdate() ";
                _strSQL += "where Company = '" + strCompany + "' ";
                _strSQL += "and Key1 = '" + strPickListNum + "' ";

                bool IsUpdatedUD103a = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);



                IsUpdated = oEpicor._PickedOrder(ref oEpicEnv, out strMessage, ref OrderList, strUID, strPass, strCompany, strCurPlant, strPickListNum, strConsignment, Math.Round(dTotalWeight, 1), strTransporter, dTotalBox, strStation);
				if (IsUpdated)
				{
					_MSSQL._exeSDSCreateLabelSP_Reprint(_strSQLCon, strCompany, strUID, "SHP-LBL", DateTime.Now, "", "", "", 0, Convert.ToInt32(dTotalBox), "", "", "", strPickListNum);
				}

				if (IsUpdated)
				{
					strMessage = "";
					IsError = false;
				}
				else
				{
					// strMessage = "Can't perform update.";
					IsError = true;

                    _strSQL = "update UD103 set SD_PackedComplete_c = 0, SD_PackedCompleteDate_c = NULL, SD_PackedCompleteTime_c = NULL, " +
						"SD_Status_c = 'PACKING', SD_PackedBy_c = '' where Key1 = '" + strPickListNum + "' ";

					bool rollbackSuccess = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                    _strSQL = "update UD103a ";
                    _strSQL += "set SD_PackedBy_c = '' ";
                    _strSQL += ", SD_PackedDate_c = null ";
                    _strSQL += ", SD_PackedTime_c = null ";
                    _strSQL += "where Company = '" + strCompany + "' ";
                    _strSQL += "and Key1 = '" + strPickListNum + "' ";

                    bool rollbackSuccessUD103A = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                    strMessage += " Created shipment will not be invoiced due to the error.";

				}

			}
			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}

			return (IsError ? false : true);
		}

		public bool _ResolvedEscalate(ref EpicEnv oEpicEnv, string strCompany, string strPickNum, string strReason, string strRemark, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;

            try
            {
                string _strSQL = "SDS_ResolveEscalate";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company",strCompany),
                    new SqlParameter("@PickListNum",strPickNum),
                    new SqlParameter("@Reason",strReason),
                    new SqlParameter("@Remark",strRemark)
                };



                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                IsUpdated = _MSSQL._exeStoredProcedureCommand(_strSQL, _strSQLCon, parameters);

                if (IsUpdated)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Can't perform update.";
                    IsError = true;

                }
            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);

        }

		public bool _ResolvedEscalateUD103(ref EpicEnv oEpicEnv, string strCompany, string strPickNum, string strReason, string strRemark, out string strMessage)
		{
			bool IsError = false;
			bool IsUpdated;

			try
			{
                //string _strSQL = "SDS_ResolveEscalate";

                //IDataParameter[] parameters = new IDataParameter[]
                //{
                //	new SqlParameter("@Company",strCompany),
                //	new SqlParameter("@PickListNum",strPickNum),
                //	new SqlParameter("@Reason",strReason),
                //	new SqlParameter("@Remark",strRemark)
                //};



                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                //IsUpdated = _MSSQL._exeStoredProcedureCommand(_strSQL, _strSQLCon, parameters);

                string _strSQL = "update UD103 ";
                _strSQL += "set SD_Status_c = SUBSTRING(SD_Status_c, 11, 7)";
                _strSQL += "where Company = '" + strCompany + "' ";
                _strSQL += "and Key1 = '" + strPickNum + "' ";
                _strSQL += "and SD_Status_c like '%ESCALATED%'";

				IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

				//_strSQL = "update UD103 ";
				//_strSQL += "set SD_Status_c = 'PACKING'";
				//_strSQL += "where Company = '" + strCompany + "' ";
				//_strSQL += "and Key1 = '" + strPickNum + "' ";
				//_strSQL += "and SD_Status_c = 'ESCALATED PACKING'";

				//IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                _strSQL = "update x set x.CheckBox01 = 1, x.Character02 = '" + strReason + "', x.shortchar20 = cast( FORMAT(getdate(), 'yyyy-MM-dd hh:mm:ss tt')  as nvarchar(40)), " +
					"x.Character03 = '" + strRemark + "' from ud18 x inner join ud103a on ud103a.sysrowid = x.sd_ud14guid_c where SD_PickListNum_c = '" + strPickNum + "' and x.Company = '" + strCompany + "'";

				IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

				if (IsUpdated)
				{
					strMessage = "";
					IsError = false;
				}
				else
				{
					strMessage = "Can't perform update.";
					IsError = true;

				}
			}

			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}

			return (IsError ? false : true);

		}

		public bool _ScanSignedDO(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strInvoiceNum, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;

            try
            {
                string _strSQL = "";
                strMessage = string.Empty;

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                //do checking
                _strSQL = "select FS_ChopSignVerify_c from erp.InvcHead_UD iu ";
                _strSQL += "inner join erp.InvcHead i on i.SysRowID = iu.ForeignSysRowID ";
                _strSQL += "where i.Company = '" + strCompany + "' ";
                _strSQL += "and i.LegalNumber = '" + strInvoiceNum + "' ";
                _strSQL += "and i.invoicenum not in (select distinct d.InvoiceNum ";
                _strSQL += "from erp.InvcDtl d ";
                _strSQL += "inner join ShipHead s on d.Company = s.Company and d.PackNum = s.PackNum and s.SD_IsCancel_c = 1 ";
                _strSQL += "where d.Company = '" + strCompany + "') ";

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        if (row["FS_ChopSignVerify_c"].ToString().ToUpper() == "FALSE")
                        {
                            // update into invchead
                            _strSQL = "update iu ";
                            _strSQL += "set FS_ChopSignDate_c  = '" + DateTime.Today.ToString("yyyy/MM/dd") + "'  ";
                            _strSQL += ", FS_ChopSignDateTime_c = '" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "'  ";
                            _strSQL += ", FS_ChopSignVerify_c = '1'  ";
                            _strSQL += ", FS_ChopSignBy_c = '" + strUID + "'  ";
                            _strSQL += "from erp.InvcHead_UD iu ";
                            _strSQL += "inner join erp.InvcHead i on i.SysRowID = iu.ForeignSysRowID ";
                            _strSQL += "where i.Company = '" + strCompany + "' ";
                            _strSQL += "and i.LegalNumber = '" + strInvoiceNum + "' ";

                            IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                            if (IsUpdated)
                            {
                                strMessage = "";
                                IsError = false;
                            }
                            else
                            {
                                strMessage = "Can't perform update.";
                                IsError = true;

                            }
                        }
                        else
                        {
                            strMessage = "The scanned invoice has been verified before. No updates applied.";
                            IsError = true;
                        }

                    }

                }
                else
                {
                    strMessage = "This invoice is cancelled";
                    IsError = true;
                }


            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _ConfirmDelivery(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strInvoiceNum, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;

            try
            {
                string _strSQL = "";
                strMessage = string.Empty;


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                //do checking
                _strSQL = "select SD_ConfirmDeliver_c from erp.InvcHead_UD iu ";
                _strSQL += "inner join erp.InvcHead i on i.SysRowID = iu.ForeignSysRowID ";
                _strSQL += "where i.Company = '" + strCompany + "' ";
                _strSQL += "and i.LegalNumber = '" + strInvoiceNum + "' ";


                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        if (row["SD_ConfirmDeliver_c"].ToString() == "0")
                        {
                            _strSQL = "update iu ";
                            _strSQL += "set SD_ConfirmDateTime_c  = '" + DateTime.Today.ToString("yyyy/MM/dd hh:mm:ss") + "'  ";
                            _strSQL += ", SD_ConfirmDeliver_c = '1'  ";
                            _strSQL += ", SD_ConfirmBy_c = '" + strUID + "'  ";
                            _strSQL += "from erp.InvcHead_UD iu ";
                            _strSQL += "inner join erp.InvcHead i on i.SysRowID = iu.ForeignSysRowID ";
                            _strSQL += "where i.Company = '" + strCompany + "' ";
                            _strSQL += "and i.LegalNumber = '" + strInvoiceNum + "' ";

                            IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                            if (IsUpdated)
                            {
                                strMessage = "";
                                IsError = false;
                            }
                            else
                            {
                                strMessage = "Can't perform update.";
                                IsError = true;

                            }


                        }
                        else
                        {
                            strMessage = "The scanned invoice has been verified before. No updates applied.";
                            IsError = true;

                        }



                    }


                }
                else
                {
                    strMessage = "Can not find the scanned invoice.";
                    IsError = true;
                }

            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _AssignEscalatePIC(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strKey1, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;

            try
            {
                string _strSQL = "update ice.ud18 set ShortChar05 = '" + strUID + "' , Date03 = GETDATE(), shortchar19 = FORMAT(getdate(), 'yyyy-MM-dd hh:mm:ss tt')  ";
                _strSQL += "where key1  = '" + strKey1 + "' and company = '" + strCompany + "' ";

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                if (IsUpdated)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Can't perform assign Escalate PIC.";
                    IsError = true;

                }
            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadEscalateList(ref EpicEnv oEpicEnv, string strCompany, string strCust, ref IList<PickPackEscalate> oEscalateList, string strPickNum, out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "select u18.*, pp.*, pl.expirationdate, u.codedesc ";
                _strSQL += "from UD18  u18 ";
                _strSQL += "inner join PickPack pp on pp.u14_sysrowid = u18.sd_ud14guid_c ";
                _strSQL += "left join erp.PartLot pl on pp.mq_company = pl.company and pp.mq_partnum = pl.partnum and pp.mq_lotnum = pl.lotnum ";
                _strSQL += "left join ice.UDCodes u on u18.Company = u.company and u18.shortchar03 = u.codeid ";
                _strSQL += "Where u18.Company = '" + strCompany + "' and pp.U14_Checkbox17 = 1 and u18.Checkbox01 = 0 ";


                if (strPickNum != "" && strPickNum != null)
                {
                    _strSQL += " and u14.Key5 = '" + strPickNum + "' ";
                }

                if (strCust != "" && strCust != null)
                {
                    _strSQL += " and pp.ShortChar09 like '%" + strCust + "%' ";
                }



                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PickPackEscalate oEscalate = new PickPackEscalate();
                        oEscalate.Company = row["Company"].ToString();
                        oEscalate.EscalateDateTime = DBNull.Value.Equals(row["ShortChar18"]) || row["ShortChar18"].Equals("") ? "1999-01-01" : Convert.ToDateTime((row["ShortChar18"].ToString().Replace("PTG","").Replace("PG", ""))).ToString("yyyy-MM-dd hh:mm:ss");  
                        oEscalate.MQ_SysRowID = (row["SD_MQGUID_C"].ToString());
                        oEscalate.Picker = (row["ShortChar02"].ToString());
                        oEscalate.PickPackRemarks = (row["Character01"].ToString());
                        oEscalate.Reason = row["codedesc"].ToString();
                        oEscalate.U14_SysRowID = (row["SD_UD14GUID_C"].ToString());
                        oEscalate.PickingNo = row["u14_key5"].ToString();
                        oEscalate.EntryPerson = row["ShortChar01"].ToString();
                        oEscalate.CurrentStage = row["ShortChar04"].ToString();
                        oEscalate.Customer = row["u14_Character09"].ToString();
                        oEscalate.OrderNum = DBNull.Value.Equals(row["MQ_OrderNum"]) ? 0 : int.Parse(row["MQ_OrderNum"].ToString());
                        oEscalate.OrderLine = DBNull.Value.Equals(row["MQ_OrderLine"]) ? 0 : int.Parse(row["MQ_OrderLine"].ToString());
                        oEscalate.OrderRel = DBNull.Value.Equals(row["MQ_OrderRelNum"]) ? 0 : int.Parse(row["MQ_OrderRelNum"].ToString());
                        oEscalate.Key1 = row["key1"].ToString();
                        oEscalate.AssignResolvedBy = row["ShortChar05"].ToString();
                        oEscalate.AssignedDateTime = DBNull.Value.Equals(row["ShortChar19"]) || row["ShortChar19"].Equals("") ? "1999-01-01" : Convert.ToDateTime((row["ShortChar19"])).ToString("yyyy-MM-dd hh:mm:ss");

                        oEscalate.PartNum = row["MQ_PartNum"].ToString();
                        oEscalate.PartDesc = row["MQ_PartDescription"].ToString();
                        oEscalate.AllocatedWarehouse = row["MQ_FromWhse"].ToString();
                        oEscalate.AllocatedBin = row["MQ_FromBinNum"].ToString();
                        oEscalate.AllocatedLot = row["MQ_LotNum"].ToString();
                        oEscalate.AllocatedQuantity = DBNull.Value.Equals(row["MQ_Quantity"]) ? 0 : decimal.Parse(row["MQ_Quantity"].ToString());
                        oEscalate.ExpiryDate = DBNull.Value.Equals(row["expirationdate"]) ? "1999-01-01" : Convert.ToDateTime((row["expirationdate"])).ToString("yyyy-MM-dd");
                        oEscalate.Station = row["u14_shortchar10"].ToString();

                        oEscalateList.Add(oEscalate);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Escalate listing not found ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }

		public bool _LoadEscalateListUD103(ref EpicEnv oEpicEnv, string strCompany, string strCust, ref IList<PickPackEscalate> oEscalateList, string strPickNum, out string strMessage)
		{
			bool IsError = false;

			try
			{

				string _strSQL = "select UD18.*, UD103A.*, SD_StationId_c, SD_CustID_c, pl.ExpirationDate, u.CodeDesc, p.PartDescription from UD18 ";
				_strSQL += "join UD103 on UD18.Company = UD103.Company and UD18.SD_PickListNum_c = UD103.Key1 ";
				_strSQL += "join UD103A on UD103.Company = UD103A.Company and UD103.Key1 = UD103A.Key1 and UD103A.key1 = UD18.SD_PickListNum_c  ";
                _strSQL += "join erp.Part p on UD103A.Company = p.Company and UD103A.SD_PartNum_c = p.PartNum ";
				_strSQL += "join erp.PartLot pl on UD18.Company = pl.Company and pl.PartNum = UD103A.SD_PartNum_c and pl.LotNum = UD103A.SD_LotNum_c ";
				_strSQL += "join ice.UDCodes u on UD18.Company = u.Company and UD18.ShortChar03 = u.CodeID ";
				_strSQL += "where UD18.Company = '" + strCompany + "' and UD103.SD_Status_c like '%ESCALATED%' and UD18.CheckBox01 = 0 ";
				


				if (strPickNum != "" && strPickNum != null)
				{
					_strSQL += " and UD103.Key1 = '" + strPickNum + "' ";
				}

				//if (strCust != "" && strCust != null)
				//{
				//	_strSQL += " and pp.ShortChar09 like '%" + strCust + "%' ";
				//}



				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						PickPackEscalate oEscalate = new PickPackEscalate();
						oEscalate.Company = row["Company"].ToString();
						oEscalate.EscalateDateTime = DBNull.Value.Equals(row["ShortChar18"]) || row["ShortChar18"].Equals("") ? "1999-01-01" : Convert.ToDateTime((row["ShortChar18"].ToString().Replace("PTG", "").Replace("PG", ""))).ToString("yyyy-MM-dd hh:mm:ss");
						//oEscalate.MQ_SysRowID = (row["SD_MQGUID_C"].ToString());
						oEscalate.Picker = (row["ShortChar02"].ToString());
						oEscalate.PickPackRemarks = (row["Character01"].ToString());
						oEscalate.Reason = row["codedesc"].ToString();
						//oEscalate.U14_SysRowID = (row["SD_UD14GUID_C"].ToString());
						oEscalate.PickingNo = row["SD_PickListNum_c"].ToString();
						oEscalate.EntryPerson = row["ShortChar01"].ToString();
						oEscalate.CurrentStage = row["ShortChar04"].ToString();
						oEscalate.Customer = row["SD_CustID_c"].ToString();
						oEscalate.OrderNum = DBNull.Value.Equals(row["SD_OrderNum_c"]) ? 0 : int.Parse(row["SD_OrderNum_c"].ToString());
						oEscalate.OrderLine = DBNull.Value.Equals(row["SD_OrderLine_c"]) ? 0 : int.Parse(row["SD_OrderLine_c"].ToString());
						oEscalate.OrderRel = DBNull.Value.Equals(row["SD_OrderRel_c"]) ? 0 : int.Parse(row["SD_OrderRel_c"].ToString());
						oEscalate.Key1 = row["Key1"].ToString();
						oEscalate.AssignResolvedBy = row["ShortChar05"].ToString();
						oEscalate.AssignedDateTime = DBNull.Value.Equals(row["ShortChar19"]) || row["ShortChar19"].Equals("") ? "1999-01-01" : Convert.ToDateTime((row["ShortChar19"])).ToString("yyyy-MM-dd hh:mm:ss");

						oEscalate.PartNum = row["SD_PartNum_c"].ToString();
						oEscalate.PartDesc = row["PartDescription"].ToString();
						oEscalate.AllocatedWarehouse = row["SD_Warehouse_c"].ToString();
						oEscalate.AllocatedBin = row["SD_BinNum_c"].ToString();
						oEscalate.AllocatedLot = row["SD_LotNum_c"].ToString();
						oEscalate.AllocatedQuantity = DBNull.Value.Equals(row["SD_AllocateQuantity_c"]) ? 0 : decimal.Parse(row["SD_AllocateQuantity_c"].ToString());
						oEscalate.ExpiryDate = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");
						oEscalate.Station = row["SD_StationId_c"].ToString();

						oEscalate.U14_SysRowID = (row["SD_UD14GUID_C"].ToString());

						oEscalateList.Add(oEscalate);
					}
					strMessage = "";
					IsError = false;

				}
				else
				{
					strMessage = "Escalate listing not found ";
					IsError = true;
				}

			}
			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}


			return (IsError ? false : true);

		}

		public bool _LoadShipVia(ref EpicEnv oEpicEnv, string strCompany, string strShipViaCode, ref IList<ShipVia> oShipViaList,  out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "select company, shipviacode, description ";
                _strSQL += "from erp.shipvia ";
                _strSQL += "Where Company = '" + strCompany + "' ";


                if (strShipViaCode != "" && strShipViaCode != null)
                {
                    _strSQL += " and shipviacode = '" + strShipViaCode + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        ShipVia oShipVia = new ShipVia();
                        oShipVia.ShipViaCode = row["shipviacode"].ToString();
                        oShipVia.ShipViaDescription = (row["description"].ToString());


                        oShipViaList.Add(oShipVia);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "ShipVia listing not found ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }

        public bool _LoadInvoicesForReprint(ref EpicEnv oEpicEnv, string strCompany, string strInvoiceNum, string strLegalNumber, ref IList<ReprintInvoice> InvoicesList, out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "select ih.Company, ih.InvoiceNum, ih.InvoiceDate, ih.LegalNumber, ih.InvoiceComment ";
                _strSQL += ", id.OrderNum, id.OrderLine, id.OrderRelNum, id.PackNum, id.PackLine, id.OurShipQty ";
                _strSQL += ", sd.WarehouseCode, sd.BinNum, sd.LotNum, sd.PartNum, sd.LineDesc, sd.InventoryShipUOM ";
                _strSQL += ", sh.Weight, sh.ShipViaCode, sh.SD_CartonNum_c, sh.TrackingNumber, sh.FS_PickListNo_c ";
                _strSQL += ", tu14.picker, tu14.packer ";
                _strSQL += ", st.Name, st.Address1, st.Address2, st.Address3, st.State, st.City, st.ZIP, st.Country, pl.ExpirationDate ";
                _strSQL += "from InvcHead ih ";
                _strSQL += "left join InvcDtl id on id.Company = ih.Company and id.InvoiceNum = ih.InvoiceNum  ";
                _strSQL += "left join ShipDtl sd on sd.Company = id.Company and sd.PackNum = id.PackNum and sd.PackLine = id.PackLine ";
                _strSQL += "left join ShipHead sh on sh.Company = sd.Company and sh.PackNum = sd.PackNum ";
                _strSQL += "left join ( select distinct u14.Company, u14.Key5, u14.ShortChar16 as picker, u14.shortchar11 as packer from ud14 u14) tu14 on tu14.Company = sh.Company and tu14.Key5 = sh.FS_PickListNo_c ";
                _strSQL += "left join ShipTo st on st.Company = sd.Company and st.CustNum = id.ShipToCustNum and st.ShipToNum = id.ShipToNum ";
                _strSQL += "left join PartLot pl on pl.Company = sd.Company and pl.PartNum = sd.PartNum and pl.LotNum = sd.LotNum ";

                string _strWHERE = "";


                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND ih.Company = '" + strCompany + "' "; }

                if (strInvoiceNum != null && strInvoiceNum != "")
                { _strWHERE += "AND ih.InvoiceNum = " + strInvoiceNum + " "; }

                if (strLegalNumber != null && strLegalNumber != "")
                { _strWHERE += "AND ih.LegalNumber = '" + strLegalNumber + "' "; }

                if (_strWHERE != null && _strWHERE != "")
                { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        ReprintInvoice oReprintInvoice = new ReprintInvoice();

                        oReprintInvoice.Company = row["Company"].ToString();
                        oReprintInvoice.InvoiceNum = row["InvoiceNum"].ToString();
                        oReprintInvoice.InvoiceDate = DBNull.Value.Equals(row["InvoiceDate"]) ? "1999-01-01" : Convert.ToDateTime((row["InvoiceDate"])).ToString("yyyy-MM-dd");
                        oReprintInvoice.LegalNumber = row["LegalNumber"].ToString();
                        oReprintInvoice.InvoiceComment = row["InvoiceComment"].ToString();

                        oReprintInvoice.OrderNum = DBNull.Value.Equals(row["OrderNum"]) ? 0 : int.Parse(row["OrderNum"].ToString());
                        oReprintInvoice.OrderLine = DBNull.Value.Equals(row["OrderLine"]) ? 0 : int.Parse(row["OrderLine"].ToString());
                        oReprintInvoice.OrderRel = DBNull.Value.Equals(row["OrderRelNum"]) ? 0 : int.Parse(row["OrderRelNum"].ToString());
                        oReprintInvoice.PackNum = DBNull.Value.Equals(row["PackNum"]) ? 0 : int.Parse(row["PackNum"].ToString());
                        oReprintInvoice.PackLine = DBNull.Value.Equals(row["PackLine"]) ? 0 : int.Parse(row["PackLine"].ToString());
                        oReprintInvoice.Quantity = DBNull.Value.Equals(row["OurShipQty"]) ? 0 : decimal.Parse(row["OurShipQty"].ToString());

                        oReprintInvoice.Warehouse = row["WarehouseCode"].ToString();
                        oReprintInvoice.BinNum = row["BinNum"].ToString();
                        oReprintInvoice.LotNum = row["LotNum"].ToString();
                        oReprintInvoice.PartNum = row["PartNum"].ToString();
                        oReprintInvoice.PartDescription = row["LineDesc"].ToString();
                        oReprintInvoice.UOM = row["InventoryShipUOM"].ToString();
                        oReprintInvoice.Weight = DBNull.Value.Equals(row["Weight"]) ? 0 : decimal.Parse(row["Weight"].ToString());
                        oReprintInvoice.ShipVia = row["ShipViaCode"].ToString();
                        oReprintInvoice.Carton = DBNull.Value.Equals(row["SD_CartonNum_c"]) ? 0 : int.Parse(row["SD_CartonNum_c"].ToString());
                        oReprintInvoice.TrackingNum = row["TrackingNumber"].ToString();
                        oReprintInvoice.PickListNum = row["FS_PickListNo_c"].ToString();

                        oReprintInvoice.Picker = row["picker"].ToString();
                        oReprintInvoice.Packer = row["packer"].ToString();
                        oReprintInvoice.ShipToName = row["Name"].ToString();
                        oReprintInvoice.ShipToAddr1 = row["Address1"].ToString();
                        oReprintInvoice.ShipToAddr2 = row["Address2"].ToString();
                        oReprintInvoice.ShipToAddr3 = row["Address3"].ToString();
                        oReprintInvoice.ShipToState = row["State"].ToString();
                        oReprintInvoice.ShipToCity = row["City"].ToString();
                        oReprintInvoice.ShipToZip = row["ZIP"].ToString();
                        oReprintInvoice.ShipToCountry = row["Country"].ToString();
                        oReprintInvoice.ExpiryDate = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");

                        InvoicesList.Add(oReprintInvoice);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Invoice not found.";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);


        }

		public bool _LoadInvoicesForReprintUD103(ref EpicEnv oEpicEnv, string strCompany, string strInvoiceNum, string strLegalNumber, ref IList<ReprintInvoice> InvoicesList, out string strMessage)
		{
			bool IsError = false;

			try
			{

				string _strSQL = "select ih.Company, ih.InvoiceNum, ih.InvoiceDate, ih.LegalNumber, ih.InvoiceComment ";
				_strSQL += ", id.OrderNum, id.OrderLine, id.OrderRelNum, id.PackNum, id.PackLine, id.OurShipQty ";
				_strSQL += ", sd.WarehouseCode, sd.BinNum, sd.LotNum, sd.PartNum, sd.LineDesc, sd.InventoryShipUOM ";
				_strSQL += ", sh.Weight, sh.ShipViaCode, sh.SD_CartonNum_c, sh.TrackingNumber, sh.FS_PickListNo_c ";
				_strSQL += ", tUD103.SD_PickedBy_c, tUD103.SD_PackedBy_c ";
				_strSQL += ", st.Name, st.Address1, st.Address2, st.Address3, st.State, st.City, st.ZIP, st.Country, pl.ExpirationDate ";
				_strSQL += "from InvcHead ih ";
				_strSQL += "left join InvcDtl id on id.Company = ih.Company and id.InvoiceNum = ih.InvoiceNum  ";
				_strSQL += "left join ShipDtl sd on sd.Company = id.Company and sd.PackNum = id.PackNum and sd.PackLine = id.PackLine ";
				_strSQL += "left join ShipHead sh on sh.Company = sd.Company and sh.PackNum = sd.PackNum ";
				// _strSQL += "left join ( select distinct u14.Company, u14.Key5, u14.ShortChar16 as picker, u14.shortchar11 as packer from ud14 u14) tu14 on tu14.Company = sh.Company and tu14.Key5 = sh.FS_PickListNo_c ";
                _strSQL += "left join ( select distinct Company, Key1, SD_PickedBy_c, SD_PackedBy_c from UD103) tUD103 on tUD103.Company = sh.Company and tUD103.Key1 = sh.FS_PickListNo_c ";
				_strSQL += "left join ShipTo st on st.Company = sd.Company and st.CustNum = id.ShipToCustNum and st.ShipToNum = id.ShipToNum ";
				_strSQL += "left join PartLot pl on pl.Company = sd.Company and pl.PartNum = sd.PartNum and pl.LotNum = sd.LotNum ";

				string _strWHERE = "";


				if (strCompany != null && strCompany != "")
				{ _strWHERE += "AND ih.Company = '" + strCompany + "' "; }

				if (strInvoiceNum != null && strInvoiceNum != "")
				{ _strWHERE += "AND ih.InvoiceNum = " + strInvoiceNum + " "; }

				if (strLegalNumber != null && strLegalNumber != "")
				{ _strWHERE += "AND ih.LegalNumber = '" + strLegalNumber + "' "; }

				if (_strWHERE != null && _strWHERE != "")
				{ _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }


				SQLServerBO _MSSQL = new SQLServerBO();
				string _strSQLCon = _MSSQL._retSQLConnectionString();
				_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

				DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

				if (_dts.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in _dts.Tables[0].Rows)
					{
						ReprintInvoice oReprintInvoice = new ReprintInvoice();

						oReprintInvoice.Company = row["Company"].ToString();
						oReprintInvoice.InvoiceNum = row["InvoiceNum"].ToString();
						oReprintInvoice.InvoiceDate = DBNull.Value.Equals(row["InvoiceDate"]) ? "1999-01-01" : Convert.ToDateTime((row["InvoiceDate"])).ToString("yyyy-MM-dd");
						oReprintInvoice.LegalNumber = row["LegalNumber"].ToString();
						oReprintInvoice.InvoiceComment = row["InvoiceComment"].ToString();

						oReprintInvoice.OrderNum = DBNull.Value.Equals(row["OrderNum"]) ? 0 : int.Parse(row["OrderNum"].ToString());
						oReprintInvoice.OrderLine = DBNull.Value.Equals(row["OrderLine"]) ? 0 : int.Parse(row["OrderLine"].ToString());
						oReprintInvoice.OrderRel = DBNull.Value.Equals(row["OrderRelNum"]) ? 0 : int.Parse(row["OrderRelNum"].ToString());
						oReprintInvoice.PackNum = DBNull.Value.Equals(row["PackNum"]) ? 0 : int.Parse(row["PackNum"].ToString());
						oReprintInvoice.PackLine = DBNull.Value.Equals(row["PackLine"]) ? 0 : int.Parse(row["PackLine"].ToString());
						oReprintInvoice.Quantity = DBNull.Value.Equals(row["OurShipQty"]) ? 0 : decimal.Parse(row["OurShipQty"].ToString());

						oReprintInvoice.Warehouse = row["WarehouseCode"].ToString();
						oReprintInvoice.BinNum = row["BinNum"].ToString();
						oReprintInvoice.LotNum = row["LotNum"].ToString();
						oReprintInvoice.PartNum = row["PartNum"].ToString();
						oReprintInvoice.PartDescription = row["LineDesc"].ToString();
						oReprintInvoice.UOM = row["InventoryShipUOM"].ToString();
						oReprintInvoice.Weight = DBNull.Value.Equals(row["Weight"]) ? 0 : decimal.Parse(row["Weight"].ToString());
						oReprintInvoice.ShipVia = row["ShipViaCode"].ToString();
						oReprintInvoice.Carton = DBNull.Value.Equals(row["SD_CartonNum_c"]) ? 0 : int.Parse(row["SD_CartonNum_c"].ToString());
						oReprintInvoice.TrackingNum = row["TrackingNumber"].ToString();
						oReprintInvoice.PickListNum = row["FS_PickListNo_c"].ToString();

						oReprintInvoice.Picker = row["SD_PickedBy_c"].ToString();
						oReprintInvoice.Packer = row["SD_PackedBy_c"].ToString();
						oReprintInvoice.ShipToName = row["Name"].ToString();
						oReprintInvoice.ShipToAddr1 = row["Address1"].ToString();
						oReprintInvoice.ShipToAddr2 = row["Address2"].ToString();
						oReprintInvoice.ShipToAddr3 = row["Address3"].ToString();
						oReprintInvoice.ShipToState = row["State"].ToString();
						oReprintInvoice.ShipToCity = row["City"].ToString();
						oReprintInvoice.ShipToZip = row["ZIP"].ToString();
						oReprintInvoice.ShipToCountry = row["Country"].ToString();
						oReprintInvoice.ExpiryDate = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");

						InvoicesList.Add(oReprintInvoice);
					}

					strMessage = "";
					IsError = false;
				}
				else
				{
					strMessage = "Invoice not found.";
					IsError = true;
				}

			} // ending for try
			catch (Exception ex)
			{
				strMessage = ex.Message.ToString();
				IsError = true;
			}

			return (IsError ? false : true);


		}

		public bool _ReAssignDO(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strInvoiceNum, decimal dWeight, decimal dCarton, string strTransporter, string strPackNum, string strRemark,out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;
            string _strSQL = "";
            DataSet _dts;
            string strConsignment = "";
            int lastRunning;

            try
            {
                strMessage = string.Empty;

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);


                _strSQL = "Select Company, ShipViaCode, Active01_c, PreFix01_c, BeginNum01_c, EndNum01_c, LastRunNum01_c, EndNum01_c - LastRunNum01_c as Balance01, Active02_c, PreFix02_c, BeginNum02_c, EndNum02_c, LastRunNum02_c, EndNum02_c - LastRunNum02_c as Balance02, Suffix01_c, Suffix02_c ";
                _strSQL += "from ShipVia ";
                _strSQL += "Where ShipViaCode = '" + strTransporter + "' ";
                _strSQL += "And company = '" + strCompany + "' ";

                _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        if (bool.Parse(row["Active01_c"].ToString()) == true)
                        {
                            if (int.Parse(row["Balance01"].ToString()) > 0)
                            {
                                if (int.Parse(row["LastRunNum01_c"].ToString()) == 0)
                                {
                                    lastRunning = int.Parse(row["BeginNum01_c"].ToString());
                                }
                                else
                                {
                                    lastRunning = int.Parse(row["LastRunNum01_c"].ToString());
                                    lastRunning++;
                                }

                                _strSQL = "update shipvia set LastRunNum01_c = '" + lastRunning.ToString() + "' ";
                                _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                strConsignment = row["PreFix01_c"].ToString() + lastRunning.ToString() + row["Suffix01_c"].ToString();
                            }
                            else if (int.Parse(row["Balance02"].ToString()) > 0)
                            {
                                if (int.Parse(row["LastRunNum02_c"].ToString()) == 0)
                                {
                                    lastRunning = int.Parse(row["BeginNum02_c"].ToString());
                                }
                                else
                                {
                                    lastRunning = int.Parse(row["LastRunNum02_c"].ToString());
                                    lastRunning++;
                                }

                                _strSQL = "update shipvia set LastRunNum02_c = '" + lastRunning.ToString() + "', Active02_c = 1, Active01_c = 0 ";
                                _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                strConsignment = row["PreFix02_c"].ToString() + lastRunning.ToString() + row["Suffix02_c"].ToString();

                            }
                            else
                            {
                                //IsError = true;
                                strMessage = "Not able to generate consignment number.";
                                IsError = true;
                            }
                        }
                        else
                        {
                            if (int.Parse(row["Balance02"].ToString()) > 0)
                            {
                                if (int.Parse(row["LastRunNum02_c"].ToString()) == 0)
                                {
                                    lastRunning = int.Parse(row["BeginNum02_c"].ToString());
                                }
                                else
                                {
                                    lastRunning = int.Parse(row["LastRunNum02_c"].ToString());
                                    lastRunning++;
                                }

                                _strSQL = "update shipvia set LastRunNum02_c = '" + lastRunning.ToString() + "' ";
                                _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                strConsignment = row["PreFix02_c"].ToString() + lastRunning.ToString() + row["Suffix02_c"].ToString();

                            }
                            else if (int.Parse(row["Balance01"].ToString()) > 0)
                            {

                                if (int.Parse(row["LastRunNum01_c"].ToString()) == 0)
                                {
                                    lastRunning = int.Parse(row["BeginNum01_c"].ToString());
                                }
                                else
                                {
                                    lastRunning = int.Parse(row["LastRunNum01_c"].ToString());
                                    lastRunning++;
                                }

                                _strSQL = "update shipvia set LastRunNum01_c = '" + lastRunning.ToString() + "', Active01_c = 1, Active02_c = 0 ";
                                _strSQL += "where company = '" + strCompany + "' and shipviacode = '" + strTransporter + "' ";

                                _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                                strConsignment = row["PreFix01_c"].ToString() + lastRunning.ToString() + row["Suffix01_c"].ToString();
                            }
                            else
                            {
                                strMessage = "Not able to generate consignment number.";
                                IsError = true;

                            }

                        }

                    }

                }



                if (IsError == false)
                {
                    _strSQL = "update erp.shiphead set ShipViaCode = '" + strTransporter + "' , Weight = '" + dWeight + "', trackingnumber = '" + strConsignment + "' ";
                    _strSQL += "where PackNum  = '" + strPackNum + "' and company = '" + strCompany + "' ";


                    IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);


                    _strSQL = "update sh set sh.SD_Remarks_c = '" + strRemark + "', sh.SD_CartonNum_c = '" + dCarton + "' , sh.sd_reprintby_c = '" + strUID + "', sh.sd_reprintdt_c = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    _strSQL += "from erp.Shiphead s ";
                    _strSQL += "inner join erp.shiphead_ud sh on s.SysRowId = sh.foreignsysrowid ";
                    _strSQL += "where s.PackNum  = '" + strPackNum + "' and s.company = '" + strCompany + "' ";

                    IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                    if (IsUpdated)
                    {
                        strMessage = "";
                        IsError = false;
                    }
                    else
                    {
                        strMessage = "Can't update the DO";
                        IsError = true;

                    }

                }

            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _ReprintLabelOnly(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strPickList, int iQuantity , out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;

            try
            {
                string _strSQL = "dbo.SDSCreateLabel_Reprint";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company",strCompany),
                    new SqlParameter("@EntryBy",strUID),
                    new SqlParameter("@TranType","SHP-LBL"),
                    new SqlParameter("@TranDate",DateTime.Now.ToString("yyyy-MM-dd")),
                    new SqlParameter("@PartNum",""),
                    new SqlParameter("@UOM",""),
                    new SqlParameter("@LotNum",""),
                    new SqlParameter("@TranQty",0),
                    new SqlParameter("@LabelQty",iQuantity),
                    new SqlParameter("@Parm01",""),
                    new SqlParameter("@Parm02",""),
                    new SqlParameter("@Parm03",""),
                    new SqlParameter("@Parm04",strPickList)
                };



                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                IsUpdated = _MSSQL._exeStoredProcedureCommand(_strSQL, _strSQLCon, parameters);

                if (IsUpdated)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Can't perform reprint label";
                    IsError = true;

                }
            }

            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadOrderReleases(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strPass, int iOrderNum, ref IList<Order> OrderReleases,  out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "select o.company, o.ordernum, o.orderline, o.orderrelnum, ";
                _strSQL += "o.partnum, o.SellingReqQty, o.warehousecode, o.SalesUM, o.IUM, o.ShipToNum, o.ShipToCustNum, ";
                _strSQL += "p.partdescription, p.NetWeightUOM ";
                _strSQL += "from erp.OrderRel o ";
                _strSQL += "left join erp.part p on o.Company = p.Company and o.PartNum = p.PartNum ";

                string _strWHERE = "";

                if (strCompany != null && strCompany != "")
                    { _strWHERE += "AND o.Company = '" + strCompany + "' "; }

                if (iOrderNum != 0)
                    { _strWHERE += " and o.OrderNum = '" + iOrderNum + "' ";  }

                if (_strWHERE != null && _strWHERE != "")
                    { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Order oOrder = new Order();
                        oOrder.Company = row["company"].ToString();
                        oOrder.OrderNum = DBNull.Value.Equals(row["OrderNum"]) ? 0 : int.Parse(row["OrderNum"].ToString()); 
                        oOrder.OrderLine = DBNull.Value.Equals(row["orderline"]) ? 0 : int.Parse(row["orderline"].ToString());
                        oOrder.OrderRel = DBNull.Value.Equals(row["orderrelnum"]) ? 0 : int.Parse(row["orderrelnum"].ToString());
                        oOrder.PartNum = row["partnum"].ToString();
                        oOrder.PartDescription = row["partdescription"].ToString();
                        oOrder.OrderQty = DBNull.Value.Equals(row["SellingReqQty"]) ? 0 : decimal.Parse(row["SellingReqQty"].ToString());
                        oOrder.SalesUM = row["SalesUM"].ToString();
                        oOrder.IUM = row["IUM"].ToString();
                        oOrder.NetWeightUOM = row["NetWeightUOM"].ToString();
                        oOrder.Warehouse = row["warehousecode"].ToString();
                        oOrder.ShipToNum = row["ShipToNum"].ToString();
                        oOrder.CustNum = row["ShipToCustNum"].ToString();

                        OrderReleases.Add(oOrder);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Order not found ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }

        public bool _LoadOrderFromQueue(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, EpicUser oEpicorUser, OrderHed oOrderHed,  out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated;
            string _strSQL = "";
            DataSet _ds;

            try 
            {
                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                _strSQL += "select top 1 Company, OrderNum ";
                _strSQL += "from OrderHed ";
                _strSQL += "where SD_MWSProcess_c = 1 ";
                _strSQL += "and SD_SkipAllocation_c = 1 ";
                _strSQL += "and SD_IsPicked_c = 0 ";
                _strSQL += "and OpenOrder = 1 ";
                _strSQL += "and ReadyToFulfill = 1 ";
                _strSQL += "and SD_OrderType_c in ('" + oEpicorUser.OrderType.Replace("~","','") + "') ";
                _strSQL += "and Company = '" + strCompany + "' ";

                _strSQL += "and OrderNum in (select distinct orr.ordernum ";
                _strSQL += "from erp.OrderRel orr ";
                _strSQL += "inner join ( ";
                _strSQL += "select company, partnum, WarehouseCode, sum(OnhandQty) as TotalOnhand ";
                _strSQL += "from erp.PartBin ";
                _strSQL += "where Company = '" + strCompany + "' ";
                _strSQL += "group by company, partnum, WarehouseCode ";
                _strSQL += ") tbX on orr.Company = tbx.Company and orr.PartNum = tbx.PartNum and orr.WarehouseCode = tbx.WarehouseCode ";
                _strSQL += "where orr.OpenRelease = 1 ";
                _strSQL += "and orr.OpenOrder = 1 ";
                _strSQL += "and orr.Company = '" + strCompany + "' ";
                _strSQL += "and orr.VoidRelease = 0 ) ";

                _strSQL += "order by AP_UrgentOrder_c desc ";

                _ds = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        oOrderHed.Company = row["Company"].ToString();
                        oOrderHed.OrderNum = DBNull.Value.Equals(row["OrderNum"]) ? 0 : int.Parse(row["OrderNum"].ToString()); ;
                        oOrderHed.PickListNum = "";
                    }

                    _strSQL = "update OrderHed set SD_IsPicked_c = 1 ";
                    _strSQL += "where OrderNum  = '" + oOrderHed.OrderNum + "' and company = '" + oOrderHed.Company + "' ";

                    IsUpdated = _MSSQL._exeSQLCommand(_strSQL, _strSQLCon);

                    if (IsUpdated)
                    {
                        strMessage = "";
                        IsError = false;
                    }
                    else
                    {
                        strMessage = "Can't process order";
                        IsError = true;
                    }

                    //strMessage = "";
                    //IsError = false;
                }
                else
                {
                    strMessage = "No orders found.";
                    IsError = true;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _SalesOrderAllocation(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strPass, ref OrderHed oOrder, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated = false;
            
            try 
            {
                EpicorBO oEpicor = new EpicorBO();

                // assign a picklist number
                int iPackNum;
                string strReturnMsg;
                string _strWHERE = "";
                string _strSQL = "dbo.SDS_AssignPickListNum";
                DataSet _dts;

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company",strCompany)
                };

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                _dts = _MSSQL._MSSQLDataSetResultStoreProcedure(_strSQL, _strSQLCon, parameters);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    oOrder.PickListNum = _dts.Tables[0].Rows[0]["PickListNum"].ToString();
                    //foreach (DataRow row in _dts.Tables[0].Rows)
                    //{
                    //    oOrder.PickListNum = (row["PickListNum"].ToString());
                    //}
                }

                if (oOrder.PickListNum == "" || oOrder.PickListNum == null)
                {
                    strMessage = "Can't generate picklist number.";
                    IsError = true;
                    return false;
                }

                // load sales order release into object
                IList<Order> oOrderRels = new List<Order>();
                _LoadOrderReleases(ref oEpicEnv, strCompany, strCurPlant, strUID, strPass, oOrder.OrderNum, ref oOrderRels, out strReturnMsg);

                // create customer shipment header
                IsUpdated = oEpicor._ShipHead(ref oEpicEnv, out strMessage, out iPackNum,  oOrder.OrderNum, oOrder.PickListNum, strUID, strPass, strCompany, strCurPlant);

                if (IsUpdated == false)
                {
                    strMessage = "Can't create customer shipment for sales order " + oOrder.OrderNum.ToString() + ".";
                    IsError = true;
                    return false;
                }

                // create customer shipment details
                foreach (Order oRel in oOrderRels)
                {
                    Order tmpRel = oRel;
                    decimal dRelQty;
                    decimal dOnHand;
                    decimal dShipQty;

                    dRelQty = oRel.OrderQty;

                    while (dRelQty != 0)
                    {
                        _strSQL = "select top 1 binnum, lotnum, availableqty ";
                        _strSQL += "from SDS_StockAvailability ";

                        _strWHERE = "";

                        if (strCompany != null && strCompany != "")
                        { _strWHERE += "AND Company = '" + strCompany + "' "; }

                        if (oRel.PartNum != null && oRel.PartNum != "")
                        { _strWHERE += "AND PartNum = '" + oRel.PartNum + "' "; }

                        if (oRel.Warehouse != null && oRel.Warehouse != "")
                        { _strWHERE += "AND WarehouseCode = '" + oRel.Warehouse + "' "; }

                        if (_strWHERE != null && _strWHERE != "")
                        { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }

                        _strSQL += "order by Aging, LotNum, BinNum ";

                        _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            tmpRel.Bin = _dts.Tables[0].Rows[0]["binnum"].ToString();
                            tmpRel.Lot = _dts.Tables[0].Rows[0]["lotnum"].ToString();
                            dOnHand = decimal.Parse(_dts.Tables[0].Rows[0]["availableqty"].ToString());
                            dShipQty = (dOnHand >= dRelQty ? dRelQty : dOnHand);
                            dRelQty = (dOnHand >= dRelQty ? 0 : dRelQty - dOnHand);
                            tmpRel.OrderQty = dShipQty;

                            IsUpdated = oEpicor._ShipDetail(ref oEpicEnv, out strMessage, iPackNum, ref tmpRel, strUID, strPass, strCompany, strCurPlant);
                            //return true;
                            //if (IsUpdated == false)
                            //{
                            //strMessage = "Can't create customer shipment details for sales order " + oOrder.OrderNum.ToString() + ".";
                            IsError = false;
                            //return false;
                            //}
                        }
                        else
                        {
                            //strMessage = "Can't create customer shipment details for sales order " + oOrder.OrderNum.ToString() + ".";
                            IsError = false;
                            //return true;
                        }

                    }

                }
                
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadCustomerShipment(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strPickListNum, string strPackNum, ref IList<ShipDetail> oShipments,  string strUID, string strPass, out string strMessage)
        {
            bool IsError = false;
            string _strSQL = "";
            DataSet _ds;

            try
            {
                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                _strSQL = "select sh.Company ,sh.PackNum, sh.ShipDate, sh.ShipViaCode, sh.ShipPerson, sh.EntryPerson, sh.TrackingNumber, sh.Weight, sh.WeightUOM ";
                _strSQL += ", sh.FS_PickListNo_c, sh.SD_CartonNum_c, sh.SD_StationId_c, sh.SD_Reason_c, sh.SD_Remarks_c, sh.SD_ReprintBy_c, sh.SD_ReprintDT_c, sh.SD_PalletNum_c ";
                _strSQL += ", sh.SD_IsPicked_c, sh.SD_Picker_c, sh.SD_StartPicked_c, sh.SD_EndPicked_c, sh.SD_IsPacked_c, sh.SD_Packer_c, sh.SD_StartPacked_c, sh.SD_EndPacked_c ";
                _strSQL += ", sd.PackLine, sd.OrderNum, sd.OrderLine, sd.OrderRelNum, sd.SellingInventoryShipQty, sd.PartNum, sd.LineDesc, sd.IUM, sd.SalesUM, sd.WarehouseCode ";
                _strSQL += ", sd.BinNum, sd.LotNum, sd.SD_IsComplete_c, sd.SD_IsPacked_c as LineIsPacked, sd.SD_PackedBy_c, sd.SD_PackedDate_c, sd.SD_IsPicked_c as LineIsPicked, sd.SD_PickedBy_c, sd.SD_PickedDate_c ";
                _strSQL += ", sd.SD_TagNum_c, pl.ExpirationDate ";
                _strSQL += ",c.CustID + '~' + t.Name + '~' + t.Address1 + '~' + t.Address2 + '~' + t.Address3 + '~' + t.City + ', ' + t.ZIP + ', ' + t.State + '~' + t.Country as custinfo ";
                _strSQL += "from ShipHead sh ";
                _strSQL += "inner join ShipDtl sd on sh.Company = sd.Company and sh.PackNum = sd.PackNum ";
                _strSQL += "left join PartLot pl on sd.Company = pl.Company and sd.PartNum = pl.PartNum and sd.LotNum = pl.LotNum ";
                _strSQL += "left join Customer c on sh.Company = c.Company and sh.CustNum = c.CustNum ";
                _strSQL += "left join ShipTo t on c.Company = t.Company and c.CustNum = t.CustNum and sh.ShipToNum = t.ShipToNum ";

                _strSQL += "where sh.Invoiced = 0 ";

                if (strCompany != null && strCompany != "")
                    { _strSQL += "AND sh.Company = '" + strCompany + "' "; }

                if (strPackNum != null && strPackNum != "")
                    { _strSQL += "AND sh.PackNum = '" + strPackNum + "' "; }

                if (strPickListNum != null && strPickListNum != "")
                    { _strSQL += "AND sh.FS_PickListNo_c = '" + strPickListNum + "' "; }

                _ds = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        ShipDetail oShipDtl = new ShipDetail();
                        oShipDtl.Company = row["Company"].ToString();
                        oShipDtl.Customer = row["custinfo"].ToString();
                        oShipDtl.PackNum =  DBNull.Value.Equals(row["PackNum"]) ? 0 : int.Parse(row["PackNum"].ToString());
                        oShipDtl.ShipDate = DBNull.Value.Equals(row["ShipDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ShipDate"])).ToString("yyyy-MM-dd");
                        oShipDtl.ShipViaCode  = row["ShipViaCode"].ToString();
                        oShipDtl.ShipPerson = row["ShipPerson"].ToString();
                        oShipDtl.EntryPerson = row["EntryPerson"].ToString();
                        oShipDtl.TrackingNumber = row["TrackingNumber"].ToString();
                        oShipDtl.Weight = DBNull.Value.Equals(row["Weight"]) ? 0 : decimal.Parse(row["Weight"].ToString());
                        oShipDtl.WeightUOM = row["WeightUOM"].ToString();
                        oShipDtl.PickListNum = row["FS_PickListNo_c"].ToString();
                        oShipDtl.CartonNum = DBNull.Value.Equals(row["SD_CartonNum_c"]) ? 0 : decimal.Parse(row["SD_CartonNum_c"].ToString());
                        oShipDtl.StationID = row["SD_StationId_c"].ToString();
                        oShipDtl.Reason = row["SD_Reason_c"].ToString();
                        oShipDtl.Remark = row["SD_Remarks_c"].ToString();
                        oShipDtl.RePrintBy = row["SD_ReprintBy_c"].ToString();
                        oShipDtl.RePrintDT = DBNull.Value.Equals(row["SD_ReprintDT_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_ReprintDT_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.PalletNumber = row["SD_PalletNum_c"].ToString();
                        oShipDtl.IsPicked = row["SD_IsPicked_c"].ToString() == "True" ? true : false; 
                        oShipDtl.Picker = row["SD_Picker_c"].ToString();
                        oShipDtl.StartPicked = DBNull.Value.Equals(row["SD_StartPicked_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_StartPicked_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.EndPicked = DBNull.Value.Equals(row["SD_EndPicked_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_EndPicked_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.IsPacked = row["SD_IsPacked_c"].ToString() == "True" ? true : false;
                        oShipDtl.Packer = row["SD_Packer_c"].ToString();
                        oShipDtl.StartPacked = DBNull.Value.Equals(row["SD_StartPacked_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_StartPacked_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.EndPacked = DBNull.Value.Equals(row["SD_EndPacked_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_EndPacked_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.PackLine = DBNull.Value.Equals(row["PackLine"]) ? 0 : int.Parse(row["PackLine"].ToString());
                        oShipDtl.OrderNum = DBNull.Value.Equals(row["OrderNum"]) ? 0 : int.Parse(row["OrderNum"].ToString());
                        oShipDtl.OrderLine = DBNull.Value.Equals(row["OrderLine"]) ? 0 : int.Parse(row["OrderLine"].ToString());
                        oShipDtl.OrderRel = DBNull.Value.Equals(row["OrderRelNum"]) ? 0 : int.Parse(row["OrderRelNum"].ToString());
                        oShipDtl.SellingInventoryShipQty = DBNull.Value.Equals(row["SellingInventoryShipQty"]) ? 0 : decimal.Parse(row["SellingInventoryShipQty"].ToString());
                        oShipDtl.PartNum = row["PartNum"].ToString();
                        oShipDtl.LineDesc = row["LineDesc"].ToString();
                        oShipDtl.IUM = row["IUM"].ToString();
                        oShipDtl.SalesUm = row["SalesUM"].ToString();
                        oShipDtl.WarehouseCode = row["WarehouseCode"].ToString();
                        oShipDtl.BinNum = row["BinNum"].ToString();
                        oShipDtl.LotNum = row["LotNum"].ToString();
                        oShipDtl.IsComplete = row["SD_IsComplete_c"].ToString() == "True" ? true : false;
                        oShipDtl.LineIsPacked = row["LineIsPacked"].ToString() == "True" ? true : false;
                        oShipDtl.LinePackedBy = row["SD_PackedBy_c"].ToString();
                        oShipDtl.LinePackedDate = DBNull.Value.Equals(row["SD_PackedDate_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_PackedDate_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.LineIsPicked = row["LineIsPicked"].ToString() == "True" ? true : false;
                        oShipDtl.LinePickedBy = row["SD_PickedBy_c"].ToString();
                        oShipDtl.LinePickedDate = DBNull.Value.Equals(row["SD_PickedDate_c"]) ? "1999-01-01" : Convert.ToDateTime((row["SD_PickedDate_c"])).ToString("yyyy-MM-dd");
                        oShipDtl.TagNum = row["SD_TagNum_c"].ToString();
                        oShipDtl.ExpirationDate = DBNull.Value.Equals(row["ExpirationDate"]) ? "1999-01-01" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd");

                        oShipments.Add(oShipDtl);
                    }
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No shipment found.";
                    IsError = true;
                }


            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadActiveShipment(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strPicker, string strPacker, ref ShipHeader oShipHead, string strUID, string strPass, out string strMessage)
        {
            bool IsError = false;
            string _strSQL = "";
            DataSet _ds;

            try 
            {
                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                _strSQL = "select top 1 Company, Packnum, FS_PickListNo_c ";
                _strSQL += "from ShipHead ";
                _strSQL += "where ReadyToInvoice = 0 ";

                if (strCompany != null && strCompany != "")
                { _strSQL += "AND Company = '" + strCompany + "' "; }

                if ( strPicker != "")
                {
                    _strSQL += "and SD_IsPicked_c = 0 ";
                    _strSQL += "and SD_Picker_c = '" + strPicker + "' ";
                }

                if ( strPacker != "")
                {
                    _strSQL += "and SD_IsPicked_c = 1 ";
                    _strSQL += "and SD_IsPacked_c = 0 ";
                    _strSQL += "and SD_Packer_c = '" + strPacker + "' ";
                }

                _ds = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_ds.Tables[0].Rows.Count > 0)
                {
                    oShipHead.Company = _ds.Tables[0].Rows[0]["Company"].ToString();
                    oShipHead.PackNum = DBNull.Value.Equals(_ds.Tables[0].Rows[0]["Packnum"]) ? 0 : int.Parse(_ds.Tables[0].Rows[0]["Packnum"].ToString());  
                    oShipHead.PickListNum = _ds.Tables[0].Rows[0]["FS_PickListNo_c"].ToString();

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No active shipment found.";
                    IsError = true;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);
        }

        public bool _UpdatePickPackShipment(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strPickListNum, string strPackNum, string strPackLine, string strPicker, string strPacker, string strTagNum, string strPallet, string strUID, string strPass, out string strMessage)
        {
            bool IsError = false;
            bool IsUpdated = false;

            try 
            {
                EpicorBO oEpicor = new EpicorBO();

                IsUpdated = oEpicor._ShipDetailUpd(ref oEpicEnv, out strMessage, int.Parse(strPackNum), int.Parse(strPackLine), strUID, strPass, strCompany, strCurPlant, strPicker, strPacker, strTagNum, strPallet);

                if (IsUpdated)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Can't perform updates in shipment.";
                    IsError = true;

                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);

        }

        private bool checkLineVoid(ref EpicEnv oEpicEnv, string pickNum, string pickLine)
        {
			string _strSQL;


			SQLServerBO _MSSQL = new SQLServerBO();
			string _strSQLCon = _MSSQL._retSQLConnectionString();
			_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

            _strSQL = "select count(*) as VoidCount from UD103A where Key1 = '" + pickNum + "' and ChildKey2 = '" + pickLine + "' and SD_Voided_c = 0";

			DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);
			var voided = false;
			if (_dts.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in _dts.Tables[0].Rows)
				{
					voided = Int32.Parse(row["VoidCount"].ToString()) <= 0;
				}
			}

            return voided;
		}

		public double _checkPickPartQty(ref EpicEnv oEpicEnv, string pickNum, string partNum, string lotNum)
		{
			string _strSQL;


			SQLServerBO _MSSQL = new SQLServerBO();
			string _strSQLCon = _MSSQL._retSQLConnectionString();
			_strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

			_strSQL = "select sum(SD_AllocateQuantity_c) as AllocateQty from UD103A where SD_PartNum_c = '" + partNum + "' and Key1 = '" + pickNum + "' and SD_Voided_c = 0 and SD_LotNum_c = '" + lotNum + "'";
            double count = 0;
			DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);
			if (_dts.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in _dts.Tables[0].Rows)
				{
					var countStr = row["AllocateQty"].ToString();
                    count = Double.Parse(countStr);
				}
			}

			return count;
		}


        public bool _RetrievePickPackStatus(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strPickListNum, ref PickPack3 oPickPack, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select top 1 u103.company, u103.Key1, u103.SD_Voided_c ";
                _strSQL += "from UD103 u103 ";

                string _strWHERE = "";

                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND u103.Company = '" + strCompany + "' "; }

                if (strPickListNum != "")
                { _strWHERE += " and u103.Key1 = '" + strPickListNum + "' "; }

                if (_strWHERE != null && _strWHERE != "")
                { _strSQL = _strSQL + " WHERE " + _strWHERE.Substring(3, _strWHERE.Length - 3); }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oPickPack.Company = row["company"].ToString();
                        oPickPack.PickListNum = row["Key1"].ToString();
                        oPickPack.PickListVoided = row["SD_Voided_c"].ToString() == "True" ? true : false;

                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Picklist not found ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);



        }

        public bool _LoadState(ref EpicEnv oEpicEnv, string strCompany, string strPicklistNum, out bool isControlWeight)
        {
            string _strSQL;
            bool isError = false;
            isControlWeight = false;


            SQLServerBO _MSSQL = new SQLServerBO();
            string _strSQLCon = _MSSQL._retSQLConnectionString();
            _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

            _strSQL = "SELECT UDC.* FROM Dbo.UD103 UD103 INNER JOIN Dbo.UDCodes UDC ON UD103.Company = UDC.Company AND UD103.SD_State_c = UDC.CodeID ";
            _strSQL += "WHERE UDC.CodeTypeID = 'STATE' AND UD103.Company = '" + strCompany + "' AND UD103.Key1 = '" + strPicklistNum + "' ";


            DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);
            if (_dts.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in _dts.Tables[0].Rows)
                {
                    isControlWeight = Convert.ToBoolean(row["SD_MWSControlWeight_c"]);
                }
            }
            else
            {
                isError = true;
            }

            return isError ? false : true;
        }

        public bool _LoadNewShipVia(ref EpicEnv oEpicEnv, string strCompany, string strShipViaCode, ref IList<NewShipVia> oShipViaList, out string strMessage)
        {
            bool IsError = false;

            try
            {

                string _strSQL = "select company, shipviacode, description, SD_MWS_MinWeight_c, SD_MWS_IsMinWeight_c, SD_MWS_IsSkipWeight_c ";
                _strSQL += "from dbo.shipvia ";
                _strSQL += "Where Company = '" + strCompany + "' ";


                if (strShipViaCode != "" && strShipViaCode != null)
                {
                    _strSQL += " and shipviacode = '" + strShipViaCode + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        NewShipVia oShipVia = new NewShipVia();
                        oShipVia.ShipViaCode = row["shipviacode"].ToString();
                        oShipVia.ShipViaDescription = (row["description"].ToString());
                        oShipVia.SD_MWS_MinWeight_c = Convert.ToDecimal(row["SD_MWS_MinWeight_c"]);
                        oShipVia.SD_MWS_IsMinWeight_c = Convert.ToBoolean(row["SD_MWS_IsMinWeight_c"]);
                        oShipVia.SD_MWS_IsSkipWeight_c = Convert.ToBoolean(row["SD_MWS_IsSkipWeight_c"]);


                        oShipViaList.Add(oShipVia);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "ShipVia listing not found ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }


            return (IsError ? false : true);

        }
    }
}