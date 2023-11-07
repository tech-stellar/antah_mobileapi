using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace EpicWAS.Models
{
    
    public class PartBO
    {
        private SpecialCharHandler oSpecialChar = new SpecialCharHandler();

        public bool _LoadPartById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, ref Part oPart, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, PartDescription, IUM, TypeCode, NonStock, ProdCode, CostMethod, QtyBearing, ";
                _strSQL += "TrackLots, OnHold, InActive, AttBatch, AttMfgBatch, AttMfgLot, AttHeat, AttFirmware, AttBeforeDt, AttMfgDt, ";
                _strSQL += "AttCureDt, AttExpDt ";
                _strSQL += "FROM erp.Part ";
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

                        oPart.AttBatch = row["AttBatch"].ToString();
                        oPart.AttMfgBatch = row["AttMfgBatch"].ToString();
                        oPart.AttMfgLot = row["AttMfgLot"].ToString();
                        oPart.AttHeat = row["AttHeat"].ToString();
                        oPart.AttFirmware = row["AttFirmware"].ToString();
                        oPart.AttBeforeDt = row["AttBeforeDt"].ToString();
                        oPart.AttMfgDt = row["AttMfgDt"].ToString();
                        oPart.AttCureDt = row["AttCureDt"].ToString();
                        oPart.AttExpDt = row["AttExpDt"].ToString();

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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartById_Antah(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, ref Part oPart, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, PartDescription, IUM, TypeCode, NonStock, ProdCode, CostMethod, QtyBearing, ";
                _strSQL += "TrackLots, OnHold, InActive, AttBatch, AttMfgBatch, AttMfgLot, AttHeat, AttFirmware, AttBeforeDt, AttMfgDt, ";
                _strSQL += "AttCureDt, AttExpDt, AP_SerialNo_c ";
                _strSQL += "FROM Part ";
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
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oPart.Company = row["Company"].ToString();
                        oPart.PartNum = row["PartNum"].ToString();
                        oPart.PartDescription = row["PartDescription"].ToString();
                        oPart.IUM = row["IUM"].ToString();
                        oPart.TypeCode = row["TypeCode"].ToString();
                        oPart.NonStock = Boolean.Parse(row["NonStock"].ToString());
                        oPart.ProdCode = row["ProdCode"].ToString();
                        oPart.CostMethod = row["CostMethod"].ToString();
                        oPart.QtyBearing = Boolean.Parse(row["QtyBearing"].ToString());
                        oPart.TrackLots = Boolean.Parse(row["TrackLots"].ToString());
                        oPart.OnHold = Boolean.Parse(row["OnHold"].ToString());
                        oPart.InActive = Boolean.Parse(row["InActive"].ToString());

                        oPart.AttBatch = row["AttBatch"].ToString();
                        oPart.AttMfgBatch = row["AttMfgBatch"].ToString();
                        oPart.AttMfgLot = row["AttMfgLot"].ToString();
                        oPart.AttHeat = row["AttHeat"].ToString();
                        oPart.AttFirmware = row["AttFirmware"].ToString();
                        oPart.AttBeforeDt = row["AttBeforeDt"].ToString();
                        oPart.AttMfgDt = row["AttMfgDt"].ToString();
                        oPart.AttCureDt = row["AttCureDt"].ToString();
                        oPart.AttExpDt = row["AttExpDt"].ToString();

                        oPart.AP_SerialNo_c = row["AP_SerialNo_c"].ToString();

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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }




        public bool _LoadPartLotById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strLotNum,  ref PartLot oPartLot, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, LotNum, PartLotDescription, Batch, MfgBatch, MfgLot, HeatNum, FirmWare, BestBeforeDt, MfgDt, CureDt, ExpirationDate AS ExpireDt ";
                _strSQL += "FROM erp.PartLot ";
                _strSQL += "WHERE Company = '{0}' and LotNum = '{1}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, strLotNum);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                string _strSQL2 = "SDS_Fulfillment_Patch ";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company","No_thing"),
                    new SqlParameter("@PartNum","No_thing")

                };

                _MSSQL._exeStoredProcedureCommand(_strSQL2, _strSQLCon, parameters);




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
                        oPartLot.BestBeforeDt = (row["BestBeforeDt"].ToString());
                        oPartLot.MfgDt = (row["MfgDt"].ToString());
                        oPartLot.CureDt = (row["CureDt"].ToString());
                        oPartLot.ExpireDt = (DBNull.Value.Equals(row["ExpireDt"]) ? "" : Convert.ToDateTime((row["ExpireDt"])).ToString("yyyy-MM-dd"));

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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartLotByPartId(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, ref IList<PartLot> PartLotList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, LotNum, PartLotDescription, Batch, MfgBatch, MfgLot, HeatNum, FirmWare, BestBeforeDt, MfgDt, CureDt, ExpirationDate AS ExpireDt ";
                _strSQL += "FROM erp.PartLot ";
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
                        oPartLot.BestBeforeDt = (row["BestBeforeDt"].ToString());
                        oPartLot.MfgDt = (row["MfgDt"].ToString());
                        oPartLot.CureDt = (row["CureDt"].ToString());
                        oPartLot.ExpireDt = (DBNull.Value.Equals(row["ExpireDt"]) ? "" : Convert.ToDateTime((row["ExpireDt"])).ToString("yyyy-MM-dd"));

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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartWhseById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWhseCode, ref PartWhse oPartWhse, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT a.Company, a.PartNum, a.WarehouseCode, a.OnHandQty, b.description ";
                _strSQL += "FROM erp.PartWhse a ";
                _strSQL += "INNER JOIN erp.Warehse b on a.Company = b.Company and a.Warehousecode = b.WarehouseCode ";
                _strSQL += "WHERE a.Company = '{0}' and a.WarehouseCode = '{1}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND a.PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, strWhseCode);

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
                        oPartWhse.WarehouseDescription = row["description"].ToString();
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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        public bool _LoadPartWhseBinById(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWhseCode,string strWhseBin, ref PartBin oPartBin, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT a.Company, a.PartNum, a.WarehouseCode, a.BinNum, a.OnHandQty, b.Description as WhseDesc, c.Description as BinDesc, a.allocatedqty, a.SalesPickingQty ";
                _strSQL += "FROM erp.PartBin a ";
                _strSQL += "INNER join erp.Warehse b on a.Company = b.Company and a.Warehousecode = b.WarehouseCode ";
                _strSQL += "INNER join erp.WhseBin c on a.Company = c.Company and a.WarehouseCode = c.WarehouseCode and a.BinNum = c.BinNum ";
                _strSQL += "WHERE a.Company = '{0}' and a.WarehouseCode = '{1}' and a.binnum = '{2}' ";

                if (strPartNum != null && strPartNum != "")
                { _strSQL += "AND a.PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' "; }

                _strSQL = string.Format(_strSQL, strCompany, strWhseCode, strWhseBin);

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                string _strSQL2 = "SDS_Fulfillment_Patch ";

                IDataParameter[] parameters = new IDataParameter[]
                {
                    new SqlParameter("@Company","No_thing"),
                    new SqlParameter("@PartNum","No_thing")

                };

                _MSSQL._exeStoredProcedureCommand(_strSQL2, _strSQLCon, parameters);



                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oPartBin.Company = row["Company"].ToString();
                        oPartBin.PartNum = row["PartNum"].ToString();
                        oPartBin.WarehouseCode = row["WarehouseCode"].ToString();
                        oPartBin.BinNum = row["BinNum"].ToString();
                        oPartBin.OnHandQty = Decimal.Parse(row["OnHandQty"].ToString());
                        oPartBin.WarehouseDescription = row["WhseDesc"].ToString();
                        oPartBin.BinDescription = row["BinDesc"].ToString();
                        oPartBin.AllocatedQty = Decimal.Parse(row["AllocatedQty"].ToString()) + Decimal.Parse(row["SalesPickingQty"].ToString());
                        oPartBin.AvailableQty = Decimal.Parse(row["OnHandQty"].ToString()) - Decimal.Parse(row["AllocatedQty"].ToString()) - Decimal.Parse(row["SalesPickingQty"].ToString());
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
                strMessage = ex.Message.ToString();
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
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }


        //modified on 20190612
        public bool _LoadUOMForPart(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, ref IList<UOM> UOMList, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "";
                _strSQL += "select distinct tblUOM.UOMCode, u.UOMDesc ";
                _strSQL += "from ( ";
                _strSQL += "SELECT UOMCode ";
                _strSQL += "FROM erp.PartUOM ";
                _strSQL += "WHERE Company = '{0}' and PartNum = '{1}' and active = 1 ";
                _strSQL += "UNION ALL ";
                _strSQL += "SELECT a.UOMCode ";
                _strSQL += "FROM erp.UOMConv a ";
                _strSQL += "inner join erp.Part p on a.Company = p.Company and a.UOMClassID = p.UOMClassID ";
                _strSQL += "WHERE a.Company = '{0}' and p.PartNum = '{1}' and a.active = 1 ";
                _strSQL += ") tblUOM  inner join erp.UOM u on tblUOM.UOMCode = u.UOMCode and u.Company = '{0}' ";

                _strSQL = string.Format(_strSQL, strCompany, oSpecialChar.replaceSpecialChar(strPartNum));

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        UOM oUOM = new UOM();

                        oUOM.UOMCode = row["UOMCode"].ToString();
                        oUOM.UOMDescription = row["UOMDesc"].ToString();

                        UOMList.Add(oUOM);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "UOM not found. ";
                    IsError = true;
                }

            } // ending for try
            catch (Exception ex)
            {
                strMessage = ex.Message;
                IsError = true;
            }

            return (IsError ? false : true);


        }

        public bool _LoadWarehouse(ref EpicEnv oEpicEnv, string strCompany, string strWhseCode, string strWhseDesc , ref IList<Warehse> Warehouses,  out string strMessage, string strPlant = "")
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company,  WarehouseCode, description FROM Warehse ";
                _strSQL += "WHERE Company = '" + strCompany + "' ";

                _strSQL += "AND AP_Inactive_c = 0 ";

                if (strWhseCode != null)
                {
                    _strSQL += "AND WarehouseCode like '" + strWhseCode + "%' ";
                }

                if (strWhseDesc != null)
                {
                    _strSQL += "AND Description like '" + strWhseDesc + "%' ";
                }

                if (strPlant != null)
                {
                    _strSQL += "AND Plant = '" + strPlant + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Warehse oWarehouse = new Warehse();
                        oWarehouse.Company = row["Company"].ToString();
                        oWarehouse.Description = row["description"].ToString();
                        oWarehouse.WarehouseCode = row["WarehouseCode"].ToString();


                        Warehouses.Add(oWarehouse);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Warehouse listing not found ";
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

        public bool _LoadWarehouseBin(ref EpicEnv oEpicEnv, string strCompany, string strWhseCode, string strBinNum, string strBindesc, ref IList<WhseBin> WarehouseBins, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company,  WarehouseCode, binnum, description FROM erp.WhseBin ";
                _strSQL += "WHERE Company = '" + strCompany + "' ";

                if (strWhseCode != null)
                {
                    _strSQL += "AND WarehouseCode = '" + strWhseCode + "' ";
                }

                if (strBinNum != null)
                {
                    _strSQL += "AND BinNum like '" + strBinNum + "%' ";
                }

                if (strBindesc != null)
                {
                    _strSQL += "AND Description like '" + strBindesc + "%' ";
                }


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
                        oWhseBin.BinNum = row["BinNum"].ToString();
                        oWhseBin.WarehouseCode = row["WarehouseCode"].ToString();

                        WarehouseBins.Add(oWhseBin);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Warehouse Bin listing not found ";
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

        public bool _LoadPartLotBin(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strLotNum, string strWarehouse, string strBin, string strProdGroup, ref  IList<PartLot> PartLots, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT pb.Company, pb.PartNum, pb.LotNum, pb.OnhandQty, pb.WarehouseCode, pb.BinNum, pb.AllocatedQty, pb.SalesPickingQty ";
                _strSQL += ", pl.PartLotDescription, pl.Batch, pl.MfgBatch, pl.MfgLot, pl.HeatNum, pl.FirmWare, pl.BestBeforeDt, pl.MfgDt, pl.CureDt, pl.ExpirationDate AS ExpireDt ";
                _strSQL += ", p.PartDescription, p.prodcode, pg.description, ISNULL(pbud.AllocatedQty_c, 0) NewAllocQty ";
                _strSQL += "FROM erp.PartBin pb " ;
                _strSQL += "FULL JOIN erp.PartLot pl on pl.Company = pb.Company and pl.PartNum = pb.PartNum and pl.LotNum = pb.LotNum ";
                _strSQL += "left join erp.part p on p.Company = pb.Company and p.partnum = pb.partnum ";
                _strSQL += "left join erp.ProdGrup pg on pg.Company = p.Company and pg.prodcode = p.prodcode ";
                _strSQL += "left join erp.PartBin_ud pbud on pb.SysRowID = pbud.ForeignSysRowID ";
                _strSQL += "WHERE pb.Company = '" + strCompany  + "' " ;

                if (strPartNum != null )
                {
                    _strSQL += "AND pb.PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' ";
                }

                if (strLotNum != null)
                {
                    _strSQL += "AND pb.LotNum like '" + strLotNum + "%' ";
                }

                if (strWarehouse != null)
                {
                    _strSQL += "AND pb.WarehouseCode = '" + strWarehouse + "' ";
                }

                if (strBin != null)
                {
                    _strSQL += "AND pb.BinNum = '" + strBin + "' ";
                }

                if (strProdGroup != null)
                {
                    _strSQL += "AND p.prodcode = '" + strProdGroup + "' ";
                }

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
                        oPartLot.BestBeforeDt = (row["BestBeforeDt"].ToString());
                        oPartLot.MfgDt = (row["MfgDt"].ToString());
                        oPartLot.CureDt = (row["CureDt"].ToString());
                        oPartLot.ExpireDt = (DBNull.Value.Equals(row["ExpireDt"]) ? "" : Convert.ToDateTime((row["ExpireDt"])).ToString("yyyy-MM-dd"));
                        oPartLot.Warehouse = (row["WarehouseCode"].ToString());
                        oPartLot.OnHandQty = Convert.ToDecimal(row["OnhandQty"]);
                        oPartLot.BinNum = (row["BinNum"].ToString());
                        
                        oPartLot.PartDescription = row["PartDescription"].ToString();
                        oPartLot.ProdGroup = row["prodcode"].ToString();
                        oPartLot.ProdGroupName = row["description"].ToString();

                        oPartLot.AllocatedQty = Convert.ToDecimal(row["AllocatedQty"]) + Convert.ToDecimal(row["SalesPickingQty"]) + Convert.ToDecimal(row["NewAllocQty"]);
                        oPartLot.AvailableQty = oPartLot.OnHandQty - oPartLot.AllocatedQty;

                        PartLots.Add(oPartLot);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Lot listing not found. ";
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

        public bool _LoadPartLotBin_v2(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strPartDesc, string strLotNum, string strWarehouse, string strBin, string strProdGroup, string strTypeCode, ref IList<PartLot> PartLots, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT pb.Company, pb.PartNum, pb.LotNum, pb.OnhandQty, pb.WarehouseCode, pb.BinNum, pb.AllocatedQty, pb.dimcode ";
                _strSQL += ", pl.PartLotDescription, pl.Batch, pl.MfgBatch, pl.MfgLot, pl.HeatNum, pl.FirmWare, pl.BestBeforeDt, pl.MfgDt, pl.CureDt, pl.ExpirationDate AS ExpireDt ";
                _strSQL += ", p.PartDescription, p.prodcode, pg.description ";
                _strSQL += "FROM erp.PartBin pb ";
                _strSQL += "FULL JOIN erp.PartLot pl on pl.Company = pb.Company and pl.PartNum = pb.PartNum and pl.LotNum = pb.LotNum ";
                _strSQL += "left join erp.part p on p.Company = pb.Company and p.partnum = pb.partnum ";
                _strSQL += "left join erp.ProdGrup pg on pg.Company = p.Company and pg.prodcode = p.prodcode ";
                _strSQL += "WHERE pb.Company = '" + strCompany + "' ";

                if (strPartNum != null)
                {
                    _strSQL += "AND pb.PartNum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' ";
                }

                if (strPartDesc != null)
                {
                    _strSQL += "AND p.PartDescription = '" + oSpecialChar.replaceSpecialChar(strPartDesc) + "' ";
                }

                if (strTypeCode != null)
                {
                    _strSQL += "AND p.TypeCode = '" + strTypeCode + "' ";
                }

                if (strLotNum != null)
                {
                    _strSQL += "AND pb.LotNum = '" + strLotNum + "' ";
                }

                if (strWarehouse != null)
                {
                    _strSQL += "AND pb.WarehouseCode = '" + strWarehouse + "' ";
                }

                if (strBin != null)
                {
                    _strSQL += "AND pb.BinNum = '" + strBin + "' ";
                }

                if (strProdGroup != null)
                {
                    _strSQL += "AND p.prodcode = '" + strProdGroup + "' ";
                }

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
                        oPartLot.BestBeforeDt = (row["BestBeforeDt"].ToString());
                        oPartLot.MfgDt = (row["MfgDt"].ToString());
                        oPartLot.CureDt = (row["CureDt"].ToString());
                        oPartLot.ExpireDt = (DBNull.Value.Equals(row["ExpireDt"]) ? "" : Convert.ToDateTime((row["ExpireDt"])).ToString("yyyy-MM-dd"));
                        oPartLot.Warehouse = (row["WarehouseCode"].ToString());
                        oPartLot.OnHandQty = Convert.ToDecimal(row["OnhandQty"]);
                        oPartLot.BinNum = (row["BinNum"].ToString());

                        oPartLot.PartDescription = row["PartDescription"].ToString();
                        oPartLot.ProdGroup = row["prodcode"].ToString();
                        oPartLot.ProdGroupName = row["description"].ToString();

                        oPartLot.AllocatedQty = Convert.ToDecimal(row["AllocatedQty"]) ;
                        oPartLot.AvailableQty = oPartLot.OnHandQty - oPartLot.AllocatedQty;
                        oPartLot.UOM = (row["dimcode"].ToString());

                        PartLots.Add(oPartLot);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part Bin listing not found. ";
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


        public bool _LoadParts(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strPartDesc, string strProdGroup, string strTypeCode, ref IList<Part> Parts,  out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, PartDescription, IUM, TypeCode, NonStock, ProdCode, CostMethod, QtyBearing, ";
                _strSQL += "TrackLots, OnHold, InActive, AttBatch, AttMfgBatch, AttMfgLot, AttHeat, AttFirmware, AttBeforeDt, AttMfgDt, ";
                _strSQL += "AttCureDt, AttExpDt ";
                _strSQL += "FROM erp.Part ";

                _strSQL += "WHERE Company = '" + strCompany + "' ";

                if (strPartNum != null)
                {
                    _strSQL += "and Partnum like '%" + oSpecialChar.replaceSpecialChar(strPartNum) + "%' ";
                }

                if (strPartDesc != null)
                {
                    _strSQL += "and PartDescription like '%" + strPartDesc + "%' ";
                }

                if (strProdGroup != null)
                {
                    _strSQL += "and ProdCode = '" + strProdGroup + "' ";
                }

                if (strTypeCode != null)
                {
                    _strSQL += "and strTypeCode = '" + strProdGroup + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        Part oPart = new Part();
                        oPart.Company = row["Company"].ToString();
                        oPart.PartNum = row["PartNum"].ToString();
                        oPart.PartDescription = row["PartDescription"].ToString();
                        oPart.IUM = row["IUM"].ToString();
                        oPart.TypeCode = row["TypeCode"].ToString();
                        oPart.NonStock = Boolean.Parse(row["NonStock"].ToString());
                        oPart.ProdCode = row["ProdCode"].ToString();
                        oPart.CostMethod = row["CostMethod"].ToString();
                        oPart.QtyBearing = Boolean.Parse(row["QtyBearing"].ToString());
                        oPart.TrackLots = Boolean.Parse(row["TrackLots"].ToString());
                        oPart.OnHold = Boolean.Parse(row["OnHold"].ToString());
                        oPart.InActive = Boolean.Parse(row["InActive"].ToString());

                        oPart.AttBatch = row["AttBatch"].ToString();
                        oPart.AttMfgBatch = row["AttMfgBatch"].ToString();
                        oPart.AttMfgLot = row["AttMfgLot"].ToString();
                        oPart.AttHeat = row["AttHeat"].ToString();
                        oPart.AttFirmware = row["AttFirmware"].ToString();
                        oPart.AttBeforeDt = row["AttBeforeDt"].ToString();
                        oPart.AttMfgDt = row["AttMfgDt"].ToString();
                        oPart.AttCureDt = row["AttCureDt"].ToString();
                        oPart.AttExpDt = row["AttExpDt"].ToString();

                        Parts.Add(oPart);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Part listing not found. ";
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

        public bool _StockReplenishment(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWhseCode, string strBinNum, string strLotNum,  ref IList<StkRepl> StkRepls, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT Company, PartNum, WarehouseCode, BinNum, OnhandQty, LotNum, DimCode, AllocatedQty, ";
                _strSQL += "SalesAllocatedQty, SalesPickingQty, SalesPickedQty, JobAllocatedQty, JobPickingQty, JobPickedQty, ";
                _strSQL += "TFOrdAllocatedQty, TFOrdPickingQty, TFOrdPickedQty, ShippingQty, PackedQty, ";
                _strSQL += "PartLotDescription, OnHand, FirstRefDate, LastRefDate, LotLaborCost, LotBurdenCost, ";
                _strSQL += "LotMaterialCost, LotSubContCost, LotMtlBurCost, ExpirationDate, ShipDocAvail, Batch, MfgBatch, ";
                _strSQL += "MfgLot, HeatNum, FirmWare, BestBeforeDt, MfgDt, CureDt, ExpirationDate AS ExpireDt, FIFOLotLaborCost, FIFOLotBurdenCost, ";
                _strSQL += "FIFOLotMaterialCost, FIFOLotSubContCost, FIFOLotMtlBurCost, CSFLMWComOath, CSFCJ5FormNbr, CSFCJ5Vessel, ";
                _strSQL += "CSFCJ5ApvlStart, CSFCJ5ApvlEnd, ImportNum, ImportedFrom, ImportedFromDesc,PartDescription ";
                _strSQL += "FROM SDS_StockReplenishment ";
                _strSQL += "WHERE Company = '" + strCompany + "' ";

                if (strPartNum != null)
                {
                    _strSQL += "and Partnum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' ";
                }

                if (strWhseCode != null)
                {
                    _strSQL += "and WarehouseCode = '" + strWhseCode + "' ";
                }

                if (strBinNum != null)
                {
                    _strSQL += "and BinNum = '" + strBinNum + "' ";
                }

                if (strLotNum != null)
                {
                    _strSQL += "and LotNum = '" + strLotNum + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        StkRepl oStkRepl = new StkRepl();
                        oStkRepl.Company = row["Company"].ToString();
                        oStkRepl.PartNum = row["PartNum"].ToString();
                        oStkRepl.WarehouseCode = row["WarehouseCode"].ToString();
                        oStkRepl.BinNum = row["BinNum"].ToString();
                        oStkRepl.OnHandQty = Convert.ToDecimal(row["OnhandQty"]);
                        oStkRepl.LotNum = row["LotNum"].ToString();
                        oStkRepl.DimCode = row["DimCode"].ToString();
                        oStkRepl.AllocatedQty = Convert.ToDecimal(row["AllocatedQty"]);
                        oStkRepl.SalesAllocatedQty = Convert.ToDecimal(row["SalesAllocatedQty"]);
                        oStkRepl.SalesPickingQty = Convert.ToDecimal(row["SalesPickingQty"]);
                        oStkRepl.SalesPickedQty = Convert.ToDecimal(row["SalesPickedQty"]);
                        oStkRepl.JobAllocatedQty = Convert.ToDecimal(row["JobAllocatedQty"]);
                        oStkRepl.JobPickingQty = Convert.ToDecimal(row["JobPickingQty"]);
                        oStkRepl.JobPickedQty = Convert.ToDecimal(row["JobPickedQty"]);
                        oStkRepl.TFOrdAllocatedQty = Convert.ToDecimal(row["TFOrdAllocatedQty"]);
                        oStkRepl.TFOrdPickingQty = Convert.ToDecimal(row["TFOrdPickingQty"]);
                        oStkRepl.TFOrdPickedQty = Convert.ToDecimal(row["TFOrdPickedQty"]);
                        oStkRepl.ShippingQty = Convert.ToDecimal(row["ShippingQty"]);
                        oStkRepl.PackedQty = Convert.ToDecimal(row["PackedQty"]);
                        oStkRepl.PartLotDescription = row["PartLotDescription"].ToString();
                        oStkRepl.OnHand = (row["OnHand"].ToString() == "1" ? true : false);

                        oStkRepl.FirstRefDate = (DBNull.Value.Equals(row["FirstRefDate"]) ? "" : Convert.ToDateTime((row["FirstRefDate"])).ToString("yyyy-MM-dd"));
                        oStkRepl.LastRefDate =  (DBNull.Value.Equals(row["LastRefDate"]) ? "" : Convert.ToDateTime((row["LastRefDate"])).ToString("yyyy-MM-dd"));

                        oStkRepl.LotLaborCost = (DBNull.Value.Equals(row["LotLaborCost"]) ? 0: Convert.ToDecimal(row["LotLaborCost"]));
                        oStkRepl.LotBurdenCost = (DBNull.Value.Equals(row["LotBurdenCost"]) ? 0 : Convert.ToDecimal(row["LotBurdenCost"]));
                        oStkRepl.LotMaterialCost = (DBNull.Value.Equals(row["LotMaterialCost"]) ? 0 :Convert.ToDecimal(row["LotMaterialCost"]));
                        oStkRepl.LotSubContCost = (DBNull.Value.Equals(row["LotSubContCost"]) ? 0: Convert.ToDecimal(row["LotSubContCost"]));
                        oStkRepl.LotMtlBurCost = (DBNull.Value.Equals(row["LotMtlBurCost"]) ? 0 :Convert.ToDecimal(row["LotMtlBurCost"]));
                        oStkRepl.ExpirationDate = (DBNull.Value.Equals(row["ExpirationDate"])  ? "" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd"));
                        oStkRepl.ShipDocAvail = (row["ShipDocAvail"].ToString() == "1" ? true : false);
                        oStkRepl.Batch = row["Batch"].ToString();
                        oStkRepl.MfgBatch = row["MfgBatch"].ToString();
                        oStkRepl.MfgLot = row["MfgLot"].ToString();
                        oStkRepl.HeatNum = row["HeatNum"].ToString();
                        oStkRepl.FirmWare = row["FirmWare"].ToString();
                        oStkRepl.BestBeforeDt = (DBNull.Value.Equals(row["BestBeforeDt"])? "" : Convert.ToDateTime((row["BestBeforeDt"])).ToString("yyyy-MM-dd"));
                        oStkRepl.MfgDt = (DBNull.Value.Equals(row["MfgDt"]) ? "" : Convert.ToDateTime((row["MfgDt"])).ToString("yyyy-MM-dd"));
                        oStkRepl.CureDt = (DBNull.Value.Equals(row["CureDt"]) ? "" : Convert.ToDateTime((row["CureDt"])).ToString("yyyy-MM-dd"));
                        oStkRepl.ExpireDt = (DBNull.Value.Equals(row["ExpireDt"]) ? "": Convert.ToDateTime((row["ExpireDt"])).ToString("yyyy-MM-dd"));

                        oStkRepl.FIFOLotLaborCost = (DBNull.Value.Equals(row["FIFOLotLaborCost"]) ? 0: Convert.ToDecimal(row["FIFOLotLaborCost"]));
                        oStkRepl.FIFOLotBurdenCost = (DBNull.Value.Equals(row["FIFOLotBurdenCost"]) ? 0: Convert.ToDecimal(row["FIFOLotBurdenCost"]));
                        oStkRepl.FIFOLotMaterialCost = (DBNull.Value.Equals(row["FIFOLotMaterialCost"]) ? 0 : Convert.ToDecimal(row["FIFOLotMaterialCost"]));
                        oStkRepl.FIFOLotSubContCost = (DBNull.Value.Equals(row["FIFOLotSubContCost"]) ? 0 : Convert.ToDecimal(row["FIFOLotSubContCost"]));
                        oStkRepl.FIFOLotMtlBurCost = (DBNull.Value.Equals(row["FIFOLotMtlBurCost"]) ? 0 : Convert.ToDecimal(row["FIFOLotMtlBurCost"]));

                        oStkRepl.CSFLMWComOath = row["CSFLMWComOath"].ToString();
                        oStkRepl.CSFCJ5FormNbr = row["CSFCJ5FormNbr"].ToString();
                        oStkRepl.CSFCJ5Vessel = row["CSFCJ5Vessel"].ToString();
                        oStkRepl.CSFCJ5ApvlStart = (DBNull.Value.Equals(row["CSFCJ5ApvlStart"]) ? "" : Convert.ToDateTime((row["CSFCJ5ApvlStart"])).ToString("yyyy-MM-dd"));
                        oStkRepl.CSFCJ5ApvlEnd = (DBNull.Value.Equals(row["CSFCJ5ApvlEnd"]) ? "" : Convert.ToDateTime((row["CSFCJ5ApvlEnd"])).ToString("yyyy-MM-dd"));
                        oStkRepl.ImportNum = row["ImportNum"].ToString();
                        oStkRepl.ImportedFrom = (DBNull.Value.Equals(row["ImportedFrom"]) ? 0 : Convert.ToInt32(row["ImportedFrom"]));
                        oStkRepl.ImportedFromDesc = row["ImportedFromDesc"].ToString();

                        oStkRepl.PartDescription = row["PartDescription"].ToString();


                        StkRepls.Add(oStkRepl);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Stock Replenishment listing not found. ";
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


        public bool _StockReplenishment2(ref EpicEnv oEpicEnv, string strCompany, string strCurPlant, string strUID, string strPartNum, string strWhseCode, string strAgency, ref IList<StkRepl2> StkRepls, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT s.Company, w.Plant, s.PartNum, s.WarehouseCode, s.OnhandQty, s.DimCode, s.MinimumQty, s.SOQty, s.SugQty, p.ProdCode, p.PartDescription ";
                _strSQL += "FROM StockReplenishment s ";
                _strSQL += "inner join erp.Part p on s.Company = p.Company and s.PartNum = p.PartNum ";
                _strSQL += "inner join erp.Warehse w on s.Company = w.Company and s.WarehouseCode = w.WarehouseCode ";
                _strSQL += "WHERE s.Company = '" + strCompany + "' ";
				// _strSQL += "AND w.Plant in (select value from Erp.UserComp cross apply string_split(PlantList, '~') ";
				_strSQL += "AND w.Plant = '" + strCurPlant + "' ";
				// _strSQL += "where DcdUserID = '" + strUID + "' and Company = '" + strCompany + "')";

				if (strPartNum != null)
                {
                    _strSQL += "and s.Partnum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' ";
                }

                if (strWhseCode != null)
                {
                    _strSQL += "and s.WarehouseCode = '" + strWhseCode + "' ";
                }

                if (strAgency != null)
                {
                    _strSQL += "and p.ProdCode = '" + strAgency + "' ";
                }


                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        StkRepl2 oStkRepl = new StkRepl2();
                        oStkRepl.Company = row["Company"].ToString();
                        oStkRepl.PartNum = row["PartNum"].ToString();
                        oStkRepl.WarehouseCode = row["WarehouseCode"].ToString();
                        oStkRepl.OnHandQty = Convert.ToDecimal(row["OnhandQty"]);
                        oStkRepl.DimCode = row["DimCode"].ToString();
                        oStkRepl.MinimumQty = Convert.ToDecimal(row["MinimumQty"]);
                        oStkRepl.SalesOrderQty = Convert.ToDecimal(row["SOQty"]);
                        oStkRepl.SuggestQty = Convert.ToDecimal(row["SugQty"]);
                        oStkRepl.Agency = row["ProdCode"].ToString();
                        oStkRepl.PartDescription = row["PartDescription"].ToString();

                        StkRepls.Add(oStkRepl);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Stock Replenishment listing not found. ";
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


        public bool _StockReplenishment2Sub(ref EpicEnv oEpicEnv, string strCompany, string strPartNum, string strWarehouse,  ref IList<StkRepl2Sub> StkReplSubs, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT pb.Company, pb.BinNum, pb.LotNum, pb.OnhandQty, pb.DimCode, pl.ExpirationDate, pb.PartNum, pb.WarehouseCode, wb.ZoneID ";
                _strSQL += "from Erp.PartBin pb ";
                //_strSQL += "inner join Erp.Part p on pb.company = p.company and pb.partnum = p.partnum ";
                _strSQL += "inner join Erp.PartLot pl on pb.Company = pl.Company and pb.PartNum = pl.PartNum and pb.LotNum = pl.LotNum ";
                _strSQL += "inner join Erp.WhseBin wb on pb.Company = wb.Company and pb.WarehouseCode = wb.WarehouseCode and pb.BinNum = wb.BinNum ";
                _strSQL += "WHERE pb.Company = '" + strCompany + "' ";

                if (strPartNum != null)
                {
                    _strSQL += "and pb.Partnum = '" + oSpecialChar.replaceSpecialChar(strPartNum) + "' ";
                }

                if (strWarehouse != null)
                {
                    _strSQL += "and pb.WarehouseCode = '" + strWarehouse + "' ";
                }

                //add order by --Gary
                //_strSQL += "ORDER BY wb.SD_Level_c, wb.SD_Zone_c, wb.SD_Rack_c, wb.SD_BinNo_c ";

                SQLServerBO _MSSQL = new SQLServerBO();
                string _strSQLCon = _MSSQL._retSQLConnectionString();
                _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                DataSet _dts = _MSSQL._MSSQLDataSetResult(_strSQL, _strSQLCon);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        StkRepl2Sub oStkRepl = new StkRepl2Sub();
                        oStkRepl.Company = row["Company"].ToString();
                        oStkRepl.PartNum = row["PartNum"].ToString();
                        oStkRepl.WarehouseCode = row["WarehouseCode"].ToString();
                        oStkRepl.OnHandQty = Convert.ToDecimal(row["OnhandQty"]);
                        oStkRepl.DimCode = row["DimCode"].ToString();
                        oStkRepl.BinNum = row["BinNum"].ToString();
                        oStkRepl.LotNum = row["LotNum"].ToString();
                        oStkRepl.ExpirationDate = (DBNull.Value.Equals(row["ExpirationDate"]) ? "" : Convert.ToDateTime((row["ExpirationDate"])).ToString("yyyy-MM-dd"));
                        oStkRepl.ZoneID = row["ZoneID"].ToString();

                        StkReplSubs.Add(oStkRepl);
                    }

                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Stock Replenishment Sub listing not found. ";
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