using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Entity;
using Common;

namespace KindergartenProject
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LogBusiness log = new LogBusiness(ProjectType.BenimDunyamAnaokuluSezenSokak);
            log.InsertLog("methot başladı ___");

            StudentBusiness bus = new StudentBusiness(ProjectType.BenimDunyamAnaokuluSezenSokak);
            List<StudentEntity> list = bus.Get_StudentList(null).Result;

            log.InsertLog("methot ikinci aşamada ___");

            GridView1.DataSource = list;
            GridView1.DataBind();

            log.InsertLog("methot bitti ___");
        }
    }
}