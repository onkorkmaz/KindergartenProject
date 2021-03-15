using System;
using System.Collections.Generic;
using System.Linq;
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
    public class KinderGartenWebService : WebService
    {
        public static Dictionary<string, string> List = new Dictionary<string, string>();

        [WebMethod]
        public DataResultArgs<StudentEntity> GetStudentEntity(string citizenshipNumber)
        {
            return new StudentBusiness().Get_Student(citizenshipNumber);
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
                }
                else if (entity.FullName != null && GeneralFunctions.ReplaceTurkishChar(entity.FullName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }
                else if (entity.FatherName != null && GeneralFunctions.ReplaceTurkishChar(entity.FatherName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }

                else if (entity.MotherName != null && GeneralFunctions.ReplaceTurkishChar(entity.MotherName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }
                else if (entity.FatherPhoneNumber != null && GeneralFunctions.ReplaceTurkishChar(entity.FatherPhoneNumber.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }

                else if (entity.MotherPhoneNumber != null && GeneralFunctions.ReplaceTurkishChar(entity.MotherPhoneNumber.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
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

        public DataResultArgs<bool> InsertOrUpdatePaymentType(string encryptId, PaymentTypeEntity paymentTypeEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            paymentTypeEntity.Id = 0;
            if (!string.IsNullOrEmpty(encryptId))
            {
                int.TryParse(Cipher.Decrypt(encryptId), out var id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                paymentTypeEntity.Id = id;
            }

            paymentTypeEntity.DatabaseProcess = currentProcess;

            return new PaymentTypeBusiness().Set_PaymentType(paymentTypeEntity);
        }

        [WebMethod]
        public DataResultArgs<bool> DeletePaymentType(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true, ErrorDescription = "Id bilgisine ulaşılamadı."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    PaymentTypeEntity entity = new PaymentTypeEntity
                    {
                        Id = idInt, DatabaseProcess = DatabaseProcess.Deleted
                    };

                    result = new PaymentTypeBusiness().Set_PaymentType(entity);
                }
            }

            return result;
        }


        [WebMethod]
        public DataResultArgs<bool> DeleteStudent(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true, ErrorDescription = "Id bilgisine ulaşılamadı"
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    StudentEntity entity = new StudentEntity {Id = idInt, DatabaseProcess = DatabaseProcess.Deleted};

                    DataResultArgs<StudentEntity> resultSet = new StudentBusiness().Set_Student(entity);
                    result.HasError = resultSet.HasError;
                    result.MyException = resultSet.MyException;
                    result.ErrorDescription = resultSet.ErrorDescription;
                }
            }

            return result;
        }
        

        [WebMethod]
        public DataResultArgs<PaymentTypeEntity> UpdatePaymentType(string id)
        {
            DataResultArgs<PaymentTypeEntity> result = new DataResultArgs<PaymentTypeEntity>
            {
                HasError = true, ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result.Result = new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { Id = idInt }).Result.FirstOrDefault();
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
            if (!List.ContainsKey(key))
                List.Add(key, "");

            List[key] = value;
        }

        [WebMethod]
        public string GetCacheData(string key)
        {
            if (List.ContainsKey(key))
                return List[key];
            return "";
        }

        [WebMethod]

        public string Decrypt(string id)
        {
            return Cipher.Decrypt(id);
        }

        [WebMethod]

        public string Encrypt(string id)
        {
            return Cipher.Encrypt(id);
        }

        [WebMethod]

        public StudentListAndPaymentTypeInfo GetStudentListAndPaymentTypeInfoForPaymentDetail(string decryptStudentId,string year)
        {
            StudentListAndPaymentTypeInfo info = new StudentListAndPaymentTypeInfo
            {
                PaymentTypeList = GetPaymentAllPaymentType()
            };

            string id = Cipher.Decrypt(decryptStudentId);

            int idInt = GeneralFunctions.GetData<int>(id);

            if (idInt > 0)
            {
                StudentEntity studentEntity = new StudentBusiness().Get_AllStudentWithCache().Result.FirstOrDefault(o => o.Id == idInt);
                if (studentEntity != null)
                {
                    DataResultArgs<List<PaymentEntity>> paymentListResult = new PaymentBusiness().Get_Payment(idInt, year);
                    if (!paymentListResult.HasError)
                    {
                        if (paymentListResult.Result.Any())
                        {
                            studentEntity.PaymentList = paymentListResult.Result;
                        }
                    }

                    info.StudentList.Add(studentEntity);
                }
            }
            return info;
        }


        [WebMethod]
        public DataResultArgs<bool> DoPaymentOrUnPayment(string id, string encryptStudentId, string year, string month, string amount,bool isPayment, string paymentType)
        {
            PaymentEntity paymentEntity = new PaymentEntity
            {
                EncryptStudentId = encryptStudentId,
                StudentId = GeneralFunctions.GetData<int>(Cipher.Decrypt(encryptStudentId)),
                Year = GeneralFunctions.GetData<short>(year),
                Month = GeneralFunctions.GetData<short>(month),
                Amount = GeneralFunctions.GetData<decimal>(amount),
                Id = GeneralFunctions.GetData<int>(id)
            };
            paymentEntity.DatabaseProcess = (paymentEntity.Id > 0) ? DatabaseProcess.Update : DatabaseProcess.Add;
            paymentEntity.IsActive = true;
            paymentEntity.IsDeleted = true;
            paymentEntity.PaymentDate = DateTime.Now;
            paymentEntity.IsPayment = isPayment;
            paymentEntity.PaymentType = GeneralFunctions.GetData<short>(paymentType);

            return new PaymentBusiness().Set_Payment(paymentEntity);

        }

        [WebMethod]

        public StudentListAndPaymentTypeInfo GetStudentListAndPaymentTypeInfoForPaymentList()
        {
            StudentListAndPaymentTypeInfo paymentDetailEntity = new StudentListAndPaymentTypeInfo();

            List<StudentEntity> studentList = GetAllStudent();

            List<PaymentEntity> paymentEntityList = new PaymentBusiness().Get_PaymentForCurrentMonth().Result;

            foreach (PaymentEntity paymentEntity in paymentEntityList)
            {
                if (paymentEntity.StudentId != null)
                {
                    int studentId = paymentEntity.StudentId.Value;

                    StudentEntity first = studentList.FirstOrDefault(o => o.Id == studentId);
                    if (first != null)
                    {
                        studentList.FirstOrDefault(o => o.Id == studentId).PaymentList.Add(paymentEntity);
                    }
                }
            }

            paymentDetailEntity.StudentList = studentList;
            paymentDetailEntity.PaymentTypeList = new PaymentTypeBusiness().Get_PaymentType(new SearchEntity(){IsActive = true, IsDeleted = false}).Result;

            return paymentDetailEntity;
        }
    }
}
