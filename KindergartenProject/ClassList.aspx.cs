using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace KindergartenProject
{
    public partial class ClassList : BasePage
    {
        public ClassList() : base(AuthorityScreenEnum.Sinif_Izleme)
        {
        }

        WorkerBusiness business = null;       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.ClassList);
                master.SetVisibleSearchText(false);

                business = new WorkerBusiness(CurrentContext.ProjectType);

                List<WorkerEntity> currentList = business.Get_Worker(new SearchEntity() { IsActive = true, IsDeleted = false }, true).Result;
                List<WorkerEntity> list = new List<WorkerEntity>();
                list.Add(new WorkerEntity() { Id = -1, Name = "", Surname = "" });

                foreach(WorkerEntity worker in currentList)
                {
                    list.Add(worker);
                }

                drpHelperTeacher.DataSource = list;
                drpHelperTeacher.DataValueField = "Id";
                drpHelperTeacher.DataTextField = "Title";
                drpHelperTeacher.DataBind();

                drpMainTeacher.DataSource = list;
                drpMainTeacher.DataValueField = "Id";
                drpMainTeacher.DataTextField = "Title";
                drpMainTeacher.DataBind();
            }
        }
    }
}