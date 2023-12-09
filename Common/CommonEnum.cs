using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ProjectType
    {
        None = 0,
        BenimDunyamAnaokuluSezenSokak = 1,
        BenimDunyamEgitimMerkeziIstiklalCaddesi = 2
    }

    public enum DatabaseProcess
    {
        Add = 0,
        Update = 1,
        Deleted = 2,
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

    #region
    /*  
        SELECT 
        REPLACE
        (
        REPLACE(
	        REPLACE(
		        REPLACE(
			        REPLACE(
				        REPLACE(
					        REPLACE(lower(a.name), ' ', '_')
						        , 'Ö', 'ö')
				        , 'İ', 'i')
			        ,'ö','o')
		        ,'ğ','g')
	        ,'ş','s'),
        'ı','i')
        + ' = ' +convert(varchar(50), id)+',' from tbAuthorityScreen a
        where isDeleted=0
     */
    #endregion

    public enum OwnerStatusEnum
    {
        None = 0,
        SuperAdmin = 1,
        Admin = 2,
        Authority = 3
    }

    public enum SchoolClassEnum
    {
        None = 0,
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

    public enum CacheType
    {
        None = 0,
        StudentList = 1,
        PaymentList = 2,
        AttendanceBook = 3,
        AuthorityScreen = 4,
        Authority = 5,
        AuthorityType = 6,

    }
}
