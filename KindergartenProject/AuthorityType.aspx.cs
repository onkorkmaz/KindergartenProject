using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class AuthorityType : BasePage
    {
        public AuthorityType() : base(AuthorityScreenEnum.Yetki_Turu)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.AuthorityType);
                master.SetVisibleSearchText(false);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);
            }
        }
    }
}