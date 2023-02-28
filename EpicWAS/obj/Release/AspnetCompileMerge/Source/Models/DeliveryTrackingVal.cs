using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class DeliveryTrackingVal
    {
        public bool _GetPackSlip(ref EpicEnv oEpicEnv, string strCompany, string strLegalNumber, ref CustShipHead oCustShipHead, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, PackNum, CustNum ";
                _strSQL += "FROM erp.ShipHead ";
                _strSQL += "WHERE Company = '{0}' and LegalNumber = '{1}' ";
                _strSQL = string.Format(_strSQL, strCompany, strLegalNumber);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oCustShipHead.Company = row["Company"].ToString();
                        oCustShipHead.Plant = row["Plant"].ToString();
                        oCustShipHead.DO = Convert.ToInt32(row["PackNum"]);
                        oCustShipHead.CustNum = Convert.ToInt32(row["CustNum"]);

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "PackSlip not found. ";
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