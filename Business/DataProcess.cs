using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Entity;

namespace Business
{
    internal class DataProcess
    {
        public static ResultSet ExecuteNonQuery(string sql)
        {
            ResultSet resultSet = new ResultSet();

            try
            {
                resultSet.IsValid = true;
                DataTable dataTable = new DataTable();
                SqlCommand cmd = new SqlCommand(sql, Connection.Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);

                resultSet.DT = dataTable;

            }
            catch (Exception ex)
            {
                resultSet.IsValid = false;
                resultSet.ExceptionMessage = ex.Message;
                resultSet.Exception = ex;
            }

            return resultSet;
        }

    }
}
