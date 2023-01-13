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
    public partial class IncomingAndExpenseAdd : System.Web.UI.Page
    {
        #region VARIABLES
        ExpenseBusiness business = null;
        ProjectType projectType = ProjectType.None;
        List<ExpenseEntity> lst;
        #endregion VARIABLES

        public const string paymentDetail = "Ödeme Detayı";

        #region PROPERTIES
        private ExpenseEntity currentRecord;
        public ExpenseEntity CurrentRecord
        {
            set
            {
                currentRecord = value;
                hdnId.Value = currentRecord.Id.ToString();
                txtDescription.Text = currentRecord.Description;
                txtName.Text = currentRecord.Name;
                txtAmount.Text = currentRecord.Amount.ToString(); ;
                chcIsActive.Checked = (currentRecord.IsActive.HasValue) ? currentRecord.IsActive.Value : false;
                rdbMoment.Checked = true;
                rdbMontly.Checked = currentRecord.IsRecursiveMontly.HasValue && currentRecord.IsRecursiveMontly.Value;

            }
        }
        #endregion PROPERTIES

        #region CONTRUCTOR && PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
            {
                Response.Redirect("/uye-giris");
            }

            projectType = (ProjectType)Session[CommonConst.ProjectType];
            business = new ExpenseBusiness(projectType);

            divInformation.ListRecordPage = "/gelir-gider-listesi";
            divInformation.NewRecordPage = "/gelir-gider-ekle";

            divInformation.InformationVisible = false;

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.IncomingAndExpenseAdd);
            master.SetVisibleSearchText(false);
            btnDelete.Visible = false;

            if (!Page.IsPostBack)
            {
                object Id = Page.RouteData.Values["outgoing_id"];

                if (Id != null)
                {
                    string IdDecrypt = Cipher.Decrypt(Id.ToString());

                    int id = GeneralFunctions.GetData<int>(IdDecrypt);
                    if (id > 0)
                    {
                        DataResultArgs<List<ExpenseEntity>> resultSet = new ExpenseBusiness(projectType).Get_Expense(new SearchEntity() { Id = id });
                        if (resultSet.HasError)
                        {
                            divInformation.ErrorText = resultSet.ErrorDescription;
                        }
                        else if (resultSet.Result.Count > 0)
                        {
                            CurrentRecord = resultSet.Result.First();
                            btnSubmit.Text = ButtonText.Update;
                            btnDelete.Visible = true;
                        }
                    }
                }
            }
        }


        #endregion CONTRUCTOR && PAGE_LOAD

        #region METHODS

        private void processToDatabase(DatabaseProcess databaseProcess)
        {
            ExpenseEntity entity = new ExpenseEntity();
            entity.DatabaseProcess = databaseProcess;
            entity.Id = GeneralFunctions.GetData<Int32>(hdnId.Value);

            entity.Name = txtName.Text;
            entity.Amount = GeneralFunctions.GetData<decimal>(txtAmount.Text);
            entity.Description = txtDescription.Text;
            entity.IsRecursiveMontly = rdbMontly.Checked;
            entity.IsActive = chcIsActive.Checked;
           
            DataResultArgs<string> resultSet = business.Set_Expense(entity);
            if (resultSet.HasError)
            {
                divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                if (databaseProcess == DatabaseProcess.Deleted)
                {
                    divInformation.SuccessfulText = RecordMessage.Delete;
                    pnlBody.Enabled = false;
                }
                else
                {
                    divInformation.SuccessfulText = (databaseProcess == DatabaseProcess.Add) ? RecordMessage.Add : RecordMessage.Update;
                    btnSubmit.Text = ButtonText.Submit;
                    pnlBody.Enabled = false;
                }
            }
        }
        #endregion METHODS

        #region EVENTS
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            bool insert = GeneralFunctions.GetData<int>(hdnId.Value) <= 0;
            processToDatabase((insert) ? DatabaseProcess.Add : DatabaseProcess.Update);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/gelir-gider-ekle");
        }


        #endregion EVENTS

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id = GeneralFunctions.GetData<int>(hdnId.Value);

            if (id > 0)
            {
                processToDatabase(DatabaseProcess.Deleted);
            }
        }
    }
}