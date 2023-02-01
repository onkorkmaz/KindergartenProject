using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Business;
using Entity;
using Common;
using System.Web.Script.Services;
using System.Web;

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

        #region Authority

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<AuthorityEntity>> GetAuthorityList()
        {
            return new AuthorityBusiness(GetProjectType()).Get_Authority(new SearchEntity() { IsDeleted = false });
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateAuthority(string encryptId, AuthorityEntity authorityEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            authorityEntity.Id = 0;
            if (!string.IsNullOrEmpty(encryptId))
            {
                int.TryParse(Cipher.Decrypt(encryptId), out var id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                authorityEntity.Id = id;
            }

            authorityEntity.DatabaseProcess = currentProcess;

            return new AuthorityBusiness(GetProjectType()).Set_Authority(authorityEntity);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> DeleteAuthority(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı"
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    AuthorityEntity entity = new AuthorityEntity { Id = idInt, DatabaseProcess = DatabaseProcess.Deleted };
                    result = new AuthorityBusiness(GetProjectType()).Set_Authority(entity);
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<AuthorityEntity> GetAuthorityWithId(string id)
        {
            DataResultArgs<AuthorityEntity> result = new DataResultArgs<AuthorityEntity>
            {
                HasError = true,
                ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result = new AuthorityBusiness(GetProjectType()).Get_AuthorityWithId(CommonFunctions.GetData<int>(id));
                    if (result.Result != null)
                        result.HasError = false;
                }
            }

            return result;
        }

        #endregion Authority

        public static Dictionary<string, string> List = new Dictionary<string, string>();

        [WebMethod(EnableSession = true)]
        public ProjectType GetProjectType()
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                return ProjectType.None;
            }
            else
            {
                return (ProjectType)Session[CommonConst.ProjectType];
            }
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<StudentEntity> GetStudentEntity(string citizenshipNumber)
        {
            return new StudentBusiness(GetProjectType()).Get_Student(citizenshipNumber);
        }

        [WebMethod(EnableSession = true)]
        public List<StudentEntity> SearchStudent(string searchValue)
        {
            List<StudentEntity> resultSet = new StudentBusiness(GetProjectType()).Get_AllStudentWithCache().Result;

            if (string.IsNullOrEmpty(searchValue))
            {
                return resultSet;
            }

            searchValue = CommonFunctions.ReplaceTurkishChar(searchValue.ToLower());

            List<StudentEntity> searchList = new List<StudentEntity>();

            foreach (StudentEntity entity in resultSet)
            {
                if (entity.CitizenshipNumber != null && entity.CitizenshipNumber.Contains(searchValue))
                {
                    searchList.Add(entity);
                }
                else if (entity.FullName != null && CommonFunctions.ReplaceTurkishChar(entity.FullName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }
                else if (entity.FatherName != null && CommonFunctions.ReplaceTurkishChar(entity.FatherName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }

                else if (entity.MotherName != null && CommonFunctions.ReplaceTurkishChar(entity.MotherName.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }
                else if (entity.FatherPhoneNumber != null && CommonFunctions.ReplaceTurkishChar(entity.FatherPhoneNumber.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }

                else if (entity.MotherPhoneNumber != null && CommonFunctions.ReplaceTurkishChar(entity.MotherPhoneNumber.ToLower()).Contains(searchValue))
                {
                    searchList.Add(entity);
                }
            }
            return searchList;
        }

        [WebMethod(EnableSession = true)]
        public List<PaymentTypeEntity> GetPaymentAllPaymentType()
        {
            List<PaymentTypeEntity> result = new PaymentTypeBusiness(GetProjectType()).Get_PaymentType(new SearchEntity() { IsDeleted = false }).Result;

            return result;
        }

        [WebMethod(EnableSession = true)]

        public string CalculateRecordedStudentCount(string classId)
        {
            int cId = CommonFunctions.GetData<int>(classId);
            if (cId > 0)
            {
                ClassEntity classEntity = new ClassBusiness(GetProjectType()).Get_ClassWithId(cId).Result;
                if (classEntity != null && classEntity.WarningOfStudentCount > 0)
                {
                    int recordedStudentNumber = new StudentBusiness(GetProjectType()).Get_AllStudentWithCache().Result.Where(o => o.ClassId.HasValue && o.ClassId.Value == cId && o.IsActive.Value).ToList().Count();
                    return "Max Öğrenci Adeti : " + classEntity.WarningOfStudentCount.ToString() + " <br/>Kayıtlı Öğrenci : " + recordedStudentNumber;
                }
            }
            return "";
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<WorkerEntity>> GetActiveTeacher()
        {
            return new WorkerBusiness(GetProjectType()).Get_Worker(new SearchEntity() { IsActive = true, IsDeleted = false }, true);
        }

        [WebMethod(EnableSession = true)]
        public List<WorkerEntity> GetAllWorker()
        {
            List<WorkerEntity> result = new WorkerBusiness(GetProjectType()).Get_Worker(new SearchEntity() { IsDeleted = false }, null).Result;

            return result;
        }

        [WebMethod(EnableSession = true)]
        public List<IncomeAndExpenseTypeEntity> GetAllIncomeAndExpenseType()
        {
            List<IncomeAndExpenseTypeEntity> result = new IncomeAndExpenseTypeBusiness(GetProjectType()).Get_IncomeAndExpenseType(new SearchEntity() { IsDeleted = false }).Result;

            return result;
        }



        [WebMethod(EnableSession = true)]
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

            return new PaymentTypeBusiness(GetProjectType()).Set_PaymentType(paymentTypeEntity);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateWorker(string encryptId, WorkerEntity workerEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            workerEntity.Id = 0;
            if (!string.IsNullOrEmpty(encryptId))
            {
                int.TryParse(Cipher.Decrypt(encryptId), out var id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                workerEntity.Id = id;
            }

            workerEntity.DatabaseProcess = currentProcess;

            return new WorkerBusiness(GetProjectType()).Set_Worker(workerEntity);
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> DeleteWorker(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    WorkerEntity entity = new WorkerEntity
                    {
                        Id = idInt,
                        DatabaseProcess = DatabaseProcess.Deleted
                    };

                    result = new WorkerBusiness(GetProjectType()).Set_Worker(entity);
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> DeleteIncomeAndExpenseType(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    IncomeAndExpenseTypeEntity entity = new IncomeAndExpenseTypeEntity
                    {
                        Id = idInt,
                        DatabaseProcess = DatabaseProcess.Deleted
                    };

                    result = new IncomeAndExpenseTypeBusiness(GetProjectType()).Set_IncomeAndExpenseType(entity);
                }
            }

            return result;
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> DeletePaymentType(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    PaymentTypeEntity entity = new PaymentTypeEntity
                    {
                        Id = idInt,
                        DatabaseProcess = DatabaseProcess.Deleted
                    };

                    result = new PaymentTypeBusiness(GetProjectType()).Set_PaymentType(entity);
                }
            }

            return result;
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> DeleteStudent(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı"
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    StudentEntity entity = new StudentEntity { Id = idInt, DatabaseProcess = DatabaseProcess.Deleted };

                    DataResultArgs<StudentEntity> resultSet = new StudentBusiness(GetProjectType()).Set_Student(entity);
                    result.HasError = resultSet.HasError;
                    result.MyException = resultSet.MyException;
                    result.ErrorDescription = resultSet.ErrorDescription;
                    result.Result = !resultSet.HasError;
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<StudentEntity> ConvertStudent(string id)
        {
            DataResultArgs<StudentEntity> result = new DataResultArgs<StudentEntity>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı"
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    StudentEntity studentEntity = new StudentBusiness(GetProjectType()).Get_Student(idInt).Result;

                    if (studentEntity != null)
                    {
                        studentEntity.IsStudent = true;
                        studentEntity.DatabaseProcess = DatabaseProcess.Update;
                        studentEntity.StudentDetail.AddUnPaymentRecordAfterStundetInsert = true;
                        result = new StudentBusiness(GetProjectType()).Set_Student(studentEntity);
                    }
                }
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentTypeEntity> GetPaymentTypeWithId(string id)
        {
            DataResultArgs<PaymentTypeEntity> result = new DataResultArgs<PaymentTypeEntity>
            {
                HasError = true,
                ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result = new PaymentTypeBusiness(GetProjectType()).Get_PaymentTypeWithId(CommonFunctions.GetData<int>(id));
                    if (result.Result != null)
                        result.HasError = false;
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<WorkerEntity> GetWorkerWithId(string id)
        {
            DataResultArgs<WorkerEntity> result = new DataResultArgs<WorkerEntity>
            {
                HasError = true,
                ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result = new WorkerBusiness(GetProjectType()).Get_Worker_WithId(CommonFunctions.GetData<int>(id), null);
                    if (result.Result != null)
                        result.HasError = false;
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<IncomeAndExpenseTypeEntity> GetIncomeAndExpenseTypeWithId(string id)
        {
            DataResultArgs<IncomeAndExpenseTypeEntity> result = new DataResultArgs<IncomeAndExpenseTypeEntity>

            {
                HasError = true,
                ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result = new IncomeAndExpenseTypeBusiness(GetProjectType()).Get_IncomeAndExpenseType_WithId(idInt);
                    if (result.Result != null)
                        result.HasError = false;
                }
            }

            return result;
        }


        [WebMethod(EnableSession = true)]
        public List<StudentEntity> GetAllStudent()
        {
            List<StudentEntity> resultSet = new StudentBusiness(GetProjectType()).Get_AllStudentWithCache().Result;
            return resultSet;
        }

        [WebMethod(EnableSession = true)]
        public List<StudentEntity> GetAllStudentAndAttendanceList()
        {
            List<StudentEntity> studentList = new StudentBusiness(GetProjectType()).Get_AllStudentWithCache().Result;

            List<StudentAttendanceBookEntity> attendanceList = new StudentAttendanceBookBusiness(GetProjectType()).Get_StudentAttendanceBookWithCache(new SearchEntity() { IsActive = true, IsDeleted = false });

            studentList.ForEach(o => o.StudentDetail.StudentAttendanceBookList = new List<StudentAttendanceBookEntity>());

            foreach (StudentAttendanceBookEntity entity in attendanceList)
            {
                studentList.FirstOrDefault(o => o.Id == entity.StudentId).StudentDetail.StudentAttendanceBookList.Add(entity);
            }

            return studentList;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<StudentAttendanceBookEntity>> GetAttenDanceList()
        {
            return new StudentAttendanceBookBusiness(GetProjectType()).Get_StudentAttendanceBook(new SearchEntity() { IsActive = true });
        }

        [WebMethod(EnableSession = true)]
        public void SetCacheData(string key, string value)
        {
            if (!List.ContainsKey(key))
                List.Add(key, "");

            List[key] = value;
        }

        [WebMethod(EnableSession = true)]
        public string GetCacheData(string key)
        {
            if (List.ContainsKey(key))
                return List[key];
            return "";
        }

        [WebMethod(EnableSession = true)]
        public string Decrypt(string id)
        {
            return Cipher.Decrypt(id);
        }

        [WebMethod(EnableSession = true)]
        public string Encrypt(string id)
        {
            return Cipher.Encrypt(id);
        }

        [WebMethod(EnableSession = true)]
        public List<StudentListAndPaymentTypeInfo> GetPaymentDetailSeason(string decryptStudentId, string year)
        {
            StudentListAndPaymentTypeInfo currentYear = GetStudentListAndPaymentTypeInfoForPaymentDetail(decryptStudentId, year);
            int yearInt = Convert.ToInt32(year);
            StudentListAndPaymentTypeInfo nextYear = GetStudentListAndPaymentTypeInfoForPaymentDetail(decryptStudentId, (yearInt + 1).ToString());


            List<StudentListAndPaymentTypeInfo> returnList = new List<StudentListAndPaymentTypeInfo>();


            List<PaymentEntity> list = currentYear.StudentList.First().StudentDetail.PaymentList;

            List<PaymentEntity> newList = new List<PaymentEntity>();

            for (int i = 9; i <= 12; i++)
            {
                if (list.Count > i)
                {
                    newList.Add(list[i]);
                }
            }

            currentYear.StudentList.First().StudentDetail.PaymentList = list;
            returnList.Add(currentYear);

            list = nextYear.StudentList.First().StudentDetail.PaymentList;

            for (int i = 1; i <= 8; i++)
            {
                if (list.Count > i)
                {
                    newList.Add(list[i]);
                }
            }

            nextYear.StudentList.First().StudentDetail.PaymentList = list;
            returnList.Add(nextYear);

            return returnList;

        }

        [WebMethod(EnableSession = true)]
        public StudentListAndPaymentTypeInfo GetStudentListAndPaymentTypeInfoForPaymentDetail(string decryptStudentId, string year)
        {
            StudentListAndPaymentTypeInfo info = new StudentListAndPaymentTypeInfo
            {
                PaymentTypeList = GetPaymentAllPaymentType()
            };

            string id = Cipher.Decrypt(decryptStudentId);

            int idInt = CommonFunctions.GetData<int>(id);

            if (idInt > 0)
            {
                StudentEntity studentEntity = new StudentBusiness(GetProjectType()).Get_Student().Result.FirstOrDefault(o => o.Id == idInt);
                if (studentEntity != null)
                {
                    DataResultArgs<List<PaymentEntity>> paymentListResult = new PaymentBusiness(GetProjectType()).Get_Payment(idInt, year);
                    if (!paymentListResult.HasError)
                    {
                        if (paymentListResult.Result.Any())
                        {
                            studentEntity.StudentDetail.PaymentList = paymentListResult.Result;
                        }
                    }

                    info.StudentList.Add(studentEntity);
                }
            }
            info.Year = CommonFunctions.GetData<int>(year);
            return info;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<StudentAttendanceBookEntity> ProcessAttendanceBook(int studentId, int year, int month, int day, bool isArrival)
        {
            DataResultArgs<StudentAttendanceBookEntity> resultSet = new DataResultArgs<StudentAttendanceBookEntity>();
            StudentAttendanceBookEntity entity = new StudentAttendanceBookEntity();
            entity.StudentId = studentId;
            entity.ArrivalDate = new DateTime(year, month, day);
            entity.IsArrival = isArrival;
            entity.IsActive = true;
            entity.ProjectType = GetProjectType();
            DataResultArgs<bool> result = new StudentAttendanceBookBusiness(GetProjectType()).Set_StudentAttendanceBook(entity);

            resultSet.Result = entity;
            resultSet.HasError = result.HasError;
            resultSet.ErrorCode = result.ErrorCode;
            resultSet.ErrorDescription = result.ErrorDescription;
            resultSet.MyException = result.MyException;

            return resultSet;


        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentEntity> DoPaymentOrUnPayment(string id, string encryptStudentId, string year, string month, string amount, bool isPayment, string paymentType)
        {
            PaymentEntity paymentEntity = new PaymentEntity
            {
                EncryptStudentId = encryptStudentId,
                StudentId = CommonFunctions.GetData<int>(Cipher.Decrypt(encryptStudentId)),
                Year = CommonFunctions.GetData<short>(year),
                Month = CommonFunctions.GetData<short>(month),
                Amount = CommonFunctions.GetData<decimal>(amount),
                Id = CommonFunctions.GetData<int>(id)
            };
            paymentEntity.DatabaseProcess = (paymentEntity.Id > 0) ? DatabaseProcess.Update : DatabaseProcess.Add;
            paymentEntity.IsActive = true;
            paymentEntity.IsDeleted = true;
            paymentEntity.PaymentDate = DateTime.Now;
            paymentEntity.IsPayment = isPayment;
            paymentEntity.PaymentType = CommonFunctions.GetData<short>(paymentType);

            DataResultArgs<string> resultSet = new PaymentBusiness(GetProjectType()).Set_Payment(paymentEntity);

            paymentEntity.Id = CommonFunctions.GetData<int>(resultSet.Result);
            DataResultArgs<PaymentEntity> returnResultSet = new DataResultArgs<PaymentEntity>();
            returnResultSet.ErrorCode = resultSet.ErrorCode;
            returnResultSet.ErrorDescription = resultSet.ErrorDescription;
            returnResultSet.HasError = resultSet.HasError;
            returnResultSet.Result = paymentEntity;

            return returnResultSet;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentEntity> SetPaymentAmount(string id, string encryptStudentId, string year, string month,
            string currentAmount, string paymentType)
        {
            PaymentEntity paymentEntity = new PaymentEntity
            {
                EncryptStudentId = encryptStudentId,
                StudentId = CommonFunctions.GetData<int>(Cipher.Decrypt(encryptStudentId)),
                Year = CommonFunctions.GetData<short>(year),
                Month = CommonFunctions.GetData<short>(month),
                Amount = CommonFunctions.GetData<decimal>(currentAmount),
                Id = CommonFunctions.GetData<int>(id)
            };
            paymentEntity.DatabaseProcess = (paymentEntity.Id > 0) ? DatabaseProcess.Update : DatabaseProcess.Add;
            paymentEntity.IsActive = true;
            paymentEntity.IsDeleted = false;
            paymentEntity.PaymentDate = DateTime.Now;
            paymentEntity.IsPayment = null;
            paymentEntity.PaymentType = CommonFunctions.GetData<short>(paymentType);

            DataResultArgs<string> resultSet = new PaymentBusiness(GetProjectType()).Set_Payment(paymentEntity);

            paymentEntity.Id = CommonFunctions.GetData<int>(resultSet.Result);
            DataResultArgs<PaymentEntity> returnResultSet = new DataResultArgs<PaymentEntity>
            {
                ErrorCode = resultSet.ErrorCode,
                ErrorDescription = resultSet.ErrorDescription,
                HasError = resultSet.HasError,
                Result = paymentEntity
            };

            return returnResultSet;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentEntity> SetAnotherPaymentAmount(string id, string encryptStudentId, string year, string month,
            string currentAmount, string paymentType)
        {
            PaymentEntity paymentEntity = new PaymentEntity
            {
                EncryptStudentId = encryptStudentId,
                StudentId = CommonFunctions.GetData<int>(Cipher.Decrypt(encryptStudentId)),
                Year = CommonFunctions.GetData<short>(year),
                Month = CommonFunctions.GetData<short>(month),
                Amount = CommonFunctions.GetData<decimal>(currentAmount),
                Id = CommonFunctions.GetData<int>(id),
                IsChangeAmountPaymentNotOK = true
            };
            paymentEntity.DatabaseProcess = (paymentEntity.Id > 0) ? DatabaseProcess.Update : DatabaseProcess.Add;
            paymentEntity.IsActive = true;
            paymentEntity.IsDeleted = false;
            paymentEntity.PaymentDate = DateTime.Now;
            paymentEntity.IsPayment = null;
            paymentEntity.PaymentType = CommonFunctions.GetData<short>(paymentType);

            DataResultArgs<string> resultSet = new PaymentBusiness(GetProjectType()).Set_Payment(paymentEntity);

            paymentEntity.Id = CommonFunctions.GetData<int>(resultSet.Result);

            PaymentEntity dbPaymentEntity = new PaymentBusiness(GetProjectType()).Get_PaymentWidthId(paymentEntity.Id);
            dbPaymentEntity.EncryptStudentId = encryptStudentId;

            DataResultArgs<PaymentEntity> returnResultSet = new DataResultArgs<PaymentEntity>
            {
                ErrorCode = resultSet.ErrorCode,
                ErrorDescription = resultSet.ErrorDescription,
                HasError = resultSet.HasError,
                Result = dbPaymentEntity
            };

            return returnResultSet;
        }

        [WebMethod(EnableSession = true)]
        public StudentListAndPaymentTypeInfo GetStudentListAndPaymentTypeInfoForCurrentMonth()
        {
            StudentListAndPaymentTypeInfo paymentDetailEntity = new StudentListAndPaymentTypeInfo();

            List<StudentEntity> studentList =
                new StudentBusiness(GetProjectType()).Get_Student().Result.Where(o => o.IsStudent == true).ToList();

            List<PaymentEntity> paymentEntityList = new PaymentBusiness(GetProjectType()).Get_PaymentForCurrentMonth().Result;

            foreach (PaymentEntity paymentEntity in paymentEntityList)
            {
                if (paymentEntity.StudentId != null)
                {
                    int studentId = paymentEntity.StudentId.Value;

                    StudentEntity first = studentList.FirstOrDefault(o => o.Id == studentId);
                    if (first != null)
                    {
                        studentList.FirstOrDefault(o => o.Id == studentId).StudentDetail.PaymentList.Add(paymentEntity);
                    }
                }
            }

            paymentDetailEntity.StudentList = studentList.OrderBy(o => o.Name).ToList();
            paymentDetailEntity.PaymentTypeList = new PaymentTypeBusiness(GetProjectType()).Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;
            paymentDetailEntity.Month = DateTime.Today.Month;
            paymentDetailEntity.Year = DateTime.Today.Year;

            return paymentDetailEntity;
        }

        [WebMethod(EnableSession = true)]
        public StudentListAndPaymentTypeInfo GetStudentListAndPaymentTypeInfoForLastTwoMonths()
        {
            StudentListAndPaymentTypeInfo paymentDetailEntity = new StudentListAndPaymentTypeInfo();

            List<StudentEntity> studentList =
                new StudentBusiness(GetProjectType()).Get_Student().Result.Where(o => o.IsStudent == true).ToList();

            List<PaymentEntity> paymentEntityList = new PaymentBusiness(GetProjectType()).Get_LastTwoMonths().Result;

            foreach (PaymentEntity paymentEntity in paymentEntityList)
            {
                if (paymentEntity.StudentId != null)
                {
                    int studentId = paymentEntity.StudentId.Value;

                    StudentEntity first = studentList.FirstOrDefault(o => o.Id == studentId);
                    if (first != null)
                    {
                        studentList.First(o => o.Id == studentId).StudentDetail.PaymentList.Add(paymentEntity);
                    }
                }
            }

            paymentDetailEntity.StudentList = studentList.OrderBy(o => o.Name).ToList();
            paymentDetailEntity.PaymentTypeList = new PaymentTypeBusiness(GetProjectType()).Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;
            paymentDetailEntity.Month = DateTime.Today.Month;
            paymentDetailEntity.Year = DateTime.Today.Year;

            return paymentDetailEntity;
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<ClassEntity> ControlClassName(string className)
        {
            return new ClassBusiness(GetProjectType()).ControlClassName(className);
        }


        [WebMethod(EnableSession = true)]
        public StudentEntity GetStudentEntityWithFullName(string fullName)
        {
            return new StudentBusiness(GetProjectType()).Get_StudentWithFullName(fullName);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<ClassEntity>> GetClassList()
        {
            return new ClassBusiness(GetProjectType()).Get_Class(new SearchEntity() { IsDeleted = false });
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<IncomeAndExpenseEntity>> GetIncomeAndExpenseList()
        {
            return new IncomeAndExpenseBusiness(GetProjectType()).Get_IncomeAndExpense(new SearchEntity() { IsDeleted = false });

        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<IncomeAndExpenseEntity>> GetIncomeAndExpenseListForCurrentDate()
        {
            return new IncomeAndExpenseBusiness(GetProjectType()).Get_IncomeAndExpenseWithYearAndMonth(new SearchEntity() { IsDeleted = false }, DateTime.Today.Year, DateTime.Today.Month);

        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<IncomeAndExpenseEntity>> GetIncomeAndExpenseListWithMonthAndYear(int year, int month)
        {
            return new IncomeAndExpenseBusiness(GetProjectType()).Get_IncomeAndExpenseWithYearAndMonth(new SearchEntity() { IsDeleted = false }, year, month);

        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateClass(string encryptId, ClassEntity classEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            classEntity.Id = 0;
            if (!string.IsNullOrEmpty(encryptId))
            {
                int.TryParse(Cipher.Decrypt(encryptId), out var id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                classEntity.Id = id;
            }

            classEntity.DatabaseProcess = currentProcess;

            return new ClassBusiness(GetProjectType()).Set_Class(classEntity);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateIncomeAndExpenseTypeEntity(string encryptId, IncomeAndExpenseTypeEntity IncomeAndExpenseTypeEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            IncomeAndExpenseTypeEntity.Id = 0;
            if (!string.IsNullOrEmpty(encryptId))
            {
                int.TryParse(Cipher.Decrypt(encryptId), out var id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                IncomeAndExpenseTypeEntity.Id = id;
            }

            IncomeAndExpenseTypeEntity.DatabaseProcess = currentProcess;

            return new IncomeAndExpenseTypeBusiness(GetProjectType()).Set_IncomeAndExpenseType(IncomeAndExpenseTypeEntity);
        }


        [WebMethod(EnableSession = true)]
        public DataResultArgs<ClassEntity> GetClassWithId(string id)
        {
            DataResultArgs<ClassEntity> result = new DataResultArgs<ClassEntity>
            {
                HasError = true,
                ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result = new ClassBusiness(GetProjectType()).Get_ClassWithId(CommonFunctions.GetData<int>(id));
                    if (result.Result != null)
                        result.HasError = false;
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<WorkerEntity>> GetWorkerList()
        {
            return new WorkerBusiness(GetProjectType()).Get_Worker(new SearchEntity() { IsDeleted = false }, null);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> AddIncomeAndExpense(string encryptId, IncomeAndExpenseEntity incomeAndExpenseEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            incomeAndExpenseEntity.Id = 0;
            if (!string.IsNullOrEmpty(encryptId))
            {
                int.TryParse(Cipher.Decrypt(encryptId), out var id);
                if (id > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                incomeAndExpenseEntity.Id = id;
            }

            incomeAndExpenseEntity.DatabaseProcess = currentProcess;

            return new IncomeAndExpenseBusiness(GetProjectType()).Set_IncomeAndExpense(incomeAndExpenseEntity);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<PaymentSummary>> Get_IncomeAndExpenseSummaryForCurrentMonth()
        {
            return new PaymentBusiness(GetProjectType()).Get_IncomeAndExpenseSummaryWithMonthAndYear(DateTime.Today.Year, DateTime.Today.Month);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<PaymentSummary>> Get_IncomeAndExpenseSummaryWithYearAndMonth(int year,int month)
        {
            return new PaymentBusiness(GetProjectType()).Get_IncomeAndExpenseSummaryWithMonthAndYear(year, month);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> DeleteIncomeAndExpenseWithId(string id)
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>
            {
                HasError = true,
                ErrorDescription = "Id bilgisine ulaşılamadı"
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    IncomeAndExpenseEntity entity = new IncomeAndExpenseEntity { Id = idInt, DatabaseProcess = DatabaseProcess.Deleted };
                    result = new IncomeAndExpenseBusiness(GetProjectType()).Set_IncomeAndExpense(entity);
                }
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<IncomeAndExpenseEntity> GetIncomeAndExpenseWithId(string id)
        {
            DataResultArgs<IncomeAndExpenseEntity> result = new DataResultArgs<IncomeAndExpenseEntity>
            {
                HasError = true,
                ErrorDescription = "Entity ulaşılamadı..."
            };

            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    result = new IncomeAndExpenseBusiness(GetProjectType()).Get_IncomeAndExpenseWithId(CommonFunctions.GetData<int>(id));
                    if (result.Result != null)
                        result.HasError = false;
                }
            }

            return result;
        }
    }
}
