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
    public class PaymentTypeBusiness
    {
        private static DataResultArgs<List<PaymentTypeEntity>> staticPaymentTypeList;

        public DataResultArgs<bool> Set_PaymentType(PaymentTypeEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@amount", entity.Amount);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);

            DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(cmd, "set_PaymentType");
            staticPaymentTypeList = null;
            return resultSet;
        }

        public DataResultArgs<List<PaymentTypeEntity>> Get_PaymentType(SearchEntity entity)
        {
            if (staticPaymentTypeList == null)
            {

                DataResultArgs<List<PaymentTypeEntity>> resultSet = new DataResultArgs<List<PaymentTypeEntity>>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "get_PaymentType");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<PaymentTypeEntity> lst = new List<PaymentTypeEntity>();
                    PaymentTypeEntity elist;
                    while (dr.Read())
                    {
                        elist = new PaymentTypeEntity();
                        elist.Id = GeneralFunctions.GetData<Int32?>(dr["id"]);
                        elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                        elist.Amount = GeneralFunctions.GetData<Decimal?>(dr["amount"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                }

                staticPaymentTypeList = resultSet;
            }

            return staticPaymentTypeList;
        }
    }
}
