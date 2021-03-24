using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Business
{
    public class Connection
    {
        private bool isServer = true;
        private static SqlConnection connection = null;
        public SqlConnection Conn
        {
            get
            {
                if (isServer)
                {
                    connection =
                        new SqlConnection(
                            "Data Source=178.210.171.41,13000; Initial Catalog=dbKinderGarten; User ID=onkor;Password=ONkor123");
                }
                else
                {
                    connection =
                        new SqlConnection(
                            "Data Source=DESKTOP-U63HM5J; Initial Catalog=dbKinderGarten; Integrated Security=True");
                }

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