using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class IncomeAndExpenseAdd : System.Web.UI.Page
    {
        #region VARIABLES
        IncomeAndExpenseBusiness business = null;
        ProjectType projectType = ProjectType.None;
        List<IncomeAndExpenseEntity> lst;
        #endregion VARIABLES

        public const string paymentDetail = "Ödeme Detayı";

        #region PROPERTIES
        private IncomeAndExpenseEntity currentRecord;
        public IncomeAndExpenseEntity CurrentRecord
        {
            set
            {
                currentRecord = value;
                hdnId.Value = currentRecord.Id.ToString();
                txtDescription.Text = currentRecord.Description;
                drpIncomeAndExpenseType.SelectedValue = currentRecord.IncomeAndExpenseTypeId.ToString();
                txtAmount.Text = currentRecord.Amount.ToString(); ;
                chcIsActive.Checked = (currentRecord.IsActive.HasValue) ? currentRecord.IsActive.Value : false;
                txtProcessDate.Text = currentRecord.ProcessDate.ToString("yyyy-MM-dd");

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
            business = new IncomeAndExpenseBusiness(projectType);

            divInformation.ListRecordPage = "/gelir-gider-listesi";
            divInformation.NewRecordPage = "/gelir-gider-ekle";

            divInformation.InformationVisible = false;

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.IncomeAndExpenseAdd);
            master.SetVisibleSearchText(false);
            btnDelete.Visible = false;

            if (!Page.IsPostBack)
            {
                loadIncomeAndExpenseType();
                loadWorker();

                txtProcessDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                object Id = Page.RouteData.Values["incomeAndExpense_id"];

                if (Id != null)
                {
                    string IdDecrypt = Cipher.Decrypt(Id.ToString());

                    int id = GeneralFunctions.GetData<int>(IdDecrypt);
                    if (id > 0)
                    {
                        DataResultArgs<List<IncomeAndExpenseEntity>> resultSet = 
                            new IncomeAndExpenseBusiness(projectType).Get_IncomeAndExpense(new SearchEntity() { Id = id });
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

        private void loadWorker()
        {
            List<WorkerEntity> lst = new WorkerBusiness(projectType).Get_Worker(new SearchEntity() { IsActive = true, IsDeleted = false }, null).Result;

            int count = 0;

            if (lst.Count == 0)
                return;

            string price = lst.Sum(o => o.Price).Value.ToString(CommonConst.TL);
            drpWorker.Items.Add(new ListItem("Hepsi - " + price, "-1"));
            drpWorker.Items[count].Attributes.Add("calculatePrice", price);
            count++;

            List<WorkerEntity> activeList = lst.Where(o => o.IsActive == true).ToList();
            price = activeList.Sum(o => o.Price).Value.ToString(CommonConst.TL);

            drpWorker.Items.Add(new ListItem("Sadece Aktif Olanlar - "+ price, "-2"));

            drpWorker.Items[count].Attributes.Add("calculatePrice", price);
            count++;

            foreach (WorkerEntity entity in lst)
            {
                string displayText =  entity.Title + " - " + entity.PriceStr;
                drpWorker.Items.Add(new ListItem(displayText, entity.Id.ToString()));

                drpWorker.Items[count].Attributes.Add("calculatePrice", entity.PriceStr);

                count++;
            }

        }

        private void loadIncomeAndExpenseType()
        {
            DataResultArgs<List<IncomeAndExpenseTypeEntity>> typeListResult = new IncomeAndExpenseTypeBusiness(projectType).Get_IncomeAndExpenseType(new SearchEntity() { IsActive = true, IsDeleted = false }); ;

            if (typeListResult.HasError)
            {
                divInformation.ErrorText = typeListResult.ErrorDescription;
                return;
            }
            else
            {
                List<IncomeAndExpenseTypeEntity> typeList = typeListResult.Result;

                int count = 0;

                if (typeList.Count == 0)
                    return;

                foreach (IncomeAndExpenseTypeEntity entity in typeList)
                {
                    string name = entity.Name;
                    if (entity.Type == 1)
                        name += " -> Gelir ";
                    else if (entity.Type == 2)
                        name += " -> Gider";
                    else if (entity.Type == 3)
                        name += " -> Çalışan Gideri";

                    drpIncomeAndExpenseType.Items.Add(new ListItem(name, entity.Id.ToString()));
                    drpIncomeAndExpenseType.Items[count].Attributes.Add("incomeAndExpenseSubType", entity.Type.ToString());
                    count++;
                }

                drpIncomeAndExpenseType.SelectedIndex = 0;
                
                if(drpIncomeAndExpenseType.Items[0].Attributes["incomeAndExpenseSubType"] == IncomeAndExpenseSubType.Income.ToString())
                {
                    txtIncomeAndExpenseTypeName.BackColor = Color.Green;
                    txtIncomeAndExpenseTypeName.ForeColor = Color.White;
                    txtIncomeAndExpenseTypeName.Text = "Gelir";
                }
                else
                {
                    txtIncomeAndExpenseTypeName.BackColor = Color.Red;
                    txtIncomeAndExpenseTypeName.Text = "Gider";
                }
            }
        }


        #endregion CONTRUCTOR && PAGE_LOAD

        #region METHODS

        private void processToDatabase(DatabaseProcess databaseProcess)
        {
            IncomeAndExpenseEntity entity = new IncomeAndExpenseEntity();
            entity.DatabaseProcess = databaseProcess;
            entity.Id = GeneralFunctions.GetData<Int32>(hdnId.Value);

            entity.IncomeAndExpenseTypeId = GeneralFunctions.GetData<int>(drpIncomeAndExpenseType.SelectedValue);
            entity.Amount = GeneralFunctions.GetData<decimal>(txtAmount.Text);
            entity.Description = txtDescription.Text;
            entity.IsActive = chcIsActive.Checked;
           
            DataResultArgs<bool> resultSet = business.Set_IncomeAndExpense(entity);
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