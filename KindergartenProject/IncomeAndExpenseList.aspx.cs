using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class IncomeAndExpenseList : System.Web.UI.Page
    {
        #region VARIABLES
        ProjectType projectType = ProjectType.None;
        #endregion VARIABLES

        #region CONTRUCTOR && PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            projectType = (ProjectType)Session[CommonConst.ProjectType];

            divInformation.ListRecordPage = "/gelir-gider-listesi";
            divInformation.NewRecordPage = "/gelir-gider-ekle";

            divInformation.InformationVisible = false;

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.IncomeAndExpenseList);
            master.SetVisibleSearchText(false);

            if (!Page.IsPostBack)
            {
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

                drpMonth.Items.Clear();
                drpMonth.Items.Add(new ListItem("Seçiniz", "-1"));
                foreach (SeasonEntity entity in months)
                {
                    drpMonth.Items.Add(new ListItem(entity.MonthName, entity.Month.ToString()));
                }

                drpMonth.SelectedValue = currentMonth.ToString();

                drpIncomeAndExpenseType.Items.Clear();
                drpIncomeAndExpenseType.Items.Add(new ListItem("Seçiniz", "-1"));

                List<IncomeAndExpenseTypeEntity> typeList = new IncomeAndExpenseTypeBusiness(projectType).Get_IncomeAndExpenseType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;
                foreach(IncomeAndExpenseTypeEntity type in typeList)
                {
                    drpIncomeAndExpenseType.Items.Add(new ListItem(type.Name, type.Id.ToString()));
                }
            }
        }

        #endregion CONTRUCTOR && PAGE_LOAD

    }
}