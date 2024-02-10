using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class ChangePassword : BasePage
    {
        public ChangePassword() : base(AuthorityScreenEnum.Sifre_Degistir_Islem)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnId.Value = _AdminEntity.Id.ToString();
                txtUserName.Text = _AdminEntity.UserName;
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.ChangePassword);
                master.SetVisibleSearchText(false);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);
            }
        }
    }
}