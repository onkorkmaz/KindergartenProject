using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace Business
{
    internal class Connection
    {
        private static SqlConnection _conn;

        public static SqlConnection Conn
        {
            get
            {
                if (_conn == null || _conn.State != System.Data.ConnectionState.Open)
                {
                    _conn = initalizeConnection();
                }
                return _conn;
            }
        }

        private static SqlConnection initalizeConnection()
        {
            _conn = new SqlConnection("server=DESKTOP-U63HM5J;database=dbNovermber;Integrated Security=True;");
            _conn.Open();
            return _conn;
        }
    }
}
