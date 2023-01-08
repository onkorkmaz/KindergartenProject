using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace KindergartenProject
{
    public partial class SendEmail : System.Web.UI.Page
    {
        ProjectType projectType = ProjectType.None;

        private const string studentDoesNotFound = "Ödeme detayı için öğrenci seçmelisiniz !!!";

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            divInformation.InformationVisible = false;
            divInformation.ListRecordPage = "/odeme-plani";
            divInformation.SetVisibleLink(true, false);

            lblSeason.Text = DateTime.Today.Year + " - " + (DateTime.Today.Year + 1);

            if (this.Master is kindergarten master)
            {
                master.SetActiveMenuAttiributes(MenuList.PaymentPlan);
                master.SetVisibleSearchText(false);
            }

            ProjectType projectType = (ProjectType)Session[CommonConst.ProjectType];

            if (!Page.IsPostBack)
            {
                btnSendEmailCopy.Attributes.Add("Style", "display:none;");

                object Id = Page.RouteData.Values["student_id"];

                if (Id == null)
                {
                    divInformation.ErrorText = studentDoesNotFound;
                    divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                    divInformation.ErrorLink = "~/odeme-plani";
                    return;
                }

                string IdDecrypt = Cipher.Decrypt(Id.ToString());
                int id = GeneralFunctions.GetData<int>(IdDecrypt);
                if (id <= 0)
                {
                    divInformation.ErrorText = studentDoesNotFound;
                    divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                    divInformation.ErrorLink = "~/odeme-plani";
                }
                else
                {
                    int width = 15;
                    int height = 15;
                    StudentEntity entity = new StudentBusiness(projectType).Get_StudentWithPaymentList(id);

                    lblStudentInto.Text = "<a href = \"/ogrenci-guncelle/" + entity.EncryptId + "\">" +
                        "<div id='btnUniqueNameSurnam' class='btn btn-primary' >" + entity.FullName.ToUpper() + "</div></a> &nbsp;&nbsp;&nbsp;";
                    lblStudentInto.Text += "<a href= '/odeme-plani-detay/" + entity.EncryptId +
                                           "'><div id='btnPaymentDetail' class='btn btn-warning'>Ödeme Detayı</div></a>";


                    hdnStudentName.Value = entity.FullName;

                    txtEmail.Text = entity.Email;

                    DataResultArgs<List<PaymentTypeEntity>> resultSet =
                        new PaymentTypeBusiness(projectType).Get_PaymentType(new SearchEntity()
                        { IsActive = true, IsDeleted = false });

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
                            "' onclick =chcAllChange(); /></th><th scope='col'>Yıl</th> <th scope='col'>Ay</th>");

                        List<PaymentTypeEntity> paymentTypeList = resultSet.Result;

                        foreach (PaymentTypeEntity payment in paymentTypeList)
                        {
                            sb.AppendLine("<th scope='col'>" + payment.Name + "</th>");
                        }

                        var seasonList = CommonUIFunction.GetSeasonList(DateTime.Today.Year);

                        foreach (SeasonEntity obje in seasonList)
                        {
                            var uniqueName = "_" + obje.Year + "_" + obje.Month;

                            var chcPaymentName = "chc" + uniqueName;

                            string isCheck = "";
                            if (DateTime.Today.Month == obje.Month)
                            {
                                isCheck = "checked='checked'";
                                hdnSelectedMonth.Value = "" + obje.Month + ",'" + obje.MonthName + "'";
                            }

                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td><input type='checkbox' id='" + chcPaymentName + "' name='" +
                                          chcPaymentName +
                                          "' onclick =chcChange(); " + isCheck + " /></td>");
                            sb.AppendLine("<td>" + obje.Year + "</td>");
                            sb.AppendLine("<td>" + obje.MonthName + "</td>");


                            foreach (PaymentTypeEntity payment in paymentTypeList)
                            {
                                PaymentEntity paymentEntity = entity.StudentDetail.PaymentList.FirstOrDefault(o =>
                                    o.Month == obje.Month && o.Year == obje.Year && o.PaymentType == payment.Id);

                                if (paymentEntity != null)
                                {
                                    if (paymentEntity.IsPayment.HasValue)
                                    {
                                        if (paymentEntity.IsPayment.Value)
                                        {
                                            if (paymentEntity.Amount > 0)
                                            {
                                                sb.AppendLine(
                                                "<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" +
                                                paymentEntity.AmountDesc +
                                                "</td><td><img width='" + width + "' height='" + height +
                                                "' src=\"/img/icons/greenSmile2.png\"/></td></tr></table></td>");
                                            }
                                            else
                                            {
                                                sb.AppendLine("<td>&nbsp;&nbsp;&nbsp;&nbsp;-</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (paymentEntity.Amount > 0)
                                            {
                                                sb.AppendLine(
                                                    "<td><table cellpadding='4'><tr style='vertical-align: middle' ><td>" +
                                                    paymentEntity.AmountDesc +
                                                    "</td><td><img width='" + width + "' height='" + height +
                                                    "' src=\"/img/icons/unPayment2.png\"/></td></tr></table></td>");
                                            }
                                            else
                                            {
                                                sb.AppendLine("<td>&nbsp;&nbsp;&nbsp;&nbsp;-</td>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (paymentEntity.Amount > 0)
                                        {
                                            sb.AppendLine(
                                            "<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" +
                                            paymentEntity.AmountDesc +
                                            "</td><td><img width='" + width + "' height='" + height +
                                            "' src=\"/img/icons/unPayment2.png\"/></td></tr></table></td>");
                                        }
                                        else
                                        {
                                            sb.AppendLine("<td>&nbsp;&nbsp;&nbsp;&nbsp;-</td>");
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(payment.AmountDesc))
                                    {
                                        sb.AppendLine("<td><table cellpadding='4'><tr style='vertical-align: middle'><td>" +
                                                      payment.AmountDesc +
                                                      "</td><td><img width='" + width + "' height='" + height +
                                                      "' src=\"/img/icons/unPayment2.png\"/></td></tr></table></td>");
                                    }
                                    else
                                    {
                                        sb.AppendLine("<td>&nbsp;&nbsp;&nbsp;&nbsp;-</td>");
                                    }
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

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                btnSendEmail.Enabled = false;
                object Id = Page.RouteData.Values["student_id"];

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
                    StudentEntity entity = new StudentBusiness(projectType).Get_StudentWithPaymentList(id);

                    DataResultArgs<List<PaymentTypeEntity>> resultSet =
                        new PaymentTypeBusiness(projectType).Get_PaymentType(new SearchEntity() { IsActive = true, IsDeleted = false });

                    StringBuilder sb = InitializeHtmlTable(entity, resultSet.Result);

                    //string body = drawTable(entity, resultSet.Result);

                    try
                    {
                        sendMail(sb, entity);
                        divInformation.SuccessfulText = "Mail gönderim işlemi başarıyla tamamlanmıştır";
                        divInformation.SetVisibleLink(true, false);
                        pnlBody.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        divInformation.ErrorText =
                            ex.Message + " - " + ex.InnerException + " - " + ex.StackTrace;
                        btnSendEmail.Enabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                divInformation.ErrorText =
                    exception.Message + " - " + exception.InnerException + " - " + exception.StackTrace;
                btnSendEmail.Enabled = true;
            }
        }

        //private string drawTable(StudentEntity entity, List<PaymentTypeEntity> paymentTypeEntityList)
        //{
        //    List<EmailPaymentEntity> emailPaymentList = GetEmailPaymentList(paymentTypeEntityList, entity.PaymentList);

        //    Dictionary<int, string> selectedMonthList = GetSelectedMonthList();


        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine("<table>");

        //    foreach (int month in selectedMonthList.Keys)
        //    {
        //        sb.AppendLine("<tr>");
        //        sb.AppendLine("<td>" + selectedMonthList[month] + "</td>");

        //        foreach (EmailPaymentEntity emailPaymentEntity in emailPaymentList)
        //        {
        //            if (emailPaymentEntity.Month == month)
        //            {
        //                switch ((PaymentTypeEnum)emailPaymentEntity.PaymentTypeId)
        //                {
        //                    case PaymentTypeEnum.Okul:
        //                    case PaymentTypeEnum.Servis:
        //                    case PaymentTypeEnum.Kirtasiye:
        //                    case PaymentTypeEnum.Mental:
        //                    case PaymentTypeEnum.Diger:
        //                        sb.AppendLine("<td>" + emailPaymentEntity.AmountDescription + "</td>");
        //                        break;
        //                    case PaymentTypeEnum.None:
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }

        //        sb.AppendLine("</tr>");
        //    }

        //}

        private void ThrowError()
        {
            divInformation.ErrorText = studentDoesNotFound;
            divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
            divInformation.ErrorLink = "~/odeme-plani";
            return;
        }

        private StringBuilder InitializeHtmlTable(StudentEntity entity, List<PaymentTypeEntity> paymentTypeEntityList)
        {
            List<EmailPaymentEntity> emailPaymentList = GetEmailPaymentList(paymentTypeEntityList, entity.StudentDetail.PaymentList);
            Dictionary<int, string> selectedMonthList = GetSelectedMonthList();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"
        

<table width='800' align='center' style ='border: 1px solid black' cellpadding='20'>       
            <tr>
                <td>
                    <table width='100%' cellpadding='2' cellspacing='8' >
                    <tr>
                            <td>
                               <table width='100%' cellpadding='2' cellspacing='8'> 
                                   <tr>
                                        <td width='100'>ADI SOYADI</td>
                                        <td width='10'>:</td>
                                        <td>" + entity.FullName + @"</td>
                                    </tr>
                                    <tr>
                                        <td><u>Sayın Velimiz,</u></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan='3' align='center'>
                                            <hr/>
                                    <div style='color:blue;' >" + DateTime.Today.Year.ToString() + @"-" + (DateTime.Today.Year + 1).ToString() + @" Eğitim Öğretim Yılı Okul Ücreti Ödeme Tablonuz
                                    </div>
                                            <hr/>
                                        </td>
                                    </tr>
                               </table>
                            </td>
                            <td align='right'><img src='http://benimdunyamogrencitakip.com/img/icons/benim_dunyam2.png' width='120' /></td>
                    <tr>
                       
                    </table>
                </td>
            </tr>
			<tr>
				<td>
					<table width='100%' cellpadding='2' cellspacing='2'>
					<tr style='color:white; background-color:#336666;'><td style ='border:1px solid #000000;' width='50'>&nbsp;</td>");

            foreach (PaymentTypeEntity _paymentEntity in paymentTypeEntityList)
            {
                sb.AppendLine("<td style ='border:1px solid #000000;' width='50'>" + _paymentEntity.Name + "</td>");
            }

            sb.AppendLine(@"</tr>
                     <tr>
						<td colspan='6'>&nbsp;</td>
					</tr>");

            foreach (int month in selectedMonthList.Keys)
            {
                EmailPaymentEntity paymentSchool = null;
                EmailPaymentEntity paymentService = null;
                EmailPaymentEntity paymentBooking = null;
                EmailPaymentEntity paymentMental = null;
                EmailPaymentEntity paymentAnother = null;

                foreach (EmailPaymentEntity emailPaymentEntity in emailPaymentList)
                {
                    if (emailPaymentEntity.Month == month)
                    {
                        switch ((PaymentTypeEnum)emailPaymentEntity.PaymentTypeId)
                        {
                            case PaymentTypeEnum.Okul:
                                paymentSchool = emailPaymentEntity;
                                break;
                            case PaymentTypeEnum.None:
                                break;
                            case PaymentTypeEnum.Servis:
                                paymentService = emailPaymentEntity;
                                break;
                            case PaymentTypeEnum.Kirtasiye:
                                paymentBooking = emailPaymentEntity;
                                break;
                            case PaymentTypeEnum.Mental:
                                paymentMental = emailPaymentEntity;
                                break;
                            case PaymentTypeEnum.Diger:
                                paymentAnother = emailPaymentEntity;
                                break;
                            default:
                                break;
                        }
                    }
                }


                sb.AppendLine(@"<tr style='color:black; background-color:#FFFF99;'>
						<td style ='border:1px solid #000000;' width='50' style='color:#336666;'><b>" + selectedMonthList[month] + @"</b></td>");

                AddPayment(sb, paymentSchool, month, PaymentTypeEnum.Okul, paymentTypeEntityList);
                AddPayment(sb, paymentService, month, PaymentTypeEnum.Servis, paymentTypeEntityList);
                AddPayment(sb, paymentBooking, month, PaymentTypeEnum.Kirtasiye, paymentTypeEntityList);
                AddPayment(sb, paymentMental, month, PaymentTypeEnum.Mental, paymentTypeEntityList);
                AddPayment(sb, paymentAnother, month, PaymentTypeEnum.Diger, paymentTypeEntityList);
            }


            sb.AppendLine(@"</table>
				</td>
			</tr>
            <tr><td><table width='100%'><tr><td align='right'>Benim Dünyam Montessori Okulları</td><td width='50'></td></tr></table></td></tr>
        </table>");

            return sb;



        }

        private static void AddPayment(StringBuilder sb, EmailPaymentEntity emailPaymentEntity, int month, PaymentTypeEnum paymentType, List<PaymentTypeEntity> paymentTypeList)
        {
            int year = DateTime.Today.Year;
            if (string.IsNullOrEmpty(emailPaymentEntity.AmountColor))
                emailPaymentEntity.AmountColor = "black";

            if (emailPaymentEntity != null)
            {
                sb.AppendLine("<td style ='border:1px solid #000000; color:" + emailPaymentEntity.AmountColor + ";' width='50'><b>" + emailPaymentEntity.AmountDescription + "</b></td>");
            }
            else
            {
                if (year <= DateTime.Today.Year && month <= DateTime.Today.Month)
                {
                    PaymentTypeEntity payType = paymentTypeList.FirstOrDefault(o => o.Id == (int)paymentType);
                    if(payType!=null && payType.Amount>0)
                    {
                        sb.AppendLine("<td style ='border:1px solid #000000; color:red;'><b>" + payType.AmountDesc + "</b></td>");
                    }
                    else
                    {
                        sb.AppendLine("<td style ='border:1px solid #000000;' >&nbsp;</td>");
                    }
                }
                else
                {
                    sb.AppendLine("<td style ='border:1px solid #000000;' >&nbsp;</td>");
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

        private void sendMail(StringBuilder sb,StudentEntity entity)
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
                mail.Subject = "Benim Dünyam Montessori Okulları Ödeme Tablosu " + monthName;
                mail.Body = sb.ToString();

                mail.IsBodyHtml = true;

                using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587))
                {
                    SmtpServer.UseDefaultCredentials = false; //Need to overwrite this
                    SmtpServer.Credentials = new System.Net.NetworkCredential("benimdunyamanaokullari@gmail.com", "enrjpjxobnefsqac");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
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

            var monthList = CommonUIFunction.GetSeasonList(DateTime.Today.Year);

            foreach (SeasonEntity seasonEntity in monthList)
            {
                foreach (PaymentTypeEntity paymentType in paymentTypeList)
                {
                    EmailPaymentEntity entity = new EmailPaymentEntity();
                    entity.Year = seasonEntity.Year;
                    entity.Month = seasonEntity.Month;
                    entity.PaymentTypeId = paymentType.Id;
                    entity.AmountDescription = CommonConst.EmptyAmount; ;                    

                    PaymentEntity paymentEntity = paymentList.FirstOrDefault(o =>
                        o.Year == seasonEntity.Year && o.Month == seasonEntity.Month && o.PaymentType == paymentType.Id);


                    if (paymentEntity == null)
                    {
                        if (seasonEntity.Year > DateTime.Today.Year || (seasonEntity.Month >= DateTime.Today.Month && seasonEntity.Year >= DateTime.Today.Year))
                        {
                            entity.AmountDescription = CommonConst.EmptyAmount;
                            entity.Amount = 0;
                        }
                        else
                        {
                            entity.AmountDescription = paymentType.AmountDesc;
                            entity.AmountColor = "red";
                            entity.Amount = (paymentType.Amount.HasValue) ? paymentType.Amount.Value : 0;
                        }
                    }
                    else if (paymentEntity.Month > DateTime.Today.Month && paymentEntity.Year >= DateTime.Today.Year)
                    {
                        if (paymentEntity.IsPayment.HasValue && paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue && paymentEntity.Amount.Value > 0)
                        {
                            entity.AmountDescription = paymentEntity.AmountDesc;
                            entity.IsPayment = true;
                            entity.AmountColor = "green";
                            entity.Amount = paymentEntity.Amount.Value;
                        }
                        //else if (paymentEntity.IsPayment.HasValue && !paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue && paymentEntity.Amount.Value > 0)
                        //{
                        //    entity.AmountDescription = paymentEntity.AmountDesc;
                        //    entity.IsPayment = false;
                        //    entity.AmountColor = "red";
                        //    entity.Amount = paymentEntity.Amount.Value;
                        //}
                        else
                        {
                            entity.AmountDescription = CommonConst.EmptyAmount;
                            entity.Amount = 0;
                        }
                    }
                    else
                    {
                        if (paymentEntity.IsPayment.HasValue)
                        {
                            if (paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue && paymentEntity.Amount.Value > 0)
                            {
                                entity.AmountDescription = paymentEntity.AmountDesc;
                                entity.IsPayment = true;
                                entity.AmountColor = "green";
                                entity.Amount = paymentEntity.Amount.Value;
                            }
                            else if (!paymentEntity.IsPayment.Value && paymentEntity.Amount.HasValue && paymentEntity.Amount.Value > 0)
                            {
                                entity.AmountDescription = paymentEntity.AmountDesc;
                                entity.AmountColor = "red";
                                entity.Amount = paymentEntity.Amount.Value;
                            }
                            else
                            {
                                entity.AmountDescription = paymentType.AmountDesc;
                                entity.AmountColor = "red";
                                entity.Amount = (paymentType.Amount.HasValue) ? paymentType.Amount.Value : 0;
                            }
                        }
                        else
                        {
                            entity.AmountDescription = paymentType.AmountDesc;
                            entity.AmountColor = "red";
                            entity.Amount = (paymentType.Amount.HasValue) ? paymentType.Amount.Value : 0;
                        }
                    }

                    if (entity.AmountColor == "red" && entity.Amount > 0)
                    {
                        entity.AmountDescription += "TL (Ödenmedi)";
                    }
                    else if (entity.AmountColor == "green" && entity.Amount > 0)
                    {
                        entity.AmountDescription += "TL (Ödendi)";
                    }

                    if (string.IsNullOrEmpty(entity.AmountDescription))
                    {
                        entity.AmountColor = "";
                        entity.AmountDescription = CommonConst.EmptyAmount;
                    }

                    emailPaymentList.Add(entity);
                }
            }

            return emailPaymentList;

        }

    }
}