using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class WarehseBO
    {
        public bool _LoadWhseById(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strWhseCode, ref Warehse oWarehse, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, WarehouseCode, Description FROM erp.Warehse ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' and WarehouseCode = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant, strWhseCode);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oWarehse.Company = row["Company"].ToString();
                        oWarehse.Plant = row["Plant"].ToString();
                        oWarehse.WarehouseCode = row["WarehouseCode"].ToString();
                        oWarehse.Description = row["Description"].ToString();
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Warehouse not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Warehouse not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadWhseBinById(ref EpicEnv oEpicEnv, string strCompany, string strWhseCode, ref WhseBin oWhseBin, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, WarehouseCode, BinNum, Description FROM erp.WhseBin ";
                _strSQL += "WHERE Company = '{0}' and WarehouseCode = '{1}' ";
                _strSQL = string.Format(_strSQL, strCompany, strWhseCode);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oWhseBin.Company = row["Company"].ToString();
                        oWhseBin.BinNum = row["BinNum"].ToString();
                        oWhseBin.WarehouseCode = row["WarehouseCode"].ToString();
                        oWhseBin.Description = row["Description"].ToString();
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Warehouse Bin not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Warehouse Bin not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }




    }
}