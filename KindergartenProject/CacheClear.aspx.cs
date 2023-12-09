using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class CacheClear : BasePage
    {
        public CacheClear() : base(AuthorityScreenEnum.Cache_Islemleri)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.ClearCache);
                master.SetVisibleSearchText(false);
            }
        }
    }
}