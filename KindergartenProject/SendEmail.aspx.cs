using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Common;
using Entity;
using Microsoft.Office.Interop.Word;

namespace KindergartenProject
{
    public partial class SendEmail : System.Web.UI.Page
    {
        private const string studentDoesNotFound = "Ödeme detayı için öğrenci seçmelisiniz !!!";

        protected void Page_Load(object sender, EventArgs e)
        {
            divInformation.InformationVisible = false;
            divInformation.ListRecordPage = "PaymentList.aspx";

            if (this.Master is kindergarten master)
            {
                master.SetActiveMenuAttiributes(MenuList.PaymentPlan);
                master.SetVisibleSearchText(false);
            }

            object Id = Request.QueryString["Id"];

            if (Id == null)
            {
                divInformation.ErrorText = studentDoesNotFound;
                divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                divInformation.ErrorLink = "PaymentPlan.aspx";
            }

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
                int width = 15;
                int height = 15;
                StudentEntity entity = new StudentBusiness().Get_StudentWithPaymentList(id);

                lblStudentInto.Text = "<a href = \"AddStudent.aspx?Id=" + entity.EncryptId + "\">" +
                                      entity.FullName.ToUpper() +
                                      "</a> &nbsp;&nbsp;&nbsp;";
                lblStudentInto.Text += "<a href= 'PaymentDetail.aspx?Id=" + entity.EncryptId +
                                       "'><img title='Ödeme Detayı' src ='img/icons/paymentPlan.png'/></a>";


                txtEmail.Text = entity.Email;

                DataResultArgs<List<PaymentTypeEntity>> resultSet =
                    new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() {IsActive = true, IsDeleted = false});

                if (resultSet.HasError)
                {
                    divInformation.ErrorText = resultSet.ErrorDescription;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(
                        @"<table class='table mb - 0'><thead><tr><th scope='col'><input type='checkbox' id='chc_All' name='chc_All' year='" +
                        DateTime.Today.Year.ToString() +
                        "' onclick =chcAllChange(); /></th> <th scope='col'>Ay</th>");

                    List<PaymentTypeEntity> paymentTypeList = resultSet.Result;

                    foreach (PaymentTypeEntity payment in paymentTypeList)
                    {
                        sb.AppendLine("<th scope='col'>" + payment.Name + "</th>");
                    }

                    var monthList = GetMonthList();

                    foreach (int  month in monthList.Keys)
                    {
                        var uniqueName = "_" + DateTime.Today.Year.ToString() + "_" + month;

                        var chcPaymentName = "chc" + uniqueName;

                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td><input type='checkbox' id='" + chcPaymentName + "' name='" + chcPaymentName +
                                      "'/></td>");
                        sb.AppendLine("<td>" + monthList[month] + "</td>");


                        foreach(PaymentTypeEntity payment in paymentTypeList)
                        {
                            PaymentEntity paymentEntity = entity.PaymentList.FirstOrDefault(o =>
                                o.Month == month && o.Year == DateTime.Today.Year && o.PaymentType == payment.Id);

                            if (paymentEntity != null)
                            {
                                if (paymentEntity.IsNotPayable)
                                {
                                    sb.AppendLine("<td> - </td>");
                                }
                                else if (paymentEntity.IsPayment.HasValue)
                                {
                                    if (paymentEntity.IsPayment.Value)
                                    {
                                        sb.AppendLine("<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" + paymentEntity.AmountDesc +
                                                      "</td><td><img width='"+width+"' height='"+height+ "' src=\"img/icons/greenSmile2.png\"/></td></tr></table></td>");
                                    }
                                    else
                                    {
                                        sb.AppendLine("<td><table cellpadding='4'><tr style='vertical-align: middle' ><td>" + paymentEntity.AmountDesc +
                                                      "</td><td><img width='" + width + "' height='" + height + "' src=\"img/icons/unPayment2.png\"/></td></tr></table></td>");
                                    }
                                }
                                else
                                {
                                    sb.AppendLine("<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" + paymentEntity.AmountDesc +
                                                  "</td><td><img width='" + width + "' height='" + height + "' src=\"img/icons/unPayment2.png\"/></td></tr></table></td>");
                                }
                            }
                            else
                            {
                                sb.AppendLine("<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" + payment.AmountDesc +
                                              "</td><td><img width='" + width + "' height='" + height + "' src=\"img/icons/unPayment2.png\"/></td></tr></table></td>");
                            }
                        }

                        sb.AppendLine("</tr>");
                    }

                    sb.AppendLine(@"</tr></thead>");

                    sb.AppendLine("</table>");

                    divMain.InnerHtml = sb.ToString();
                }
            }
        }

        private static Dictionary<int, string> GetMonthList()
        {
            Dictionary<int, string> monthList = new Dictionary<int, string>();
            monthList.Add(1, "Ocak");
            monthList.Add(2, "Şubat");
            monthList.Add(3, "Mart");
            monthList.Add(4, "Nisan");
            monthList.Add(5, "Mayıs");
            monthList.Add(6, "Haziran");
            monthList.Add(7, "Temmuz");
            monthList.Add(8, "Ağustos");
            monthList.Add(9, "Eylül");
            monthList.Add(10, "Ekim");
            monthList.Add(11, "Kasım");
            monthList.Add(12, "Aralık");
            return monthList;
        }


        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            object Id = Request.QueryString["Id"];

            if (Id == null)
            {
                divInformation.ErrorText = studentDoesNotFound;
                divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                divInformation.ErrorLink = "PaymentPlan.aspx";
            }

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
                StudentEntity entity = new StudentBusiness().Get_StudentWithPaymentList(id);
                Application wordApp = new Application();
                string savePathToFiles = Server.MapPath("/PaymentDocument/" + GeneralFunctions.ReplaceTurkishChar(entity.FullName));

                string templatePath = Server.MapPath("/Template");

                if (!Directory.Exists(savePathToFiles))
                    Directory.CreateDirectory(savePathToFiles);

                Document document =
                    wordApp.Documents.Open(templatePath + "/odemePlani.docx", ReadOnly: true, Visible: true);
                document.Activate();

                FindAndReplace(wordApp, "<fullName>", entity.FullName.ToUpper());

                DataResultArgs<List<PaymentTypeEntity>> resultSet =
                    new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false });

                List<EmailPaymentEntity> emailPaymentList = GetEmailPaymentList(resultSet.Result, entity.PaymentList);

                foreach (EmailPaymentEntity emailPaymentEntity in emailPaymentList)
                {
                    FindAndReplace(wordApp, emailPaymentEntity.TemplateName, emailPaymentEntity.AmountDescription);
                }

                string fileName = savePathToFiles + "/" + GeneralFunctions.ReplaceTurkishChar(entity.FullName) +
                                  "_odemePlani_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".docx";

                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();

                document.SaveAs(fileName);
                document.Close();
                GC.Collect();
            }

        }

        private List<EmailPaymentEntity> GetEmailPaymentList(List<PaymentTypeEntity> paymentTypeList, List<PaymentEntity> paymentList)
        {
            List<EmailPaymentEntity> emailPaymentList = new List<EmailPaymentEntity>();

            var monthList = GetMonthList();
            int year = DateTime.Today.Year;

            foreach (int month in monthList.Keys)
            {
                foreach (PaymentTypeEntity paymentType in paymentTypeList)
                {
                    EmailPaymentEntity entity = new EmailPaymentEntity();
                    entity.Year = year;
                    entity.Month = month;
                    entity.PaymentTypeId = paymentType.Id.Value;

                    if (paymentType.Id == (int) PaymentTypeEnum.Okul)
                        entity.TemplateName = "<" + GeneralFunctions.ReplaceTurkishChar(monthList[month]) + "Okul>";

                    if (paymentType.Id == (int)PaymentTypeEnum.Servis)
                        entity.TemplateName = "<" + GeneralFunctions.ReplaceTurkishChar(monthList[month]) + "Srv>";

                    if (paymentType.Id == (int)PaymentTypeEnum.Kirtasiye)
                        entity.TemplateName = "<" + GeneralFunctions.ReplaceTurkishChar(monthList[month]) + "Krt>";

                    if (paymentType.Id == (int)PaymentTypeEnum.Mental)
                        entity.TemplateName = "<" + GeneralFunctions.ReplaceTurkishChar(monthList[month]) + "Mnt>";

                    if (paymentType.Id == (int)PaymentTypeEnum.Diger)
                        entity.TemplateName = "<" + GeneralFunctions.ReplaceTurkishChar(monthList[month]) + "Diger>";


                    PaymentEntity paymentEntity = paymentList.FirstOrDefault(o =>
                        o.Year == year && o.Month == month && o.PaymentType == paymentType.Id);


                    if (paymentEntity == null)
                    {
                        if (month > DateTime.Today.Month)
                            entity.AmountDescription = " - ";
                        else
                        {
                            entity.AmountDescription = paymentType.AmountDesc ;
                            entity.Image = "~/img/icons/unPayment2.png";
                        }
                    }
                    else
                    {
                        if (paymentEntity.IsNotPayable)
                        {
                            entity.AmountDescription = " - ";
                        }
                        else
                        {
                            if (paymentEntity.IsPayment.HasValue)
                            {
                                if (paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue &&
                                    paymentEntity.Amount.Value > 0)
                                {
                                    entity.AmountDescription = "";
                                    entity.Image = "~/img/icons/greenSmile2.png";

                                }
                                else if (!paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue &&
                                         paymentEntity.Amount.Value > 0)
                                {
                                    entity.AmountDescription = paymentEntity.AmountDesc;
                                    entity.Image = "~/img/icons/unPayment2.png";
                                }

                                else
                                {
                                    entity.AmountDescription = paymentType.AmountDesc;
                                    entity.Image = "~/img/icons/unPayment2.png";
                                }
                            }
                            else
                            {
                                entity.AmountDescription = paymentType.AmountDesc;
                                entity.Image = "~/img/icons/unPayment2.png";
                            }
                        }
                    }
                    emailPaymentList.Add(entity);
                }
            }

            return emailPaymentList;

        }

        private void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            //execute find and replace
            doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }
    }
}