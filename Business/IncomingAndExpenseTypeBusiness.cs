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
    public class IncomeAndExpenseTypeBusiness : BaseBusiness
    {
        public IncomeAndExpenseTypeBusiness(ProjectType projectType) : base(projectType)
        {

        }
        public DataResultArgs<bool> Set_IncomeAndExpenseType(IncomeAndExpenseTypeEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@type", entity.Type);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_IncomeAndExpenseType");
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<List<IncomeAndExpenseTypeEntity>> Get_IncomeAndExpenseType(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<IncomeAndExpenseTypeEntity>> resultSet = new DataResultArgs<List<IncomeAndExpenseTypeEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);


            List<IncomeAndExpenseTypeEntity> lst = new List<IncomeAndExpenseTypeEntity>();
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_IncomeAndExpenseType");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;

                    IncomeAndExpenseTypeEntity elist;
                    while (dr.Read())
                    {
                        elist = new IncomeAndExpenseTypeEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                        elist.Type = GeneralFunctions.GetData<Int16>(dr["type"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"]);
                        elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);

                        lst.Add(elist);
                    }

                    dr.Close();
                    con.Close();
                }
                resultSet.Result = lst;
            }
            return resultSet;
        }


        public DataResultArgs<IncomeAndExpenseTypeEntity> Get_IncomeAndExpenseType_WithId(int id)
        {
            DataProcess.ControlAdminAuthorization(true);
            var resultSet = Get_IncomeAndExpenseType(new SearchEntity() { Id = id });

            DataResultArgs<IncomeAndExpenseTypeEntity> result = new DataResultArgs<IncomeAndExpenseTypeEntity>();

            EntityHelper.CopyDataResultArgs<IncomeAndExpenseTypeEntity>(resultSet, result);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }

    }
}
