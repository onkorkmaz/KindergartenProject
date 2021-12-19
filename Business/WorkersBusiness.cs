using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Common;
namespace Business
{
    public class WorkersBusiness
    {
        public DataResultArgs<bool> Set_Worker(WorkerEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@surname", entity.Surname);
                cmd.Parameters.AddWithValue("@isManager", entity.IsManager);
                cmd.Parameters.AddWithValue("@price", entity.Price);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@phoneNumber", entity.PhoneNumber);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(con, cmd, "set_Workers");
                    con.Close();

                    StudentBusiness.AllStudentWithCache = null;
                    new StudentBusiness().Get_AllStudentWithCache();

                    return resultSet;
                }
            }
            catch (Exception ex)
            {
                DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
                resultSet.HasError = true;
                resultSet.ErrorDescription = ex.Message;
                resultSet.MyException = ex;
                return resultSet;
            }

        }

        public DataResultArgs<WorkerEntity> Get_Workers_WithId(int id)
        {
            DataProcess.ControlAdminAuthorization(true);
            var resultSet = Get_Workers(new SearchEntity() { Id = id });

            DataResultArgs<WorkerEntity> result = new DataResultArgs<WorkerEntity>();

            EntityHelper.CopyDataResultArgs<WorkerEntity>(resultSet, result);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }

        public DataResultArgs<List<WorkerEntity>> Get_Workers(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization(true);
            DataResultArgs<List<WorkerEntity>> resultSet = new DataResultArgs<List<WorkerEntity>>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
                DataResultArgs<SqlDataReader> result = new DataResultArgs<SqlDataReader>();
                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Workers");


                    if (result.HasError)
                    {
                        resultSet.HasError = result.HasError;
                        resultSet.ErrorDescription = result.ErrorDescription;
                        resultSet.ErrorCode = result.ErrorCode;
                    }
                    else
                    {
                        SqlDataReader dr = result.Result;
                        List<WorkerEntity> lst = new List<WorkerEntity>();
                        WorkerEntity elist;
                        while (dr!=null &&  dr.Read())
                        {
                            elist = new WorkerEntity();
                            elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                            elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                            elist.Surname = GeneralFunctions.GetData<String>(dr["surname"]);
                            elist.IsManager = GeneralFunctions.GetData<Boolean?>(dr["isManager"]);
                            elist.Price = GeneralFunctions.GetData<Decimal?>(dr["price"]);
                            elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                            elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);
                            elist.PhoneNumber = GeneralFunctions.GetData<String>(dr["phoneNumber"]);
                            if (string.IsNullOrEmpty(elist.PhoneNumber))
                                elist.PhoneNumber = "";

                            lst.Add(elist);
                        }

                        if (dr != null)
                            dr.Close();
                        con.Close();
                        resultSet.Result = lst;
                    }
                }
                return resultSet;
            }
            catch (Exception ex)
            {
                resultSet.HasError = true;
                resultSet.ErrorDescription = ex.Message;
                resultSet.MyException = ex;
                return resultSet;
            }
        }
    }
}
