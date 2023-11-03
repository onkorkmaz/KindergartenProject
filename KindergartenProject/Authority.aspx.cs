using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;

namespace KindergartenProject
{
    public partial class Authority : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            AdminEntity adminEntity = null;
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            adminEntity = (AdminEntity)Session[CommonConst.Admin];

            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.Authority);
                master.SetVisibleSearchText(false);
            }

            controlAuthorization(adminEntity);

            if (!Page.IsPostBack)
            {
                DataResultArgs<List<AuthorityTypeEntity>> resultSet = new AuthorityTypeBusiness(adminEntity.ProjectType).Get_AuthorityType(new SearchEntity() { IsDeleted = false, IsActive = true });


                if (resultSet.HasError)
                {
                    divInformation.ErrorText = resultSet.ErrorDescription;
                    return;
                }
                else
                {
                    List<AuthorityTypeEntity> listOrder = resultSet.Result.OrderBy(o => o.Name).ToList();
                    List <AuthorityTypeEntity> list = new List<AuthorityTypeEntity>();
                    list.Add(new AuthorityTypeEntity() { Id = -1, Name = "Seçiniz..." });
                    if (resultSet.Result != null)
                    {
                        foreach (AuthorityTypeEntity entity in listOrder)
                        {
                            list.Add(entity);
                        }
                    }

                    drpAuthorityType.DataSource = list;
                    drpAuthorityType.DataValueField = "Id";
                    drpAuthorityType.DataTextField = "Name";
                    drpAuthorityType.DataBind();
                }
            }
        }

        private void controlAuthorization(AdminEntity adminEntity)
        {
            if (AdminContext.AdminEntity.OwnerStatusEnum != OwnerStatusEnum.Developer)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
            }
        }
    }
}