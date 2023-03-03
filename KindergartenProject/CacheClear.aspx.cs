using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class CacheClear : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            AdminEntity adminEntity = null;
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            adminEntity = (AdminEntity)Session[CommonConst.Admin];

            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.Authority);
                master.SetVisibleSearchText(false);
            }

            controlAuthorization(adminEntity);

        }

        private void controlAuthorization(AdminEntity adminEntity)
        {
            if (CurrentContex.Contex.AuthorityTypeEnum != AuthorityTypeEnum.Developer)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
            }
        }
    }
}