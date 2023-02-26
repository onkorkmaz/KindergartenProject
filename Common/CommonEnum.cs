using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ProjectType
    {
        None = 0,
        BenimDuntamKinderGartenSezenSokak = 1,
        BenimDunyamEgitimMerkeziİstiklalCaddesi = 2
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
        IncomeAndExpenseType = 11,
        Authority = 12
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
        + ' = ' +convert(varchar(50), id)+',' from tbAuthority a
        where isDeleted=0
     */
    #endregion

    public enum Authority
    {
        none = 0,
        ogrenci_islem = 1,
        ogrenci_izleme = 2,
        odeme_plani_izleme = 3,
        odeme_plani_ozet_izleme = 4,
        yoklama_izleme = 5,
        yoklama_islem = 6,
        gelir_gider_izleme = 7,
        gelir_gider_islem = 8,
        odeme_tipleri_izleme = 9,
        odeme_tipleri_islem = 10,
        sinif_izleme = 11,
        sinif_islem = 12,
        çalisan_yonetimi_izleme = 13,
        çalisan_yonetimi_islem = 14,
        gelir_gider_tipi_islem = 15,
        gelir_gider_tipi_izleme = 16,
        sifre_degistir_islem = 17,
        sifre_degistir_izleme = 18,
    }

    public enum AuthorityTypeEnum
    {
        None = 0,
        Developer = 1,
        SuperAdmin = 2,
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
        AttendanceBook = 3

    }
}
