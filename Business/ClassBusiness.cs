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
    public class ClassBusiness
    {
        public DataResultArgs<bool> Set_Class(ClassEntity entity)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@warningOfStudentCount", entity.WarningOfStudentCount);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_Class");
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<List<ClassEntity>> Get_Class(SearchEntity entity)
        {
            DataResultArgs<List<ClassEntity>> resultSet = new DataResultArgs<List<ClassEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);


            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Class");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<ClassEntity> lst = new List<ClassEntity>();
                    ClassEntity elist;
                    while (dr.Read())
                    {
                        elist = new ClassEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                        elist.Description = GeneralFunctions.GetData<String>(dr["description"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.WarningOfStudentCount = GeneralFunctions.GetData<Int32?>(dr["warningOfStudentCount"]);
                        elist.UpdatedOn = (GeneralFunctions.GetData<DateTime>(dr["updatedOn"]) > DateTime.MinValue) ? GeneralFunctions.GetData<DateTime>(dr["updatedOn"]) : GeneralFunctions.GetData<DateTime>(dr["addedOn"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<ClassEntity> Get_ClassWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<ClassEntity>> resultSet = Get_Class(new SearchEntity()
            { IsDeleted = false, Id = id });

            DataResultArgs<ClassEntity> result = new DataResultArgs<ClassEntity>();

            EntityHelper.CopyDataResultArgs<ClassEntity>(resultSet, result);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;

        }
    }
}
