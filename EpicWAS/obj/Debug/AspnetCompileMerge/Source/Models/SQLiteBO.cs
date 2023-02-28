using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace EpicWAS.Models
{
    public class SQLiteBO
    {
        public DataSet _SQLiteDataSetResult(string strQuery)
        {
            _chkLocalDBFileExist();
            string _strSQLiteCn = _retSQLiteConnectionString();
            
            using (SQLiteConnection _sqliteCn = new SQLiteConnection(_strSQLiteCn))
            {
                _sqliteCn.Open();

                SQLiteDataAdapter _sqliteAdp = new SQLiteDataAdapter(strQuery, _sqliteCn);
                DataSet _dt = new DataSet();
                _sqliteAdp.Fill(_dt);

                _sqliteCn.Close();

                return _dt;

            } // end sqlite connection
        }

        public bool _exeSQLiteCommand(string strQuery)
        {
            string _strSQLiteCn = _retSQLiteConnectionString();

            try
            {
                using (SQLiteConnection _sqliteCn = new SQLiteConnection(_strSQLiteCn))
                {
                    _sqliteCn.Open();

                    using (SQLiteCommand sqliteCmd = new SQLiteCommand(strQuery, _sqliteCn))
                    {

                        sqliteCmd.ExecuteNonQuery();
                    }

                    _sqliteCn.Close();
                }

                return true;

            } // end try
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool _chkForTableExist(string strTableName)
        {
            string _strSQLiteCn = _retSQLiteConnectionString();
            string _strSQL = string.Format("SELECT count(1) FROM sqlite_master WHERE type = 'table' AND name = '{0}'", strTableName);
            int _retRow;

            using (SQLiteConnection _sqliteCn = new SQLiteConnection(_strSQLiteCn))
            {
                _sqliteCn.Open();

                using (SQLiteCommand _sqliteCmd = new SQLiteCommand(_strSQL, _sqliteCn))
                {
                    _retRow = Convert.ToInt32( _sqliteCmd.ExecuteScalar());
                }// end for sqlitecommand

                _sqliteCn.Close();

            } // end sqlite connection

            return (_retRow > 0 ? true : false);

        }

        private void _chkLocalDBFileExist()
        {
            if (!File.Exists(@"C:\\EpicWAS\\EpicWAS.db"))
            {
                SQLiteConnection.CreateFile(@"C:\\EpicWAS\\EpicWAS.db");
            } // end if

        }

        private string _retSQLiteConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["localdb"].ToString();
        }


        public void _chkEpicEnvTable()
        {
            string strEpicEnvTableName = "EpicEnv";

            if (_chkForTableExist(strEpicEnvTableName) == false)
            {
                string strCreateCmd = "create table EpicEnv ( ";
                strCreateCmd += "Env_ID varchar(10), Env_Description varchar(100), Env_IsActive varchar(1), ";
                strCreateCmd += "Env_AppServer varchar(100), Env_AppEpicor varchar(100), Env_AppUserId varchar(100), Env_AppPassKey varchar(100), ";
                strCreateCmd += "Env_SQLServer varchar(100), Env_SQLDB varchar(100), Env_SQLUserId varchar(100), Env_SQLPassKey varchar(100) ";
                strCreateCmd += ") ";

                _exeSQLiteCommand(strCreateCmd);

            } // end for if

        }

        public void Dispose(bool v)
        {
            throw new NotImplementedException();
        }

    }
}