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
        Workers = 7,
        OutgoingAdd = 8,
        OutgoingList = 9

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
}
