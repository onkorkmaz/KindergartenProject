using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using  Common;
using Entity;

namespace KindergartenProject
{
    public partial class Exit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            if (Session[CommonConst.Admin] != null)
            {
                CurrentContex.Contex = null;
                Session[CommonConst.Admin] = null;
                Session[CommonConst.ProjectType] = null;
                Session.Abandon();
                Response.Redirect("uye-giris");
            }
        }
    }
}