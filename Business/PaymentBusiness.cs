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
            while (dr != null && dr.Read())
            {
                var paymentEntity = new PaymentEntity
                {
                    Id = CommonFunctions.GetData<Int32>(dr["id"]),
                    StudentId = CommonFunctions.GetData<Int32?>(dr["studentId"]),
                    Year = CommonFunctions.GetData<Int16?>(dr["year"]),
                    Month = CommonFunctions.GetData<Int16?>(dr["month"]),
                    Amount = CommonFunctions.GetData<Decimal?>(dr["amount"]),
                    IsPayment = CommonFunctions.GetData<Boolean?>(dr["isPayment"]),
                    PaymentDate = CommonFunctions.GetData<DateTime?>(dr["paymentDate"]),
                    PaymentType = CommonFunctions.GetData<Int32?>(dr["paymentType"]),
                    IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]),
                    ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"])

                };
                lst.Add(paymentEntity);
            }
            if (dr != null)
                dr.Close();
            return lst;
        }

        internal DataResultArgs<bool> SetStatusUnpaymentRecords(int studentId, bool active)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@isActive", active);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);


            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "set_StatusUnpaymentRecords");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }

                con.Close();
            }

            return resultSet;
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
                    DataProcess.ExecuteProcDataReader(con, cmd, "get_LastTwoMonth");
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


        public DataResultArgs<List<PaymentSummary>> Get_IncomeAndExpenseSummaryWithMonthAndYear(int year, int month)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentSummary>> resultSet = new DataResultArgs<List<PaymentSummary>>();
            List<PaymentSummary> list = new List<PaymentSummary>();
            SqlCommand cmd = new SqlCommand();
            PaymentSummary summary = new PaymentSummary();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_IncomeAndExpenseSummaryWithMonthAndYear");
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
                            IncomeForStudentPayment = CommonFunctions.GetData<decimal>(dr["incomeForStudentPayment"]),
                            WaitingIncomeForStudentPayment = CommonFunctions.GetData<decimal>(dr["waitingIncomeForStudentPayment"]),
                            IncomeWithoutStudentPayment = CommonFunctions.GetData<decimal>(dr["incomeWithoutStudentPayment"]),

                            WorkerExpenses = CommonFunctions.GetData<decimal>(dr["workerExpenses"]),
                            ExpenseWithoutWorker = CommonFunctions.GetData<decimal>(dr["expenseWithoutWorker"]),
                            CurrentBalance = CommonFunctions.GetData<decimal>(dr["currentBalance"]),
                            TotalBalance = CommonFunctions.GetData<decimal>(dr["totalBalance"]),

                            ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]),
                            Month = month,
                            Year = year
                        };

                        list.Add(summary);
                    }
                    con.Close();
                }

                resultSet.Result = list;
            }

            return resultSet;
        }


        public DataResultArgs<List<PaymentSummaryDetail>> Get_IncomeAndExpenseSummaryDetailWithMonthAndYear(int year, int month)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentSummaryDetail>> resultSet = new DataResultArgs<List<PaymentSummaryDetail>>();
            List<PaymentSummaryDetail> list = new List<PaymentSummaryDetail>();
            SqlCommand cmd = new SqlCommand();
            PaymentSummaryDetail summary = new PaymentSummaryDetail();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_IncomeAndExpenseSummaryDetailWithMonthAndYear");
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
                        summary = new PaymentSummaryDetail
                        {
                            IsPayment = CommonFunctions.GetData<bool>(dr["isPayment"]),
                            PaymentTypeName = CommonFunctions.GetData<string>(dr["paymentTypeName"]),
                            Amount = CommonFunctions.GetData<decimal>(dr["amount"]),
                            ProjectType = base.ProjectType,
                            Month = month,
                            Year = year
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
            return Get_Payment(new SearchEntity() { Id = id }).Result.FirstOrDefault();
        }

        public DataResultArgs<string> SetUnPaymentRecordStatusWithStudentId(int studentId, int year, int month)
        {
            DataResultArgs<string> result = new DataResultArgs<string>();
            DataResultArgs<List<PaymentEntity>> paymentListResultSet = Get_Payment(studentId, year.ToString());
            if (!paymentListResultSet.HasError)
            {
                List<PaymentEntity> paymentList = paymentListResultSet.Result;
                foreach (PaymentEntity entity in paymentList)
                {
                    if (entity.Month == month || month == -1)
                    {
                        if (entity.IsPayment.HasValue && !entity.IsPayment.Value)
                        {
                            entity.IsActive = !entity.IsActive;
                            entity.Amount = 0;
                            result = Set_Payment(entity);
                            if(result.HasError)
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            else
            {
                result.HasError = true;
                result.ErrorCode = paymentListResultSet.ErrorCode;
                result.ErrorDescription = paymentListResultSet.ErrorDescription;
                result.MyException = paymentListResultSet.MyException;
            }
            return result;
        }
    }
}
