using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using  Entity;
using Business;

namespace KindergartenProject
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as kindergarten;
            ((kindergarten) master).SelectedMenuList = MenuList.Login;

            master.SetActiveMenuAttiributes(MenuList.Login);
            master.SetVisibleSearchText(false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            DataResultArgs<AdminEntity> resultSet = new AdminBusiness().Get_Admin(userName, password);
            if (resultSet.HasError)
                divInformation.ErrorText = resultSet.ErrorDescription;
            else
            {
                if (resultSet.Result == null)
                {
                    divInformation.ErrorText = "Kullanıcı adı veya şifre yanlış!";
                }
                else
                {
                    Session[CommonConst.Admin] = resultSet.Result;
                    Response.Redirect("~/Default.aspx");
                }
            }
        }
    }
}