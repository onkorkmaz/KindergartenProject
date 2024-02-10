using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity;
using Business;

namespace KindergartenProject
{
    public partial class AdminList : BasePage
    {
        public AdminList() : base(AuthorityScreenEnum.Admin_Izleme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.AdminList);
                master.SetVisibleSearchText(false);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);

                List<AuthorityTypeEntity> lst = new AuthorityTypeBusiness(_ProjectType).Get_AuthorityType(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;

                drpAuthorityType.DataSource = lst;
                drpAuthorityType.DataTextField = "Name";
                drpAuthorityType.DataValueField = "Id";
                drpAuthorityType.DataBind();
            }
        }
    }
}