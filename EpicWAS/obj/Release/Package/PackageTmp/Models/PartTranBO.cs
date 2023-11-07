using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class PartTranBO
    {
        public bool _GetTranInfo (ref EpicEnv oEpicEnv, string strCompany,  ref PartTran oPartTran, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, SysDate, TranNum, TranType, PartNum, TranQty, WarehouseCode, BinNum, Warehouse2, BinNum2, LotNum, UM, EntryPerson, JobNum, AssemblySeq ";
                _strSQL += "FROM erp.PartTran ";
                _strSQL += "WHERE Company = '{0}' and TranNum = '{1}' ";
                _strSQL = string.Format(_strSQL, strCompany, oPartTran.TranNo);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oPartTran.Company = row["Company"].ToString();
                        oPartTran.SysDate = Convert.ToDateTime((row["SysDate"])).ToString("yyyy-MM-dd");
                        oPartTran.PartNum = row["PartNum"].ToString();
                        oPartTran.LotNum = row["LotNum"].ToString();
                        oPartTran.UOM = row["UM"].ToString();
                        oPartTran.JobNum = row["JobNum"].ToString();
                        oPartTran.AssemblySeq = Convert.ToInt32(row["AssemblySeq"]);
                        oPartTran.EntryPerson = row["EntryPerson"].ToString();

                        if (row["TranType"].ToString() == "STK-STK" && Convert.ToDecimal(row["TranQty"]) < 0)
                        {
                            oPartTran.TranType = row["TranType"].ToString();
                            oPartTran.TranQty = Math.Abs(Convert.ToDecimal(row["TranQty"]));
                            oPartTran.WarehouseCode = row["Warehouse2"].ToString();
                            oPartTran.BinNum = row["BinNum2"].ToString();
                        }
                        else if (row["TranType"].ToString() == "STK-MTL" && Convert.ToDecimal(row["TranQty"]) < 0)
                        {
                            oPartTran.TranType = "MTL-STK";
                            oPartTran.TranQty = Math.Abs(Convert.ToDecimal(row["TranQty"]));
                            oPartTran.WarehouseCode = row["WarehouseCode"].ToString();
                            oPartTran.BinNum = row["BinNum"].ToString();
                        }
                        else
                        {
                            oPartTran.TranType = row["TranType"].ToString();
                            oPartTran.TranQty = Convert.ToDecimal(row["TranQty"]);
                            oPartTran.WarehouseCode = row["WarehouseCode"].ToString();
                            oPartTran.BinNum = row["BinNum"].ToString();
                            
                        }
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "TranNum not Found. ";
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
    }
    
}