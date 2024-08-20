using Common;
using System;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class StudentListDocumentCreate : BasePage
    {
        public StudentListDocumentCreate() : base(AuthorityScreenEnum.Ogrenci_Sinif_Dokuman_Olusturma)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.StudentListDocumentCreate);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);


                CommonUIFunction.loadClass(_ProjectType, ref drpClassList, ref divInformation, true,"Tüm Sınıflar");

                object id = Page.RouteData.Values["class_id"];
                if (id != null)
                {
                    drpClassList.SelectedValue = CommonFunctions.GetData<string>(id);
                }
            }
        }
    }
}