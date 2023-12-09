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
    public partial class AuthorityCreator : BasePage
    {
        public AuthorityCreator() : base(AuthorityScreenEnum.None)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.AuthorityGenerator);
                master.SetVisibleSearchText(false);
            }
        }
    }
}