using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Business
{
    public class DataProcess
    {
        public static DataResultArgs<DataSet> ExecuteProcDataSet(SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<DataSet> dtrslt = new DataResultArgs<DataSet>();
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection = new Connection().Conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                sadp.Fill(ds);
                dtrslt.Result = ds;
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.Result = null;
                dtrslt.ErrorCode = 0;
                dtrslt.HasError = true;
                dtrslt.ErrorDescription = dbException.Message.ToString();
            }
            return dtrslt;
        }
        public static DataResultArgs<Boolean> ExecuteProc(SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<Boolean> dtrslt = new DataResultArgs<Boolean>();
            try
            {
                cmd.Connection = new Connection().Conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.ExecuteNonQuery();
                dtrslt.Result = true;
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = false;
            }
            return dtrslt;
        }
        public static DataResultArgs<String> ExecuteProcString(SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<String> dtrslt = new DataResultArgs<String>();
            try
            {
                cmd.Connection = new Connection().Conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                dtrslt.Result = cmd.ExecuteScalar().ToString();
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = "";
            }
            return dtrslt;
        }
        public static DataResultArgs<SqlDataReader> ExecuteProcDataReader(SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<SqlDataReader> dtrslt = new DataResultArgs<SqlDataReader>();
            try
            {
                cmd.Connection = new Connection().Conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                dtrslt.Result = cmd.ExecuteReader();
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = null;
            }
            return dtrslt;
        }
        public static DataResultArgs<Boolean> ExecuteNonQuery(string query)
        {
            DataResultArgs<Boolean> dtrslt = new DataResultArgs<Boolean>();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, new Connection().Conn))
                {
                    cmd.ExecuteNonQuery();
                    dtrslt.Result = true;
                }
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = false;
            }
            return dtrslt;
        }
        public static DataResultArgs<DataSet> ExecuteNonQueryDataSet(string query)
        {
            DataResultArgs<DataSet> dtrslt = new DataResultArgs<DataSet>();
            try
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand(query, new Connection().Conn))
                {
                    SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                    sadp.Fill(ds);
                    dtrslt.Result = ds;
                }
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = null;
            }
            return dtrslt;
        }
        public static DataResultArgs<DataTable> ExecuteNonQueryDataTable(string query)
        {
            DataResultArgs<DataTable> dtrslt = new DataResultArgs<DataTable>();
            SqlConnection connection = new Connection().Conn;
            {
                try
                {
                    DataTable ds = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                        sadp.Fill(ds);
                        dtrslt.Result = ds;
                    }
                }
                catch (System.Data.Common.DbException dbException)
                {
                    dtrslt.ErrorCode = 0;
                    dtrslt.ErrorDescription = dbException.Message.ToString();
                    dtrslt.HasError = true;
                    dtrslt.Result = null;
                }
            }
            return dtrslt;
        }
        public static DataResultArgs<String> ExecuteNonQueryString(string query)
        {
            DataResultArgs<String> dtrslt = new DataResultArgs<String>();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, new Connection().Conn))
                {
                    dtrslt.Result = (cmd.ExecuteScalar() == null) ? null : cmd.ExecuteScalar().ToString();
                }
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = "";
            }
            return dtrslt;
        }
        public static DataResultArgs<SqlDataReader> ExecuteNonQueryDataReader(string query)
        {
            DataResultArgs<SqlDataReader> dtrslt = new DataResultArgs<SqlDataReader>();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, new Connection().Conn))
                {
                    dtrslt.Result = cmd.ExecuteReader();
                }
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = null;
            }
            return dtrslt;
        }
        public static DataResultArgs<Boolean> ExecuteCommand(SqlCommand cmd)
        {
            DataResultArgs<Boolean> dtrslt = new DataResultArgs<Boolean>();
            try
            {
                cmd.Connection = new Connection().Conn;
                cmd.ExecuteNonQuery();
                dtrslt.Result = true;
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = false;
            }
            return dtrslt;
        }
        public static DataResultArgs<DataSet> ExecuteCommandDataSet(SqlCommand cmd)
        {
            DataResultArgs<DataSet> dtrslt = new DataResultArgs<DataSet>();
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection = new Connection().Conn;
                SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                sadp.Fill(ds);
                dtrslt.Result = ds;
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = null;
            }
            return dtrslt;
        }
        public static DataResultArgs<DataTable> ExecuteCommandDataTable(SqlCommand cmd)
        {
            DataResultArgs<DataTable> dtrslt = new DataResultArgs<DataTable>();
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection = new Connection().Conn;
                SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                sadp.Fill(ds);
                dtrslt.Result = ds.Tables[0];
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = null;
            }
            return dtrslt;
        }
        public static DataResultArgs<String> ExecuteCommandString(SqlCommand cmd)
        {
            DataResultArgs<String> dtrslt = new DataResultArgs<String>();
            try
            {
                cmd.Connection = new Connection().Conn;
                dtrslt.Result = (cmd.ExecuteScalar() == null) ? null : cmd.ExecuteScalar().ToString();
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = "";
            }
            return dtrslt;
        }
        public static DataResultArgs<SqlDataReader> ExecuteCommandDataReader(SqlCommand cmd)
        {
            DataResultArgs<SqlDataReader> dtrslt = new DataResultArgs<SqlDataReader>();
            try
            {
                cmd.Connection = new Connection().Conn;
                dtrslt.Result = cmd.ExecuteReader();
            }
            catch (System.Data.Common.DbException dbException)
            {
                dtrslt.ErrorCode = 0;
                dtrslt.ErrorDescription = dbException.Message.ToString();
                dtrslt.HasError = true;
                dtrslt.Result = null;
            }
            return dtrslt;
        }
    }
}