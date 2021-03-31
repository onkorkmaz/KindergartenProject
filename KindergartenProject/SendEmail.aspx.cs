using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WordToPDF;

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


                    hdnStudentName.Value = entity.FullName;

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
                ThrowError();
                return;
            }

            string IdDecrypt = Cipher.Decrypt(Id.ToString());
            int id = GeneralFunctions.GetData<int>(IdDecrypt);
            if (id <= 0)
            {
                ThrowError();
                return;
            }
            else
            {
                StudentEntity entity = new StudentBusiness().Get_StudentWithPaymentList(id);
                string savePathToFiles = Server.MapPath("/PaymentDocument/" + GeneralFunctions.ReplaceTurkishChar(entity.FullName));

                UpdateStudentEmailAndDeleteFolder(entity, savePathToFiles);


                string fileName = savePathToFiles + "/" + GeneralFunctions.ReplaceTurkishChar(entity.FullName) +
                                  "_odemePlani_" + DateTime.Now.ToString("yyyyMMddhhmmss");


                InitializeDocumentAndSave(entity, fileName);

                try
                {
                    sendMail(fileName);
                    divInformation.SuccessfulText = "Mail gönderim işlemi başarıyla tamamlanmıştır";
                    divInformation.SetVisibleLink(true, false);
                    pnlBody.Enabled = false;
                }
                catch (Exception ex)
                {
                    divInformation.ErrorText = ex.Message;
                }
            }
        }

        private void ThrowError()
        {
            divInformation.ErrorText = studentDoesNotFound;
            divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
            divInformation.ErrorLink = "PaymentPlan.aspx";
            return;
        }

        private void UpdateStudentEmailAndDeleteFolder(StudentEntity entity, string savePathToFiles)
        {
            if (string.IsNullOrEmpty(entity.Email) && !string.IsNullOrEmpty(txtEmail.Text))
            {
                entity.Email = txtEmail.Text;
                new StudentBusiness().Set_Student(entity);
            }

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
        }

        private void InitializeDocumentAndSave( StudentEntity entity,  string fileName)
        {
            DataResultArgs<List<PaymentTypeEntity>> resultSet =
                new PaymentTypeBusiness().Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false });

            List<EmailPaymentEntity> emailPaymentList = GetEmailPaymentList(resultSet.Result, entity.PaymentList);

            Dictionary<int, string> selectedMonthList = GetSelectedMonthList();

            string templatePath = Server.MapPath("/Template");

            byte[] byteArray = File.ReadAllBytes(templatePath + "/odemePlani2.docx");

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, (int) byteArray.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
                {
                    setFullName(doc, entity.FullName);

                    DocumentFormat.OpenXml.Wordprocessing.Table table = doc.MainDocumentPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>().First();

                    AddWordCell(selectedMonthList, emailPaymentList, table);
                }

                // Save the file with the new name
                File.WriteAllBytes(fileName+".docx" , stream.ToArray());
                FileStream fs = new FileStream(fileName + ".docx", FileMode.OpenOrCreate, FileAccess.Read);
                fs.Flush();
                fs.Close();
                GC.Collect();

            }
        }

        //public void ConvertTopdf(string path, string filename)
        //{
        //    SautinSoft.UseOffice u = new SautinSoft.UseOffice();
        //    if (u.InitWord() == 0)
        //    {
        //        //convert Word (RTF, DOC, DOCX to PDF)
        //        u.ConvertFile(path, @"I:\karthik\pdf\pdf\" + filename, SautinSoft.UseOffice.eDirection.DOC_to_PDF);
        //    }
        //    u.CloseOffice();
        //}

        private void AddWordCell(Dictionary<int, string> selectedMonthList, List<EmailPaymentEntity> emailPaymentList, Table table)
        {
            foreach (int month in selectedMonthList.Keys)
            {
                DocumentFormat.OpenXml.Wordprocessing.TableRow tr =
                    new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                AddCell(tr, selectedMonthList[month]);

                foreach (EmailPaymentEntity emailPaymentEntity in emailPaymentList)
                {
                    if (emailPaymentEntity.Month == month)
                    {
                        switch ((PaymentTypeEnum) emailPaymentEntity.PaymentTypeId)
                        {
                            case PaymentTypeEnum.Okul:
                                AddCell(tr, emailPaymentEntity.AmountDescription);
                                break;
                            case PaymentTypeEnum.None:
                                break;
                            case PaymentTypeEnum.Servis:
                                AddCell(tr, emailPaymentEntity.AmountDescription);
                                break;
                            case PaymentTypeEnum.Kirtasiye:
                                AddCell(tr, emailPaymentEntity.AmountDescription);
                                break;
                            case PaymentTypeEnum.Mental:
                                AddCell(tr, emailPaymentEntity.AmountDescription);
                                break;
                            case PaymentTypeEnum.Diger:
                                AddCell(tr, emailPaymentEntity.AmountDescription);
                                break;
                            default:
                                break;
                        }
                    }
                }

                table.Append(tr);
            }
        }

        private void setFullName(WordprocessingDocument doc, string fullName)
        {
            var body = doc.MainDocumentPart.Document.Body;
            var paras = body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();

            foreach (var para in paras)
            {
                foreach (var run in para.Elements<DocumentFormat.OpenXml.Wordprocessing.Run>())
                {
                    foreach (var text in run.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>())
                    {
                        if (text.Text.Contains("fullName"))
                        {
                            text.Text = text.Text.Replace("fullName", fullName);
                        }
                    }
                }
            }
        }

        private void AddCell(DocumentFormat.OpenXml.Wordprocessing.TableRow tr, string text)
        {
            DocumentFormat.OpenXml.Wordprocessing.TableCell tc = new DocumentFormat.OpenXml.Wordprocessing.TableCell(
                new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text(text))));
            tr.Append(tc);
        }

        private void sendMail(string fileName)
        {
            Dictionary<int, string> selectedMonthList = GetSelectedMonthList();

            string monthName = "";

            foreach (int key in selectedMonthList.Keys)
            {
                if (string.IsNullOrEmpty(monthName))
                    monthName = selectedMonthList[key];
                else
                {
                    monthName += " - " + selectedMonthList[key];
                }
            }


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("benimdunyamanaokullari@gmail.com");
                mail.To.Add(txtEmail.Text.Trim());
                mail.Subject = "Benim Dünyam Anaokulu Ödeme Tablosu " + monthName;
                mail.Body =
                    "Sayın velimiz; <br/> Öğrencimiz " + hdnStudentName.Value +
                    " ait güncel ödeme tablosu ektedir.";


                mail.IsBodyHtml = true;

                WordToPDF.Word2Pdf pdf = new Word2Pdf();
                pdf.InputLocation = fileName + ".docx";
                pdf.OutputLocation = fileName + ".pdf";
                pdf.Word2PdfCOnversion();

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(fileName + ".pdf");
                mail.Attachments.Add(attachment);

                using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587))
                {
                    SmtpServer.UseDefaultCredentials = false; //Need to overwrite this
                    SmtpServer.Credentials = new System.Net.NetworkCredential("benimdunyamanaokullari@gmail.com", "Yelay123");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
            }
        }


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
                    entity.PaymentTypeId = paymentType.Id;

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

    }
}