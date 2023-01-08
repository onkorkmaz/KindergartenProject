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
    public class StudentAttendanceBookBusiness : BaseBusiness
    {
        private static Dictionary<Int16, List<StudentAttendanceBookEntity>> AllStudentWithCacheWithProjectType = new Dictionary<Int16, List<StudentAttendanceBookEntity>>();
        public StudentAttendanceBookBusiness(ProjectType projectType) : base(projectType)
        {
        }

        public DataResultArgs<bool> Set_StudentAttendanceBook(StudentAttendanceBookEntity entity)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@studentId", entity.StudentId);
            cmd.Parameters.AddWithValue("@arrivalDate", entity.ArrivalDate);
            cmd.Parameters.AddWithValue("@isArrival", entity.IsArrival);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_StudentAttendanceBook");
                con.Close();
            }

            AllStudentWithCacheWithProjectType[base.ProjectTypeInt16] = new List<StudentAttendanceBookEntity>();

            return resultSet;
        }

        public List<StudentAttendanceBookEntity> Get_StudentAttendanceBookWithCache(SearchEntity searchEntity)
        {
            if (AllStudentWithCacheWithProjectType.ContainsKey(base.ProjectTypeInt16) && AllStudentWithCacheWithProjectType[base.ProjectTypeInt16] != null && AllStudentWithCacheWithProjectType[base.ProjectTypeInt16].Count() > 0)
            {
                return AllStudentWithCacheWithProjectType[base.ProjectTypeInt16];
            }
            else
            {
                List<StudentAttendanceBookEntity> result = Get_StudentAttendanceBook(searchEntity).Result;
                if (AllStudentWithCacheWithProjectType.ContainsKey(base.ProjectTypeInt16))
                {
                    AllStudentWithCacheWithProjectType[base.ProjectTypeInt16] = result;
                }
                else
                {
                    AllStudentWithCacheWithProjectType.Add(base.ProjectTypeInt16, result);
                }

            }
            return AllStudentWithCacheWithProjectType[base.ProjectTypeInt16];

        }

        public DataResultArgs<List<StudentAttendanceBookEntity>> Get_StudentAttendanceBook(SearchEntity entity)
        {
            DataResultArgs<SqlDataReader> result = new DataResultArgs<SqlDataReader>();
            DataResultArgs<List<StudentAttendanceBookEntity>> resultSet = new DataResultArgs<List<StudentAttendanceBookEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                result = DataProcess.ExecuteProcDataReader(con, cmd, "get_StudentAttendanceBook");

                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<StudentAttendanceBookEntity> lst = new List<StudentAttendanceBookEntity>();
                    StudentAttendanceBookEntity elist;
                    while (dr.Read())
                    {
                        elist = new StudentAttendanceBookEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.StudentId = GeneralFunctions.GetData<Int32?>(dr["studentId"]);
                        elist.ArrivalDate = GeneralFunctions.GetData<DateTime?>(dr["arrivalDate"]);
                        elist.IsArrival = GeneralFunctions.GetData<Boolean?>(dr["isArrival"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.Description = GeneralFunctions.GetData<String>(dr["description"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }
            return resultSet;
        }

    }
}
