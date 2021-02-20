using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Business
{
    public class Connection
    {
        private static SqlConnection connection = null;
        public SqlConnection Conn
        {
            get
            {
                connection = new SqlConnection("Data Source=DESKTOP-U63HM5J; Initial Catalog=dbKinderGarten; Integrated Security=True");
                connection.Open();
                return connection;
            }
        }
        public void CloseConn(SqlConnection Connection)
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }
    }
}