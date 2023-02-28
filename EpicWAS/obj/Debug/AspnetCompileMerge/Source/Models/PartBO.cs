using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class PartBO
    {
        public bool _LoadPartById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, ref Part oPart, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, PartDescription, IUM, TypeCode, NonStock, ProdCode, CostMethod, QtyBearing, TrackLots, OnHold, InActive FROM erp.Part ";
                _strSQL += "WHERE Company = '{0}' and PartNum = '{1}' ";
                _strSQL = string.Format(_strSQL, strCompany,  strPartNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oPart.Company = row["Company"].ToString();
                        oPart.PartNum = row["PartNum"].ToString();
                        oPart.PartDescription = row["PartDescription"].ToString();
                        oPart.IUM = row["IUM"].ToString();
                        oPart.TypeCode = row["TypeCode"].ToString();
                        oPart.NonStock = Boolean.Parse( row["NonStock"].ToString());
                        oPart.ProdCode = row["ProdCode"].ToString();
                        oPart.CostMethod = row["CostMethod"].ToString();
                        oPart.QtyBearing = Boolean.Parse( row["QtyBearing"].ToString());
                        oPart.TrackLots = Boolean.Parse(row["TrackLots"].ToString());
                        oPart.OnHold = Boolean.Parse(row["OnHold"].ToString());
                        oPart.InActive = Boolean.Parse(row["InActive"].ToString());

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Part not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadPartLotById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strLotNum,  ref PartLot oPartLot, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, LotNum, PartLotDescription, Batch, MfgBatch, MfgLot, HeatNum, FirmWare, BestBeforeDt, MfgDt, CureDt, ExpireDt ";
                _strSQL += "FROM erp.PartLot ";
                _strSQL += "WHERE Company = '{0}' and PartNum = '{1}' and LotNum = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPartNum, strLotNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oPartLot.Company = row["Company"].ToString();
                        oPartLot.PartNum = row["PartNum"].ToString();
                        oPartLot.LotNum = row["LotNum"].ToString();
                        oPartLot.PartLotDescription = row["PartLotDescription"].ToString();
                        oPartLot.Batch = row["Batch"].ToString();
                        oPartLot.MfgBatch = row["MfgBatch"].ToString();
                        oPartLot.MfgLot = row["MfgLot"].ToString();
                        oPartLot.HeatNum = row["HeatNum"].ToString();
                        oPartLot.FirmWare = row["FirmWare"].ToString();
                        //oPartLot.BestBeforeDt = row["BestBeforeDt"].ToString();
                        //oPartLot.MfgDt = row["MfgDt"].ToString();
                        //oPartLot.CureDt = row["CureDt"].ToString();
                        //oPartLot.ExpireDt = row["ExpireDt"].ToString();

                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Lot not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Part Lot not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartLotByPartId(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, ref IList<PartLot> PartLotList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, LotNum, PartLotDescription, Batch, MfgBatch, MfgLot, HeatNum, FirmWare, BestBeforeDt, MfgDt, CureDt, ExpireDt ";
                _strSQL += "FROM erp.PartLot ";
                _strSQL += "WHERE Company = '{0}' and PartNum = '{1}'  ";
                _strSQL = string.Format(_strSQL, strCompany, strPartNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PartLot oPartLot = new PartLot();

                        oPartLot.Company = row["Company"].ToString();
                        oPartLot.PartNum = row["PartNum"].ToString();
                        oPartLot.LotNum = row["LotNum"].ToString();
                        oPartLot.PartLotDescription = row["PartLotDescription"].ToString();
                        oPartLot.Batch = row["Batch"].ToString();
                        oPartLot.MfgBatch = row["MfgBatch"].ToString();
                        oPartLot.MfgLot = row["MfgLot"].ToString();
                        oPartLot.HeatNum = row["HeatNum"].ToString();
                        oPartLot.FirmWare = row["FirmWare"].ToString();
                        //oPartLot.BestBeforeDt = row["BestBeforeDt"].ToString();
                        //oPartLot.MfgDt = row["MfgDt"].ToString();
                        //oPartLot.CureDt = row["CureDt"].ToString();
                        //oPartLot.ExpireDt = row["ExpireDt"].ToString();

                        PartLotList.Add(oPartLot);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Lot not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Part Lot not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartWhseById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWhseCode, ref PartWhse oPartWhse, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, WarehouseCode, OnHandQty FROM erp.PartWhse ";
                _strSQL += "WHERE Company = '{0}' and PartNum = '{1}' and WarehouseCode = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPartNum, strWhseCode);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {

                        oPartWhse.Company = row["Company"].ToString();
                        oPartWhse.PartNum = row["PartNum"].ToString();
                        oPartWhse.WarehouseCode = row["WarehouseCode"].ToString();
                        oPartWhse.OnHandQty =Decimal.Parse( row["OnHandQty"].ToString());
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Warehouse not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Part Warehouse not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartWhseBin(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWhseCode, ref IList<PartBin> PartBinList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, WarehouseCode, BinNum, OnHandQty FROM erp.PartBin ";
                _strSQL += "WHERE Company = '{0}' and PartNum = '{1}' and WarehouseCode = '{2}' ";
                _strSQL = string.Format(_strSQL, strCompany, strPartNum, strWhseCode);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        PartBin oPartBin = new PartBin();
                        oPartBin.Company = row["Company"].ToString();
                        oPartBin.PartNum = row["PartNum"].ToString();
                        oPartBin.WarehouseCode = row["WarehouseCode"].ToString();
                        oPartBin.BinNum = row["BinNum"].ToString();
                        oPartBin.OnHandQty = Decimal.Parse(row["OnHandQty"].ToString());
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Bin not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Part Bin not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadWhseBin(ref EpicEnv oEpicEnv, string strCompany, string strWhseCode, ref IList<WhseBin> WhseBinList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company,  WarehouseCode, BinNum, description FROM erp.WhseBin ";
                _strSQL += "WHERE Company = '{0}' and WarehouseCode = '{1}' ";
                _strSQL = string.Format(_strSQL, strCompany,  strWhseCode);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        WhseBin oWhseBin = new WhseBin();
                        oWhseBin.Company = row["Company"].ToString();
                        oWhseBin.Description = row["description"].ToString();
                        oWhseBin.WarehouseCode = row["WarehouseCode"].ToString();
                        oWhseBin.BinNum = row["BinNum"].ToString();

                        WhseBinList.Add(oWhseBin);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Bin not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = "Part Bin not found ";
                IsError = true;
            }

            return (IsError ? false : true);
        }





    }
}