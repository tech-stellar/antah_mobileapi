using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace EpicWAS.Models
{
    public class EpicEnvBO
    {
        public bool _LoadEpicEnv(bool blnIsActiveOnly, ref IList<EpicEnv> lstEpicEnv , out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT * FROM EpicEnv ";
                _strSQL += (blnIsActiveOnly == true? "WHERE Env_IsActive = '1' " : "");

                SQLiteBO _sqlite = new SQLiteBO();
                _sqlite._chkEpicEnvTable();

                DataSet _dts = _sqlite._SQLiteDataSetResult(_strSQL);

                foreach (DataRow row in _dts.Tables[0].Rows)
                {
                    EpicEnv _Epic = new EpicEnv();
                    _Epic.Env_ID = row["Env_ID"].ToString();
                    _Epic.Env_Description = row["Env_Description"].ToString();
                    _Epic.Env_IsActive = ( row["Env_IsActive"].ToString() == "1"? true : false);

                    _Epic.Env_AppServer = row["Env_AppServer"].ToString();
                    _Epic.Env_AppEpicor = row["Env_AppEpicor"].ToString();
                    _Epic.Env_AppUserId = row["Env_AppUserId"].ToString();
                    _Epic.Env_AppPassKey = row["Env_AppPassKey"].ToString();

                    _Epic.Env_SQLServer = row["Env_SQLServer"].ToString();
                    _Epic.Env_SQLDB = row["Env_SQLDB"].ToString();
                    _Epic.Env_SQLUserId = row["Env_SQLUserId"].ToString();
                    _Epic.Env_SQLPassKey = row["Env_SQLPassKey"].ToString();
                    _Epic.Env_BarCodeSeperator = row["Env_BarCodeSeperator"].ToString();
                    _Epic.Env_BarCodeSeperator2 = row["Env_BarCodeSeperator2"].ToString();
                    _Epic.Env_UsedEpicLogin = (row["Env_UsedEpicLogin"].ToString() == "1" ? true : false);
                    _Epic.Env_RESTAPIURL = row["Env_RESTURL"].ToString();
                    _Epic.Env_RESTAPIKEY = row["Env_RESTKEY"].ToString();

                    lstEpicEnv.Add(_Epic);

                } // end for foreach loop
                strMessage = "";
                IsError = false;

                _sqlite.Dispose(true);

            } // end for try
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _LoadEpicEnvById(string strEpicEnvId, ref EpicEnv oEpicEnv, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "SELECT * FROM EpicEnv ";
                _strSQL += string.Format ("WHERE Env_ID = '{0}' ", strEpicEnvId);

                SQLiteBO _sqlite = new SQLiteBO();
                _sqlite._chkEpicEnvTable();

                DataSet _dts = _sqlite._SQLiteDataSetResult(_strSQL);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _dts.Tables[0].Rows)
                    {
                        oEpicEnv.Env_ID = row["Env_ID"].ToString();
                        oEpicEnv.Env_Description = row["Env_Description"].ToString();
                        oEpicEnv.Env_IsActive = (row["Env_IsActive"].ToString() == "1" ? true :false);

                        oEpicEnv.Env_AppServer = row["Env_AppServer"].ToString();
                        oEpicEnv.Env_AppEpicor = row["Env_AppEpicor"].ToString();
                        oEpicEnv.Env_AppUserId = row["Env_AppUserId"].ToString();
                        oEpicEnv.Env_AppPassKey = row["Env_AppPassKey"].ToString();

                        oEpicEnv.Env_SQLServer = row["Env_SQLServer"].ToString();
                        oEpicEnv.Env_SQLDB = row["Env_SQLDB"].ToString();
                        oEpicEnv.Env_SQLUserId = row["Env_SQLUserId"].ToString();
                        oEpicEnv.Env_SQLPassKey = row["Env_SQLPassKey"].ToString();
                        oEpicEnv.Env_BarCodeSeperator = row["Env_BarCodeSeperator"].ToString();
                        oEpicEnv.Env_BarCodeSeperator2 = row["Env_BarCodeSeperator2"].ToString();
                        oEpicEnv.Env_UsedEpicLogin = (row["Env_UsedEpicLogin"].ToString() == "1" ? true : false);

                        oEpicEnv.Env_RESTAPIURL = row["Env_RESTURL"].ToString();
                        oEpicEnv.Env_RESTAPIKEY = row["Env_RESTKEY"].ToString();

                    } // end for foreach loop
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Epicor environment setup not found. ";
                    IsError = true;
                }

                //_sqlite.Dispose(true);
            } // end for try
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _EditEpicEnv(ref EpicEnv oEpicEnv, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "UPDATE EpicEnv SET ";
                _strSQL += "Env_Description='{0}', Env_IsActive='{1}',  ";
                _strSQL += "Env_AppServer='{2}', Env_AppEpicor='{3}', Env_AppUserId='{4}', Env_AppPassKey='{5}', ";
                _strSQL += "Env_SQLServer='{6}', Env_SQLDB = '{7}', Env_SQLUserId='{8}', Env_SQLPassKey='{9}', Env_BarCodeSeperator = '{11}', ";
                _strSQL += "Env_BarCodeSeperator2 = '{12}', Env_UsedEpicLogin = '{13}', Env_RESTURL = '{14}', Env_RESTKEY = '{15}' ";
                _strSQL += "where Env_ID ='{10}' ";
                _strSQL = string.Format(_strSQL, oEpicEnv.Env_Description, (oEpicEnv.Env_IsActive ? "1" : "0"),
                                                  oEpicEnv.Env_AppServer, oEpicEnv.Env_AppEpicor, oEpicEnv.Env_AppUserId, oEpicEnv.Env_AppPassKey,
                                                  oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId,
                                                  oEpicEnv.Env_SQLPassKey, oEpicEnv.Env_ID, oEpicEnv.Env_BarCodeSeperator,
                                                  oEpicEnv.Env_BarCodeSeperator2, oEpicEnv.Env_UsedEpicLogin, oEpicEnv.Env_RESTAPIURL, oEpicEnv.Env_RESTAPIKEY);

                SQLiteBO _sqlite = new SQLiteBO();
                _sqlite._chkEpicEnvTable();

                if (_sqlite._exeSQLiteCommand(_strSQL))
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Module: _EditEpicEnv. Error";
                    IsError = true;
                }

                //_sqlite.Dispose(true);

            }
            catch (Exception ex)
            {
                strMessage = "Module: _EditEpicEnv. " + ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _NewEpicEnv(ref EpicEnv oEpicEnv, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "INSERT INTO EpicEnv (";
                _strSQL += "Env_ID, Env_Description, Env_IsActive,  ";
                _strSQL += "Env_AppServer, Env_AppEpicor, Env_AppUserId, Env_AppPassKey,";
                _strSQL += "Env_SQLServer, Env_SQLDB, Env_SQLUserId, Env_SQLPassKey, ";
                _strSQL += "Env_BarCodeSeperator, Env_BarCodeSeperator2, Env_UsedEpicLogin, Env_RESTURL, Env_RESTKEY ";
                _strSQL += ") VALUES ( ";
                _strSQL += "'{0}','{1}', '{2}',";
                _strSQL += "'{3}', '{4}', '{5}', '{6}',";
                _strSQL += "'{7}', '{8}', '{9}', '{10}', ";
                _strSQL += "'{11}', '{12}', '{13}', '{14}', '{15}' ) ";
                _strSQL = string.Format(_strSQL, oEpicEnv.Env_ID, oEpicEnv.Env_Description, (oEpicEnv.Env_IsActive ? "1" : "0"),
                                                  oEpicEnv.Env_AppServer, oEpicEnv.Env_AppEpicor, oEpicEnv.Env_AppUserId, oEpicEnv.Env_AppPassKey,
                                                  oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey,
                                                  oEpicEnv.Env_BarCodeSeperator, oEpicEnv.Env_BarCodeSeperator2, oEpicEnv.Env_UsedEpicLogin, oEpicEnv.Env_RESTAPIURL, oEpicEnv.Env_RESTAPIKEY);

                SQLiteBO _sqlite = new SQLiteBO();
                _sqlite._chkEpicEnvTable();

                if (_sqlite._exeSQLiteCommand(_strSQL))
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Module: _NewEpicEnv. Error";
                    IsError = true;
                }

                // _sqlite.Dispose(true);

            }
            catch (Exception ex)
            {
                strMessage = "Module: _NewEpicEnv. " + ex.Message.ToString();
                IsError = true;
            }

            return (IsError ? false : true);
        }

        public bool _RemoveEpicEnv(ref EpicEnv oEpicEnv, out string strMessage)
        {
            bool IsError = false;

            try
            {
                string _strSQL = "DELETE FROM EpicEnv ";
                _strSQL += "WHERE Env_ID = '{0}' ";
                _strSQL = string.Format(_strSQL, oEpicEnv.Env_ID);


                SQLiteBO _sqlite = new SQLiteBO();
                _sqlite._chkEpicEnvTable();

                if (_sqlite._exeSQLiteCommand(_strSQL))
                {
                    strMessage = "";
                    IsError = false;
                }
                else
                {
                    strMessage = "Module: _RemoveEpicEnv. Error";
                    IsError = true;
                }

                // _sqlite.Dispose(true);

            }
            catch (Exception ex)
            {
                strMessage = "Module: _RemoveEpicEnv. " + ex.Message.ToString();
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