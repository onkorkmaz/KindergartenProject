using Common;
using System;
using System.Collections.Generic;
using System.Text;
using Business;
using System.Data.SqlClient;

namespace Business
{
    public class LogBusiness : BaseBusiness
    {
        public LogBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public void InsertLog(string message)
        {
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 
            string stmt = "INSERT INTO log.tbLog VALUES('" + message + "','" + time.ToString(format) + "'," + base.ProjectTypeInt16 + ")";

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataProcess.ExecuteNonQuery(con, stmt);
                con.Close();
                con.Dispose();
            }
        }
    
    }
}
