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
    public partial class PaymentType : BasePage
    {
        public PaymentType() : base(AuthorityScreenEnum.Odeme_Tipleri_Izleme)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.PaymentType);
                master.SetVisibleSearchText(false);

                if (CurrentContext.AdminEntity.IsDeveleporOrSuperAdmin)
                {
                    btnSubmit.Enabled = true;
                }
                else
                {
                    btnSubmit.Enabled = false;
                }
            }
        }
    }
}