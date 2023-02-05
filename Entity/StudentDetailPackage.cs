using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class StudentDetailPackage
    {
        public StudentDetailPackage()
        {
            PaymentList = new List<PaymentEntity>();
            StudentAttendanceBookList = new List<StudentAttendanceBookEntity>();
            ClassInfo = new ClassEntity();
            StudentDetailEntity = new StudentDetailEntity();
        }

        public StudentDetailEntity StudentDetailEntity { get; set; }
        public List<PaymentEntity> PaymentList { get; set; }

        public bool AddUnPaymentRecordAfterStundetInsert { get; set; }

        public List<StudentAttendanceBookEntity> StudentAttendanceBookList { get; set; }

        public ClassEntity ClassInfo { get; set; }
    }
}


