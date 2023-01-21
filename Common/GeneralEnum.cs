using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ProjectType
    {
        None =0,
        BenimDuntamKinderGartenSezenSokak =1,
        BenimDunyamEgitimMerkeziİstiklalCaddesi=2
    }

    public enum DatabaseProcess
    {
        Add = 0,
        Update = 1,
        Deleted = 2,
    }

    public enum MenuList
    {
        Panel = 0,
        StudentList = 1,
        StudenAdd = 2,
        PaymentPlan = 3,
        PaymentType = 4,
        Login = 5,
        ClassList = 6,
        WorkerList = 7,
        IncomeAndExpenseAdd = 8,
        IncomeAndExpenseList = 9,
        StudentAttendanceBookList = 10,
        IncomeAndExpenseType =11


    }

    public enum PaymentTypeEnum
    {
        None = 0,
        Okul = 1,
        Servis = 2,
        Kirtasiye = 3,
        Mental = 4,
        Diger = 17
    }

    public enum AdminTypeEnum
    {
        None = 0,
        SuperAdmin = 1,
        Visitor = 2
    }

    public enum SchoolClassEnum
    {
        None=0,
        Kindergarten_2_Age = 1,
        Kindergarten_3_Age = 2,
        Kindergarten_4_Age = 3,
        Kindergarten_5_Age = 4,
        Kindergarten_6_Age = 5,
        Class_1 = 6,
        Class_2 = 7,
        Class_3 = 8,
        Class_4 = 9,
        Class_5 = 10,
        Class_6 = 11,
        Class_7 = 12,
        Class_8 = 13,








    }
}
