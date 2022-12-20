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
    public class ClassBusiness : BaseBusiness
    {
        public ClassBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public DataResultArgs<bool> Set_Class(ClassEntity entity)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@warningOfStudentCount", entity.WarningOfStudentCount);
            cmd.Parameters.AddWithValue("@mainTeacher", entity.MainTeacherCode);
            cmd.Parameters.AddWithValue("@helperTeacher", entity.HelperTeacherCode);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);


            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_Class");
                con.Close();
            }

            StudentBusiness.ClearStudentCache(base.ProjectType);
            new StudentBusiness(base.ProjectType).Get_AllStudentWithCache();

            return resultSet;
        }

        public DataResultArgs<List<ClassEntity>> Get_ClassForStudent()
        {
            DataResultArgs<List<ClassEntity>> resultSet = new DataResultArgs<List<ClassEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
            cmd.CommandType = CommandType.StoredProcedure;

            DataResultArgs<SqlDataReader> result = new DataResultArgs<SqlDataReader>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                result = DataProcess.ExecuteProcDataReader(con, cmd, "get_ClassForStudent");

                getResultList(resultSet, result);
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<List<ClassEntity>> Get_Class(SearchEntity entity)
        {
            DataResultArgs<List<ClassEntity>> resultSet = new DataResultArgs<List<ClassEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

            DataResultArgs<SqlDataReader> result = new DataResultArgs<SqlDataReader>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Class");

                getResultList(resultSet, result);
                con.Close();
            }
            return resultSet;
        }

        private static void getResultList(DataResultArgs<List<ClassEntity>> resultSet, DataResultArgs<SqlDataReader> result)
        {
            if (result.HasError)
            {
                resultSet.HasError = result.HasError;
                resultSet.ErrorDescription = result.ErrorDescription;
                resultSet.ErrorCode = result.ErrorCode;
            }
            else
            {
                SqlDataReader dr = result.Result;
                List<ClassEntity> lst = new List<ClassEntity>();
                ClassEntity elist;
                while (dr != null && dr.Read())
                {
                    elist = new ClassEntity();
                    elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                    elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                    elist.Description = GeneralFunctions.GetData<String>(dr["description"]);
                    if (string.IsNullOrEmpty(elist.Description))
                        elist.Description = "";
                    elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                    elist.WarningOfStudentCount = GeneralFunctions.GetData<Int32?>(dr["warningOfStudentCount"]);
                    elist.MainTeacherCode = GeneralFunctions.GetData<Int32?>(dr["mainTeacherCode"]);
                    elist.HelperTeacherCode = GeneralFunctions.GetData<Int32?>(dr["helperTeacherCode"]);
                    elist.MainTeacher = GeneralFunctions.GetData<string>(dr["mainTeacher"]);
                    elist.HelperTeacher = GeneralFunctions.GetData<string>(dr["helperTeacher"]);
                    elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);
                    elist.IsActiveMainTeacher = GeneralFunctions.GetData<bool>(dr["IsActiveMainTeacher"]);
                    elist.IsActiveHelperTeacer = GeneralFunctions.GetData<bool>(dr["IsActiveHelperTeacher"]);
                    string count = GeneralFunctions.GetData<string>(dr["studentCount"]);
                    elist.StudentCount = (string.IsNullOrEmpty(count)) ? "0" : count;

                    decimal teacherOutGoing = GeneralFunctions.GetData<Decimal>(dr["teacherOutGoing"]);
                    elist.TeacherOutGoing = (teacherOutGoing > 0) ? teacherOutGoing.ToString("###,###,###.##") + " TL" : "";

                    decimal studentIncoming = GeneralFunctions.GetData<Decimal>(dr["studentIncoming"]);
                    elist.StudentIncoming = (studentIncoming > 0) ? studentIncoming.ToString("###,###,###.##") + " TL" : "";

                    decimal studentCurrentIncoming = GeneralFunctions.GetData<Decimal>(dr["studentCurrentIncoming"]);
                    elist.StudentCurrentIncoming = (studentCurrentIncoming > 0) ? studentCurrentIncoming.ToString("###,###,###.##") + " TL" : "";
                    elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16?>(dr["projectType"]);
                    lst.Add(elist);
                }

                if (dr != null)
                    dr.Close();
                resultSet.Result = lst;
            }
        }

        public DataResultArgs<ClassEntity> Get_ClassWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            SearchEntity searchEntity = new SearchEntity() { IsDeleted = false, Id = id };

            DataResultArgs<List<ClassEntity>> resultSet = Get_Class(searchEntity);



            DataResultArgs <ClassEntity> result = new DataResultArgs<ClassEntity>();

            EntityHelper.CopyDataResultArgs<ClassEntity>(resultSet, result);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;

        }

        public DataResultArgs<ClassEntity> ControlClassName(string className)
        {
            DataResultArgs<ClassEntity> resultSet = new DataResultArgs<ClassEntity>();

            DataResultArgs<List<ClassEntity>> currentResultSet = Get_Class(new SearchEntity() { });

            if (currentResultSet.HasError)
            {
                EntityHelper.CopyDataResultArgs<ClassEntity>(currentResultSet, resultSet);
            }
            else
            {
                List<ClassEntity> list = currentResultSet.Result;
                if (list != null)
                {
                    foreach (ClassEntity entity in list)
                    {
                        if (GeneralFunctions.IsSameStrings(entity.Name, className))
                        {
                            resultSet.Result = entity;
                        }
                    }
                }
            }
            return resultSet;
        }

    }
}
