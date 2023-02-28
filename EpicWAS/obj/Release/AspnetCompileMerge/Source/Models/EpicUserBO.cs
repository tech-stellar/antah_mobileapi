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
                string _strSQL = "SELECT a.UserID, a.Name, a.CurComp, a.CompList, b.curplant, ";
                _strSQL += "sud.*,c.Name as companyname, p.Name as plantname, z.EmpID ";
                _strSQL += "FROM Ice.SysUserFile a ";
                _strSQL += "inner join ice.SysUserFile_UD sud on a.SysRowID = sud.foreignsysrowid ";
                _strSQL += "inner join ice.SysUserComp b on a.userid = b.userid and a.CurComp = b.Company ";
                _strSQL += "left join erp.company c on c.company = a.CurComp ";
                _strSQL += "left join erp.Plant p on p.Plant = b.CurPlant and p.Company = a.CurComp ";
                _strSQL += "left join erp.UserComp z on a.UserID  = z.DcdUserID and a.CurComp = z.Company ";
                _strSQL += string.Format("WHERE a.UserID = '{0}' ", strEpicUserId);

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

                        oEpicUser.Epic_Plant = row["CurPlant"].ToString();
                        oEpicUser.Epic_CurCompanyName = row["companyname"].ToString();
                        oEpicUser.Epic_PlantName = row["plantname"].ToString();
                        //COMMENTED DUE TO RETURN DATATYPE IS IN BOOLEAN (TRUE / FALSE) NOT 0 / 1
                        /*oEpicUser.IsEnable_JobReceipts = (row["IsEnable_JobReceipts_c"].ToString() == "1"? true : false);
                        oEpicUser.IsEnable_MiscIssue = (row["IsEnable_MiscIssue_c"].ToString() == "1" ? true : false);
                        oEpicUser.IsEnable_MoveInventory = (row["IsEnable_MoveInventory_c"].ToString() == "1" ? true : false);
                        oEpicUser.IsEnable_MoveInventoryApproval = (row["IsEnable_MoveInventoryApproval_c"].ToString() == "1" ? true : false);
                        oEpicUser.IsEnable_MoveInventoryRequest = (row["IsEnable_MoveInventoryRequest_c"].ToString() == "1" ? true : false);
                        oEpicUser.IsEnable_POReceipts = (row["IsEnable_POReceipts_c"].ToString() == "1" ? true : false);
                        oEpicUser.IsEnable_ReturnMaterial = (row["IsEnable_ReturnMaterial_c"].ToString() == "1" ? true : false);
                        oEpicUser.IsEnable_SalvageReceipts = (row["IsEnable_SalvageReceipts_c"].ToString() == "1" ? true : false);*/
                        oEpicUser.IsEnable_JobReceipts = Convert.ToBoolean(row["IsEnable_JobReceipts_c"]);
                        oEpicUser.IsEnable_MiscIssue = Convert.ToBoolean(row["IsEnable_MiscIssue_c"]);
                        oEpicUser.IsEnable_MoveInventory = Convert.ToBoolean(row["IsEnable_MoveInventory_c"]);
                        oEpicUser.IsEnable_MoveInventoryApproval = Convert.ToBoolean(row["IsEnable_MoveInventoryApproval_c"]);
                        oEpicUser.IsEnable_MoveInventoryRequest = Convert.ToBoolean(row["IsEnable_MoveInventoryRequest_c"]);
                        oEpicUser.IsEnable_POReceipts = Convert.ToBoolean(row["IsEnable_POReceipts_c"]);
                        oEpicUser.IsEnable_ReturnMaterial = Convert.ToBoolean(row["IsEnable_ReturnMaterial_c"]);
                        oEpicUser.IsEnable_SalvageReceipts = Convert.ToBoolean(row["IsEnable_SalvageReceipts_c"]);
                        oEpicUser.IsEnable_IssueAssembly = Convert.ToBoolean(row["IsEnable_IssueAssembly_c"]);
                        oEpicUser.IsEnable_ReturnAssembly = Convert.ToBoolean(row["IsEnable_ReturnAssembly_c"]);
                        oEpicUser.IsEnable_DeliveryTrack = Convert.ToBoolean(row["IsEnable_DeliveryTrack_c"]);
                        oEpicUser.IsEnable_Reprint = Convert.ToBoolean(row["IsEnable_Reprint_c"]);
                        oEpicUser.IsEnable_SplitMergeUOM = Convert.ToBoolean(row["IsEnable_SplitMergeUOM_c"]);

                        oEpicUser.IsEnable_IssueMiscMaterial = Convert.ToBoolean(row["IsEnable_IssueMiscMaterial_c"]);
                        oEpicUser.IsEnable_ReturnMiscMaterial = Convert.ToBoolean(row["IsEnable_ReturnMiscMaterial_c"]);
                        oEpicUser.IsEnable_QtyAdjustment = Convert.ToBoolean(row["IsEnable_QtyAdjustment_c"]);

                        try { oEpicUser.IsEnable_StartEndOp = Convert.ToBoolean(row["IsEnable_StartEndOp_c"]); } catch { oEpicUser.IsEnable_StartEndOp = true; }
                        try { oEpicUser.IsEnable_ClockInOut = Convert.ToBoolean(row["IsEnable_IsEnable_ClockInOut_c"]); } catch { oEpicUser.IsEnable_ClockInOut = true; }
                        try { oEpicUser.IsEnable_ClockInOut = Convert.ToBoolean(row["IsEnable_IsEnable_WorkQueue_c"]); } catch { oEpicUser.IsEnable_WorkQueue = true; }

                        oEpicUser.IsEnable_UseDefaultLabelQty = Convert.ToBoolean(row["IsEnable_UseDefaultLabelQty_c"]);
                        oEpicUser.DefaultLabelQty = Int32.Parse(row["DefaultLabelQty_c"].ToString());
                        oEpicUser.Epic_EmpId = row["EmpID"].ToString();

                        oEpicUser.IsEnable_UserDefine01 = Convert.ToBoolean(row["IsEnable_UserDefine01_c"]);
                        oEpicUser.IsEnable_UserDefine02 = Convert.ToBoolean(row["IsEnable_UserDefine02_c"]);
                        oEpicUser.IsEnable_UserDefine03 = Convert.ToBoolean(row["IsEnable_UserDefine03_c"]);
                        oEpicUser.IsEnable_UserDefine04 = Convert.ToBoolean(row["IsEnable_UserDefine04_c"]);
                        oEpicUser.IsEnable_UserDefine05 = Convert.ToBoolean(row["IsEnable_UserDefine05_c"]);
                        oEpicUser.IsEnable_UserDefine06 = Convert.ToBoolean(row["IsEnable_UserDefine06_c"]);
                        oEpicUser.IsEnable_UserDefine07 = Convert.ToBoolean(row["IsEnable_UserDefine07_c"]);
                        oEpicUser.IsEnable_UserDefine08 = Convert.ToBoolean(row["IsEnable_UserDefine08_c"]);
                        oEpicUser.IsEnable_UserDefine09 = Convert.ToBoolean(row["IsEnable_UserDefine09_c"]);
                        oEpicUser.IsEnable_UserDefine10 = Convert.ToBoolean(row["IsEnable_UserDefine10_c"]);
                        oEpicUser.OrderType = row["OrderType_c"].ToString();

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
                strMessage = ex.Message.ToString();
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
                strMessage = ex.Message;
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
                strMessage = ex.Message; 
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadEpicCompanyPlantByCompanyId(ref EpicEnv oEpicEnv, ref EpicUser oEpicUser, string strCompany, ref IList<Plant> lstPlant, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQLPlant = "SELECT company, Plant, Name FROM erp.Plant ";
                _strSQLPlant += string.Format("WHERE Company = '{0}' ", strCompany);

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dtPlant = _MSSQL._MSSQLDataSetResult(_strSQLPlant, _strSQLCon);

                if (_dtPlant.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow plantrow in _dtPlant.Tables[0].Rows)
                    {
                        Plant oPlant = new Plant();
                        oPlant.Company = plantrow["Company"].ToString();
                        oPlant.Name = plantrow["name"].ToString();
                        oPlant.SitePlant = plantrow["plant"].ToString();

                        lstPlant.Add(oPlant);
                    }
                    strMessage = " ";
                    IsError = false;

                }
                else
                {
                    strMessage = "Plant not found ";
                    IsError = true;
                } // end if for _dtcompany

            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                IsError = true;
            }

            return (IsError ? false : true);

        }

        public bool _VerifyIMEI(ref EpicEnv oEpicEnv, ref EpicUser oEpicUser, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM dbo.Ext_IMEI ";
                _strSQL = _strSQL + "WHERE IMEI = '" + oEpicUser.IMEI + "' and IsActive = 1";
                //_strSQL = _strSQL.Replace("~", "//");

                SQLServerBO _MSSQL = new SQLServerBO();

                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    // imei not exist then auto add in the imei into table
                    string _strInsertImei = "insert into Ext_IMEI (IMEI, IsActive) values ('" + oEpicUser.IMEI + "' ,1) ";

                    bool IsImeiRegistered = false;

                    IsImeiRegistered = _MSSQL._exeSQLCommand(_strInsertImei, _strSQLCon);

                    if (IsImeiRegistered == false)
                    {
                        strMessage = "IMEI is not register";
                        IsError = true;
                    }
                    else
                    {
                        strMessage = "";
                        IsError = false;
                    }


                    
                }

            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
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