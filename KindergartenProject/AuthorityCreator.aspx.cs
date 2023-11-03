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
    public partial class AuthorityCreator : System.Web.UI.Page
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
                master.SetActiveMenuAttiributes(MenuList.AuthorityGenerator);
                master.SetVisibleSearchText(false);
            }

            controlAuthorization(adminEntity);

        }

        private void controlAuthorization(AdminEntity adminEntity)
        {
            if (AdminContext.AdminEntity.AuthorityTypeEnum != OwnerStatusEnum.Developer)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
            }
        }
    }
}