using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class EpicUserBO
    {
        public bool _VerifyEpicorLogin(ref EpicEnv oEpicEnv, ref EpicUser oEpicUser, out string strMessage)
        {
            bool IsLoginSuccess;

            EpicorBO oEpicor = new EpicorBO();
            IsLoginSuccess = oEpicor._LoginIntoEpicor(ref oEpicUser,ref oEpicEnv,out strMessage);

            //oEpicor.Dispose(true);

            return IsLoginSuccess;
        }

        public bool _LoadEpicUserByUserId(string strEpicUserId, ref EpicEnv oEpicEnv, ref EpicUser oEpicUser, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT UserID, Name, CurComp, CompList FROM Ice.SysUserFile ";
                _strSQL += string.Format("WHERE UserID = '{0}' ", strEpicUserId);

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oEpicUser.Epic_UserId = row["UserID"].ToString();
                        oEpicUser.Epic_UserName = row["Name"].ToString();
                        oEpicUser.Epic_Company = row["CompList"].ToString();
                        oEpicUser.Epic_CurCompany = row["CurComp"].ToString();
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "User not found. ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = "Invalid user ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadEpicCompanyByUserId(ref EpicEnv oEpicEnv, ref EpicUser oEpicUser,ref  IList<EpicCompany> lstEpicCompany,  out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT company, Name FROM erp.Company ";
                _strSQL = _strSQL + "WHERE Company IN ('" + oEpicUser.Epic_Company.Replace("~","','") + "') ";
                //_strSQL = _strSQL.Replace("~", "//");

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        EpicCompany _EpicCompany = new EpicCompany();

                        _EpicCompany.Company_Code = row["company"].ToString();
                        _EpicCompany.Company_Name = row["Name"].ToString();

                        lstEpicCompany.Add(_EpicCompany);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Company not found. ";
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                strMessage = "Company not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadEpicCompanyAndPlantByUserId(ref EpicEnv oEpicEnv, ref EpicUser oEpicUser, ref IList<EpicCompPlant> lstEpicCompPlant, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQLCompany = "SELECT company, Name FROM erp.Company ";
                _strSQLCompany = _strSQLCompany + "WHERE Company IN ('" + oEpicUser.Epic_Company.Replace("~", "','") + "') ";

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dtCompany = _MSSQL._MSSQLDataSetResult(_strSQLCompany, _strSQLCon);

                if (_dtCompany.Tables[0].Rows.Count > 0)
                {
                    

                    foreach (DataRow row in _dtCompany.Tables[0].Rows)
                    {
                        EpicCompPlant oCompPlant = new EpicCompPlant();

                        EpicCompany oCompany = new EpicCompany();
                        IList<Plant> oPlantList = new List<Plant>();

                        oCompany.Company_Code = row["company"].ToString();
                        oCompany.Company_Name = row["Name"].ToString();
                   
                        oCompPlant.oEpicCompany = oCompany;

                        string _strSQLPlant = "SELECT company, Plant, Name FROM erp.Plant ";
                        _strSQLPlant += string.Format("WHERE Company = '{0}' ", row["company"].ToString());

                        DataSet _dtPlant = _MSSQL._MSSQLDataSetResult(_strSQLPlant, _strSQLCon);

                        if (_dtPlant.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow plantrow in _dtPlant.Tables[0].Rows)
                            {
                                Plant oPlant = new Plant();
                                oPlant.Company = plantrow["Company"].ToString();
                                oPlant.Name = plantrow["name"].ToString();
                                oPlant.SitePlant = plantrow["plant"].ToString();

                                oPlantList.Add(oPlant);
                            }

                            oCompPlant.oEpicCompPlant = oPlantList;

                        }

                        lstEpicCompPlant.Add(oCompPlant);
                    } // end for foreach loop
                    
                    strMessage = " ";
                    IsError = false;

                }
                else
                {
                    strMessage = "Company not found ";
                    IsError = true;
                } // end if for _dtcompany


            }
            catch (Exception ex)
            {
                strMessage = "Company not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }



        //public void Dispose(bool v)
        //{
        //    throw new NotImplementedException();
        //}

    }
}