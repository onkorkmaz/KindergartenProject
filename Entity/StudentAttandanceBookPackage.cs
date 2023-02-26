using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class StudentAttandanceBookPackage
    {
        public StudentAttandanceBookPackage()
        {
            StudentEntity = new StudentEntity();
            StudentAttendanceBookEntityList = new List<StudentAttendanceBookEntity>();
        }
        public StudentEntity StudentEntity { get; set; }
        public List<StudentAttendanceBookEntity> StudentAttendanceBookEntityList { get; set; }
    }
}
