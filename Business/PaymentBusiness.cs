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
    public class PaymentBusiness
    {
        public DataResultArgs<bool> Set_Payment(PaymentEntity entity)
        {
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
            cmd.Parameters.AddWithValue("@isNotPayable", entity.IsNotPayable);

            DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(cmd, "set_Payment");

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(SearchEntity entity)
        {
            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

            DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "get_Payment");
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
            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId,string year)
        {

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@year", year);

            DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "get_Payment");
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
            return resultSet;
        }

        private static List<PaymentEntity> GetList(DataResultArgs<SqlDataReader> result)
        {
            SqlDataReader dr = result.Result;
            List<PaymentEntity> lst = new List<PaymentEntity>();
            while (dr.Read())
            {
                var paymentEntity = new PaymentEntity
                {
                    Id = GeneralFunctions.GetData<Int32?>(dr["id"]),
                    StudentId = GeneralFunctions.GetData<Int32?>(dr["studentId"]),
                    Year = GeneralFunctions.GetData<Int16?>(dr["year"]),
                    Month = GeneralFunctions.GetData<Int16?>(dr["month"]),
                    Amount = GeneralFunctions.GetData<Decimal?>(dr["amount"]),
                    IsPayment = GeneralFunctions.GetData<Boolean?>(dr["isPayment"]),
                    PaymentDate = GeneralFunctions.GetData<DateTime?>(dr["paymentDate"]),
                    PaymentType = GeneralFunctions.GetData<Int32?>(dr["paymentType"]),
                    IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]),
                    IsNotPayable = GeneralFunctions.GetData<Boolean>(dr["isNotPayable"])
                };
                lst.Add(paymentEntity);
            }

            dr.Close();
            return lst;
        }


        public DataResultArgs<List<PaymentEntity>> Get_PaymentForCurrentMonth()
        {
            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "Get_PaymentForCurrentMonth");
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
            return resultSet;
        }
    }
}
