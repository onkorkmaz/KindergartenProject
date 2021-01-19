using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Entity;

namespace Business
{
    public class AdminBusiness
    {
        public ResultSet GetAdmin()
        {
            ResultSet resultSet = DataProcess.ExecuteNonQuery("select * from tbAdmin");
            return resultSet;
        }
    }
}
