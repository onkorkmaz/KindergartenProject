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
                lblAllStudent.Attributes.Add("onclick", "allStudent();");
                lblActiveStudent.Attributes.Add("onclick", "activeStudent();");
                lblInterview.Attributes.Add("onclick", "interviewStudent();");
                lblPassiveStudent.Attributes.Add("onclick", "passiveStudent();");
            }

            divInformation.ListRecordPage = "StudentList.aspx";
            divInformation.NewRecordPage = "AddStudent.aspx";

            setDefaultValues();

        }

        private void setDefaultValues()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();

            resultSet = new StudentBusiness().Get_Student(new SearchEntity() { IsDeleted = false });
            if (!resultSet.HasError)
            {
                List<StudentEntity> entityList = resultSet.Result;

                IEnumerable<StudentEntity> currentList = entityList.Where(o => o.IsStudent == true && o.IsActive == true);
                setLabel(currentList, lblActiveStudent, "Aktif");

                currentList = entityList.Where(o => o.IsStudent == true && o.IsActive == false);
                setLabel(currentList, lblPassiveStudent, "Pasif");

                currentList = entityList.Where(o => o.IsStudent == false);
                setLabel(currentList, lblInterview, "Görüşme");

                setLabel(entityList, lblAllStudent, "Toplam");

            }
        }
        private void setLabel(IEnumerable<StudentEntity> currentList, Label lbl, string text)
        {
            string quantity = "";
            if (currentList != null && currentList.ToList() != null && currentList.ToList().Count > 0)
            {
                quantity = currentList.ToList().Count.ToString(); ;
            }

            lbl.Text = text + " Öğrenci Sayısı : " + quantity;
        }
    }
}