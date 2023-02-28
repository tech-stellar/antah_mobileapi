using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class MaintenanceBO
    {

        public bool _LoadVendors(ref EpicEnv oEpicEnv, string strCompany, string strVendId, string strVendName, string strGroup,  ref IList<Vendor> Vendors, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company,  VendorId, Name, VendorNum, Inactive, groupcode, Address1, Address2, Address3, City, State, Zip, Country ";
                _strSQL += "FROM erp.Vendor ";
                _strSQL += "WHERE Company = '" + strCompany + "' ";

                if (strVendId != null)
                {
                    _strSQL += "AND VendorId Like '" + strVendId + "%' ";
                }

                if (strVendName != null)
                {
                    _strSQL += "AND Name like '" + strVendName + "%' ";
                }

                if (strGroup != null)
                {
                    _strSQL += "AND groupcode = '" + strGroup + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Vendor oVendor = new Vendor();
                        oVendor.Company = row["Company"].ToString();
                        oVendor.VendorId = row["VendorId"].ToString();
                        oVendor.VendorNum = Convert.ToInt32(row["VendorNum"]);
                        oVendor.Vendorname = row["Name"].ToString();
                        oVendor.GroupCode = row["groupcode"].ToString();
                        oVendor.InActive = Convert.ToBoolean(row["Inactive"]);

                        oVendor.Address1 = row["Address1"].ToString();
                        oVendor.Address2 = row["Address2"].ToString();
                        oVendor.Address3 = row["Address3"].ToString();
                        oVendor.City = row["City"].ToString();
                        oVendor.State = row["State"].ToString();
                        oVendor.Zip = row["Zip"].ToString();
                        oVendor.Country = row["Country"].ToString();

                        Vendors.Add(oVendor);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Supplier listing not found ";
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

        public bool _LoadVendorById(ref EpicEnv oEpicEnv, string strCompany, string strVendId,ref Vendor oVendor, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company,  VendorId, Name, VendorNum, Inactive, groupcode, Address1, Address2, Address3, City, State, Zip, Country ";
                _strSQL += "FROM erp.Vendor ";
                _strSQL += "WHERE Company = '" + strCompany + "' ";

                if (strVendId != null)
                {
                    _strSQL += "AND VendorId = '" + strVendId + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oVendor.Company = row["Company"].ToString();
                        oVendor.VendorId = row["VendorId"].ToString();
                        oVendor.VendorNum = Convert.ToInt32(row["VendorNum"]);
                        oVendor.Vendorname = row["Name"].ToString();
                        oVendor.GroupCode = row["groupcode"].ToString();
                        oVendor.InActive = Convert.ToBoolean(row["Inactive"]);

                        oVendor.Address1 = row["Address1"].ToString();
                        oVendor.Address2 = row["Address2"].ToString();
                        oVendor.Address3 = row["Address3"].ToString();
                        oVendor.City = row["City"].ToString();
                        oVendor.State = row["State"].ToString();
                        oVendor.Zip = row["Zip"].ToString();
                        oVendor.Country = row["Country"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Supplier not found ";
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

        public bool _LoadVendorByNum(ref EpicEnv oEpicEnv, string strCompany, int iVendNum, ref Vendor oVendor, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company,  VendorId, Name, VendorNum, Inactive, groupcode, Address1, Address2, Address3, City, State, Zip, Country ";
                _strSQL += "FROM erp.Vendor ";
                _strSQL += "WHERE Company = '" + strCompany + "' ";

                if (iVendNum != 0)
                {
                    _strSQL += "AND VendorNum = " + iVendNum + " ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oVendor.Company = row["Company"].ToString();
                        oVendor.VendorId = row["VendorId"].ToString();
                        oVendor.VendorNum = Convert.ToInt32(row["VendorNum"]);
                        oVendor.Vendorname = row["Name"].ToString();
                        oVendor.GroupCode = row["groupcode"].ToString();
                        oVendor.InActive = Convert.ToBoolean(row["Inactive"]);

                        oVendor.Address1 = row["Address1"].ToString();
                        oVendor.Address2 = row["Address2"].ToString();
                        oVendor.Address3 = row["Address3"].ToString();
                        oVendor.City = row["City"].ToString();
                        oVendor.State = row["State"].ToString();
                        oVendor.Zip = row["Zip"].ToString();
                        oVendor.Country = row["Country"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Supplier not found ";
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