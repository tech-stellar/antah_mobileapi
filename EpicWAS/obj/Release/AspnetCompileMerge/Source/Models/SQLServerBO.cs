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

        public bool _exeStoredProcedureCommand(string strQuery, string strSQLCon, params IDataParameter[] sqlParams)
        {
            try
            {
                using (SqlConnection _sqlCon = new SqlConnection(strSQLCon))
                {
                    _sqlCon.Open();

                    using (SqlCommand sqlCmd = new SqlCommand(strQuery, _sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        foreach (IDataParameter para in sqlParams)
                        {
                            sqlCmd.Parameters.Add(para);
                        }


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







        public DataSet _MSSQLDataSetResultStoreProcedure(string strProcedureName, string strSQLCon, params IDataParameter[] sqlParams)
        {
            using (SqlConnection _sqlCon = new SqlConnection(strSQLCon))
            {
                _sqlCon.Open();

                SqlCommand _sqlCmd = new SqlCommand(strProcedureName, _sqlCon);

                _sqlCmd.CommandType = CommandType.StoredProcedure;

                foreach (IDataParameter para in sqlParams)
                {
                    _sqlCmd.Parameters.Add(para);
                }

                SqlDataAdapter _sqlAdp = new SqlDataAdapter();
                _sqlAdp.SelectCommand = _sqlCmd;

                DataSet _dt = new DataSet();

                _sqlAdp.Fill(_dt);

                _sqlCon.Close();

                return _dt;

            } // end for sqlconnection        

        }






        public bool _exeSDSCreateLabelSP(string strSQLCon, string strCompany, string strEntryBy, string strTranType, DateTime dtTranDate, string strPartNum,
                                        string strUOM, string strLotNum, decimal iTranQty, int iLabelQty, string strParm01, string strParm02, string strParm03)
        {
            try
            {
                using (SqlConnection _sqlCon = new SqlConnection(strSQLCon))
                {
                    _sqlCon.Open();

                    SqlCommand _sqlCmd = new SqlCommand();
                    _sqlCmd.CommandText = "dbo.SDSCreateLabel";
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    _sqlCmd.Connection = _sqlCon;

                    _sqlCmd.Parameters.Add(new SqlParameter("@Company", strCompany));
                    _sqlCmd.Parameters.Add(new SqlParameter("@EntryBy", strEntryBy));
                    _sqlCmd.Parameters.Add(new SqlParameter("@TranType", strTranType));
                    _sqlCmd.Parameters.Add(new SqlParameter("@TranDate", dtTranDate.ToString("yyyy-MM-dd")));
                    _sqlCmd.Parameters.Add(new SqlParameter("@PartNum", strPartNum));
                    _sqlCmd.Parameters.Add(new SqlParameter("@UOM", strUOM));
                    _sqlCmd.Parameters.Add(new SqlParameter("@LotNum", strLotNum));
                    _sqlCmd.Parameters.Add(new SqlParameter("@TranQty", iTranQty));
                    _sqlCmd.Parameters.Add(new SqlParameter("@LabelQty", iLabelQty));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm01", strParm01));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm02", strParm02));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm03", strParm03));

                    _sqlCmd.ExecuteNonQuery();

                    _sqlCon.Close();
                }

                return true;

            } // end try
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool _exeSDSCreateLabelSP_Reprint(string strSQLCon, string strCompany, string strEntryBy, string strTranType, DateTime dtTranDate, string strPartNum,
                                        string strUOM, string strLotNum, decimal iTranQty, int iLabelQty, string strParm01, string strParm02, string strParm03, string strParm04="")
        {
            try
            {
                using (SqlConnection _sqlCon = new SqlConnection(strSQLCon))
                {
                    _sqlCon.Open();

                    SqlCommand _sqlCmd = new SqlCommand();
                    _sqlCmd.CommandText = "dbo.SDSCreateLabel_Reprint";
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    _sqlCmd.Connection = _sqlCon;

                    _sqlCmd.Parameters.Add(new SqlParameter("@Company", strCompany));
                    _sqlCmd.Parameters.Add(new SqlParameter("@EntryBy", strEntryBy));
                    _sqlCmd.Parameters.Add(new SqlParameter("@TranType", strTranType));
                    _sqlCmd.Parameters.Add(new SqlParameter("@TranDate", dtTranDate.ToString("yyyy-MM-dd")));
                    _sqlCmd.Parameters.Add(new SqlParameter("@PartNum", strPartNum));
                    _sqlCmd.Parameters.Add(new SqlParameter("@UOM", strUOM));
                    _sqlCmd.Parameters.Add(new SqlParameter("@LotNum", strLotNum));
                    _sqlCmd.Parameters.Add(new SqlParameter("@TranQty", iTranQty));
                    _sqlCmd.Parameters.Add(new SqlParameter("@LabelQty", iLabelQty));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm01", strParm01));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm02", strParm02));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm03", strParm03));
                    _sqlCmd.Parameters.Add(new SqlParameter("@Parm04", strParm04));

                    _sqlCmd.ExecuteNonQuery();

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