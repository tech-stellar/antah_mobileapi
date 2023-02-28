using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class ReceiptBO
    {
        public bool _LoadPORelDetailById(ref EpicEnv oEpicEnv, string strCompany, int iPONum, ref IList<ReceiptDetail> oRecvDtl, string strLegalNumber, out string strMessage)
        {
            bool IsError = false;

            try
            {
                /* 2021-04-30 yew mun add in legal number parameter  */
                /* 2021-07-12 yew mun add in the balance qty   */
                string _strSQL = "select * from ( "; 
                _strSQL += "SELECT podtl.Company, podtl.PONUM, podtl.POLine, podtl.PartNum, podtl.LineDesc, podtl.PUM "; //addded PUM - KOO
                _strSQL += ", porel.PORelNum, porel.RelQty, porel.Plant, PORel.WarehouseCode ";
                _strSQL += ", poh.VendorNum, v.VendorID, poh.OpenOrder, poh.LegalNumber, ";
                _strSQL += " isnull((select sum(OurQty) from erp.RcvDtl r ";
                _strSQL += " where r.Company = porel.Company and r.PONum = porel.PONum ";
                _strSQL += " and r.POLine = porel.POLine and r.PORelNum = porel.PORelNum ),0) ArrivedQty,  ";
                _strSQL += " isnull((select sum(OurQty) from erp.RcvDtl r ";
                _strSQL += " where r.Company = porel.Company and r.PONum = porel.PONum ";
                _strSQL += " and r.POLine = porel.POLine and r.PORelNum = porel.PORelNum and r.Received = 1),0) ReceivedQty, podtl.XOrderQty  ";
                _strSQL += "from erp.PODetail podtl ";
                _strSQL += "inner join erp.PORel porel on porel.Company = podtl.Company and porel.PONum = podtl.PONUM and porel.POLine = podtl.POLine ";
                _strSQL += "inner join erp.POHeader poh on poh.Company = podtl.Company and poh.PONum = podtl.PONUM ";
                _strSQL += "inner join erp.Vendor v on v.Company = poh.Company and v.VendorNum = poh.VendorNum ";
                _strSQL += "where podtl.company = '{0}' and podtl.openline = 1 ";

                if (iPONum != 0)
                {
                    _strSQL += "and podtl.PONUM = " + iPONum ;
                }

                if (strLegalNumber != "" && strLegalNumber != null)
                {
                    _strSQL += " and poh.LegalNumber = '" + strLegalNumber + "' ";
                }

                _strSQL += ") tbA where tbA.relqty > tbA.ReceivedQty ";

                _strSQL = string.Format(_strSQL, strCompany);


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        ReceiptDetail oRecDtl = new ReceiptDetail();
                        oRecDtl.Company = row["Company"].ToString();
                        oRecDtl.PONum = int.Parse(row["PONum"].ToString());
                        oRecDtl.POLine = int.Parse(row["POLine"].ToString());
                        oRecDtl.POLineRel = int.Parse(row["PORelNum"].ToString());
                        oRecDtl.PartNum = row["PartNum"].ToString();
                        oRecDtl.PartDesc = row["LineDesc"].ToString();
                        oRecDtl.VendorID = row["VendorID"].ToString();
                        oRecDtl.VendorNum = int.Parse(row["VendorNum"].ToString());
                        oRecDtl.PORelQty = decimal.Parse(row["RelQty"].ToString());
                        oRecDtl.RecvUOM = row["PUM"].ToString(); //added PUM - KOO 22072019
                        oRecDtl.OpenOrder = (row["OpenOrder"].ToString() == "True" ? true : false);

                        /* 2021-04-30 yew mun add in legal number parameter  */
                        oRecDtl.LegalNumber = row["LegalNumber"].ToString();
                        oRecDtl.PORelPlant = row["Plant"].ToString();
                        oRecDtl.PORelWarehouse = row["WarehouseCode"].ToString();

                        /* 2021-07-12 yew mun add in the balance qty   */
                        oRecDtl.ReceivedQty = decimal.Parse(row["ReceivedQty"].ToString());
                        oRecDtl.ArrivedQty = decimal.Parse(row["ArrivedQty"].ToString());
                        oRecDtl.POQty = decimal.Parse(row["RelQty"].ToString());
                        oRecDtl.BalancedQty = oRecDtl.POQty - oRecDtl.ArrivedQty;
                        oRecvDtl.Add(oRecDtl);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "PO Receipts listing not found ";
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


        public bool _LoadPORelDetail(ref EpicEnv oEpicEnv, string strCompany, int iPONum, ref IList<ReceiptDetail> oRecvDtl, string strLegalNumber, string strVendId, string strPartNum, out string strMessage)
        {
            bool IsError = false;

            try
            {
                /* 2021-04-30 yew mun add in legal number parameter  */
                /* 2021-07-12 yew mun add in the balance qty   */
                string _strSQL = "select * from ( ";
                _strSQL += "SELECT podtl.Company, podtl.PONUM, podtl.POLine, podtl.PartNum, podtl.LineDesc, podtl.PUM "; //addded PUM - KOO
                _strSQL += ", porel.PORelNum, porel.RelQty, porel.Plant, PORel.WarehouseCode ";
                _strSQL += ", poh.VendorNum, v.VendorID, poh.OpenOrder, poh.LegalNumber, ";
                _strSQL += " isnull((select sum(OurQty) from erp.RcvDtl r ";
                _strSQL += " where r.Company = porel.Company and r.PONum = porel.PONum ";
                _strSQL += " and r.POLine = porel.POLine and r.PORelNum = porel.PORelNum ),0) ArrivedQty,  ";
                _strSQL += " isnull((select sum(OurQty) from erp.RcvDtl r ";
                _strSQL += " where r.Company = porel.Company and r.PONum = porel.PONum ";
                _strSQL += " and r.POLine = porel.POLine and r.PORelNum = porel.PORelNum and r.Received = 1),0) ReceivedQty, podtl.XOrderQty, poh.Approve, poh.Confirmed,poh.OrderDate  ";
                _strSQL += "from erp.PODetail podtl ";
                _strSQL += "inner join erp.PORel porel on porel.Company = podtl.Company and porel.PONum = podtl.PONUM and porel.POLine = podtl.POLine ";
                _strSQL += "inner join erp.POHeader poh on poh.Company = podtl.Company and poh.PONum = podtl.PONUM ";
                _strSQL += "inner join erp.Vendor v on v.Company = poh.Company and v.VendorNum = poh.VendorNum ";
                _strSQL += "where podtl.company = '{0}' ";

                if (iPONum != 0)
                {
                    _strSQL += "and podtl.PONUM = " + iPONum;
                }

                if (strLegalNumber != "" && strLegalNumber != null)
                {
                    _strSQL += " and poh.LegalNumber = '" + strLegalNumber + "' ";
                }

                if (strVendId != "" && strVendId != null)
                {
                    _strSQL += " and v.VendorID = '" + strVendId + "' ";
                }

                if (strPartNum != "" && strPartNum != null)
                {
                    _strSQL += " and podtl.PartNum = '" + strPartNum + "' ";
                }

                _strSQL += ") tbA ";

                _strSQL = string.Format(_strSQL, strCompany);


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        ReceiptDetail oRecDtl = new ReceiptDetail();
                        oRecDtl.Company = row["Company"].ToString();
                        oRecDtl.PONum = int.Parse(row["PONum"].ToString());
                        oRecDtl.POLine = int.Parse(row["POLine"].ToString());
                        oRecDtl.POLineRel = int.Parse(row["PORelNum"].ToString());
                        oRecDtl.PartNum = row["PartNum"].ToString();
                        oRecDtl.PartDesc = row["LineDesc"].ToString();
                        oRecDtl.VendorID = row["VendorID"].ToString();
                        oRecDtl.VendorNum = int.Parse(row["VendorNum"].ToString());
                        oRecDtl.PORelQty = decimal.Parse(row["RelQty"].ToString());
                        oRecDtl.RecvUOM = row["PUM"].ToString(); //added PUM - KOO 22072019
                        oRecDtl.OpenOrder = (row["OpenOrder"].ToString() == "True" ? true : false);

                        /* 2021-04-30 yew mun add in legal number parameter  */
                        oRecDtl.LegalNumber = row["LegalNumber"].ToString();
                        oRecDtl.PORelPlant = row["Plant"].ToString();
                        oRecDtl.PORelWarehouse = row["WarehouseCode"].ToString();

                        /* 2021-07-12 yew mun add in the balance qty   */
                        oRecDtl.ReceivedQty = decimal.Parse(row["ReceivedQty"].ToString());
                        oRecDtl.ArrivedQty = decimal.Parse(row["ArrivedQty"].ToString());
                        oRecDtl.POQty = decimal.Parse(row["RelQty"].ToString());
                        oRecDtl.BalancedQty = oRecDtl.POQty - oRecDtl.ArrivedQty;

                        oRecDtl.Confirmed = (row["Confirmed"].ToString() == "True" ? true : false);
                        oRecDtl.Approve = (row["Approve"].ToString() == "True" ? true : false);

                        oRecDtl.PODate = Convert.ToDateTime((row["OrderDate"]));

                        oRecvDtl.Add(oRecDtl);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "PO Receipts listing not found ";
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






        public bool _LoadReceiptsById(ref EpicEnv oEpicEnv, string strCompany, int iVendNum, string strPackSlip,ref ReceiptHead oRecvHead, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Packslip, ponum ";
                _strSQL += "from erp.RcvHead ";
                _strSQL += "where company = '{0}' and VendorNum = {1} and packslip = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, iVendNum, strPackSlip);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oRecvHead.Company = row["Company"].ToString();
                        oRecvHead.PackSlip = row["Packslip"].ToString();
                        oRecvHead.PONum =int.Parse( row["ponum"].ToString());
                    }

                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "PO Receipts : " + strPackSlip + " not found";
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

        public bool _LoadReceiptsById2(ref EpicEnv oEpicEnv, string strCompany, int iPONum, string strPackSlip, string strLegalNumber, ref ReceiptHead oRecvHead, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Packslip, ponum, LegalNumber ";
                _strSQL += "from erp.RcvHead ";
                _strSQL += "where company = '{0}'  ";
                

                if (iPONum != 0)
                {
                    _strSQL += "and PONum = " + iPONum;
                }

                if (strPackSlip != "" && strPackSlip != null)
                {
                    _strSQL += " and packslip = '" + strPackSlip + "' ";
                }

                if (strLegalNumber != "" && strLegalNumber != null)
                {
                    _strSQL += " and LegalNumber = '" + strLegalNumber + "' ";
                }

                _strSQL = string.Format(_strSQL, strCompany, iPONum, strPackSlip);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oRecvHead.Company = row["Company"].ToString();
                        oRecvHead.PackSlip = row["Packslip"].ToString();
                        oRecvHead.PONum = int.Parse(row["ponum"].ToString());
                        oRecvHead.LegalNumber = row["LegalNumber"].ToString();
                    }

                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Unable to find Packslip :" + strPackSlip + " with PO Num:" + iPONum.ToString();
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                IsError = true;
            }


            return (IsError ? false : true);

        }


        public bool _LoadPurchaseOrders(ref EpicEnv oEpicEnv, string strCompany, int iPONum, string sBeginDate, string sEndDate, string strVendorId, string strVendorName, string strBuyer, ref IList<PurchaseOrder> oPOList, string strLegalNumber, out string strMessage)
        {
            bool IsError = false;

            try
            {
                /* 2021-04-30 yew mun add in legal number parameter  */

                string _strSQL = "select p.Company, p.PONum, p.OrderDate, p.ShipViaCode, p.TermsCode, p.BuyerID, p.PurPoint, p.CurrencyCode, p.Approve, p.DocTotalOrder "; 
                _strSQL += ", v.VendorID, v.Name, p.LegalNumber, p.Confirmed ";
                _strSQL += "from erp.POHeader p ";
                _strSQL += "left join erp.Vendor v on p.Company = v.Company and p.VendorNum = v.VendorNum ";

                _strSQL += "Where p.Company = '" + strCompany + "' ";

                if (iPONum != 0)
                {
                    _strSQL += "AND p.PONum = " + iPONum + " ";
                }

                if (sBeginDate != null)
                {
                    _strSQL += "AND p.OrderDate >= '" + sBeginDate + "' ";
                }

                if (sEndDate != null)
                {
                    _strSQL += "AND p.OrderDate <= '" + sEndDate + "' ";
                }

                if (strVendorId != null)
                {
                    _strSQL += "AND v.VendorID = '" + strVendorId + "' ";
                }

                if (strVendorName != null)
                {
                    _strSQL += "AND v.Name like '" + strVendorName + "%' ";
                }

                if (strBuyer != null)
                {
                    _strSQL += "AND p.BuyerID = '" + strBuyer + "' ";
                }

                /* 2021-04-30 yew mun add in legal number parameter  */
                if (strLegalNumber != null)
                {
                    _strSQL += "AND p.LegalNumber like '" + strLegalNumber + "%' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PurchaseOrder oPurOrd = new PurchaseOrder();
                        oPurOrd.Company = row["Company"].ToString();
                        oPurOrd.PONum = int.Parse(row["PONum"].ToString());
                        oPurOrd.Approve = row["Approve"].ToString() == "True" ? true : false;
                        oPurOrd.Buyer = (row["BuyerID"].ToString());
                        oPurOrd.CurrencyCode = row["CurrencyCode"].ToString();
                        oPurOrd.DocTotalOrder = decimal.Parse(row["DocTotalOrder"].ToString());
                        oPurOrd.PODate = Convert.ToDateTime((row["OrderDate"])); 
                        oPurOrd.PurPoint = (row["PurPoint"].ToString());
                        oPurOrd.ShipViaCode = row["ShipViaCode"].ToString();
                        oPurOrd.TermsCode = row["TermsCode"].ToString();
                        oPurOrd.VendorId = (row["VendorId"].ToString());
                        oPurOrd.VendorName = (row["Name"].ToString());

                        /* 2021-04-30 yew mun add in legal number parameter  */
                        oPurOrd.LegalNumber = (row["LegalNumber"].ToString());
                        oPurOrd.Confirmed = row["Confirmed"].ToString() == "True" ? true : false;

                        oPOList.Add(oPurOrd);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "PO listing not found ";
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


        public bool _LoadPurchaseOrdersByLegalId(ref EpicEnv oEpicEnv, string strCompany, int iPONum,  ref IList<PurchaseOrder> oPOList, string strLegalNumber, out string strMessage)
        {
            bool IsError = false;

            try
            {
                /* 2021-04-30 yew mun add in legal number parameter  */

                string _strSQL = "select p.Company, p.PONum, p.OrderDate, p.ShipViaCode, p.TermsCode, p.BuyerID, p.PurPoint, p.CurrencyCode, p.Approve, p.DocTotalOrder ";
                _strSQL += ", v.VendorID, v.Name, p.LegalNumber, p.Confirmed ";
                _strSQL += "from erp.POHeader p ";
                _strSQL += "left join erp.Vendor v on p.Company = v.Company and p.VendorNum = v.VendorNum ";

                _strSQL += "Where p.Company = '" + strCompany + "' ";

                if (iPONum != 0)
                {
                    _strSQL += "AND p.PONum = " + iPONum + " ";
                }


                /* 2021-04-30 yew mun add in legal number parameter  */
                if (strLegalNumber != null)
                {
                    _strSQL += "AND p.LegalNumber = '" + strLegalNumber + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PurchaseOrder oPurOrd = new PurchaseOrder();
                        oPurOrd.Company = row["Company"].ToString();
                        oPurOrd.PONum = int.Parse(row["PONum"].ToString());
                        oPurOrd.Approve = row["Approve"].ToString() == "True" ? true : false;
                        oPurOrd.Buyer = (row["BuyerID"].ToString());
                        oPurOrd.CurrencyCode = row["CurrencyCode"].ToString();
                        oPurOrd.DocTotalOrder = decimal.Parse(row["DocTotalOrder"].ToString());
                        oPurOrd.PODate = Convert.ToDateTime((row["OrderDate"]));
                        oPurOrd.PurPoint = (row["PurPoint"].ToString());
                        oPurOrd.ShipViaCode = row["ShipViaCode"].ToString();
                        oPurOrd.TermsCode = row["TermsCode"].ToString();
                        oPurOrd.VendorId = (row["VendorId"].ToString());
                        oPurOrd.VendorName = (row["Name"].ToString());

                        /* 2021-04-30 yew mun add in legal number parameter  */
                        oPurOrd.LegalNumber = (row["LegalNumber"].ToString());
                        oPurOrd.Confirmed = row["Confirmed"].ToString() == "True" ? true : false;

                        oPOList.Add(oPurOrd);
                    }
                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "PO listing not found ";
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