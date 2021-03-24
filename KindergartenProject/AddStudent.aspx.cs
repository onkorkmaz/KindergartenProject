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
        StudentBusiness business = new StudentBusiness();
        List<StudentEntity> lst;
        #endregion VARIABLES

        public const string paymentDetail = "Ödeme Detayı";

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
                if (currentRecord.Birthday.HasValue)
                {
                    txtBirthday.Text = currentRecord.Birthday.Value.ToString("yyyy-MM-dd");
                }
                txtFatherPhoneNumber.Text = currentRecord.FatherPhoneNumber;
                txtMotherPhoneNumber.Text = currentRecord.MotherPhoneNumber;
                chcIsActive.Checked = (currentRecord.IsActive.HasValue) ? currentRecord.IsActive.Value : false;
                drpStudentState.SelectedValue = (currentRecord.IsStudent) ? "0" : "1";

                if (currentRecord.DateOfMeeting.HasValue)
                {
                    txtDateOfMeeting.Text = currentRecord.DateOfMeeting.Value.ToString("yyyy-MM-dd");
                }

                txtNotes.Text = currentRecord.Notes;
                txtSpokenPrice.Text = currentRecord.SpokenPrice.ToString();
                txtEmail.Text = currentRecord.Email;
            }
        }
        #endregion PROPERTIES

        #region CONTRUCTOR && PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            divInformation.ListRecordPage = "StudentList.aspx";
            divInformation.NewRecordPage = "AddStudent.aspx";

            divInformation.InformationVisible = false ;

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.AddStudenList);
            master.SetVisibleSearchText(false);

            if (!Page.IsPostBack)
            {
                txtBirthday.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDateOfMeeting.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

                            if (!currentRecord.IsStudent)
                            {
                                btnPaymentDetail.Visible = false;
                            }
                        }
                    }
                }
            }
        }

       
        #endregion CONTRUCTOR && PAGE_LOAD

        #region METHODS

        private void processToDatabase(DatabaseProcess databaseProcess)
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

            if (!string.IsNullOrEmpty(txtBirthday.Text))
            {
                entity.Birthday = GeneralFunctions.GetData<DateTime>(txtBirthday.Text);
            }
            entity.FatherPhoneNumber = txtFatherPhoneNumber.Text;
            entity.MotherPhoneNumber = txtMotherPhoneNumber.Text;
            entity.IsActive = chcIsActive.Checked;
            entity.IsStudent = drpStudentState.SelectedValue == "0";
            entity.Notes = txtNotes.Text;
            entity.DateOfMeeting = GeneralFunctions.GetData<DateTime>(txtDateOfMeeting.Text);
            entity.SpokenPrice = GeneralFunctions.GetData<decimal>(txtSpokenPrice.Text);
            entity.Email = txtEmail.Text;

            DataResultArgs<StudentEntity> resultSet = business.Set_Student(entity);
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
                    divInformation.SetAnotherText("<a href = \"PaymentDetail.aspx?Id=" + resultSet.Result.EncryptId + "\">" + paymentDetail + "</a>");
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
            Response.Redirect("/AddStudent.aspx");
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

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            if (currentRecord.Id > 0)
            {
                string link = "PaymentDetail.aspx?Id=" + currentRecord.EncryptId;
                Response.Redirect(link);
            }
        }
    }
}