using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class JobBO
    {
        private SpecialCharHandler oSpecialChar = new SpecialCharHandler();

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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadJobHeadById(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref JobHead oJobHead, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, JobType, PartNum, PartDescription, RevisionNum, ProdQty, ium FROM erp.JobHead ";
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
                        oJobHead.IUM = row["IUM"].ToString();
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
                strMessage = ex.Message.ToString();
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
                strMessage = ex.Message.ToString();
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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadJobMtl(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref IList<JobMtl> lstJobMtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, AssemblySeq, PartNum, Description, MtlSeq, RequiredQty, IUM, relatedoperation, IssuedQty ";
                _strSQL += "FROM erp.JobMtl ";
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
                        _JobMtl.RelatedOperation = Int32.Parse(row["relatedoperation"].ToString());
                        _JobMtl.TotalIssuedQty = Convert.ToDecimal(row["IssuedQty"].ToString());

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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadJobMtlByMtlSeq(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, Int32 iAssemblySeq, Int32 iMtlSeq, ref JobMtl oJobMtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, Plant, JobNum, AssemblySeq, PartNum, Description, MtlSeq, RequiredQty, IUM, relatedoperation, IssuedQty ";
                _strSQL += "FROM erp.JobMtl ";
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
                        oJobMtl.RelatedOperation = Int32.Parse(row["relatedoperation"].ToString());
                        oJobMtl.IUM = row["IUM"].ToString();
                        oJobMtl.TotalIssuedQty = Convert.ToDecimal(row["IssuedQty"].ToString());
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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadDefaultJobMtlWhseAndBin(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, Int32 iAssemblySeq, Int32 iOprSeq, ref JobOprDtl oJobOprDtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select jod.Company, jod.JobNum, jod.AssemblySeq, jod.OprSeq, jod.OpDtlSeq, jod.ResourceGrpID, jod.ResourceID, jo.PrimaryProdOpDtl, ";
                _strSQL += "rg.InputWhse, rg.InputBinNum, r.InputWhse as resource_inputwhse, r.InputBinNum as resource_inputbin, ";
                _strSQL += "rg.OutputWhse, rg.OutputBinNum, r.OutputWhse as resource_outputwhse, r.OutputBinNum as resource_outputbin ";
                _strSQL += "from erp.JobOpDtl jod ";
                _strSQL += "inner join erp.JobOper jo on jod.Company = jo.Company and jod.JobNum = jo.JobNum and jod.AssemblySeq = jo.AssemblySeq and jod.OprSeq = jo.OprSeq and jod.OpDtlSeq = jo.PrimaryProdOpDtl ";
                _strSQL += "inner join erp.ResourceGroup rg on jod.Company = rg.Company and jod.ResourceGrpID = rg.ResourceGrpID ";
                _strSQL += "left join erp.Resource r on jod.Company = r.Company and jod.ResourceGrpID = r.ResourceGrpID and jod.ResourceID = r.ResourceID  ";
                _strSQL += "where jod.Company = '{0}' and jod.JobNum = '{1}' and jod.AssemblySeq = '{2}' and jod.OprSeq = '{3}' ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum, iAssemblySeq, iOprSeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oJobOprDtl.Company = row["Company"].ToString();
                        oJobOprDtl.JobNum = row["JobNum"].ToString();
                        oJobOprDtl.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJobOprDtl.OprSeq = Int32.Parse(row["OprSeq"].ToString());
                        oJobOprDtl.OpDtlSeq = Int32.Parse(row["OpDtlSeq"].ToString());
                        oJobOprDtl.ResourceGrpID = row["ResourceGrpID"].ToString();
                        oJobOprDtl.ResourceID = row["ResourceID"].ToString();
                        oJobOprDtl.PrimaryProdOpDtl = row["PrimaryProdOpDtl"].ToString();
                        oJobOprDtl.InputWhse = row["InputWhse"].ToString();
                        oJobOprDtl.InputBinNum = row["InputBinNum"].ToString();
                        oJobOprDtl.resource_inputwhse = row["resource_inputwhse"].ToString();
                        oJobOprDtl.resource_inputbin = row["resource_inputbin"].ToString();

                        oJobOprDtl.OutputWhse = row["OutputWhse"].ToString();
                        oJobOprDtl.OutputBinNum = row["OutputBinNum"].ToString();
                        oJobOprDtl.resource_outputwhse = row["resource_outputwhse"].ToString();
                        oJobOprDtl.resource_outputbin = row["resource_outputbin"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job material default warehouse and bin not found. ";
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


        public bool _LoadDefaultJobMtlWhseAndBin2(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, Int32 iAssemblySeq, ref JobOprDtl oJobOprDtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select jod.Company, jod.JobNum, jod.AssemblySeq, jod.OprSeq, jod.OpDtlSeq, jod.ResourceGrpID, jod.ResourceID, jo.PrimaryProdOpDtl, ";
                _strSQL += "rg.InputWhse, rg.InputBinNum, ";
                _strSQL += "rg.OutputWhse, rg.OutputBinNum ";
                _strSQL += "from erp.JobOpDtl jod ";
                _strSQL += "inner join erp.JobOper jo on jod.Company = jo.Company and jod.JobNum = jo.JobNum and jod.AssemblySeq = jo.AssemblySeq and jod.OprSeq = jo.OprSeq and jod.OpDtlSeq = jo.PrimaryProdOpDtl ";
                _strSQL += "inner join erp.ResourceGroup rg on jod.Company = rg.Company and jod.ResourceGrpID = rg.ResourceGrpID ";
                _strSQL += "where jod.Company = '{0}' and jod.JobNum = '{1}' and jod.AssemblySeq = '{2}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum, iAssemblySeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oJobOprDtl.Company = row["Company"].ToString();
                        oJobOprDtl.JobNum = row["JobNum"].ToString();
                        oJobOprDtl.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJobOprDtl.OprSeq = Int32.Parse(row["OprSeq"].ToString());
                        oJobOprDtl.OpDtlSeq = Int32.Parse(row["OpDtlSeq"].ToString());
                        oJobOprDtl.ResourceGrpID = row["ResourceGrpID"].ToString();
                        oJobOprDtl.ResourceID = row["ResourceID"].ToString();
                        oJobOprDtl.PrimaryProdOpDtl = row["PrimaryProdOpDtl"].ToString();
                        oJobOprDtl.InputWhse = row["InputWhse"].ToString();
                        oJobOprDtl.InputBinNum = row["InputBinNum"].ToString();
                        oJobOprDtl.OutputWhse = row["OutputWhse"].ToString();
                        oJobOprDtl.OutputBinNum = row["OutputBinNum"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job material default warehouse and bin not found. ";
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


        public bool _LoadDefaultCompanyWhseAndBin(ref EpicEnv oEpicEnv, string strCompany, string strPlant,  ref DefWhseBin oDefWhseBin, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select DefaultWhse, DefGenBin ";
                _strSQL += "from erp.PlantConfCtrl ";
                _strSQL += "where Company = '{0}' and Plant = '{1}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strPlant);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oDefWhseBin.Whse = row["DefaultWhse"].ToString();
                        oDefWhseBin.BinNum = row["DefGenBin"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Company default warehouse and bin not found. ";
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



        public bool _LoadJobForSalvageById(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref JobSalvage oJSalvage, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select top 1 m.Company, m.JobNum, m.AssemblySeq, m.MtlSeq, m.PartNum as MatPart, m.Plant, m.IUM, ";
                _strSQL += "h.PartNum as JobPart ";
                _strSQL += "from erp.JobMtl m ";
                _strSQL += "inner join erp.JobHead h on m.Company = h.Company and m.JobNum = h.JobNum ";
                _strSQL += "where m.Company = '{0}' and m.JobNum = '{1}' ";
                _strSQL += "order by m.totalcost desc ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oJSalvage.Company = row["Company"].ToString();
                        oJSalvage.JobNum = row["JobNum"].ToString();
                        oJSalvage.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJSalvage.MtlSeq = Int32.Parse(row["MtlSeq"].ToString());
                        oJSalvage.JobPartNum = row["JobPart"].ToString();
                        oJSalvage.PartNum = row["MatPart"].ToString();
                        oJSalvage.IUM = row["IUM"].ToString();
                        oJSalvage.Plant = row["Plant"].ToString();
                       
                    }

                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Job material not found. ";
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


        public bool _LoadMostExpensiveJobMtlById(ref EpicEnv oEpicEnv, string strCompany, string strPlant, string strJobNum, ref JobMtl oJobMtl, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select top 1 m.Company, m.JobNum, m.AssemblySeq, m.MtlSeq, m.PartNum as MatPart, m.Plant, m.IUM, ";
                _strSQL += "h.PartNum as JobPart ";
                _strSQL += "from erp.JobMtl m ";
                _strSQL += "inner join erp.JobHead h on m.Company = h.Company and m.JobNum = h.JobNum ";
                _strSQL += "where m.Company = '{0}' and m.JobNum = '{1}' ";
                _strSQL += "order by m.totalcost desc ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oJobMtl.Company = row["Company"].ToString();
                        oJobMtl.JobNum = row["JobNum"].ToString();
                        oJobMtl.AssemblySeq = Int32.Parse(row["AssemblySeq"].ToString());
                        oJobMtl.MtlSeq = Int32.Parse(row["MtlSeq"].ToString());
                        oJobMtl.PartNum = row["MatPart"].ToString();
                        oJobMtl.IUM = row["IUM"].ToString();
                        oJobMtl.Plant = row["Plant"].ToString();

                    }

                    strMessage = "";
                    IsError = false;

                }
                else
                {
                    strMessage = "Job material not found. ";
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


        public bool _LoadDefaultWhseAndBinForPerformMaterialToInventory(ref EpicEnv oEpicEnv, string strCompany, string strPart, ref DefWhseBin oDefWhseBin, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "select top 1 warehousecode, binnum ";
                _strSQL += "from erp.partbin  ";
                _strSQL += "where Company = '{0}' ";

                if (strPart != null && strPart != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPart) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oDefWhseBin.Whse = row["warehousecode"].ToString();
                        oDefWhseBin.BinNum = row["binnum"].ToString();
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Job material default warehouse and bin not found. ";
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


        public bool _LoadDefaultWhseAndBinFromSiteConfig(
              ref EpicEnv oEpicEnv,
              string strCompany,
              string strCurPlant,
              ref DefWhseBin oDefWhseBin,
              out string strMessage)
        {
            bool flag;
            try
            {
                string strQuery = string.Format("select DefaultWhse, DefGenBin " + "from erp.PlantConfCtrl  " + "where Company = '{0}' and plant = '{1}' ", (object)strCompany, (object)strCurPlant);
                SQLServerBO sqlServerBo = new SQLServerBO();
                string strSQLCon = string.Format(sqlServerBo._retSQLConnectionString(), (object)oEpicEnv.Env_SQLServer, (object)oEpicEnv.Env_SQLDB, (object)oEpicEnv.Env_SQLUserId, (object)oEpicEnv.Env_SQLPassKey);
                DataSet dataSet = sqlServerBo._MSSQLDataSetResult(strQuery, strSQLCon);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        oDefWhseBin.Whse = row["DefaultWhse"].ToString();
                        oDefWhseBin.BinNum = row["DefGenBin"].ToString();
                    }
                    strMessage = "";
                    flag = false;
                }
                else
                {
                    strMessage = "Company default warehouse and bin not setup. ";
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                flag = true;
            }
            return !flag;
        }
    }


}