using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Entity;
using Business;
using System.Text;

namespace KindergartenProject
{
    public partial class PaymentDetail : System.Web.UI.Page
    {
        ProjectType projectType = ProjectType.None;
        private const string studentDoesNotFound = "Ödeme detayı için öğrenci seçmelisiniz !!!";
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            projectType = (ProjectType)Session[CommonConst.ProjectType];
            int currentYear = DateTime.Today.Year;
            int currentMonth = DateTime.Today.Month;
            if (!Page.IsPostBack)
            {
                int year = 2021;
                for (int i = -1; i < 10; i++)
                {
                    string displatText = (year + i).ToString() + "-" + (year + i + 1).ToString();
                    drpYear.Items.Add(new ListItem(displatText, (year + i).ToString()));
                }

               
                if (currentMonth < 8)
                    currentYear--;

                drpYear.SelectedValue = currentYear.ToString();
            }

            divInformation.InformationVisible = false;
            divInformation.ListRecordPage = "/odeme-plani";

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.PaymentPlan);
            master.SetVisibleSearchText(false);


            object Id = Page.RouteData.Values["student_id"];


            if (Id == null)
            {
                divInformation.ErrorText = studentDoesNotFound;
                divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                divInformation.ErrorLink = "/odeme-plani";
            }
            else
            {
                string IdDecrypt = Cipher.Decrypt(Id.ToString());
                hdnId.Value = IdDecrypt;
                int id = CommonFunctions.GetData<int>(IdDecrypt);
                if (id <= 0)
                {
                    divInformation.ErrorText = studentDoesNotFound;
                    divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                    divInformation.ErrorLink = "/odeme-plani";
                }
                else
                {
                    StudentEntity entity = new StudentBusiness(projectType).Get_Student(new SearchEntity() { Id = id }).Result[0];

                    lblStudentInto.Text = "<a href = \"/ogrenci-guncelle/" + entity.Id + "\">" +
                        "<div id = 'btnUniqueNameSurnam' class='btn btn-primary' >" + entity.FullName.ToUpper() + "</div></a> &nbsp;&nbsp;&nbsp;";

                    if (true.Equals(entity.IsActive))
                    {
                        lblStudentInto.Text += "<a href= '/email-gonder/" + entity.Id +
                                           "'><div id = 'btnSendEmail' class='btn btn-warning'>Mail Gönder</div></a>";
                    }

                    if (false.Equals(entity.IsActive))
                    {
                        lblStudentInto.Text += "&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:red;'><b>Öğrenci Pasiftir...</b></span>";
                    }

                }
            }
        }
    }
}