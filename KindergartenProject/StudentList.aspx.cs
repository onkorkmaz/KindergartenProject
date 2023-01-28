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
        ProjectType projectType = ProjectType.None;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            projectType = (ProjectType)Session[CommonConst.ProjectType];

            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.StudentList);
                //lblAllStudent.Attributes.Add("onclick", "allStudent();");
                lblActiveStudent.Attributes.Add("onclick", "activeStudent();");
                lblInterview.Attributes.Add("onclick", "interviewStudent();");
                lblPassiveStudent.Attributes.Add("onclick", "passiveStudent();");

                loadClass();
                object id = Page.RouteData.Values["class_id"];
                if (id != null)
                {
                    drpClassList.SelectedValue = CommonFunctions.GetData<string>(id);
                }
            }

            divInformation.ListRecordPage = "/ogrenci-listesi";
            divInformation.NewRecordPage = "/ogrenci-ekle";

            //loadClass();
        }

        private void loadClass()
        {
            DataResultArgs<List<ClassEntity>> resultSet = new ClassBusiness(projectType).Get_ClassForStudent();

            if (resultSet.HasError)
            {
                divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                List<ClassEntity> classList = resultSet.Result;
                List<ClassEntity> list = new List<ClassEntity>();

                list.Add(new ClassEntity() { Id = -1, Name = "-" });
                if (resultSet.Result != null)
                {

                    foreach (ClassEntity entity in classList)
                    {
                        list.Add(entity);
                    }
                }


                list.Add(new ClassEntity() { Id = -2 , Name ="Atanmayanlar" });

                drpClassList.DataSource = list;
                drpClassList.DataValueField = "Id";
                drpClassList.DataTextField = "ClassAndMainTeacherName";
                drpClassList.DataBind();
            }
        }

        private void setDefaultValues()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();

            resultSet = new StudentBusiness(projectType).Get_Student(new SearchEntity() { IsDeleted = false });
            if (!resultSet.HasError)
            {
                List<StudentEntity> entityList = resultSet.Result;

                IEnumerable<StudentEntity> currentList = entityList.Where(o => o.IsStudent == true && o.IsActive == true);
                setLabel(currentList, lblActiveStudent, "Aktif");

                currentList = entityList.Where(o => o.IsActive == false);
                setLabel(currentList, lblPassiveStudent, "Pasif");

                currentList = entityList.Where(o => o.IsStudent == false && o.IsActive == true);
                setLabel(currentList, lblInterview, "Görüşme");

                //setLabel(entityList, lblAllStudent, "Toplam");

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