using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
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
        AddStudenList = 2,
        PaymentPlan = 3,
        PaymentType = 4,
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
}
