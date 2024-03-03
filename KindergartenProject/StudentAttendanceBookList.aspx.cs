using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class StudentAttendanceBookList : BasePage
    {
        public StudentAttendanceBookList() : base(AuthorityScreenEnum.Yoklama_Izleme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CommonUIFunction.loadClass(_ProjectType, ref drpClassList, ref divInformation, false);
            }

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.StudentAttendanceBookList);

            int year = 2021;

            for (int i = -1; i < 10; i++)
            {
                string displatText = (year + i).ToString() + "-" + (year + i + 1).ToString();
                drpYear.Items.Add(new ListItem(displatText, (year + i).ToString()));
            }

            int currentYear = DateTime.Today.Year;
            int currentMonth = DateTime.Today.Month;
            if (currentMonth < 8)
                currentYear--;

            drpYear.SelectedValue = currentYear.ToString();

            var months = CommonUIFunction.GetSeasonList(currentYear);

            foreach (SeasonEntity entity in months)
            {
                drpMonth.Items.Add(new ListItem(entity.MonthName, entity.Month.ToString()));
            }

            drpMonth.SelectedValue = currentMonth.ToString();

            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);
            drpDays.Items.Add(new ListItem("1-15", "15"));
            drpDays.Items.Add(new ListItem("16-" + lastDay + "", lastDay.ToString()));

            if(DateTime.Today.Day>15)
            {
                drpDays.SelectedValue = lastDay.ToString();
            }
        }
    }
}