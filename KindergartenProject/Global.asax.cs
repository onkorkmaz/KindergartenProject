using Common;
using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace KindergartenProject
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            UrlReWrite(RouteTable.Routes);
        }

        void UrlReWrite(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute("uye-giris", "uye-giris", "~/login.aspx");
            routes.MapPageRoute("benim-dunyam-montessori-okullari", "benim-dunyam-montessori-okullari", "~/Default.aspx");
            routes.MapPageRoute("ogrenci-listesi", "ogrenci-listesi", "~/StudentList.aspx");
            routes.MapPageRoute("ogrenci-sinif-listesi", "ogrenci-listesi/{class_id}", "~/StudentList.aspx");

            routes.MapPageRoute("sinif-ogreci-listesi", "sinif-ogreci-listesi", "~/ClassAndStudentList.aspx");

            routes.MapPageRoute("ogrenci-ekle", "ogrenci-ekle", "~/StudentAdd.aspx");
            routes.MapPageRoute("ogrenci-guncelle", "ogrenci-guncelle/{student_id}", "~/StudentAdd.aspx");

            routes.MapPageRoute("odeme-plani", "odeme-plani", "~/PaymentPlan.aspx");

            routes.MapPageRoute("yoklama-sayfasi", "yoklama-sayfasi", "~/StudentAttendanceBookList.aspx");

            routes.MapPageRoute("odeme-plani-detay", "odeme-plani-detay/{student_id}", "~/PaymentDetail.aspx");
            routes.MapPageRoute("email-gonder", "email-gonder/{student_id}", "~/SendEmail.aspx");

            routes.MapPageRoute("odeme-tipleri", "odeme-tipleri", "~/PaymentType.aspx");
            routes.MapPageRoute("sinif-listesi", "sinif-listesi", "~/ClassList.aspx");
            routes.MapPageRoute("calisan-listesi", "calisan-listesi", "~/WorkerList.aspx");

            routes.MapPageRoute("gelir-gider-ekle", "gelir-gider-ekle", "~/IncomeAndExpenseAdd.aspx");
            routes.MapPageRoute("gelir-gider-listesi", "gelir-gider-listesi", "~/IncomeAndExpenseList.aspx");
            routes.MapPageRoute("gelir-gider-tipi", "gelir-gider-tipi", "~/IncomeAndExpenseType.aspx");
            routes.MapPageRoute("yetki-tipi", "yetki-tipi", "~/Authority.aspx");
            routes.MapPageRoute("cache-clear", "cache-clear", "~/CacheClear.aspx");

            routes.MapPageRoute("cikis", "cikis", "~/Exit.aspx");

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}