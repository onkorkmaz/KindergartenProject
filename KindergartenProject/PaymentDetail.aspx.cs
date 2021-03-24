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
        private const string studentDoesNotFound = "Ödeme detayı için öğrenci seçmelisiniz !!!";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int year = DateTime.Today.Year;

                for(int i = 0;i<5;i++)
                {
                    drpYear.Items.Add(new ListItem((year + i).ToString(), (year + i).ToString()));
                }
                drpYear.SelectedValue = year.ToString();
            }

            divInformation.InformationVisible = false;
            divInformation.ListRecordPage = "PaymentList.aspx";

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.PaymentPlan);
            master.SetVisibleSearchText(false);

            
            object Id = Request.QueryString["Id"];

            if (Id == null)
            {
                divInformation.ErrorText = studentDoesNotFound;
                divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                divInformation.ErrorLink = "PaymentPlan.aspx";
            }
            else
            {
                string IdDecrypt = Cipher.Decrypt(Id.ToString());
                int id = GeneralFunctions.GetData<int>(IdDecrypt);
                if (id <= 0)
                {
                    divInformation.ErrorText = studentDoesNotFound;
                    divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                    divInformation.ErrorLink = "PaymentPlan.aspx";
                }
                else
                {
                    StudentEntity entity = new StudentBusiness().Get_Student(new SearchEntity() { Id = id }).Result[0];

                    lblStudentInto.Text = "<a href = \"AddStudent.aspx?Id=" + entity.EncryptId + "\">" +
                                          entity.FullName.ToUpper() + "</a> &nbsp;&nbsp;&nbsp;";
                    lblStudentInto.Text += "<a href= 'SendEmail.aspx?Id=" + entity.EncryptId +
                                           "'><img title='Mail Gönder' src ='img/icons/email.png'/></a>";

                    //DataResultArgs<List<PaymentTypeEntity>> resultSet = new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false });

                    //if (resultSet.HasError)
                    //{
                    //    divInformation.ErrorText = resultSet.ErrorDescription;
                    //}
                    //else
                    //{
                    //    StringBuilder sb = new StringBuilder();
                    //    sb.AppendLine(@"<table class='table mb - 0'><thead><tr>");

                    //    sb.AppendLine("<th scope='col'>Ay</th>");
                    //    /*
                    //     * <th scope="col">&nbsp;</th>
                    //        <th scope="col">#####</th>
                    //        <th scope="col">Ay</th>
                    //        <th scope="col">Doğum Tarihi</th>
                    //        <th scope="col">Baba Bilg.</th>
                    //        <th scope="col">Anne Bilg.</th>
                    //        <th scope="col">Kayıt D.</th>
                    //        <th scope="col">Aktif</th>
                    //     */
                    //    List<PaymentTypeEntity> list = resultSet.Result;
                    //    foreach (PaymentTypeEntity entity in list)
                    //    {
                    //        sb.AppendLine("<th scope='col'>"+entity.Name+"</th>");
                    //    }
                    //    sb.AppendLine("<th scope='col'>Ödeme Durumu</th>");

                    //    sb.AppendLine(@"</tr></thead></table>");

                    //    divMain.InnerHtml = sb.ToString();
                    //}
                }
            }
        }
    }
}