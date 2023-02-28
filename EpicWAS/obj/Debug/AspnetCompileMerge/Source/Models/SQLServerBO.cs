using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EpicWAS.Models
{
    public class SQLServerBO
    {
        public DataSet _MSSQLDataSetResult(string strQuery, string strSQLCon)
        {
            
            using (SqlConnection _sqlCon = new SqlConnection(strSQLCon))
            {
                _sqlCon.Open();

                SqlDataAdapter _sqlAdp = new SqlDataAdapter(strQuery, _sqlCon);
                DataSet _dt = new DataSet();

                _sqlAdp.Fill(_dt);

                _sqlCon.Close();

                return _dt;

            } // end for sqlconnection

        }

        public bool _exeSQLCommand(string strQuery, string strSQLCon)
        {
            try
            {
                using (SqlConnection _sqlCon = new SqlConnection(strSQLCon))
                {
                    _sqlCon.Open();

                    using (SqlCommand sqlCmd = new SqlCommand(strQuery, _sqlCon))
                    {

                        sqlCmd.ExecuteNonQuery();
                    }

                    _sqlCon.Close();
                }

                return true;

            } // end try
            catch (Exception ex)
            {
                return false;
            }

        }


        public string _retSQLConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["mssqldb"].ToString();
        }




    }
}