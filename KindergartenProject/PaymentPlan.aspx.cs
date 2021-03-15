using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class PaymentPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.PaymentPlan);

            if (!Page.IsPostBack)
            {
                lblPaymentStudent.Attributes.Add("onclick", "paymentStudentList();");
                lblUnpaymentStudentList.Attributes.Add("onclick", "UnpaymentStudentList();");
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
                setLabel(currentList, lblPaymentStudent, "Ödemesi Gelen");

                currentList = entityList.Where(o => o.IsStudent == true && o.IsActive == false);
                setLabel(currentList, lblUnpaymentStudentList, "Ödemesini Yapan");

            }
        }
        private void setLabel(IEnumerable<StudentEntity> currentList, Label lbl, string text)
        {
            string quantity = "";
            if (currentList != null && currentList.ToList() != null && currentList.ToList().Count > 0)
            {
                quantity = currentList.ToList().Count.ToString(); ;
            }

            lbl.Text = text + " Öğrenci Listesi : " + quantity;
        }
    }
}