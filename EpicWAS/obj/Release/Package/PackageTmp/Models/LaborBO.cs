using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class LaborBO
    {

        public bool _LoadWorkQueue(ref EpicEnv oEpicEnv, string strCompany, ref IList<WorkQueue> WorkQueueList, out string strMessage, string strEmpId = "", string strResourceGrp = "", string strJobNum = "", int iAssemblySeq = 0, int iOperation = 0, string strResourceId = "")
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT l.Company, l.ClockInDate, l.ClockIntime, l.JobNum, l.AssemblySeq, l.OprSeq, l.OpCode, l.ResourceGrpID, l.ResourceID, l.LaborType, l.ActiveTrans, l.EmployeeNum, l.LaborHedSeq, l.LaborDtlSeq, e.Name ";
                _strSQL += "FROM erp.LaborDtl l left join erp.EmpBasic e on  l.Company = e.Company and l.EmployeeNum = e.EmpID ";
                _strSQL += "WHERE l.ActiveTrans = 1 ";

                if (strEmpId != null && strEmpId != "")
                    { _strSQL += "AND l.EmployeeNum = '" + strEmpId  + "' ";  }

                if (strResourceGrp != null && strResourceGrp != "")
                    { _strSQL += "AND l.ResourceGrpID = '" + strResourceGrp + "' "; }

                if (strCompany != null && strCompany != "")
                    { _strSQL += "AND l.Company = '" + strCompany + "' "; }

                if (strJobNum != null && strJobNum != "")
                    { _strSQL += "AND l.JobNum = '" + strJobNum + "' "; }

                if (strResourceId != null && strResourceId != "")
                    { _strSQL += "AND l.ResourceID = '" + strResourceId + "' "; }

                _strSQL += "AND l.AssemblySeq = " + iAssemblySeq.ToString() + " ";

                if (iOperation != 0)
                    { _strSQL += "AND l.OprSeq = " + iOperation.ToString() + " "; }

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        WorkQueue oWorkQueue = new  WorkQueue();

                        oWorkQueue.Company = row["Company"].ToString();
                        oWorkQueue.JobNum = row["JobNum"].ToString();
                        oWorkQueue.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oWorkQueue.OprSeq = Int32.Parse(row["OprSeq"].ToString());
                        oWorkQueue.OpCode = row["OpCode"].ToString();
                        oWorkQueue.ResourceGrpID = row["ResourceGrpID"].ToString();
                        oWorkQueue.ResourceID = row["ResourceID"].ToString();
                        oWorkQueue.LaborType = row["LaborType"].ToString();
                        oWorkQueue.ActiveTrans = row["ActiveTrans"].ToString();
                        oWorkQueue.EmployeeNum = (row["EmployeeNum"].ToString());
                        oWorkQueue.ClockInDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row["ClockInDate"].ToString()));// (row["ClockInDate"].ToString("dd/MM/yyyy"));
                        oWorkQueue.ClockIntime = (row["ClockIntime"].ToString());
                        oWorkQueue.LaborHedSeq = Int32.Parse(row["LaborHedSeq"].ToString());
                        oWorkQueue.LaborDtlSeq = Int32.Parse(row["LaborDtlSeq"].ToString());
                        oWorkQueue.EmployeeName = row["Name"].ToString();

                        WorkQueueList.Add(oWorkQueue);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No Work Queue found ";
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

        public bool _LoadWorkQueueExceptSup(ref EpicEnv oEpicEnv, string strCompany, ref IList<WorkQueue> WorkQueueList, out string strMessage, string strEmpId = "", string strResourceGrp = "", string strJobNum = "", int iAssemblySeq = 0, int iOperation = 0, string strResourceId = "")
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT l.Company, l.ClockInDate, l.ClockIntime, l.JobNum, l.AssemblySeq, l.OprSeq, l.OpCode, l.ResourceGrpID, l.ResourceID, l.LaborType, l.ActiveTrans, l.EmployeeNum, l.LaborHedSeq, l.LaborDtlSeq, e.Name ";
                _strSQL += "FROM erp.LaborDtl l left join erp.EmpBasic e on  l.Company = e.Company and l.EmployeeNum = e.EmpID ";
                _strSQL += "WHERE l.ActiveTrans = 1 ";

                if (strEmpId != null && strEmpId != "")
                { _strSQL += "AND l.EmployeeNum <> '" + strEmpId + "' "; }

                if (strResourceGrp != null && strResourceGrp != "")
                { _strSQL += "AND l.ResourceGrpID = '" + strResourceGrp + "' "; }

                if (strCompany != null && strCompany != "")
                { _strSQL += "AND l.Company = '" + strCompany + "' "; }

                if (strJobNum != null && strJobNum != "")
                { _strSQL += "AND l.JobNum = '" + strJobNum + "' "; }

                if (strResourceId != null && strResourceId != "")
                { _strSQL += "AND l.ResourceID = '" + strResourceId + "' "; }

                _strSQL += "AND l.AssemblySeq = " + iAssemblySeq.ToString() + " ";

                if (iOperation != 0)
                { _strSQL += "AND l.OprSeq = " + iOperation.ToString() + " "; }

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        WorkQueue oWorkQueue = new WorkQueue();

                        oWorkQueue.Company = row["Company"].ToString();
                        oWorkQueue.JobNum = row["JobNum"].ToString();
                        oWorkQueue.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oWorkQueue.OprSeq = Int32.Parse(row["OprSeq"].ToString());
                        oWorkQueue.OpCode = row["OpCode"].ToString();
                        oWorkQueue.ResourceGrpID = row["ResourceGrpID"].ToString();
                        oWorkQueue.ResourceID = row["ResourceID"].ToString();
                        oWorkQueue.LaborType = row["LaborType"].ToString();
                        oWorkQueue.ActiveTrans = row["ActiveTrans"].ToString();
                        oWorkQueue.EmployeeNum = (row["EmployeeNum"].ToString());
                        oWorkQueue.ClockInDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row["ClockInDate"].ToString()));// (row["ClockInDate"].ToString("dd/MM/yyyy"));
                        oWorkQueue.ClockIntime = (row["ClockIntime"].ToString());
                        oWorkQueue.LaborHedSeq = Int32.Parse(row["LaborHedSeq"].ToString());
                        oWorkQueue.LaborDtlSeq = Int32.Parse(row["LaborDtlSeq"].ToString());
                        oWorkQueue.EmployeeName = row["Name"].ToString();

                        WorkQueueList.Add(oWorkQueue);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No Work Queue found ";
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

        public bool _LoadCurrentActiveEmployeeByEmpId(ref EpicEnv oEpicEnv, string strCompany, string strEmployeeNum, ref ActiveEmployee oActiveEmployee, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, EmployeeNum, LaborHedSeq, Shift, PayrollDate, ClockInDate, ClockInTime ";
                _strSQL += "FROM erp.LaborHed ";
                _strSQL += "WHERE ActiveTrans = 1 ";

                if (strEmployeeNum != null && strEmployeeNum != "")
                { _strSQL += "AND EmployeeNum = '" + strEmployeeNum + "' "; }

                if (strCompany != null && strCompany != "")
                { _strSQL += "AND Company = '" + strCompany + "' "; }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oActiveEmployee.Company = row["Company"].ToString();
                        oActiveEmployee.EmployeeNum = row["EmployeeNum"].ToString();
                        oActiveEmployee.LaborHedSeq = Int32.Parse(row["LaborHedSeq"].ToString());
                        oActiveEmployee.Shift = Int32.Parse(row["Shift"].ToString());
                        oActiveEmployee.PayrollDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row["PayrollDate"].ToString()));
                        oActiveEmployee.ClockInDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row["ClockInDate"].ToString()));
                        oActiveEmployee.ClockInTime = row["ClockIntime"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No active employee for the id's ";
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

        public bool _LoadCurrentActiveEmployee(ref EpicEnv oEpicEnv, string strCompany, ref IList<ActiveEmployee> ActiveEmployeeList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT l.Company, l.EmployeeNum, l.LaborHedSeq, l.Shift, l.PayrollDate, l.ClockInDate, l.ClockInTime, e.Name ";
                _strSQL += "FROM erp.LaborHed l left join erp.EmpBasic e on l.Company = e.Company and l.EmployeeNum = e.EmpID ";
                _strSQL += "WHERE l.ActiveTrans = 1 ";

                if (strCompany != null && strCompany != "")
                { _strSQL += "AND l.Company = '" + strCompany + "' "; }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        ActiveEmployee oActiveEmployee = new ActiveEmployee();

                        oActiveEmployee.Company = row["Company"].ToString();
                        oActiveEmployee.EmployeeNum = row["EmployeeNum"].ToString();
                        oActiveEmployee.LaborHedSeq = Int32.Parse(row["LaborHedSeq"].ToString());
                        oActiveEmployee.Shift = Int32.Parse(row["Shift"].ToString());
                        oActiveEmployee.PayrollDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row["PayrollDate"].ToString()));
                        oActiveEmployee.ClockInDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(row["ClockInDate"].ToString()));
                        oActiveEmployee.ClockInTime = row["ClockIntime"].ToString();
                        oActiveEmployee.EmployeeName = row["Name"].ToString();

                        ActiveEmployeeList.Add(oActiveEmployee);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No active employee found";
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


        public bool _LoadReasonCodes(ref EpicEnv oEpicEnv, string strCompany, string strReasonType, ref IList<ReasonCodes> ReasonCodesList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT company, ReasonType, ReasonCode, Description ";
                _strSQL += "FROM erp.Reason  ";

                string _strWHERE ="";


                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND Company = '" + strCompany + "' "; }

                if (strReasonType != null && strReasonType != "")
                { _strWHERE += "AND ReasonType = '" + strReasonType + "' "; }

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
                        ReasonCodes oReasonCode = new ReasonCodes();

                        oReasonCode.Company = row["Company"].ToString();
                        oReasonCode.ReasonType = row["ReasonType"].ToString();
                        oReasonCode.ReasonCode = row["ReasonCode"].ToString();
                        oReasonCode.Description = row["Description"].ToString();

                        ReasonCodesList.Add(oReasonCode);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No reason code found";
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


        public bool _LoadResourceByJobOperation(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, int iAssemblySeq, int iOperation, ref IList<JobResources> ResourcesList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select jo.Company, jo.JobNum, jo.AssemblySeq, jo.OprSeq, jo.OpCode, om.ResourceGrpID, r.ResourceID, r.Description as resourcedesc, h.Plant , rg.Description as resourcegrp ";
                _strSQL += "from erp.JobOper jo ";
                _strSQL += "left join erp.OpMasDtl om on jo.Company = om.Company and jo.OpCode = om.OpCode and om.PrimaryProd = 1 ";
                _strSQL += "left join erp.Resource r on jo.Company = r.Company and om.ResourceGrpID = r.ResourceGrpID ";
                _strSQL += "left join erp.JobHead h on jo.Company = h.Company and jo.JobNum = h.JobNum  ";
                _strSQL += "left join erp.ResourceGroup rg on jo.Company = rg.Company and om.ResourceGrpID = rg.ResourceGrpID and h.Plant = rg.Plant ";


                string _strWHERE = "";

                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND jo.Company = '" + strCompany + "' "; }

                if (strJobNum != null && strJobNum != "")
                { _strWHERE += "AND jo.jobnum = '" + strJobNum + "' "; }

                if (iAssemblySeq != 0)
                { _strWHERE += "AND jo.AssemblySeq = " + iAssemblySeq.ToString() + " "; }

                if (iOperation != 0)
                { _strWHERE += "AND jo.OprSeq = " + iOperation.ToString() + " "; }

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
                        JobResources oResource = new JobResources();

                        oResource.Company = row["Company"].ToString();
                        oResource.JobNum = row["JobNum"].ToString();
                        oResource.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oResource.OperationSeq = Int32.Parse(row["OprSeq"].ToString());
                        oResource.ResourceDescription = row["resourcedesc"].ToString();
                        oResource.ResourceId = row["ResourceID"].ToString();
                        oResource.ResourceGroupDescription = row["resourcegrp"].ToString();
                        oResource.ResourceGroupId = row["ResourceGrpID"].ToString();
                        oResource.Opcode = row["OpCode"].ToString();
                        oResource.Plant = row["Plant"].ToString();

                        ResourcesList.Add(oResource);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No resources found";
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

        public bool _LoadResourceByJobOperationById(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, int iAssemblySeq, int iOperation, string strResourceId,  ref JobResources oJobResources, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select jo.Company, jo.JobNum, jo.AssemblySeq, jo.OprSeq, jo.OpCode, om.ResourceGrpID, r.ResourceID, r.Description as resourcedesc, h.Plant , rg.Description as resourcegrp ";
                _strSQL += "from erp.JobOper jo ";
                _strSQL += "left join erp.OpMasDtl om on jo.Company = om.Company and jo.OpCode = om.OpCode and om.PrimaryProd = 1 ";
                _strSQL += "left join erp.Resource r on jo.Company = r.Company and om.ResourceGrpID = r.ResourceGrpID ";
                _strSQL += "left join erp.JobHead h on jo.Company = h.Company and jo.JobNum = h.JobNum  ";
                _strSQL += "left join erp.ResourceGroup rg on jo.Company = rg.Company and om.ResourceGrpID = rg.ResourceGrpID and h.Plant = rg.Plant ";


                string _strWHERE = "";

                if (strCompany != null && strCompany != "")
                { _strWHERE += "AND jo.Company = '" + strCompany + "' "; }

                if (strJobNum != null && strJobNum != "")
                { _strWHERE += "AND jo.jobnum = '" + strJobNum + "' "; }

                if (iAssemblySeq != 0)
                { _strWHERE += "AND jo.AssemblySeq = " + iAssemblySeq.ToString() + " "; }

                if (iOperation != 0)
                { _strWHERE += "AND jo.OprSeq = " + iOperation.ToString() + " "; }

                if (strResourceId != null && strResourceId != "")
                { _strWHERE += "AND r.ResourceID = '" + strResourceId + "' "; }

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
                        //JobResources oResource = new JobResources();

                        oJobResources.Company = row["Company"].ToString();
                        oJobResources.JobNum = row["JobNum"].ToString();
                        oJobResources.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJobResources.OperationSeq = Int32.Parse(row["OprSeq"].ToString());
                        oJobResources.ResourceDescription = row["resourcedesc"].ToString();
                        oJobResources.ResourceId = row["ResourceID"].ToString();
                        oJobResources.ResourceGroupDescription = row["resourcegrp"].ToString();
                        oJobResources.ResourceGroupId = row["ResourceGrpID"].ToString();
                        oJobResources.Opcode = row["OpCode"].ToString();
                        oJobResources.Plant = row["Plant"].ToString();

                        //ResourcesList.Add(oResource);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No resources found";
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


        public bool _LoadEmployee(ref EpicEnv oEpicEnv, string strCompany, ref IList<Employee> EmployeeList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select Company, EmpID, Name, EmpStatus  ";
                _strSQL += "FROM erp.EmpBasic ";
                _strSQL += "WHERE EmpStatus = 'A' ";

                if (strCompany != null && strCompany != "")
                { _strSQL += "AND Company = '" + strCompany + "' "; }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Employee oEmployee = new Employee();

                        oEmployee.Company = row["Company"].ToString();
                        oEmployee.EmployeeId = row["EmpID"].ToString();
                        oEmployee.EmployeeName = row["Name"].ToString();
                        oEmployee.EmployeeStatus = row["EmpStatus"].ToString();

                        EmployeeList.Add(oEmployee);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "No employee found";
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