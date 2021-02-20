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
    public partial class AddStudent : System.Web.UI.Page
    {
        #region VARIABLES
        DataResultArgs<List<StudentEntity>> resultSet;
        StudentBusiness business = new StudentBusiness();
        List<StudentEntity> lst;
        #endregion VARIABLES

        #region PROPERTIES
        private StudentEntity currentRecord;
        public StudentEntity CurrentRecord
        {
            set
            {
                currentRecord = value;
                hdnId.Value = currentRecord.Id.ToString();
                txtTckn.Text = currentRecord.CitizenshipNumber;
                txtName.Text = currentRecord.Name;
                txtSurname.Text = currentRecord.Surname;
                txtMiddleName.Text = currentRecord.MiddleName;
                txtFatherName.Text = currentRecord.FatherName;
                txtMotherName.Text = currentRecord.MotherName;
                if (currentRecord.Birthdate.HasValue)
                {
                    txtDay.Text = currentRecord.Birthdate.Value.Day.ToString();
                    txtMonth.Text = currentRecord.Birthdate.Value.Month.ToString();
                    txtYear.Text = currentRecord.Birthdate.Value.Year.ToString();
                }
                txtFatherPhoneNumber.Text = currentRecord.FatherPhoneNumber;
                txtMotherPhoneNumber.Text = currentRecord.MotherPhoneNumber;
                chcIsActive.Checked = (currentRecord.IsActive.HasValue) ? currentRecord.IsActive.Value : false;
                drpStudentState.SelectedValue = (currentRecord.IsStudent) ? "0" : "1";
            }
        }
        #endregion PROPERTIES

        #region CONTRUCTOR && PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            divInformation.ListRecordPage = "StudentList.aspx";
            divInformation.NewRecordPage = "AddStudent.aspx";

            setInformationVisible(false);

            if (!Page.IsPostBack)
            {
                var master = this.Master as kindergarten;
                master.SetActiveMenuAttiributes(MenuList.AddStudenList);

                object Id = Request.QueryString["Id"];

                if (Id != null)
                {
                    string IdDecrypt = Cipher.Decrypt(Id.ToString());

                    int id = GeneralFunctions.GetData<int>(IdDecrypt);
                    if (id > 0)
                    {
                        DataResultArgs<List<StudentEntity>> resultSet = new StudentBusiness().Get_Student(new SearchEntity() { Id = id });
                        if (resultSet.HasError)
                        {
                            divInformation.ErrorText = resultSet.ErrorDescription;
                        }
                        else if (resultSet.Result.Count > 0)
                        {
                            CurrentRecord = resultSet.Result.First();
                            btnSubmit.Text = ButtonText.Update;
                        }
                    }
                }
            }
        }
        #endregion CONTRUCTOR && PAGE_LOAD

        #region METHODS


        private void setInformationVisible(bool visible)
        {
            divInformation.InformationVisible = visible;
        }

        private void processToDatabase(DatabaseProcess databaseProcess, string id = null)
        {
            StudentEntity entity = new StudentEntity();
            entity.DatabaseProcess = databaseProcess;
            entity.Id = GeneralFunctions.GetData<Int32>(hdnId.Value);
            entity.CitizenshipNumber = txtTckn.Text;
            entity.Name = txtName.Text;
            entity.Surname = txtSurname.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.FatherName = txtFatherName.Text;
            entity.MotherName = txtMotherName.Text;

            if (!string.IsNullOrEmpty(txtDay.Text) && !string.IsNullOrEmpty(txtMonth.Text) && !string.IsNullOrEmpty(txtYear.Text))
            {
                entity.Birthdate = new DateTime(GeneralFunctions.GetData<int>(txtYear.Text), GeneralFunctions.GetData<int>(txtMonth.Text), GeneralFunctions.GetData<int>(txtDay.Text));
            }
            entity.FatherPhoneNumber = txtFatherPhoneNumber.Text;
            entity.MotherPhoneNumber = txtMotherPhoneNumber.Text;
            entity.IsActive = chcIsActive.Checked;
            entity.IsStudent = drpStudentState.SelectedValue == "0";
            DataResultArgs<bool> resultSet = business.Set_Student(entity);
            if (resultSet.HasError)
            {
                divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                divInformation.SuccessfulText = (databaseProcess == DatabaseProcess.Add) ? RecordMessage.Add : RecordMessage.Update;
                btnSubmit.Text = ButtonText.Submit;
                pnlBody.Enabled = false;

            }
        }
        #endregion METHODS

        #region EVENTS
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            bool insert = GeneralFunctions.GetData<int>(hdnId.Value) <= 0;
            processToDatabase((insert) ? DatabaseProcess.Add : DatabaseProcess.Update, (insert) ? RecordMessage.Add : RecordMessage.Update);
        }
        protected void btnUpdate_Command(object sender, CommandEventArgs e)
        {
            String id = e.CommandArgument.ToString();
            CurrentRecord = lst.FirstOrDefault(o => o.Id == GeneralFunctions.GetData<Int32>(id));
            btnSubmit.Text = ButtonText.Update;
            //setEnabledToControls();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/AddStudent.aspx");
        }
        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            //lblResult.Text = e.CommandArgument.ToString();
            string id = e.CommandArgument.ToString();
            //processToDatabase(DatabaseProcess.Delete, RecordMessage.Delete, id);
        }

        #endregion EVENTS
    }
}