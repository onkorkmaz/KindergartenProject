using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class IncomingAndExpenseType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.IncomingAndExpenseType);
                master.SetVisibleSearchText(false);
                drpType.Items.Add(new ListItem("Gelir", "1"));
                drpType.Items.Add(new ListItem("Gider", "2"));

            }
        }
    }
}