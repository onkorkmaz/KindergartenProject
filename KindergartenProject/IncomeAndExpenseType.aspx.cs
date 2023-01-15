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
    public partial class IncomeAndExpenseType : System.Web.UI.Page
    {
        ProjectType projectType = ProjectType.None;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            if (!Page.IsPostBack)
            {
                projectType = (ProjectType)Session[CommonConst.ProjectType];
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.IncomeAndExpenseType);
                master.SetVisibleSearchText(false);
                drpType.Items.Add(new ListItem("Gelir", "1"));
                drpType.Items.Add(new ListItem("Gider", "2"));
                drpType.Items.Add(new ListItem("Çalışan Gideri", "3")); 
            }
        }
    }
}