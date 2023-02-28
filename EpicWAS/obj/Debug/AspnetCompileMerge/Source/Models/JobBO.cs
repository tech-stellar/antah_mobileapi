using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class JobBO
    {
        public bool _LoadJobHead(ref EpicEnv oEpicEnv, string strCompany, string strPlant, ref IList<JobHead> lstJobHead, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, JobType, PartNum, PartDescription, RevisionNum, ProdQty FROM erp.JobHead ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        JobHead _JobHead = new JobHead();

                        _JobHead.Company = row["Company"].ToString();
                        _JobHead.Plant = row["Plant"].ToString();
                        _JobHead.JobNum = row["JobNum"].ToString();
                        _JobHead.JobType = row["JobType"].ToString();
                        _JobHead.PartDescription = row["PartDescription"].ToString();
                        _JobHead.PartNum = row["PartNum"].ToString();
                        _JobHead.RevisionNum = row["RevisionNum"].ToString();
                        _JobHead.ProdQty =Convert.ToDecimal( row["ProdQty"].ToString());

                        lstJobHead.Add(_JobHead);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Job not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadJobHeadById(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref JobHead oJobHead, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, JobType, PartNum, PartDescription, RevisionNum, ProdQty FROM erp.JobHead ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' and JobNum = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oJobHead.Company = row["Company"].ToString();
                        oJobHead.Plant = row["Plant"].ToString();
                        oJobHead.JobNum = row["JobNum"].ToString();
                        oJobHead.JobType = row["JobType"].ToString();
                        oJobHead.PartDescription = row["PartDescription"].ToString();
                        oJobHead.PartNum = row["PartNum"].ToString();
                        oJobHead.RevisionNum = row["RevisionNum"].ToString();
                        oJobHead.ProdQty = Convert.ToDecimal(row["ProdQty"].ToString());

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Job not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }




        public bool _LoadJobAsmbl(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref IList<JobAsmbl> lstJobAsmbl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, AssemblySeq, PartNum, Description, RevisionNum, RequiredQty, IUM FROM erp.JobAsmbl ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' and JobNum = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        JobAsmbl _JobAsmbl = new JobAsmbl();

                        _JobAsmbl.Company = row["Company"].ToString();
                        _JobAsmbl.Plant = row["Plant"].ToString();
                        _JobAsmbl.JobNum = row["JobNum"].ToString();
                        _JobAsmbl.AssemblySeq = Int32.Parse( row["AssemblySeq"].ToString());
                        _JobAsmbl.Description = row["Description"].ToString();
                        _JobAsmbl.PartNum = row["PartNum"].ToString();
                        _JobAsmbl.RevisionNum = row["RevisionNum"].ToString();
                        _JobAsmbl.RequiredQty = Convert.ToDecimal(row["RequiredQty"].ToString());
                        _JobAsmbl.IUM = row["IUM"].ToString();

                        lstJobAsmbl.Add(_JobAsmbl);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job assembly not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Job assembly not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadJobAsmblBySeqId(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, Int32 iAssemblySeq, ref JobAsmbl oJobAsmbl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, AssemblySeq, PartNum, Description, RevisionNum, RequiredQty, IUM FROM erp.JobAsmbl ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' and JobNum = '{2}' and AssemblySeq = {3}";
                _strSQL = string.Format(_strSQL, strCompany, strPlant, strJobNum, iAssemblySeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oJobAsmbl.Company = row["Company"].ToString();
                        oJobAsmbl.Plant = row["Plant"].ToString();
                        oJobAsmbl.JobNum = row["JobNum"].ToString();
                        oJobAsmbl.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJobAsmbl.Description = row["Description"].ToString();
                        oJobAsmbl.PartNum = row["PartNum"].ToString();
                        oJobAsmbl.RevisionNum = row["RevisionNum"].ToString();
                        oJobAsmbl.RequiredQty = Convert.ToDecimal(row["RequiredQty"].ToString());
                        oJobAsmbl.IUM = row["IUM"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job assembly not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Job assembly not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }




        public bool _LoadJobMtl(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref IList<JobMtl> lstJobMtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, AssemblySeq, PartNum, Description, MtlSeq, RequiredQty, IUM FROM erp.JobMtl ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' and JobNum = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        JobMtl _JobMtl = new JobMtl();

                        _JobMtl.Company = row["Company"].ToString();
                        _JobMtl.Plant = row["Plant"].ToString();
                        _JobMtl.JobNum = row["JobNum"].ToString();
                        _JobMtl.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        _JobMtl.Description = row["Description"].ToString();
                        _JobMtl.PartNum = row["PartNum"].ToString();
                        _JobMtl.MtlSeq = Int32.Parse(row["MtlSeq"].ToString());
                        _JobMtl.RequiredQty = Convert.ToDecimal(row["RequiredQty"].ToString());
                        _JobMtl.IUM = row["IUM"].ToString();

                        lstJobMtl.Add(_JobMtl);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job material not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Job material not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadJobMtlByMtlSeq(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, Int32 iAssemblySeq, Int32 iMtlSeq, ref JobMtl oJobMtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, AssemblySeq, PartNum, Description, MtlSeq, RequiredQty, IUM FROM erp.JobMtl ";
                _strSQL += "WHERE Company = '{0}' and Plant = '{1}' and JobNum = '{2}' and AssemblySeq='{3}' and MtlSeq='{4}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant, strJobNum, iAssemblySeq, iMtlSeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oJobMtl.Company = row["Company"].ToString();
                        oJobMtl.Plant = row["Plant"].ToString();
                        oJobMtl.JobNum = row["JobNum"].ToString();
                        oJobMtl.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJobMtl.Description = row["Description"].ToString();
                        oJobMtl.PartNum = row["PartNum"].ToString();
                        oJobMtl.MtlSeq = Int32.Parse(row["MtlSeq"].ToString());
                        oJobMtl.RequiredQty = Convert.ToDecimal(row["RequiredQty"].ToString());
                        oJobMtl.IUM = row["IUM"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job material not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Job material not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }



        //public bool _LoadJobJobAsmblJobMtlById(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref JobAsmblMtl oJobAsmblMtl, out string strMessage)
        //{




        //}

    }
}