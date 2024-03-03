using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Entity;
using System.Text;
using System.Globalization;

namespace KindergartenProject
{
    public partial class StudentList : BasePage
    {
        public StudentList() : base(AuthorityScreenEnum.Ogrenci_Izleme)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.StudentList);
                this.Title = this.Title + " - " + master.SetTitle(_ProjectType);

                //lblAllStudent.Attributes.Add("onclick", "allStudent();");
                lblActiveStudent.Attributes.Add("onclick", "activeStudent();");
                lblInterview.Attributes.Add("onclick", "interviewStudent();");
                lblPassiveStudent.Attributes.Add("onclick", "passiveStudent();");

                CommonUIFunction.loadClass(_ProjectType, ref drpClassList, ref divInformation, true);

                object id = Page.RouteData.Values["class_id"];
                if (id != null)
                {
                    drpClassList.SelectedValue = CommonFunctions.GetData<string>(id);
                }
            }

            divInformation.ListRecordPage = "/ogrenci-listesi";
            divInformation.NewRecordPage = "/ogrenci-ekle";
        }

    }
}