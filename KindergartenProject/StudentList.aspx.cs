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
    public partial class StudentList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.StudentList);
            }

            divInformation.ListRecordPage = "StudentList.aspx";
            divInformation.NewRecordPage = "AddStudent.aspx";

        }
    }
}