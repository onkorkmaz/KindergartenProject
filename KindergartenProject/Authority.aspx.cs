using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;

namespace KindergartenProject
{
    public partial class Authority : BasePage
    {
        public Authority() : base(AuthorityScreenEnum.Yetkilendirme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.Authority);
                master.SetVisibleSearchText(false);
            }

            if (!Page.IsPostBack)
            {
                DataResultArgs<List<AuthorityTypeEntity>> resultSet = new AuthorityTypeBusiness(CurrentContext.ProjectType).Get_AuthorityType(new SearchEntity() { IsDeleted = false, IsActive = true });


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
            if (CurrentContext.AdminEntity.OwnerStatusEnum != OwnerStatusEnum.SuperAdmin)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bu sayfa için yetkiniz bulunmamaktadır.');window.location ='/benim-dunyam-montessori-okullari';", true);
            }
        }
    }
}