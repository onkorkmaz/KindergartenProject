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
            if (entity.ProcessDate < new DateTime(1900, 1, 1))
            {
                cmd.Parameters.AddWithValue("@processDate", DateTime.Today.Date);
            }
            else
            {
                cmd.Parameters.AddWithValue("@processDate", entity.ProcessDate);
            }

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
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_IncomeAndExpense");
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
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.IncomeAndExpenseTypeId = CommonFunctions.GetData<Int32>(dr["incomeAndExpenseTypeId"]);
                        elist.IncomeAndExpenseTypeName = CommonFunctions.GetData<String>(dr["incomeAndExpenseTypeName"]);
                        elist.IncomeAndExpenseType = CommonFunctions.GetData<Int16?>(dr["incomeAndExpenseType"]);
                        elist.Amount = CommonFunctions.GetData<Decimal>(dr["amount"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]);
                        string desc = CommonFunctions.GetData<String>(dr["description"]);
                        elist.Description = string.IsNullOrEmpty(desc) ? "" : desc;
                        elist.WorkerId = CommonFunctions.GetData<Int32?>(dr["workerId"]);
                        elist.ProcessDate = CommonFunctions.GetData<DateTime>(dr["processDate"]);
                        elist.WorkerName = CommonFunctions.GetData<String>(dr["workerName"]);
                        elist.WorkerSurname = CommonFunctions.GetData<String>(dr["workerSurname"]);
                        lst.Add(elist);
                    }

                    dr.Close();
                    con.Close();
                    resultSet.Result = lst;

                }
            }
            return resultSet;
        }


        public DataResultArgs<List<IncomeAndExpenseEntity>> Get_IncomeAndExpenseWithIncomeAndExpenseTypeId(SearchEntity entity, int year, int incomeAndExpenseTypeId)
        {
            DataResultArgs<List<IncomeAndExpenseEntity>> resultSet = new DataResultArgs<List<IncomeAndExpenseEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@incomeAndExpenseTypeId", incomeAndExpenseTypeId);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_IncomeAndExpenseWithIncomeAndExpenseTypeId");
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
                        elist = getEntity(dr, lst);
                        elist.Description = CommonFunctions.GetMonthName(elist.ProcessDate.Month);
                    }

                    dr.Close();
                    con.Close();
                    resultSet.Result = lst;

                }
            }
            return resultSet;
        }

        public DataResultArgs<List<IncomeAndExpenseEntity>> Get_IncomeAndExpenseWithMonthAndYear(SearchEntity entity, string month, string year)
        {
            int monthInt = CommonFunctions.GetData<int>(month);
            int yearInt = CommonFunctions.GetData<int>(year);

            DataResultArgs<List<IncomeAndExpenseEntity>> resultSet = new DataResultArgs<List<IncomeAndExpenseEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            cmd.Parameters.AddWithValue("@year", yearInt);
            cmd.Parameters.AddWithValue("@month", monthInt);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_IncomeAndExpense");
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
                        elist = getEntity(dr, lst);
                    }

                    dr.Close();
                    con.Close();
                    resultSet.Result = lst;

                }
            }
            return resultSet;
        }

        private static IncomeAndExpenseEntity getEntity(SqlDataReader dr, List<IncomeAndExpenseEntity> lst)
        {
            IncomeAndExpenseEntity elist = new IncomeAndExpenseEntity();
            elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
            elist.IncomeAndExpenseTypeId = CommonFunctions.GetData<Int32>(dr["incomeAndExpenseTypeId"]);
            elist.IncomeAndExpenseTypeName = CommonFunctions.GetData<String>(dr["incomeAndExpenseTypeName"]);
            elist.IncomeAndExpenseType = CommonFunctions.GetData<Int16?>(dr["incomeAndExpenseType"]);
            elist.Amount = CommonFunctions.GetData<Decimal>(dr["amount"]);
            elist.IsActive = CommonFunctions.GetData<Boolean>(dr["isActive"]);
            elist.ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]);
            string desc = CommonFunctions.GetData<String>(dr["description"]);
            elist.Description = string.IsNullOrEmpty(desc) ? "" : desc;
            elist.WorkerId = CommonFunctions.GetData<Int32?>(dr["workerId"]);
            elist.ProcessDate = CommonFunctions.GetData<DateTime>(dr["processDate"]);
            elist.WorkerName = CommonFunctions.GetData<String>(dr["workerName"]);
            elist.WorkerSurname = CommonFunctions.GetData<String>(dr["workerSurname"]);
            lst.Add(elist);
            return elist;
        }

        public DataResultArgs<IncomeAndExpenseEntity> Get_IncomeAndExpenseWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            SearchEntity searchEntity = new SearchEntity() { IsDeleted = false, Id = id };
            DataResultArgs<List<IncomeAndExpenseEntity>> resultSet = Get_IncomeAndExpense(searchEntity);

            DataResultArgs<IncomeAndExpenseEntity> result = EntityHelper.CopyDataResultArgs<IncomeAndExpenseEntity>(resultSet);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);
            return result;
        }
    }
}
