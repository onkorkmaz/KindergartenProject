using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Entity;
using Business;

namespace KindergartenProject
{
    public partial class kindergarten : System.Web.UI.MasterPage
    {

        public MenuList SelectedMenuList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            sidebar.Visible = false;
            if (SelectedMenuList != MenuList.Login && (Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }
            else
            {
                if (new BasePage()._AdminEntity == null)
                    new BasePage()._AdminEntity = Session[CommonConst.Admin] as AdminEntity;

                if (new BasePage()._AdminEntity == null)
                {
                    return;
                }
                setVisibleMenuItems();
                setScreenAuthorityAndVisible();

            }

            if (KinderGartenWebService.List.ContainsKey(ListKey.SearchValue))
                txtSearchStudent.Text = KinderGartenWebService.List[ListKey.SearchValue];

            sidebar.Visible = SelectedMenuList != MenuList.Login;

            lblProjectType.Text = (new BasePage()._AdminEntity.ProjectType == ProjectType.BenimDunyamEgitimMerkeziIstiklalCaddesi) ? "Benim Dünyam Anaokulu" : "Benim Dünyam Eğitim Merkezi";
        }

        private void setScreenAuthorityAndVisible()
        {
            if (new BasePage()._AdminEntity.IsDeveleporOrSuperAdmin){
                return;
            }

            //Ogrenci İzleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Ogrenci_Izleme, menuStudenList);
            
            //Ogrenci İşlem
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Ogrenci_Islem, menuStudentAdd);

            //Ödeme Planı Izleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Odeme_Plani_Izleme, menuPaymentPlan);

            //Yoklama Defteri Izleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Yoklama_Izleme, menuStudentAttendanceBookList);

            //Gelir Gider Ekleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Gelir_Gider_Islem, menuIncomeAndExpenseAdd);

            //Gelir Gider İzleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Gelir_Gider_Izleme, menuIncomeAndExpenseList);

            //Ödeme Tipleri İzleme 
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Odeme_Tipleri_Islem, menuPaymentType);

            //Sınıf İzleme 
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Sinif_Izleme, menuClassList);

            //Çalışan Yönetimi İzleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Calisan_Yonetim_Izleme, menuWorkerList);

            //Gelir-Gider Tipi İzleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Gelir_Gider_Tipi_Izleme, menuIncomeAndExpenseType);

            //Admin Listesi İzleme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Admin_Izleme, menuAdminList);

            //Şifre Değiştirme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Sifre_Degistir_Izleme, menuChangePassword);

            //Cache Temizliği
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Cache_Islemleri, menuClearCache);

            //Yetki Türü
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Yetki_Turu, menuAuthorityType);

            //Yetkilendirme
            new CommonUIFunction().SetVisibility(AuthorityScreenEnum.Ekran_Icin_Yetkilendirme, menuAuthorityScreen);

        }

        private void setVisibleMenuItems()
        {
            bool isDeveloperOrAdmin = new BasePage()._AdminEntity.IsDeveleporOrSuperAdmin;
            bool isDeveloper = new BasePage()._AdminEntity.OwnerStatusEnum == OwnerStatusEnum.Developer;


            menuAuthority.Visible = isDeveloperOrAdmin;
            menuAuthorityGenerator.Visible = isDeveloper;
            menuAuthorityScreen.Visible = isDeveloperOrAdmin;
            menuAuthorityType.Visible = isDeveloperOrAdmin;
            menuChangePassword.Visible = isDeveloperOrAdmin;
            menuClassList.Visible = isDeveloperOrAdmin;
            menuClearCache.Visible = isDeveloper;
            menuIncomeAndExpenseAdd.Visible = isDeveloperOrAdmin;
            menuIncomeAndExpenseList.Visible = isDeveloperOrAdmin;
            menuIncomeAndExpenseType.Visible = isDeveloperOrAdmin;
            menuPaymentPlan.Visible = isDeveloperOrAdmin;
            menuPaymentType.Visible = isDeveloperOrAdmin;
            menuStudenList.Visible = isDeveloperOrAdmin;
            menuStudentAdd.Visible = isDeveloperOrAdmin;
            menuStudentAttendanceBookList.Visible = isDeveloperOrAdmin;
            menuWorkerList.Visible = isDeveloperOrAdmin;
            menuAdminList.Visible = isDeveloperOrAdmin;
        }

        public void SetActiveMenuAttiributes(MenuList selectedMenuList)
        {
            clearMenuActiveStyle(menuPanel);
            clearMenuActiveStyle(menuStudenList);
            clearMenuActiveStyle(menuStudentAdd);
            clearMenuActiveStyle(menuPaymentPlan);
            clearMenuActiveStyle(menuStudentAttendanceBookList);
            clearMenuActiveStyle(menuIncomeAndExpenseList);
            clearMenuActiveStyle(menuIncomeAndExpenseAdd);
            clearMenuActiveStyle(menuSettings);
            clearSubMenuStyle();

            SetMenuAttiributes(menuPanel, selectedMenuList == MenuList.Panel);
            SetMenuAttiributes(menuStudenList, selectedMenuList == MenuList.StudentList);
            SetMenuAttiributes(menuStudentAdd, selectedMenuList == MenuList.StudenAdd);
            SetMenuAttiributes(menuPaymentPlan, selectedMenuList == MenuList.PaymentPlan);
            SetMenuAttiributes(menuStudentAttendanceBookList, selectedMenuList == MenuList.StudentAttendanceBookList);
            SetMenuAttiributes(menuIncomeAndExpenseAdd, selectedMenuList == MenuList.IncomeAndExpenseAdd);
            SetMenuAttiributes(menuIncomeAndExpenseList, selectedMenuList == MenuList.IncomeAndExpenseList);

            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.PaymentType, menuPaymentType);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.ClassList, menuClassList);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.WorkerList, menuWorkerList);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.IncomeAndExpenseType, menuIncomeAndExpenseType);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.AuthorityScreen, menuAuthorityScreen);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.AuthorityType, menuAuthorityType);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.Authority, menuAuthority);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.ClearCache, menuClearCache);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.AuthorityGenerator, menuAuthorityGenerator);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.AdminList, menuAdminList);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.ChangePassword, menuChangePassword);

        }

        private void clearSubMenuStyle()
        {
            ui.Attributes.Remove("class");
            ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse");
        }
        private void clearMenuActiveStyle(HtmlGenericControl panel)
        {
            panel.Attributes.Remove("class");
            panel.Attributes.Add("class", "sidebar-item");
        }

        private void SetMenuAttiributes(HtmlGenericControl panel, bool isActiveMenu, HtmlGenericControl subMenuId)
        {
            if (isActiveMenu)
            {
                ui.Attributes.Remove("class");
                ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse show");
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
        }

        public void SetVisibleSearchText(bool isVisible)
        {
            txtSearchStudent.Visible = isVisible;
        }


    }
}