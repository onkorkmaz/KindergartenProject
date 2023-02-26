using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class StudentAndListOfPaymentPackage
    {
        public StudentAndListOfPaymentPackage()
        {
            StudentAndListOfPayment = new StudentAndListOfPayment();
            PaymentTypeEntityList = new List<PaymentTypeEntity>();
        }

        public StudentAndListOfPayment StudentAndListOfPayment { get; set; }

        public List<PaymentTypeEntity> PaymentTypeEntityList { get; set; }

        public int Year { get; set; }
    }

    public class StudentAndListOfPaymentListPackage
    {
        public StudentAndListOfPaymentListPackage()
        {
            StudentAndListOfPaymentList = new List<StudentAndListOfPayment>();
            PaymentTypeEntityList = new List<PaymentTypeEntity>();
        }

        public List<StudentAndListOfPayment> StudentAndListOfPaymentList { get; set; }

        public List<PaymentTypeEntity> PaymentTypeEntityList { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

    }


    public class StudentAndListOfPayment
    {
        public StudentAndListOfPayment()
        {
            StudentEntity = new StudentEntity();
            PaymentEntityList = new List<PaymentEntity>();
        }
        public StudentEntity StudentEntity { get; set; }
        public List<PaymentEntity> PaymentEntityList { get; set; }
    }

}
