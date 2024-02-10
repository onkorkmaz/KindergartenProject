using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.Panel);
                master.SetVisibleSearchText(false);
                this.Title = this.Title + " - " + master.SetTitle(new BasePage()._ProjectType);
            }
            
            setScreenAuthority();
            setDefaultValues();
        }

        private void setScreenAuthority()
        {
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Analiz_Paneli_Odeme_Ozeti, rowPaymentSummaryPanel);
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Analiz_Paneli_Kayit_Sayisi, rowStudent);
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Analiz_Paneli_Kayit_Sayisi, rowInterview);
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Analiz_Paneli_Sinif_Dagilimi, rowClassSummary);
        }

        private void setDefaultValues()
        {
            fillStudentCount();
            fillClassList();
        }

        private void fillClassList()
        {
            DataResultArgs<List<ClassEntity>> resultSet = new ClassBusiness(new BasePage()._ProjectType).Get_ClassForStudent();
            if (!resultSet.HasError)
            {
                List<ClassEntity> list = resultSet.Result;

                string html = "<table class='table mb-0'>";

                foreach (ClassEntity entity in list)
                {
                    html += "<tr><td><a href='ogrenci-listesi/"+entity.Id+"'>" + entity.Name + "</a></td><td>:</td><td><b>"+entity.StudentCount+"</b></td></tr>";
                }
                html += "</table>";

                divTblClass.InnerHtml = html;
            }
        }

        private void fillStudentCount()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();

            resultSet = new StudentBusiness(new BasePage()._ProjectType).Get_Student();
            if (!resultSet.HasError)
            {
                List<StudentEntity> entityList = resultSet.Result;

                List<StudentEntity> currentList = entityList.Where(o => o.IsStudent == true && o.IsActive.Value).ToList();
                setLabel(currentList, lblStudent);

                currentList = entityList.Where(o => o.IsStudent == true && o.AddedOn > DateTime.Now.AddMonths(-1)).ToList();
                setLabel(currentList, lblMonthStudent);

                currentList = entityList.Where(o => o.IsStudent == false).ToList();
                setLabel(currentList, lblInterview);

                currentList = entityList.Where(o => o.IsStudent == false && o.AddedOn > DateTime.Now.AddMonths(-1)).ToList();
                setLabel(currentList, lblMonthInterview);

                currentList = entityList.Where(o => o.Birthday != null && o.Birthday.Value > DateTime.MinValue && new DateTime(DateTime.Today.Year, o.Birthday.Value.Month, o.Birthday.Value.Day) == DateTime.Today).ToList();


                setBirthdayInfo(currentList.OrderBy(o => o.BirthDayCurrentYear).ToList(), lblBirthdayToday);

                currentList = entityList.Where(o => o.Birthday != null && o.Birthday.Value > DateTime.MinValue && new DateTime(DateTime.Today.Year, o.Birthday.Value.Month, o.Birthday.Value.Day)
                < DateTime.Today.AddMonths(1) && new DateTime(DateTime.Today.Year, o.Birthday.Value.Month, o.Birthday.Value.Day) > DateTime.Today).ToList();

                setBirthdayInfo(currentList.OrderBy(o => o.BirthDayCurrentYear).ToList(), lblBirthdayThisMonth);
            }
        }

        private void setLabel(IEnumerable<StudentEntity> currentList, Label lbl)
        {
            if (currentList == null || currentList.ToList() == null || currentList.ToList().Count <= 0)
            {
                lbl.Text = "0";
            }
            else
            {
                lbl.Text = currentList.ToList().Count.ToString();
            }
        }

        private void setBirthdayInfo(List<StudentEntity> studentList, Label lbl)
        {

            StringBuilder sb = new StringBuilder();
            if (studentList == null || studentList.ToList() == null || studentList.ToList().Count <= 0)
            {
                lbl.Text = " - ";
            }
            else
            {
                sb.AppendLine("<table cellpadding='6'>");
                foreach (StudentEntity student in studentList)
                {
                    DateTime tempStudentBirthday = new DateTime(DateTime.Today.Year, student.Birthday.Value.Month, student.Birthday.Value.Day);
                    TimeSpan value = tempStudentBirthday.Subtract(DateTime.Today);

                    sb.AppendLine("<tr>");
                    sb.Append("<td>");

                    sb.Append("<a href = \"/ogrenci-guncelle/" + student.Id + "\">");
                    sb.Append(student.FullName + " "); sb.AppendLine("</td>");
                    sb.AppendLine("<td>");
                    sb.Append(CommonFunctions.ToDateWithCulture(student.Birthday));
                    sb.AppendLine("</a>");
                    sb.AppendLine("</td>");
                    _ = (value.TotalDays == 0) ? sb.Append("<td>(Bugün)</td>") : sb.Append("<td>(" + value.TotalDays + " gün kaldı)</td>");
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table>");
                lbl.Text = sb.ToString();
            }
        }
    }
}