using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Common;

namespace Business
{
    public class DataProcess
    {
        public static DataResultArgs<DataSet> ExecuteProcDataSet(SqlConnection connection, SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<DataSet> dtrslt = new DataResultArgs<DataSet>();
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection =  connection;
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

        internal static void ControlAdminAuthorization(bool isProcess = false)
        {
            if (isProcess)
            {
                if(CurrentContex.Contex.AdminTypeEnum != AdminTypeEnum.SuperAdmin)
                {
                    throw new Exception("Yetkiniz bulunmamaktadır!");
                }
            }
        }

        public static DataResultArgs<Boolean> ExecuteProc(SqlConnection connection, SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<Boolean> dtrslt = new DataResultArgs<Boolean>();
            try
            {
                cmd.Connection = connection;
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
        public static DataResultArgs<String> ExecuteProcString(SqlConnection connection, SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<String> dtrslt = new DataResultArgs<String>();
            try
            {
                cmd.Connection = connection;
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
        public static DataResultArgs<SqlDataReader> ExecuteProcDataReader(SqlConnection connection, SqlCommand cmd, string storeProcedureName)
        {
            DataResultArgs<SqlDataReader> dtrslt = new DataResultArgs<SqlDataReader>();
            try
            {
                cmd.Connection = connection;
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
        public static DataResultArgs<Boolean> ExecuteNonQuery(SqlConnection connection, string query)
        {
            DataResultArgs<Boolean> dtrslt = new DataResultArgs<Boolean>();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
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
        public static DataResultArgs<DataSet> ExecuteNonQueryDataSet(SqlConnection connection, string query)
        {
            DataResultArgs<DataSet> dtrslt = new DataResultArgs<DataSet>();
            try
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand(query, connection))
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

        public static DataResultArgs<DataTable> ExecuteNonQueryDataTable(SqlConnection connection, string query)
        {
            DataResultArgs<DataTable> dtrslt = new DataResultArgs<DataTable>();

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

            return dtrslt;
        }

        public static DataResultArgs<String> ExecuteNonQueryString(SqlConnection connection, string query)
        {
            DataResultArgs<String> dtrslt = new DataResultArgs<String>();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
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
        public static DataResultArgs<SqlDataReader> ExecuteNonQueryDataReader(SqlConnection connection, string query)
        {
            DataResultArgs<SqlDataReader> dtrslt = new DataResultArgs<SqlDataReader>();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
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
        public static DataResultArgs<Boolean> ExecuteCommand(SqlConnection connection, SqlCommand cmd)
        {
            DataResultArgs<Boolean> dtrslt = new DataResultArgs<Boolean>();
            try
            {
                cmd.Connection = connection;
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
        public static DataResultArgs<DataSet> ExecuteCommandDataSet(SqlConnection connection, SqlCommand cmd)
        {
            DataResultArgs<DataSet> dtrslt = new DataResultArgs<DataSet>();
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection = connection;
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
        public static DataResultArgs<DataTable> ExecuteCommandDataTable(SqlConnection connection, SqlCommand cmd)
        {
            DataResultArgs<DataTable> dtrslt = new DataResultArgs<DataTable>();
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection = connection;
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
        public static DataResultArgs<String> ExecuteCommandString(SqlConnection connection, SqlCommand cmd)
        {
            DataResultArgs<String> dtrslt = new DataResultArgs<String>();
            try
            {
                cmd.Connection = connection;
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
        public static DataResultArgs<SqlDataReader> ExecuteCommandDataReader(SqlConnection connection, SqlCommand cmd)
        {
            DataResultArgs<SqlDataReader> dtrslt = new DataResultArgs<SqlDataReader>();
            try
            {
                cmd.Connection = connection;
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