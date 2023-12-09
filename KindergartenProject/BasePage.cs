using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace KindergartenProject
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        { 
        }

        protected override void OnInit(EventArgs e)
        {
            controlAuthorization();
            base.OnInit(e);
        }

        public BasePage(AuthorityScreenEnum screenAuthorityEnum)
        {
            Entity.CurrentContext.ScreenAuthorityEnum = screenAuthorityEnum;
            
        }

        private void controlAuthorization()
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            CurrentContext.AdminEntity = (AdminEntity)Session[CommonConst.Admin];
            CurrentContext.ProjectType = (ProjectType)Session[CommonConst.ProjectType];
            CurrentContext.ScreenAuthorityEnum = CurrentContext.ScreenAuthorityEnum;

            if (!CurrentContext.AdminEntity.IsDeveleporOrSuperAdmin)
            {
                if (CurrentContext.ScreenAuthorityEnum == AuthorityScreenEnum.None)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
                }

                bool authorityDoesNotExists = true;
                AuthorityEntity entity = new AuthorityBusiness(CurrentContext.ProjectType).GetAuthorityWithScreenAndTypeId(CurrentContext.ScreenAuthorityEnum);
                if (entity != null && entity.HasAuthority)
                {
                    authorityDoesNotExists = false;
                }

                if (authorityDoesNotExists)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
                }
            }
        }
    }
}