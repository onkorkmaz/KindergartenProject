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
             _ScreenAuthorityEnum = screenAuthorityEnum;
            
        }

        private void controlAuthorization()
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }


            if (!_AdminEntity.IsDeveleporOrSuperAdmin)
            {
                if (_ScreenAuthorityEnum == AuthorityScreenEnum.None)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
                }

                bool authorityDoesNotExists = true;
                AuthorityEntity entity = new AuthorityBusiness(_ProjectType,_AdminEntity.Id).GetAuthorityWithScreenAndTypeId(_ScreenAuthorityEnum, _AdminEntity.AuthorityTypeId);
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

        public AdminEntity _AdminEntity
        {
            get
            {
                return (AdminEntity)Session[CommonConst.Admin];

            }
        }

        public ProjectType _ProjectType
        {
            get
            {
                return (ProjectType)Session[CommonConst.ProjectType];
            }
        }

        public AuthorityScreenEnum _ScreenAuthorityEnum { get; set; }
    }
}