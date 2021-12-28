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
            routes.MapPageRoute("benim-dunyam-anaokulu", "benim-dunyam-anaokulu", "~/Default.aspx");
            routes.MapPageRoute("ogrenci-listesi", "ogrenci-listesi", "~/StudentList.aspx");
            routes.MapPageRoute("sinif-ogreci-listesi", "sinif-ogreci-listesi", "~/ClassAndStudentList.aspx");

            routes.MapPageRoute("ogrenci-ekle", "ogrenci-ekle", "~/AddStudent.aspx");
            routes.MapPageRoute("ogrenci-guncelle", "ogrenci-guncelle/{student_id}", "~/AddStudent.aspx");


            routes.MapPageRoute("odeme-plani", "odeme-plani", "~/PaymentPlan.aspx");

            routes.MapPageRoute("odeme-plani-detay", "odeme-plani-detay/{student_id}", "~/PaymentDetail.aspx");
            routes.MapPageRoute("email-gonder", "email-gonder/{student_id}", "~/SendEmail.aspx");



            routes.MapPageRoute("odeme-tipleri", "odeme-tipleri", "~/PaymentType.aspx");
            routes.MapPageRoute("sinif-listesi", "sinif-listesi", "~/ClassList.aspx");
            routes.MapPageRoute("calisan-listesi", "calisan-listesi", "~/Workers.aspx");
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