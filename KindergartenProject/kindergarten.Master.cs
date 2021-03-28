using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Entity;

namespace KindergartenProject
{
    public partial class kindergarten : System.Web.UI.MasterPage
    {
        public MenuList SelectedMenuList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SelectedMenuList != MenuList.Login &&  Session[CommonConst.Admin] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (CurrentContex.Contex == null)
                    CurrentContex.Contex = Session[CommonConst.Admin] as AdminEntity;
            }

            if (KinderGartenWebService.List.ContainsKey(ListKey.SearchValue))
                txtSearchStudent.Text = KinderGartenWebService.List[ListKey.SearchValue];

            setNavbarVisible(SelectedMenuList != MenuList.Login);
        }

        public void SetActiveMenuAttiributes(MenuList selectedMenuList)
        {
            SetMenuAttiributes(menuPanel, selectedMenuList == MenuList.Panel);
            SetMenuAttiributes(menuStudenList, selectedMenuList == MenuList.StudentList);
            SetMenuAttiributes(menuAddStudent, selectedMenuList == MenuList.AddStudenList);
            SetMenuAttiributes(menuPaymentPlan, selectedMenuList == MenuList.PaymentPlan);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.PaymentType, menuPaymentType);

        }

        private void SetMenuAttiributes(HtmlGenericControl panel, bool isActiveMenu, HtmlGenericControl subMenuId)
        {
            if (isActiveMenu)
            {
                ui.Attributes.Remove("class");
                ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse show");
            }
            else
            {
                ui.Attributes.Remove("class");
                ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse");
            }

            SetMenuAttiributes(panel, isActiveMenu);
            SetMenuAttiributes(subMenuId, isActiveMenu);
        }

        private void SetMenuAttiributes(HtmlGenericControl panel, bool isActiveMenu)
        {
            if (isActiveMenu)
            {
                panel.Attributes.Remove("class");
                panel.Attributes.Add("class", "sidebar-item active");
            }
            else
            {
                panel.Attributes.Remove("class");
                panel.Attributes.Add("class", "sidebar-item");
            }
        }

        public void SetVisibleSearchText(bool isVisible)
        {
            txtSearchStudent.Visible = isVisible;
        }

        public void setNavbarVisible(bool isVisible)
        {
            sidebar.Visible = isVisible;
        }
    }
}