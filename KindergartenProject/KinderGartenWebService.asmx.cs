using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Business;
using Entity;
using Common;
using System.Web.Script.Services;

namespace KindergartenProject
{
    /// <summary>
    /// Summary description for KinderGartenWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    [ScriptService]
    public class KinderGartenWebService : System.Web.Services.WebService
    {
        public static Dictionary<string, string> list = new Dictionary<string, string>();

        [WebMethod]
        public DataResultArgs<StudentEntity> GetStudenEntity(string citizenshipNumber)
        {
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
            resultSet = new StudentBusiness().Get_Student(citizenshipNumber);
            return resultSet;
        }

        [WebMethod]
        public List<StudentEntity> SearchStudent(string searchValue)
        {
            List<StudentEntity> resultSet = new StudentBusiness().Get_AllStudentWithCache().Result;

            if (string.IsNullOrEmpty(searchValue))
            {
                return resultSet;
            }

            searchValue = GeneralFunctions.ReplaceTurkishChar(searchValue.ToLower());

            List<StudentEntity> searchList = new List<StudentEntity>();

            foreach (StudentEntity entity in resultSet)
            {
                if (entity.CitizenshipNumber != null && entity.CitizenshipNumber.Contains(searchValue))
                {
                    searchList.Add(entity);
                    continue;
                }

                else if (entity.FullName != null && GeneralFunctions.ReplaceTurkishChar(entity.FullName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                    continue;
                }

                else if (entity.FatherName != null && GeneralFunctions.ReplaceTurkishChar(entity.FatherName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                    continue;
                }

                else if (entity.MotherName != null && GeneralFunctions.ReplaceTurkishChar(entity.MotherName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                    continue;
                }

                else if (entity.FatherPhoneNumber != null && GeneralFunctions.ReplaceTurkishChar(entity.FatherPhoneNumber.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                    continue;
                }

                else if (entity.MotherPhoneNumber != null && GeneralFunctions.ReplaceTurkishChar(entity.MotherPhoneNumber.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                    continue;
                }
            }
            return searchList;
        }

        [WebMethod]
        public List<PaymentTypeEntity> GetPaymentAllPaymentType()
        {
            List<PaymentTypeEntity> result = new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { IsDeleted = false }).Result;

            return result;
        }

        [WebMethod]

        public DataResultArgs<bool> InsertOrUpdatePaymentType(PaymentTypeEntity paymentTypeEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            paymentTypeEntity.Id = 0;
            if (!string.IsNullOrEmpty(paymentTypeEntity.EncryptId))
            {
                int id = 0;
                Int32.TryParse(Cipher.Decrypt(paymentTypeEntity.EncryptId), out id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                    paymentTypeEntity.Id = id;
                }
            }

            paymentTypeEntity.DatabaseProcess = currentProcess;

            return new PaymentTypeBusiness().Set_PaymentType(paymentTypeEntity);
        }

        [WebMethod]
        public DataResultArgs<bool> DeletePaymentType(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>();
            result.HasError = true;
            result.ErrorDescription = "Id bilgisine ulaşılamadı";

            if (!string.IsNullOrEmpty(id))
            {
                int idINT = 0;
                Int32.TryParse(Cipher.Decrypt(id), out idINT);
                if (idINT > 0)
                {
                    PaymentTypeEntity entity = new PaymentTypeEntity();
                    entity.Id = idINT;
                    entity.DatabaseProcess = DatabaseProcess.Deleted;

                    result = new PaymentTypeBusiness().Set_PaymentType(entity);
                }
            }

            return result;
        }


        [WebMethod]
        public DataResultArgs<bool> DeleteStudent(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>();
            result.HasError = true;
            result.ErrorDescription = "Id bilgisine ulaşılamadı";

            if (!string.IsNullOrEmpty(id))
            {
                int idINT = 0;
                Int32.TryParse(Cipher.Decrypt(id), out idINT);
                if (idINT > 0)
                {
                    StudentEntity entity = new StudentEntity();
                    entity.Id = idINT;
                    entity.DatabaseProcess = DatabaseProcess.Deleted;

                    result = new StudentBusiness().Set_Student(entity);
                }
            }

            return result;
        }
        

        [WebMethod]
        public DataResultArgs<PaymentTypeEntity> UpdatePaymentType(string id)
        {
            DataResultArgs<PaymentTypeEntity> result = new DataResultArgs<PaymentTypeEntity>();
            result.HasError = true;
            result.ErrorDescription = "Entity ulaşılamadı...";

            if (!string.IsNullOrEmpty(id))
            {
                int idINT = 0;
                Int32.TryParse(Cipher.Decrypt(id), out idINT);
                if (idINT > 0)
                {
                    PaymentTypeEntity entity = new PaymentTypeEntity();
                    entity.Id = idINT;
                    entity.DatabaseProcess = DatabaseProcess.Deleted;

                    result.Result = new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { Id = idINT }).Result.FirstOrDefault();
                    if (result.Result != null)
                        result.HasError = false;
                }
            }
            return result;
        }


        [WebMethod]
        public List<StudentEntity> GetAllStudent()
        {
            List<StudentEntity> resultSet = new StudentBusiness().Get_AllStudentWithCache().Result;
            return resultSet;
        }

        [WebMethod]
        public void SetCacheData(string key,string value)
        {
            if (!list.ContainsKey(key))
                list.Add(key, "");

            list[key] = value;
        }

        [WebMethod]
        public string GetCacheData(string key)
        {
            if (list.ContainsKey(key))
                return list[key];
            return "";
        }
    }
}
