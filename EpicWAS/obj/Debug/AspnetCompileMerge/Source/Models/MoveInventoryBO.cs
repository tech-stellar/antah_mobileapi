using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class MoveInventoryBO
    {
        public bool _LoadMoveInvTrx(ref EpicEnv oEpicEnv, string strCompany, int iTrxStatus, ref IList<MoveInventory> oMoveInvList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT * FROM dbo.Ext_MovInvReq ";
                _strSQL += "WHERE Company = '{0}' and ReqStatus = '{1}'  ";
                _strSQL = string.Format(_strSQL, strCompany, iTrxStatus);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        MoveInventory oMoveInventory = new MoveInventory();
                        oMoveInventory.Company = row["Company"].ToString();
                        oMoveInventory.PartNum = row["PartNum"].ToString();
                        oMoveInventory.IUM = row["IUM"].ToString();
                        oMoveInventory.TranQty = Decimal.Parse(row["TranQty"].ToString());
                        oMoveInventory.IUM = row["IUM"].ToString();
                        oMoveInventory.Reference = "";

                        oMoveInventory.FromWarehouseCode = row["FromWhse"].ToString();
                        oMoveInventory.FromLotNum = row["FromLot"].ToString();
                        oMoveInventory.FromBinNum = row["FromBin"].ToString();

                        oMoveInventory.ToWarehouseCode = row["ToWhse"].ToString();
                        oMoveInventory.ToLotNum = row["ToLot"].ToString();
                        oMoveInventory.ToBinNum = row["ToBin"].ToString();

                        oMoveInventory.UpdBy = row["updBy"].ToString();
                        oMoveInventory.UpdDate = DateTime.Parse( row["UpdDate"].ToString());

                        oMoveInventory.ReqNum = row["ReqNum"].ToString();
                        oMoveInventory.ReqBy = row["ReqBy"].ToString();
                        oMoveInventory.ReqDate =DateTime.Parse( row["ReqDate"].ToString());
                        
                        oMoveInventory.ReqStatus =int.Parse( row["ReqStatus"].ToString());

                        oMoveInvList.Add(oMoveInventory);

                    }

                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Move Inventory transactions not found. ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = "Move Inventory transactions not found ";
                IsError = true;
            }


            return (IsError ? false : true);
        }


        public bool _LoadMoveInvTrxById(ref EpicEnv oEpicEnv, string strCompany, string strReqNum, ref MoveInventory oMoveInventory, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT * FROM dbo.Ext_MovInvReq ";
                _strSQL += "WHERE Company = '{0}' and ReqNum = '{1}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strReqNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oMoveInventory.Company = row["Company"].ToString();
                        oMoveInventory.PartNum = row["PartNum"].ToString();
                        oMoveInventory.IUM = row["IUM"].ToString();
                        oMoveInventory.TranQty = Decimal.Parse(row["TranQty"].ToString());
                        oMoveInventory.IUM = row["IUM"].ToString();
                        oMoveInventory.Reference = "";

                        oMoveInventory.FromWarehouseCode = row["FromWhse"].ToString();
                        oMoveInventory.FromLotNum = row["FromLot"].ToString();
                        oMoveInventory.FromBinNum = row["FromBin"].ToString();

                        oMoveInventory.ToWarehouseCode = row["ToWhse"].ToString();
                        oMoveInventory.ToLotNum = row["ToLot"].ToString();
                        oMoveInventory.ToBinNum = row["ToBin"].ToString();

                        oMoveInventory.UpdBy = row["updBy"].ToString();
                        oMoveInventory.UpdDate = DateTime.Parse(row["UpdDate"].ToString());

                        oMoveInventory.ReqNum = row["ReqNum"].ToString();
                        oMoveInventory.ReqBy = row["ReqBy"].ToString();
                        oMoveInventory.ReqDate = DateTime.Parse(row["ReqDate"].ToString());

                        oMoveInventory.ReqStatus = int.Parse(row["ReqStatus"].ToString());

                    }

                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Move Inventory transactions not found. ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = "Move Inventory transactions not found ";
                IsError = true;
            }


            return (IsError ? false : true);
        }


        public bool _NewMoveInvTrxReq(ref EpicEnv oEpicEnv, ref MoveInventory oMoveInventory, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "INSERT INTO Ext_MovInvReq (";
                _strSQL += "Company, ReqNum, PartNum,  ";
                _strSQL += "IUM, TranQty, FromWhse, FromBin, FromLot, ";
                _strSQL += "ToWhse, ToBin, ToLot, Ref, ReqStatus,  ";
                _strSQL += "ReqBy, ReqDate, UpdBy, UpdDate ";
                _strSQL += ") VALUES ( ";
                _strSQL += "'{0}','{1}', '{2}',";
                _strSQL += "'{3}', '{4}', '{5}', '{6}','{7}', ";
                _strSQL += "'{8}', '{9}', '{10}','{11}', '{12}' ";
                _strSQL += "'{13}', '{14}', '{15}', '{16}' )";
                _strSQL += "";
                _strSQL = string.Format(_strSQL, oMoveInventory.Company, oMoveInventory.ReqNum, oMoveInventory.PartNum,
                                                  oMoveInventory.IUM, oMoveInventory.TranQty, oMoveInventory.FromWarehouseCode, oMoveInventory.FromBinNum, oMoveInventory.FromLotNum,
                                                  oMoveInventory.ToWarehouseCode, oMoveInventory.ToBinNum, oMoveInventory.ToLotNum, oMoveInventory.Reference, oMoveInventory.ReqStatus,
                                                  oMoveInventory.ReqBy, oMoveInventory.ReqDate, oMoveInventory.UpdBy, oMoveInventory.UpdDate);

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                if (_MSSQL._exeSQLCommand(_strSQL, _strSQLCon))
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Module: _NewMoveInvTrxReq. Error. Not able to insert into database.";
                    IsError = true;
                }

                // _sqlite.Dispose(true);

            }
            catch (Exception ex)
            {
                strMessage = "Module: _NewEpicEnv. " + ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _UpdMoveInvTrxStatus(ref EpicEnv oEpicEnv, ref MoveInventory oMoveInventory, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "UPDATE Ext_MovInvReq ";
                _strSQL += "SET ReqStatus = {0} ";
                _strSQL += "WHERE Company ='{1}' and ReqNum = '{2}' ";
                _strSQL = string.Format(_strSQL, oMoveInventory.ReqStatus, oMoveInventory.Company, oMoveInventory.ReqNum);

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                if (_MSSQL._exeSQLCommand(_strSQL, _strSQLCon))
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Module: _UpdMoveInvTrxStatus. Error. Not able to update into database.";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = "Module: _UpdMoveInvTrxStatus. " + ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }





    }
}