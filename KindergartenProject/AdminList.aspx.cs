using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity;
using Business;

namespace KindergartenProject
{
    public partial class AdminList : System.Web.UI.Page
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
                master.SetActiveMenuAttiributes(MenuList.AdminList);
                master.SetVisibleSearchText(false);
                ProjectType projectType = (ProjectType)Session[CommonConst.ProjectType];

                List<AuthorityTypeEntity> lst = new AuthorityTypeBusiness(projectType).Get_AuthorityType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;

                drpAuthorityType.DataSource = lst;
                drpAuthorityType.DataTextField = "Name";
                drpAuthorityType.DataValueField = "Id";
                drpAuthorityType.DataBind();
            }
        }
    }
}