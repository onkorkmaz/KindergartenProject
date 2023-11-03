using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        WorkerBusiness business = null;
        ProjectType projectType = ProjectType.None;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            if (!Page.IsPostBack)
            {
                hdnId.Value = AdminContext.AdminEntity.Id.ToString();
                txtUserName.Text = AdminContext.AdminEntity.UserName;
            }
        }
    }
}