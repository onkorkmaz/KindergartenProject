using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Entity;
using System.Text;
using System.Globalization;

namespace KindergartenProject
{
    public partial class StudentList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.StudentList);
                lblAllStudent.Attributes.Add("onclick", "allStudent();");
                lblActiveStudent.Attributes.Add("onclick", "activeStudent();");
                lblInterview.Attributes.Add("onclick", "interviewStudent();");
                lblPassiveStudent.Attributes.Add("onclick", "passiveStudent();");
            }

            divInformation.ListRecordPage = "/ogrenci-listesi";
            divInformation.NewRecordPage = "/ogrenci-ekle";

            setDefaultValues();
            loadClass();

            //loadData();

        }

        private void loadClass()
        {
            DataResultArgs<List<ClassEntity>> resultSet = new ClassBusiness().Get_ClassForStudent();

            if (resultSet.HasError)
            {
                divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                List<ClassEntity> classList = resultSet.Result;
                List<ClassEntity> list = new List<ClassEntity>();

                list.Add(new ClassEntity() { Id = -1 });
                if (resultSet.Result != null)
                {

                    foreach (ClassEntity entity in classList)
                    {
                        list.Add(entity);
                    }
                }

                drpClassList.DataSource = list;
                drpClassList.DataValueField = "Id";
                drpClassList.DataTextField = "ClassAndMainTeacherName";
                drpClassList.DataBind();
            }
        }

        private void loadData()
        {
            DateTime first = DateTime.Now;

            DataResultArgs<List<StudentEntity>> resultSet = new StudentBusiness().Get_AllStudentWithCache();

            DateTime second = DateTime.Now;

            TimeSpan ts = second.Subtract(first);

            lblTimer.Text = ts.TotalSeconds.ToString();

            string tBody = getBody(resultSet.Result);

            DateTime third = DateTime.Now;

            ts = third.Subtract(second);

            lblTimer.Text += "___" + ts.TotalSeconds.ToString();

        }

        private string getBody(List<StudentEntity> entityList)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < entityList.Count(); i++)
            {

                sb.AppendLine( "<tr>");
                sb.AppendLine( "<td style='cursor: pointer;' onclick =onDetailRow(\"" + entityList[i].EncryptId +
                         "\") >+</td>");
                sb.AppendLine( "<td>");
                sb.AppendLine("<a href = \"/ogrenci-guncelle/" + entityList[i].EncryptId +
                         "\"><img title='Güncelle' src =\"/img/icons/update10.png\"/></a> ");
                //sb.AppendLine( "<a href = \"#\"><img src =\"img/icons/trush1.png\" title ='Sil' onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";

                if (entityList[i].IsStudent == true)
                {
                    sb.AppendLine("<a href = \"/odeme-plani-detay/" + entityList[i].EncryptId +
                             "\"><img title = 'Ödeme detayı' src =\"/img/icons/paymentPlan.png\"/></a> ");
                    sb.AppendLine("<a href = \"/SendEmail.aspx?Id=" + entityList[i].EncryptId +
                             "\"><img title = 'Email gönder' src =\"/img/icons/email4.png\"/></a>");
                }

                sb.AppendLine( "</td>");
                sb.AppendLine( "<td>" + entityList[i].FullName + "</td>");
                sb.AppendLine( "<td>" + entityList[i].BirthdayWithFormat + "</td>");
                sb.AppendLine( "<td>" + entityList[i].FatherInfo + "</td>");
                sb.AppendLine( "<td>" + entityList[i].MotherInfo + "</td>");

                if (entityList[i].IsStudent)
                    sb.AppendLine( "<td>&nbsp;<img src='img/icons/student3.png' width='20' height ='20' /></td>");
                else
                    sb.AppendLine(
                        "<td>&nbsp;<a href = \"#\"><img title='Öğrenciye Çevir' src='img/icons/interview.png' width='23' height ='23' onclick='convertStudent(\"" +
                        entityList[i].EncryptId + "\")' /></a></td>");

                sb.AppendLine( "</tr> ");

                sb.AppendLine( "<tr style='display: none;' id='tr" + entityList[i].EncryptId + "' >");
                {
                    sb.AppendLine( "<td colspan=2></td >");
                    sb.AppendLine( "<td colspan=6>");
                    {
                        sb.AppendLine( "<table border='1' width='100%' cellpadding='8'>");
                        {
                            sb.AppendLine(
                                "<tr><td width='150'><b>TCKN</b></td><td width='20'>:</td><td style='text-align: left'>" +
                                entityList[i].CitizenshipNumberStr + "</td></tr>");

                            sb.AppendLine(
                                "<tr><td width='150'><b>Konuşulan ücret</b></td><td width='20'>:</td><td style='text-align: left'>" +
                                entityList[i].SpokenPriceStr + "</td></tr>");
                            sb.AppendLine( "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" +
                                     entityList[i].DateOfMeetingWithFormat + "</td></tr>");
                            sb.AppendLine( "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" +
                                     entityList[i].EmailStr + "</td></tr>");
                            sb.AppendLine( "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" +
                                     entityList[i].NotesStr + "</td></tr>");

                            var isActive = entityList[i].IsActive;
                            if (isActive != null && isActive.Value)
                                sb.AppendLine(
                                    "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/active.png' width='20' height ='20' /></td></tr>");
                            else
                                sb.AppendLine(
                                    "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/passive.png' width='20' height ='20' /></td></tr>");

                            sb.AppendLine(
                                "<tr><td><a href = \"#\"><img src =\"/img/icons/trush1.png\" title ='Sil' onclick='deleteCurrentRecord(\"" +
                                entityList[i].EncryptId + "\")' /></a></td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                        }
                        sb.AppendLine( "</table>");
                    }
                    sb.AppendLine( "</td > ");
                }
                sb.AppendLine("</tr>");

        }

            return sb.ToString();
        }



        private void setDefaultValues()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();

            resultSet = new StudentBusiness().Get_Student(new SearchEntity() { IsDeleted = false });
            if (!resultSet.HasError)
            {
                List<StudentEntity> entityList = resultSet.Result;

                IEnumerable<StudentEntity> currentList = entityList.Where(o => o.IsStudent == true && o.IsActive == true);
                setLabel(currentList, lblActiveStudent, "Aktif");

                currentList = entityList.Where(o => o.IsStudent == true && o.IsActive == false);
                setLabel(currentList, lblPassiveStudent, "Pasif");

                currentList = entityList.Where(o => o.IsStudent == false);
                setLabel(currentList, lblInterview, "Görüşme");

                setLabel(entityList, lblAllStudent, "Toplam");

            }
        }
        private void setLabel(IEnumerable<StudentEntity> currentList, Label lbl, string text)
        {
            string quantity = "";
            if (currentList != null && currentList.ToList() != null && currentList.ToList().Count > 0)
            {
                quantity = currentList.ToList().Count.ToString(); ;
            }

            lbl.Text = text + " Öğrenci Sayısı : " + quantity;
        }
    }
}