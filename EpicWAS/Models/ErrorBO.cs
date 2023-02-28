using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class ErrorBO
    {

        private SpecialCharHandler oSpecialChar = new SpecialCharHandler();

        public bool _CheckPartNum(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;
            

            try
            {
                string _strSQL = "SELECT 1 FROM erp.Part ";
                _strSQL += "WHERE Company = '{0}' ";

                if (strPartNum != null && strPartNum != "")
                    { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }


                _strSQL = string.Format(_strSQL, strCompany);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Part Num : {0} not found ", strPartNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);

                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _CheckJobMaterial(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, int iAssemblySeq, int iMaterialSeq, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobMtl ";
                _strSQL += "WHERE Company = '{0}' and JobNum = '{1}' and AssemblySeq='{2}' and MtlSeq='{3}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum, iAssemblySeq, iMaterialSeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Material Sequence : {0} not found for Job Num:{1} and Assembly Seq: {2} ", iMaterialSeq, strJobNum, iAssemblySeq);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);

                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _CheckJob(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, string strCurPlant, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobHead ";
                _strSQL += "WHERE Company = '{0}' and JobNum = '{1}' and Plant='{2}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum, strCurPlant);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Job Num : {0} is not from {1} ", strJobNum, strCurPlant);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }

        public bool _CheckJobNum(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobHead ";
                _strSQL += "WHERE Company = '{0}' and JobNum = '{1}'   ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Job Num : {0} is not found ", strJobNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }


        public bool _CheckJobAssembly(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, int iAssemblySeq, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobAsmbl ";
                _strSQL += "WHERE Company = '{0}' and JobNum = '{1}' and AssemblySeq = '{2}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum, iAssemblySeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Job Assembly : {0} is not found ", iAssemblySeq);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }

        public bool _CheckJobAssemblyPart(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, int iAssemblySeq, string strPartNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobAsmbl ";
                _strSQL += "WHERE Company = '{0}' and AssemblySeq = '{1}'  ";

                if (strJobNum != null && strJobNum != "")
                { _strSQL += "AND JobNum = '" + strJobNum + "' "; }

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, iAssemblySeq);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Assembly Part : {0} is not found ", strPartNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }

        public bool _CheckJobPart(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, string strPartNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobPart ";
                _strSQL += "WHERE Company = '{0}' ";

                if (strJobNum != null && strJobNum != "")
                { _strSQL += "AND JobNum = '" + strJobNum + "' "; }

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Job Part : {0} is not found ", strPartNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }



        public bool _CheckJobRelease(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobHead ";
                _strSQL += "WHERE Company = '{0}' and JobNum = '{1}' and JobReleased = 1  ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Job Num : {0} is not released ", strJobNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }


        public bool _CheckJobComplete(ref EpicEnv oEpicEnv, string strCompany, string strJobNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.JobHead ";
                _strSQL += "WHERE Company = '{0}' and JobNum = '{1}' and JobComplete = 0  ";
                _strSQL = string.Format(_strSQL, strCompany, strJobNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Job Num : {0} is completed ", strJobNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);


        }


        public bool _CheckPartLot(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strLotNum, string strSource, ref IList<Error> oErrorList)
        {

            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.PartLot ";
                _strSQL += "WHERE Company = '{0}' and LotNum = '{1}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany,  strLotNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Lot Num : {0} is not found for Part Num : {1} ", strLotNum, strPartNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


        public bool _CheckWhseBinNum(ref EpicEnv oEpicEnv, string strCompany, string strWhse, string strBinNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM erp.WhseBin ";
                _strSQL += "WHERE Company = '{0}' and warehousecode = '{1}' and binnum = '{2}'   ";
                _strSQL = string.Format(_strSQL, strCompany, strWhse, strBinNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Bin Num : {0} is not found ", strBinNum);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


        public bool _CheckWhse(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strWhse, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM Erp.Warehse ";
                _strSQL += "WHERE Company = '{0}' and warehousecode = '{1}' and plant = '{2}'   ";
                _strSQL = string.Format(_strSQL, strCompany, strWhse, strCurPlant);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Warehouse : {0} is not found ", strWhse);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


        public bool _CheckPartWhse(ref EpicEnv oEpicEnv, string strCompany, string strWhse, string strPartNum, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 FROM Erp.PartWhse ";
                _strSQL += "WHERE Company = '{0}' and warehousecode = '{1}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, strWhse);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Part Warehouse : {0} is not found ", strWhse);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


        public bool _CheckAvailableQty(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWhse, string strBinNum, string strLotNum, string strUOM, decimal dTranQty, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;
            bool IsPartUOMOK = false;
            string strNegAction = "";
            string strUOMClassID = "";
            decimal dPartUOM_ConvFactor = 1;
            string strPartUOM_COnvOperator = "";

            decimal dOnHandPartUOM_ConvFactor = 1;
            string strOnHandPartUOM_COnvOperator = "";

            decimal dUOMCOnv_ConvFactor = 1;
            string strUOMCOnv_COnvOperator = "";

            decimal dOnHandUOMCOnv_ConvFactor = 1;
            string strOnHandUOMCOnv_COnvOperator = "";

            decimal dOnHandQty = 0;
            string strOnHandQtyUOM = "";
            decimal dActTranQty = 0;
            string strTrackDimension = "";

            try
            {

                // get the class negative action
                string _strSQL = "SELECT c.NegQtyAction, p.UOMClassID, p.TrackDimension ";
                _strSQL += "from erp.Part p ";
                _strSQL += "left join erp.PartClass c on p.Company = c.Company and p.ClassID = c.ClassID ";
                _strSQL += "WHERE p.Company = '{0}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND p.PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        strNegAction = row["NegQtyAction"].ToString();
                        strUOMClassID = row["UOMClassID"].ToString();
                        strTrackDimension = row["TrackDimension"].ToString();
                    }
                }
                else
                {
                    strNegAction = "None";
                }


                // get the partuom conversion
                _strSQL = "select ConvFactor, ConvOperator from erp.PartUOM ";
                _strSQL += "WHERE Company = '{0}' and UOMCode = '{1}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, strUOM);

                DataSet _dtsPartUOM = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dtsPartUOM.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dtsPartUOM.Tables[0].Rows)
                    {
                        dPartUOM_ConvFactor = decimal.Parse(row["ConvFactor"].ToString());
                        strPartUOM_COnvOperator = row["ConvOperator"].ToString();
                    }
                    IsPartUOMOK = true;
                }
                else
                {
                    dPartUOM_ConvFactor = 1;
                    strPartUOM_COnvOperator = "*";
                    IsPartUOMOK = false;
                }

                // get UOMConv 
                _strSQL = "select ConvFactor, ConvOperator from erp.UOMConv ";
                _strSQL += "WHERE Company = '{0}' and UOMClassID = '{1}' and uomcode = '{2}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strUOMClassID, strUOM);

                DataSet _dtsUOMConv = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dtsUOMConv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dtsUOMConv.Tables[0].Rows)
                    {
                        dUOMCOnv_ConvFactor = decimal.Parse(row["ConvFactor"].ToString());
                        strUOMCOnv_COnvOperator = row["ConvOperator"].ToString();
                    }
                }
                else
                {
                    dUOMCOnv_ConvFactor = 1;
                    strUOMCOnv_COnvOperator = "*";
                }


                // get on hand qty
                //_strSQL = "select onhandqty from erp.partbin ";
                //_strSQL += "WHERE Company = '{0}' and PartNum = '{1}' and WarehouseCode = '{2}' and binnum = '{3}' and lotnum = '{4}'  ";
                //_strSQL = string.Format(_strSQL, strCompany, strPartNum, strWhse, strBinNum, strLotNum);

                if (strTrackDimension == "False")
                {
                    _strSQL = "select onhandqty, dimcode from erp.partbin ";
                    _strSQL += "WHERE Company = '{0}' and WarehouseCode = '{1}' and binnum = '{2}' and lotnum = '{3}'  ";

                    if (strPartNum != null && strPartNum != "")
                    { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                    _strSQL = string.Format(_strSQL, strCompany, strWhse, strBinNum, strLotNum);
                }
                else
                {
                    _strSQL = "select onhandqty, dimcode from erp.partbin ";
                    _strSQL += "WHERE Company = '{0}' and WarehouseCode = '{1}' and binnum = '{2}' and lotnum = '{3}' and dimcode = '{4}'  ";

                    if (strPartNum != null && strPartNum != "")
                    { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                    _strSQL = string.Format(_strSQL, strCompany, strWhse, strBinNum, strLotNum, strUOM);
                }

                DataSet _dtsOnHand = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dtsOnHand.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dtsOnHand.Tables[0].Rows)
                    {
                        dOnHandQty += Decimal.Parse(row["onhandqty"].ToString());
                        strOnHandQtyUOM = row["dimcode"].ToString();
                    }
                }
                else
                {
                    dOnHandQty = 0;
                    strOnHandQtyUOM = strUOM;
                }

                //added on 2019-05-31
                // get the partuom conversion for partbin on hand
                _strSQL = "select ConvFactor, ConvOperator from erp.PartUOM ";
                _strSQL += "WHERE Company = '{0}' and UOMCode = '{1}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, strOnHandQtyUOM);

                DataSet _dtsOnHandPartUOM = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dtsOnHandPartUOM.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dtsOnHandPartUOM.Tables[0].Rows)
                    {
                        dOnHandPartUOM_ConvFactor = decimal.Parse(row["ConvFactor"].ToString());
                        strOnHandPartUOM_COnvOperator = row["ConvOperator"].ToString();
                    }
                }
                else
                {
                    dOnHandPartUOM_ConvFactor = 1;
                    strOnHandPartUOM_COnvOperator = "*";
                }

                // get UOMConv 
                _strSQL = "select ConvFactor, ConvOperator from erp.UOMConv ";
                _strSQL += "WHERE Company = '{0}' and UOMClassID = '{1}' and uomcode = '{2}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strUOMClassID, strOnHandQtyUOM);

                DataSet _dtsOnHandUOMConv = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dtsOnHandUOMConv.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dtsOnHandUOMConv.Tables[0].Rows)
                    {
                        dOnHandUOMCOnv_ConvFactor = decimal.Parse(row["ConvFactor"].ToString());
                        strOnHandUOMCOnv_COnvOperator = row["ConvOperator"].ToString();
                    }
                }
                else
                {
                    dOnHandUOMCOnv_ConvFactor = 1;
                    strOnHandUOMCOnv_COnvOperator = "*";
                }


                if (strNegAction == "Stop")
                {
                    if (strTrackDimension == "True")
                    {
                        dActTranQty = dTranQty;
                    }
                    else
                    {
                        if (IsPartUOMOK == true)
                        {
                            dActTranQty = (strPartUOM_COnvOperator == "*" ? dTranQty * dPartUOM_ConvFactor : dTranQty / dPartUOM_ConvFactor);
                            dOnHandQty = (strOnHandPartUOM_COnvOperator == "*" ? dOnHandQty * dOnHandPartUOM_ConvFactor : dOnHandQty / dOnHandPartUOM_ConvFactor);
                        }
                        else
                        {
                            dActTranQty = (strUOMCOnv_COnvOperator == "*" ? dTranQty * dUOMCOnv_ConvFactor : dTranQty / dUOMCOnv_ConvFactor);
                            dOnHandQty = (strOnHandUOMCOnv_COnvOperator == "*" ? dOnHandQty * dOnHandUOMCOnv_ConvFactor : dOnHandQty / dOnHandUOMCOnv_ConvFactor);
                        }


                    }


                    if (dActTranQty <= dOnHandQty)
                    {
                        IsError = false;
                    }
                    else
                    {
                        Error oErr = new Error();
                        oErr.ErrorCode = strSource;
                        oErr.ErrorDescription = String.Format("Insufficient quantity ");

                        oErrorList.Add(oErr);
                        IsError = true;
                    }

                }


            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _CheckEmployeeClockIn(ref EpicEnv oEpicEnv, string strCompany, string strEmployeeId, int iShift, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 ";
                _strSQL += "from erp.LaborHed  lh ";
                _strSQL += "WHERE lh.Company = '{0}' and lh.EmployeeNum = '{1}' and lh.Shift = {2} and activetrans = 1 ";
                _strSQL = string.Format(_strSQL, strCompany, strEmployeeId, iShift);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count == 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Employee already clock in.");

                    oErrorList.Add(oErr);
                    IsError = true;
                }


            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


        public bool _CheckEmployeeId(ref EpicEnv oEpicEnv, string strCompany, string strEmployeeId, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;
            string strEmpStatus = "";

            try
            {
                string _strSQL = "SELECT empstatus ";
                _strSQL += "from erp.EmpBasic  e ";
                _strSQL += "WHERE e.Company = '{0}' and e.EmpID = '{1}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strEmployeeId);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        strEmpStatus = row["empstatus"].ToString();
                    }

                    IsError = (strEmpStatus == "A" ? false : true);

                    if (IsError == true)
                    {
                        Error oErr = new Error();
                        oErr.ErrorCode = strSource;
                        oErr.ErrorDescription = String.Format("Employee: {0} is not active", strEmployeeId);

                        oErrorList.Add(oErr);
                    }

                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Employee: {0} is not a valid employee", strEmployeeId);

                    oErrorList.Add(oErr);
                    IsError = true;
                }


            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }

        public bool _CheckShift(ref EpicEnv oEpicEnv, string strCompany, int iShift, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 ";
                _strSQL += "from erp.JCShift   ";
                _strSQL += "WHERE Company = '{0}' and Shift = {1}  ";
                _strSQL = string.Format(_strSQL, strCompany, iShift);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Shift : {0} is not found ", iShift.ToString());

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


        public bool _CheckClockIn(ref EpicEnv oEpicEnv, string strCompany, string strEmployeeId, string strSource, ref IList<Error> oErrorList)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT 1 ";
                _strSQL += "from erp.LaborHed   ";
                _strSQL += "WHERE Company = '{0}' and EmployeeNum = '{1}' and ActiveTrans = 1  ";
                _strSQL = string.Format(_strSQL, strCompany, strEmployeeId);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count == 0)
                {
                    IsError = false;
                }
                else
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = strSource;
                    oErr.ErrorDescription = String.Format("Employee : {0} is already clock in ", strEmployeeId);

                    oErrorList.Add(oErr);
                    IsError = true;
                }

            }
            catch (Exception ex)
            {
                Error oErr = new Error();
                oErr.ErrorCode = strSource;
                oErr.ErrorDescription = String.Format(ex.Message);
                oErrorList.Add(oErr);
                IsError = true;
            }

            return (IsError ? false : true);

        }


    }



}