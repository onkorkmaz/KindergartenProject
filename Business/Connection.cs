using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Business
{
    public class Connection
    {
        private static bool isServer = true;
        private static SqlConnection connection = null;

        public static SqlConnection Conn
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

                return connection;
            }
        }
    }
}