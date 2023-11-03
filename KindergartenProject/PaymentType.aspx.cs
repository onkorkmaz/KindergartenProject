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
    public partial class PaymentType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
                {
                    Response.Redirect("/uye-giris");
                }

                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.PaymentType);
                master.SetVisibleSearchText(false);

                if (AdminContext.AdminEntity.IsDeveleporOrSuperAdmin)
                {
                    btnSubmit.Enabled = true;
                }
                else
                {
                    btnSubmit.Enabled = false;
                }

            }
        }
    }
}