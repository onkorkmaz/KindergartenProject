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
    public partial class StudentAdd : System.Web.UI.Page
    {
        #region VARIABLES
        StudentBusiness business = null;
        ProjectType projectType = ProjectType.None;
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

                hdnStudentState.Value = (currentRecord.IsStudent) ? "0" : "1";

                //if (currentRecord.DateOfMeeting.HasValue)
                //{
                //    txtDateOfMeeting.Text = currentRecord.DateOfMeeting.Value.ToString("yyyy-MM-dd");
                //}

                if (currentRecord.InterviewDate.HasValue)
                {
                    txtInterviewDate.Text = currentRecord.InterviewDate.Value.ToString("yyyy-MM-dd");
                }

                chcInterview.Checked = currentRecord.IsInterview;

                txtNotes.Text = currentRecord.Notes;
                txtSpokenPrice.Text = currentRecord.SpokenPrice.ToString();
                txtEmail.Text = currentRecord.Email;

                if (currentRecord.ClassId > 0)
                {
                    drpClassList.SelectedValue = currentRecord.ClassId.ToString();
                    hdnCurrentClassId.Value = currentRecord.ClassId.ToString();
                }

                if (GeneralFunctions.GetData<int>(currentRecord.SchoolClass) > 0)
                {
                    drpSchoolClass.SelectedIndex = GeneralFunctions.GetData<int>(currentRecord.SchoolClass);
                }


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
            business = new StudentBusiness(projectType);

            divInformation.ListRecordPage = "ogrenci-listesi";
            divInformation.NewRecordPage = "ogrenci-ekle";

            divInformation.InformationVisible = false;

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.StudenAdd);
            master.SetVisibleSearchText(false);
            btnPaymentDetail.Visible = false;
            btnDelete.Visible = false;

            if (!Page.IsPostBack)
            {
                //txtBirthday.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //txtDateOfMeeting.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtInterviewDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataResultArgs<List<ClassEntity>> resultSetClassList = new ClassBusiness(projectType).Get_Class(new SearchEntity() { IsActive = true, IsDeleted = false });
                if (resultSetClassList.HasError)
                {
                    divInformation.ErrorText = resultSetClassList.ErrorDescription;
                    return;
                }
                else
                {
                    List<ClassEntity> classList = resultSetClassList.Result;
                    List<ClassEntity> list = new List<ClassEntity>();

                    list.Add(new ClassEntity() { Id = -1 });
                    if (resultSetClassList.Result != null)
                    {

                        foreach (ClassEntity entity in classList)
                        {
                            list.Add(entity);
                        }
                    }

                    drpClassList.DataSource = list;
                    drpClassList.DataValueField = "Id";
                    drpClassList.DataTextField = "ClassAndMainTeacherName";
                    drpClassList.DataBind();
                }

                object Id = Page.RouteData.Values["student_id"];

                if (Id != null)
                {
                    string IdDecrypt = Cipher.Decrypt(Id.ToString());

                    int id = GeneralFunctions.GetData<int>(IdDecrypt);
                    if (id > 0)
                    {
                        DataResultArgs<List<StudentEntity>> resultSet = new StudentBusiness(projectType).Get_Student(new SearchEntity() { Id = id });
                        if (resultSet.HasError)
                        {
                            divInformation.ErrorText = resultSet.ErrorDescription;
                        }
                        else if (resultSet.Result.Count > 0)
                        {
                            CurrentRecord = resultSet.Result.First();
                            btnSubmit.Text = ButtonText.Update;
                            btnPaymentDetail.Visible = currentRecord.IsStudent;
                            btnDelete.Visible = true;
                        }
                    }
                }

                int classId = GeneralFunctions.GetData<int>(drpClassList.SelectedValue);
                if (classId > 0)
                {
                    lblMaxStudentCount.Text = new KinderGartenWebService().CalculateRecordedStudentCount(classId.ToString());
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
            //entity.DateOfMeeting = GeneralFunctions.GetData<DateTime>(txtDateOfMeeting.Text);
            entity.SpokenPrice = GeneralFunctions.GetData<decimal>(txtSpokenPrice.Text);
            entity.Email = txtEmail.Text;
            entity.ClassId = GeneralFunctions.GetData<int>(drpClassList.SelectedValue);
            entity.IsInterview = chcInterview.Checked;
            entity.InterviewDate = GeneralFunctions.GetData<DateTime>(txtInterviewDate.Text); 

            if (drpSchoolClass.SelectedIndex > 0)
            {
                entity.SchoolClass = drpSchoolClass.SelectedValue;
            }


            if (entity.DatabaseProcess == DatabaseProcess.Add)
                entity.IsAddAfterPaymentUnPayment = true;

            if (hdnStudentState.Value == "1" && entity.IsStudent)
            {
                entity.IsAddAfterPaymentUnPayment = true;
            }

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

                    if (entity.IsStudent)
                    {
                        divInformation.SetAnotherText("<a href = \"/odeme-plani-detay/" + resultSet.Result.EncryptId + "\">" + paymentDetail + "</a>");
                    }
                    else
                    {
                        btnPaymentDetail.Visible = false;
                    }
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
            Response.Redirect("ogrenci-ekle");
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
            object Id = Page.RouteData.Values["student_id"];

            if (Id != null)
            {
                string IdDecrypt = Cipher.Decrypt(Id.ToString());

                int id = GeneralFunctions.GetData<int>(IdDecrypt);
                if (id > 0)
                {
                    string link = "~/odeme-plani-detay/" + Cipher.Encrypt(id.ToString());
                    Response.Redirect(link);
                }
            }
        }
    }
}