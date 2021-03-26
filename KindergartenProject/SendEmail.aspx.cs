using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Common;
using Entity;
using System.Net.Mail;
using MailMessage = System.Net.Mail.MailMessage;
using System.Net.Mime;
using DocumentFormat.OpenXml.Packaging;

namespace KindergartenProject
{
    public partial class SendEmail : System.Web.UI.Page
    {
        private const string studentDoesNotFound = "Ödeme detayı için öğrenci seçmelisiniz !!!";

        protected void Page_Load(object sender, EventArgs e)
        {
            divInformation.InformationVisible = false;
            divInformation.ListRecordPage = "PaymentPlan.aspx";
            divInformation.SetVisibleLink(true, false);

            if (this.Master is kindergarten master)
            {
                master.SetActiveMenuAttiributes(MenuList.PaymentPlan);
                master.SetVisibleSearchText(false);
            }

            if (!Page.IsPostBack)
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
                        new PaymentTypeBusiness().Get_PaymentType(new SearchEntity()
                            {IsActive = true, IsDeleted = false});

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

                        foreach (int month in monthList.Keys)
                        {
                            var uniqueName = "_" + DateTime.Today.Year.ToString() + "_" + month;

                            var chcPaymentName = "chc" + uniqueName;

                            string isCheck = "";
                            if (DateTime.Today.Month == month)
                            {
                                isCheck = "checked='checked'";
                                hdnSelectedMonth.Value = "" + month + ",'" + monthList[month] + "'";
                            }

                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td><input type='checkbox' id='" + chcPaymentName + "' name='" +
                                          chcPaymentName +
                                          "' onclick =chcChange(); " + isCheck + " /></td>");
                            sb.AppendLine("<td>" + monthList[month] + "</td>");


                            foreach (PaymentTypeEntity payment in paymentTypeList)
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
                                            sb.AppendLine(
                                                "<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" +
                                                paymentEntity.AmountDesc +
                                                "</td><td><img width='" + width + "' height='" + height +
                                                "' src=\"img/icons/greenSmile2.png\"/></td></tr></table></td>");
                                        }
                                        else
                                        {
                                            sb.AppendLine(
                                                "<td><table cellpadding='4'><tr style='vertical-align: middle' ><td>" +
                                                paymentEntity.AmountDesc +
                                                "</td><td><img width='" + width + "' height='" + height +
                                                "' src=\"img/icons/unPayment2.png\"/></td></tr></table></td>");
                                        }
                                    }
                                    else
                                    {
                                        sb.AppendLine(
                                            "<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" +
                                            paymentEntity.AmountDesc +
                                            "</td><td><img width='" + width + "' height='" + height +
                                            "' src=\"img/icons/unPayment2.png\"/></td></tr></table></td>");
                                    }
                                }
                                else
                                {
                                    sb.AppendLine("<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" +
                                                  payment.AmountDesc +
                                                  "</td><td><img width='" + width + "' height='" + height +
                                                  "' src=\"img/icons/unPayment2.png\"/></td></tr></table></td>");
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
                string savePathToFiles = Server.MapPath("/PaymentDocument/" + GeneralFunctions.ReplaceTurkishChar(entity.FullName));

                if (string.IsNullOrEmpty(entity.Email) && !string.IsNullOrEmpty(txtEmail.Text))
                {
                    entity.Email = txtEmail.Text;
                    new StudentBusiness().Set_Student(entity);
                }

                string templatePath = Server.MapPath("/Template");

                if (!Directory.Exists(savePathToFiles))
                    Directory.CreateDirectory(savePathToFiles);
                else
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(savePathToFiles);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }

                DataResultArgs<List<PaymentTypeEntity>> resultSet =
                    new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false });

                List<EmailPaymentEntity> emailPaymentList = GetEmailPaymentList(resultSet.Result, entity.PaymentList);

                Dictionary<int,string> selectedMonthList = GetSelectedMonthList();


                byte[] byteArray = File.ReadAllBytes(templatePath + "/odemePlani2.docx");

       
                

                //foreach (int month in selectedMonthList.Keys)
                //{

                //    foreach (DocumentFormat.OpenXml.Drawing.Table t in body.Descendants<DocumentFormat.OpenXml.Drawing.Table>())
                //    {
                //        t.Append(new DocumentFormat.OpenXml.Drawing.TableRow(
                //            new DocumentFormat.OpenXml.Drawing.TableCell(new Paragraph(new Run(new Text("test"))))));
                //    }

                //    //foreach (EmailPaymentEntity emailPaymentEntity in emailPaymentList)
                //    //{
                //    //    if (emailPaymentEntity.Month == month)
                //    //    {
                //    //        switch ((PaymentTypeEnum)emailPaymentEntity.PaymentTypeId)
                //    //        {
                //    //            case PaymentTypeEnum.Okul:
                //    //                AddCell(row, emailPaymentEntity, 2);
                //    //                break;
                //    //            case PaymentTypeEnum.None:
                //    //                break;
                //    //            case PaymentTypeEnum.Servis:
                //    //                AddCell(row, emailPaymentEntity, 3);
                //    //                break;
                //    //            case PaymentTypeEnum.Kirtasiye:
                //    //                AddCell(row, emailPaymentEntity, 4);
                //    //                break;
                //    //            case PaymentTypeEnum.Mental:
                //    //                AddCell(row, emailPaymentEntity, 5);
                //    //                break;
                //    //            case PaymentTypeEnum.Diger:
                //    //                AddCell(row, emailPaymentEntity, 6);
                //    //                break;
                //    //            default:
                //    //                break;

                //    //        }
                //    //    }
                //    //}
                //}


                string fileName = savePathToFiles + "/" + GeneralFunctions.ReplaceTurkishChar(entity.FullName) +
                                  "_odemePlani_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".docx";


                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, (int)byteArray.Length);
                    using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
                    {

                        //Table table =
                        //    doc.MainDocumentPart.Document.Body.Elements<Table>().First();
                    }
                    // Save the file with the new name
                    File.WriteAllBytes(fileName, stream.ToArray());
                }
                GC.Collect();

                try
                {
                    //sendMail(fileName);
                    divInformation.SuccessfulText = "Mail gönderim işlemi başarıyla tamamlanmıştır";
                    divInformation.SetVisibleLink(true, false);
                    pnlBody.Enabled = false;
                }
                catch (Exception ex)
                {
                    divInformation.ErrorText = ex.Message;
                }
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
            }
        }

        private void sendMail(string fileName)
        {

            MailMessage message = new MailMessage("korkmazonur44@gmail.com",txtEmail.Text);

            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(fileName, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(fileName);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(fileName);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(fileName);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);

            //Send the message.
            //SmtpClient client = new SmtpClient(server);
            // Add credentials if the SMTP server requires them.
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;


        }
        //private static void AddCell(Row row, EmailPaymentEntity emailPaymentEntity, int index)
        //{
        //    row.Cells[index].Range.Text = emailPaymentEntity.AmountDescription;
        //    row.Cells[index].Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
        //    row.Cells[index].Height = 20;
        //    if (emailPaymentEntity.IsPayment)
        //    {
        //        row.Cells[index].Shading.BackgroundPatternColor = WdColor.wdColorLightGreen;
        //    }
        //    else if (emailPaymentEntity.AmountDescription.Trim() != CommonConst.EmptyAmount.Trim())
        //    {
        //        row.Cells[index].Shading.BackgroundPatternColor = WdColor.wdColorLightYellow;
        //    }
        //    else
        //    {
        //        row.Cells[index].Shading.BackgroundPatternColor = WdColor.wdColorWhite;
        //    }
        //}

        private Dictionary<int, string> GetSelectedMonthList()
        {
            string[] selectedMonthList = hdnSelectedMonth.Value.Split('_');
            Dictionary<int, string> list = new Dictionary<int, string>();

            foreach (string monthIdAndName in selectedMonthList)
            {
                string[] temp = monthIdAndName.Split(',');
                if (GeneralFunctions.GetData<int>(temp[0]) > 0)
                    list.Add(GeneralFunctions.GetData<int>(temp[0]), temp[1].Replace("'", ""));
            }

            return list;

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

                    PaymentEntity paymentEntity = paymentList.FirstOrDefault(o =>
                        o.Year == year && o.Month == month && o.PaymentType == paymentType.Id);


                    if (paymentEntity == null)
                    {
                        if (month > DateTime.Today.Month)
                            entity.AmountDescription = " - ";
                        else
                        {
                            entity.AmountDescription = paymentType.AmountDesc ;
                        }
                    }
                    else if (paymentEntity.Month > DateTime.Today.Month)
                    {
                        entity.AmountDescription = CommonConst.EmptyAmount;
                    }
                    else
                    {
                        if (paymentEntity.IsNotPayable)
                        {
                            entity.AmountDescription = CommonConst.EmptyAmount;
                        }
                        else
                        {
                            if (paymentEntity.IsPayment.HasValue)
                            {
                                if (paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue &&
                                    paymentEntity.Amount.Value > 0)
                                {
                                    entity.AmountDescription = "Ödendi";
                                    entity.IsPayment = true;
                                }
                                else if (!paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue &&
                                         paymentEntity.Amount.Value > 0)
                                {
                                    entity.AmountDescription = paymentEntity.AmountDesc;
                                }

                                else
                                {
                                    entity.AmountDescription = paymentType.AmountDesc;
                                }
                            }
                            else
                            {
                                entity.AmountDescription = paymentType.AmountDesc;
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