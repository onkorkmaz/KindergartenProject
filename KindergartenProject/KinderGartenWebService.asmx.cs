using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Business;
using Entity;
using Common;
using System.Web.Script.Services;
using System.Web;
using System.Data;

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


        #region Authority

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<AuthorityEntity>> GetAuthorityList()
        {
            return new AuthorityBusiness(GetProjectType()).Get_Authority(new SearchEntity() { IsDeleted = false });
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateAuthority(string id, AuthorityEntity authorityEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            authorityEntity.Id = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                authorityEntity.Id = idInt;
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


        [WebMethod(EnableSession = true)]
        public DataResultArgs<StudentEntity> GetStudentEntity(string citizenshipNumber)
        {
            return new StudentBusiness(GetProjectType()).Get_Student(citizenshipNumber);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<StudentEntity> GetStudentEntityWithId(string id)
        {
            return new StudentBusiness(GetProjectType()).Get_Student(CommonFunctions.GetData<int>(id));
        }

        [WebMethod(EnableSession = true)]
        public List<StudentEntity> SearchStudent(string searchValue)
        {
            List<StudentEntity> resultSet = new StudentBusiness(GetProjectType()).Get_Student().Result;

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
                    int recordedStudentNumber = new StudentBusiness(GetProjectType()).Get_Student().Result.Where(o => o.ClassId.HasValue && o.ClassId.Value == cId && o.IsActive.Value).ToList().Count();
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
        public DataResultArgs<bool> InsertOrUpdatePaymentType(string id, PaymentTypeEntity paymentTypeEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            paymentTypeEntity.Id = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                paymentTypeEntity.Id = idInt;
            }

            paymentTypeEntity.DatabaseProcess = currentProcess;

            return new PaymentTypeBusiness(GetProjectType()).Set_PaymentType(paymentTypeEntity);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateWorker(string id, WorkerEntity workerEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            workerEntity.Id = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                workerEntity.Id = idInt;
            }

            workerEntity.DatabaseProcess = currentProcess;

            DataResultArgs<bool> result = new WorkerBusiness(GetProjectType()).Set_Worker(workerEntity);
            if(!result.HasError && currentProcess == DatabaseProcess.Update && false.Equals(workerEntity.IsActive))
            {
                result = new ClassBusiness(GetProjectType()).UpdateClassForDeletedWorkers(CommonFunctions.GetData<int>(workerEntity.Id));
            }

            return result;
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

                    if(!result.HasError)
                    {
                        result = new ClassBusiness(GetProjectType()).UpdateClassForDeletedWorkers(CommonFunctions.GetData<int>(id));
                    }

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
                        //studentEntity.StudentPackage.AddUnPaymentRecordAfterStundetInsert = true;
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
        public List<StudentEntity> Get_StudentFromCache()
        {
            List<StudentEntity> resultSet = new StudentBusiness(GetProjectType()).Get_Student().Result;
            return resultSet;
        }

        [WebMethod(EnableSession = true)]
        public List<StudentAttandanceBookPackage> GetAllStudentAndAttendanceList()
        {
            List<StudentAttandanceBookPackage> packageList = new List<StudentAttandanceBookPackage>();
            List<StudentEntity> studentList = new StudentBusiness(GetProjectType()).Get_Student().Result.Where(o => o.IsActive == true && o.IsStudent == true).ToList();

            foreach (StudentEntity entity in studentList)
            {
                StudentAttandanceBookPackage package = new StudentAttandanceBookPackage();
                package.StudentEntity = entity;
                package.StudentAttendanceBookEntityList = new StudentAttendanceBookBusiness(GetProjectType()).Get_StudentAttendanceBookWithCache(entity.Id);
                packageList.Add(package);
            }

            return packageList;
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
        public StudentAndListOfPaymentListPackage GetStudentAndListOfPaymentListPackageForCurrentMonth()
        {
            StudentAndListOfPaymentListPackage package = new StudentAndListOfPaymentListPackage();

            var studentEntityList = new StudentBusiness(GetProjectType()).Get_Student().Result.Where(o => o.IsStudent == true).ToList();

            foreach (StudentEntity entity in studentEntityList)
            {
                StudentAndListOfPayment pac = new StudentAndListOfPayment();
                pac.PaymentEntityList = new PaymentBusiness(GetProjectType()).Get_Payment(entity.Id, DateTime.Today.Year, DateTime.Today.Month).Result;
                pac.StudentEntity = entity;
                package.StudentAndListOfPaymentList.Add(pac);
            }


            package.PaymentTypeEntityList = new PaymentTypeBusiness(GetProjectType()).Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;
            package.Year = DateTime.Today.Year;
            package.Month = DateTime.Today.Month;

            return package;

        }

        [WebMethod(EnableSession = true)]
        public StudentAndListOfPaymentPackage GetStudentAndListOfPaymentPackage(string studentId, string year)
        {
            StudentAndListOfPaymentPackage package = new StudentAndListOfPaymentPackage();

            int yearInt = Convert.ToInt32(year);
            int studentIdInt = CommonFunctions.GetData<int>(studentId);
            package.StudentAndListOfPayment.StudentEntity = new StudentBusiness(GetProjectType()).Get_Student(studentIdInt).Result;

            List<PaymentEntity> paymentForCurrentYear = new PaymentBusiness(GetProjectType()).Get_Payment(studentIdInt, yearInt).Result;
            List<PaymentEntity> paymentForNextYear = new PaymentBusiness(GetProjectType()).Get_Payment(studentIdInt, (yearInt + 1)).Result;

            for (int i = 9; i <= 12; i++)
            {
                if (paymentForCurrentYear.Any(o => o.Month == i && o.IsActive == true))
                {
                    package.StudentAndListOfPayment.PaymentEntityList.Add(paymentForCurrentYear.First(o => o.Month == i && o.IsActive == true));
                }
            }

            for (int i = 1; i <= 8; i++)
            {
                if (paymentForNextYear.Any(o => o.Month == i && o.IsActive == true))
                {
                    package.StudentAndListOfPayment.PaymentEntityList.Add(paymentForNextYear.First(o => o.Month == i && o.IsActive == true));
                }
            }

            package.PaymentTypeEntityList = new PaymentTypeBusiness(GetProjectType()).Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;
            package.Year = yearInt;
            return package;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> ClearPaymentCache()
        {
            DataResultArgs<bool> result = new DataResultArgs<bool>();
            try
            {
                new PaymentBusiness(GetProjectType()).ClearPaymentCache();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.ErrorDescription = ex.Message;
            }

            return result;
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
            DataResultArgs<StudentAttendanceBookEntity> result = new StudentAttendanceBookBusiness(GetProjectType()).Set_StudentAttendanceBook(entity);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentEntity> DoPaymentOrUnPayment(string id, string studentId, string year, string month, string amount, bool isPayment, string paymentType)
        {
            PaymentEntity paymentEntity = new PaymentEntity
            {
                StudentId = CommonFunctions.GetData<int>(studentId),
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

            DataResultArgs<PaymentEntity> resultSet = new PaymentBusiness(GetProjectType()).Set_Payment(paymentEntity);

            paymentEntity.Id = resultSet.Result.Id;
            DataResultArgs<PaymentEntity> returnResultSet = new DataResultArgs<PaymentEntity>();
            returnResultSet.ErrorCode = resultSet.ErrorCode;
            returnResultSet.ErrorDescription = resultSet.ErrorDescription;
            returnResultSet.HasError = resultSet.HasError;
            returnResultSet.Result = paymentEntity;

            return returnResultSet;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentEntity> SetPaymentAmount(string id, string studentId, string year, string month,
            string currentAmount, string paymentType)
        {
            PaymentEntity paymentEntity = new PaymentEntity
            {
                StudentId = CommonFunctions.GetData<int>(studentId),
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

            DataResultArgs<PaymentEntity> resultSet = new PaymentBusiness(GetProjectType()).Set_Payment(paymentEntity);
            return resultSet;
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<PaymentEntity> SetAnotherPaymentAmount(string id, string studentId, string year, string month,
            string currentAmount, string paymentType)
        {
            
            PaymentEntity paymentEntity = new PaymentEntity
            {
                StudentId = CommonFunctions.GetData<int>(studentId),
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

            DataResultArgs<PaymentEntity> resultSet = new PaymentBusiness(GetProjectType()).Set_Payment(paymentEntity);
            return resultSet;

        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<ClassEntity> ControlClassName(string className)
        {
            return new ClassBusiness(GetProjectType()).ControlClassName(className);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<StudentEntity> GetStudentEntityWithFullName(string fullName)
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
        public DataResultArgs<bool> InsertOrUpdateClass(string id, ClassEntity classEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            classEntity.Id = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                classEntity.Id = idInt;
            }

            classEntity.DatabaseProcess = currentProcess;

            return new ClassBusiness(GetProjectType()).Set_Class(classEntity);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<bool> InsertOrUpdateIncomeAndExpenseTypeEntity(string id, IncomeAndExpenseTypeEntity IncomeAndExpenseTypeEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            IncomeAndExpenseTypeEntity.Id = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                IncomeAndExpenseTypeEntity.Id = idInt;
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
        public DataResultArgs<bool> AddIncomeAndExpense(string id, IncomeAndExpenseEntity incomeAndExpenseEntity)
        {
            DatabaseProcess currentProcess = DatabaseProcess.Add;
            incomeAndExpenseEntity.Id = 0;
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(Cipher.Decrypt(id), out var idInt);
                if (idInt > 0)
                {
                    currentProcess = DatabaseProcess.Update;
                }
                incomeAndExpenseEntity.Id = idInt;
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
        public DataResultArgs<List<ExpenseSummary>> Get_ExpenseSummaryWithMonthAndYear()
        {
            return new PaymentBusiness(GetProjectType()).Get_ExpenseSummaryWithMonthAndYear(DateTime.Today.Year, DateTime.Today.Month);
        }


        

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<PaymentSummaryDetail>> Get_IncomeAndExpenseSummaryForCurrentMonthDetail()
        {
            return new PaymentBusiness(GetProjectType()).Get_IncomeAndExpenseSummaryDetailWithMonthAndYear(DateTime.Today.Year, DateTime.Today.Month);
        }

        [WebMethod(EnableSession = true)]
        public DataResultArgs<List<PaymentSummary>> Get_IncomeAndExpenseSummaryWithYearAndMonth(int year, int month)
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
