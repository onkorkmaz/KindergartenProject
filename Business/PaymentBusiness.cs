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
    public class PaymentBusiness : BaseBusiness
    {
        public PaymentBusiness(ProjectType projectType) : base(projectType)
        {

        }
        public DataResultArgs<string> Set_Payment(PaymentEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@studentId", entity.StudentId);
                cmd.Parameters.AddWithValue("@year", entity.Year);
                cmd.Parameters.AddWithValue("@month", entity.Month);
                cmd.Parameters.AddWithValue("@amount", entity.Amount);
                cmd.Parameters.AddWithValue("@isPayment", entity.IsPayment);
                cmd.Parameters.AddWithValue("@paymentDate", entity.PaymentDate);
                cmd.Parameters.AddWithValue("@paymentType", entity.PaymentType);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isChangeAmountPaymentNotOK", entity.IsChangeAmountPaymentNotOK);
                cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<string> resultSet = DataProcess.ExecuteProcString(con, cmd, "set_Payment");
                    con.Close();
                    return resultSet;
                }
            }
            catch (Exception e)
            {
                DataResultArgs<string> result = new DataResultArgs<string>();
                result.HasError = true;
                result.ErrorDescription = e.Message;
                return result;
            }
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Payment");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    List<PaymentEntity> lst = GetList(result);
                    resultSet.Result = lst;
                }

                con.Close();
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId, string year)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);


            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Payment");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    List<PaymentEntity> lst = GetList(result);
                    resultSet.Result = lst;
                }

                con.Close();
            }

            return resultSet;
        }

        private static List<PaymentEntity> GetList(DataResultArgs<SqlDataReader> result)
        {
            DataProcess.ControlAdminAuthorization();

            SqlDataReader dr = result.Result;
            List<PaymentEntity> lst = new List<PaymentEntity>();
            while (dr!=null &&  dr.Read())
            {
                var paymentEntity = new PaymentEntity
                {
                    Id = GeneralFunctions.GetData<Int32>(dr["id"]),
                    StudentId = GeneralFunctions.GetData<Int32?>(dr["studentId"]),
                    Year = GeneralFunctions.GetData<Int16?>(dr["year"]),
                    Month = GeneralFunctions.GetData<Int16?>(dr["month"]),
                    Amount = GeneralFunctions.GetData<Decimal?>(dr["amount"]),
                    IsPayment = GeneralFunctions.GetData<Boolean?>(dr["isPayment"]),
                    PaymentDate = GeneralFunctions.GetData<DateTime?>(dr["paymentDate"]),
                    PaymentType = GeneralFunctions.GetData<Int32?>(dr["paymentType"]),
                    IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]),
                    ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"])

            };
                lst.Add(paymentEntity);
            }
            if (dr != null)
                dr.Close();
            return lst;
        }


        public DataResultArgs<List<PaymentEntity>> Get_PaymentForCurrentMonth()
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);



            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_PaymentForCurrentMonth");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    List<PaymentEntity> lst = GetList(result);
                    resultSet.Result = lst;
                }

                con.Close();
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_LastTwoMonths()
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);



            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_GetLastTwoMonth");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    List<PaymentEntity> lst = GetList(result);
                    resultSet.Result = lst;
                }

                con.Close();
            }

            return resultSet;
        }


        public DataResultArgs<List<PaymentSummary>> Get_PaymentForCurrentMonthWithDefaultAmount()
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentSummary>> resultSet = new DataResultArgs<List<PaymentSummary>>();
            List<PaymentSummary> list = new List<PaymentSummary>();
            SqlCommand cmd = new SqlCommand();
            PaymentSummary summary = new PaymentSummary();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);


            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_PaymentForCurrentMonthWithDefaultAmount");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;

                    while (dr != null && dr.Read())
                    {
                        summary = new PaymentSummary
                        {
                            Income = GeneralFunctions.GetData<decimal>(dr["income"]),
                            WaitingInCome = GeneralFunctions.GetData<decimal>(dr["waitingInCome"]),
                            WorkerExpenses = GeneralFunctions.GetData<decimal>(dr["workerExpenses"]),
                            ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"])
                        };

                        list.Add(summary);
                    }



                    con.Close();
                }

                resultSet.Result = list;
            }

            return resultSet;
        }


        public PaymentEntity Get_PaymentWidthId(int id)
        {
            return Get_Payment(new SearchEntity() {Id = id}).Result.FirstOrDefault();
        }

    }
}
