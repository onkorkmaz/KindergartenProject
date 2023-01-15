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
    public class IncomeAndExpenseBusiness : BaseBusiness
    {
        public IncomeAndExpenseBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public DataResultArgs<bool> Set_IncomeAndExpense(IncomeAndExpenseEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@incomeAndExpenseTypeId", entity.IncomeAndExpenseTypeId);
            cmd.Parameters.AddWithValue("@amount", entity.Amount);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectType);
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters.AddWithValue("@workerId", entity.WorkerId);


            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_IncomeAndExpense");
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<List<IncomeAndExpenseEntity>> Get_IncomeAndExpense(SearchEntity entity)
        {
            DataResultArgs<List<IncomeAndExpenseEntity>> resultSet = new DataResultArgs<List<IncomeAndExpenseEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con,cmd, "get_IncomeAndExpense");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<IncomeAndExpenseEntity> lst = new List<IncomeAndExpenseEntity>();
                    IncomeAndExpenseEntity elist;
                    while (dr.Read())
                    {
                        elist = new IncomeAndExpenseEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.IncomeAndExpenseTypeId = GeneralFunctions.GetData<Int32>(dr["IncomeAndExpenseTypeId"]);
                        elist.IncomeAndExpenseTypeName = GeneralFunctions.GetData<String>(dr["IncomeAndExpenseTypeName"]);
                        elist.IncomeAndExpenseType = GeneralFunctions.GetData<Int16?>(dr["IncomeAndExpenseType"]);
                        elist.Amount = GeneralFunctions.GetData<Decimal>(dr["amount"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"]);
                        elist.Description = GeneralFunctions.GetData<String>(dr["description"]);
                        elist.WorkerId = GeneralFunctions.GetData<Int32?>(dr["workerId"]);

                        lst.Add(elist);
                    }


                    dr.Close();
                    con.Close();
                    resultSet.Result = lst;

                }
            }
            return resultSet;
        }

    }
}
