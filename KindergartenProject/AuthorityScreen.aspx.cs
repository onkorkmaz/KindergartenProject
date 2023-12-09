using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class AuthorityScreen : BasePage
    {
        public AuthorityScreen() : base(AuthorityScreenEnum.Ekran_Icin_Yetkilendirme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.AuthorityScreen);
                master.SetVisibleSearchText(false);
            }
        }
    }
}