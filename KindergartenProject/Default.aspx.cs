using Common;
using System;
using System.Web.UI;
using Entity;
using Business;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;

namespace KindergartenProject
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.Panel);
                master.SetVisibleSearchText(false);
            }

            setDefaultValues();
        }

        private void setDefaultValues()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();

            resultSet = new StudentBusiness().Get_Student(new SearchEntity() { IsDeleted = false });
            if (!resultSet.HasError)
            {
                List<StudentEntity> entityList = resultSet.Result;

                List<StudentEntity> currentList = entityList.Where(o => o.IsStudent == true).ToList();
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

                    sb.Append("<a href = \"AddStudent.aspx?Id=" + student.EncryptId + "\">");
                    sb.Append(student.FullName + " "); sb.AppendLine("</td>");
                    sb.AppendLine("<td>");
                    sb.Append(student.Birthday.Value.ToString("dd/MM/yyyy"));
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