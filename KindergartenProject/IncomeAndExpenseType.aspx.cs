using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity;
using Common;
using Business;

namespace KindergartenProject
{
    public partial class IncomeAndExpenseType : BasePage
    {
        public IncomeAndExpenseType() : base(AuthorityScreenEnum.Gelir_Gider_Tipi_Izleme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.IncomeAndExpenseType);
                master.SetVisibleSearchText(false);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);

                drpType.Items.Add(new ListItem("Gelir", "1"));
                drpType.Items.Add(new ListItem("Gider", "2"));
                drpType.Items.Add(new ListItem("Çalışan Gideri", "3")); 
            }
        }
    }
}