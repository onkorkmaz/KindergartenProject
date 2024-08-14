using Business;
using Common;
using Entity;
using System;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as kindergarten;
            ((kindergarten)master).SelectedMenuList = MenuList.Login;

            master.SetActiveMenuAttiributes(MenuList.Login);
            master.SetVisibleSearchText(false);

            string machineName = Environment.MachineName;
            if (machineName == "DESKTOP-4ISBFD4" && !Page.IsPostBack)
            {
                txtUserName.Text = "onur";
                txtPassword.Text = "1";
                drpProjectType.SelectedValue = "2";
                btnLogin_Click(null, null);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string projectType = drpProjectType.SelectedValue;
            Int16 projectTypeInt = 0;
            Int16.TryParse(projectType, out projectTypeInt);

            ProjectType projectTypeEnum = (ProjectType)projectTypeInt;

            DataResultArgs<AdminEntity> resultSet = new AdminBusiness(projectTypeEnum).Get_Admin(userName, password);
            if (resultSet.HasError)
                divInformation.ErrorText = resultSet.ErrorDescription;
            else
            {
                if (resultSet.Result == null)
                {
                    divInformation.ErrorText = "Kullanıcı adı veya şifre yanlış!";
                }
                else
                {
                    Session[CommonConst.Admin] = resultSet.Result;
                    Session[CommonConst.ProjectType] = projectTypeEnum;
                    loadAuthority(resultSet.Result.Id, projectTypeEnum, resultSet.Result.AuthorityTypeId);

                    DataResultArgs<AdminProjectTypeRelationEntity> resultSetAdminOrgAuth = new AdminProjectTypeRelationBusiness().Get_AdminProjectTypeRelation(new BasePage()._AdminEntity.Id, projectTypeEnum);

                    if (resultSetAdminOrgAuth.Result == null)
                    {
                        divInformation.ErrorText = "Admin -Organizasyon tablosunda yetkileriniz bulunmamakadtır.";
                    }
                    else if (resultSetAdminOrgAuth.Result.HasAuthority.HasValue && resultSetAdminOrgAuth.Result.HasAuthority.Value)
                    {
                        Response.Redirect("benim-dunyam-montessori-okullari");
                        //Response.Redirect("ogrenci-listesi");                      
                    }
                    else
                    {
                        divInformation.ErrorText = resultSetAdminOrgAuth.Result.ProjectTypeName  +" giriş için yetkiniz bulunmamaktadır.";
                    }
                }
            }
        }

        private void loadAuthority(int adminId, ProjectType projectTypeEnum, short authorityTypeId)
        {
            AuthorityEntity authority = new AuthorityBusiness(projectTypeEnum, adminId).GetAuthorityWithTypeId(authorityTypeId);
        }
    }
}
