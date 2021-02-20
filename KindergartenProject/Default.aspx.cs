using Common;
using System;
using System.Web.UI;
using Entity;
using Business;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.Panel);
            }

            setDefaultValues();
        }

        private void setDefaultValues()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();

            resultSet = new StudentBusiness().Get_Student(new SearchEntity() { IsDeleted = false });
            if (!resultSet.HasError)
            {
                List<StudentEntity> entityList = resultSet.Result;

                IEnumerable<StudentEntity> currentList = entityList.Where(o => o.IsStudent == true);
                setLabel(currentList, lblStudent);

                currentList = entityList.Where(o => o.IsStudent == true && o.AddedOn > DateTime.Now.AddMonths(-1));
                setLabel(currentList, lblMonthStudent);

                currentList = entityList.Where(o => o.IsStudent == false);
                setLabel(currentList, lblInterview);

                currentList = entityList.Where(o => o.IsStudent == false && o.AddedOn > DateTime.Now.AddMonths(-1));
                setLabel(currentList, lblMonthInterview);

            }
        }

        private void setLabel(IEnumerable<StudentEntity> currentList, Label lbl)
        {
            if (currentList == null || currentList.ToList() == null || currentList.ToList().Count <= 0)
            {
                lbl.Text = "0";
            }
            else {
                lbl.Text = currentList.ToList().Count.ToString();
            }
        }
    }
}