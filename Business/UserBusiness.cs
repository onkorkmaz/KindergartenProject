using Entity;

namespace Business
{
    public class UserBusiness
    {
        public ResultSet GetUser()
        {
            ResultSet result = DataProcess.ExecuteNonQuery("select * from tbUser");
            return result;
        }
    }
}
