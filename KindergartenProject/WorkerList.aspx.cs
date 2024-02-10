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
    public partial class WorkerList : BasePage
    {
        public WorkerList() : base(AuthorityScreenEnum.Sinif_Izleme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.WorkerList);
                master.SetVisibleSearchText(false);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);
            }
        }
    }
}