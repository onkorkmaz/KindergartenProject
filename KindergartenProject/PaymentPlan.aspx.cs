using Business;
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
    public partial class PaymentPlan : BasePage
    {
        public PaymentPlan() : base(AuthorityScreenEnum.Odeme_Plani_Izleme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.PaymentPlan);

            divInformation.ListRecordPage = "/ogrenci-listesi";
            divInformation.NewRecordPage = "/ogrenci-ekle";

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
        }
    }
}