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
    public partial class IncomingAndExpenseType : System.Web.UI.Page
    {
        ProjectType projectType = ProjectType.None;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                projectType = (ProjectType)Session[CommonConst.ProjectType];
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.IncomingAndExpenseType);
                master.SetVisibleSearchText(false);
                drpType.Items.Add(new ListItem("Gelir", "1"));
                drpType.Items.Add(new ListItem("Gider", "2"));

                //List<IncomingAndExpenseTypeEntity> list = new IncomingAndExpenseTypeBusiness(projectType).Get_IncomingAndExpenseType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;

                //if (list.FirstOrDefault(o => o.Type == 3) == null)
                //{
                //    drpType.Items.Add(new ListItem("Çalışan Gideri", "3"));
                //}
            }
        }
    }
}