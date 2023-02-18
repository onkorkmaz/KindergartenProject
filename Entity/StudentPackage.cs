using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class StudentPackage
    {
        public StudentPackage()
        {
            PaymentList = new List<PaymentEntity>();
            StudentAttendanceBookList = new List<StudentAttendanceBookEntity>();
            ClassInfo = new ClassEntity();
        }

        public List<PaymentEntity> PaymentList { get; set; }
        public bool AddUnPaymentRecordAfterStundetInsert { get; set; }
        public List<StudentAttendanceBookEntity> StudentAttendanceBookList { get; set; }
        public ClassEntity ClassInfo { get; set; }

    }
}
