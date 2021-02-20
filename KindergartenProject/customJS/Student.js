function GetStudentEntity(citizenshipNumber)
{
    var jsonData = "{ citizenshipNumber: " + JSON.stringify(citizenshipNumber) + " }";
    CallServiceWithAjax('KinderGartenWebService.asmx/GetStudenEntity', jsonData, successFunctionGetStudentEntity, errorFunction);
}

function txtCitizenshipNumber_Change(citizenshipNumber) {
    GetStudentEntity(citizenshipNumber);
}

function successFunctionGetStudentEntity(obje) {

    if (!obje.HasError) {
        var entity = obje.Result;
        if (entity != null) {
            alert("Girdiğiniz kimlik numarsaı sistemde mevcuttur");

            document.getElementById("hdnId").value = entity.Id;
            document.getElementById("txtName").value = entity.Name;
            document.getElementById("txtSurname").value = entity.Surname;
            document.getElementById("txtMiddleName").value = entity.MiddleName;

            document.getElementById("txtMotherName").value = entity.MotherName;
            document.getElementById("txtFatherName").value = entity.FatherName;

            document.getElementById("txtFatherPhoneNumber").value = entity.FatherPhoneNumber;
            document.getElementById("txtMotherPhoneNumber").value = entity.MotherPhoneNumber;
            document.getElementById("chcIsActive").checked = entity.IsActive;

            var birthDate = null;

            if (entity.Birthdate != null) {

                var date = new Date(Number(entity.Birthdate.replace(/\D/g, '')));

                if (!isNaN(date.getTime())) {
                    document.getElementById("txtDay").value = date.getDate();
                    document.getElementById("txtMonth").value = date.getMonth() + 1;
                    document.getElementById("txtYear").value = "19" + date.getYear();
                }
            }

            document.getElementById("btnSubmit").value = "Güncelle";

        }
        else {

            document.getElementById("hdnId").value = "";
            document.getElementById("txtName").value = "";
            document.getElementById("txtSurname").value = "";
            document.getElementById("txtMiddleName").value = "";

            document.getElementById("txtMotherName").value = "";
            document.getElementById("txtFatherName").value = "";

            document.getElementById("txtFatherPhoneNumber").value = "";
            document.getElementById("txtMotherPhoneNumber").value = "";
            document.getElementById("chcIsActive").checked = true;

            document.getElementById("txtDay").value = "";
            document.getElementById("txtMonth").value = "";
            document.getElementById("txtYear").value = "";

            document.getElementById("btnSubmit").value = "Kaydet";


        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtTckn").value;
    //if (IsNullOrEmpty(obje))
    //    errorMessage += "Tc kimlik No boş bırakılamaz\n";

    obje = document.getElementById("txtName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "İsim boş bırakılamaz\n";

    obje = document.getElementById("txtSurname").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Soyisim boş bırakılamaz\n";

    var fatherName = document.getElementById("txtFatherName").value;
    var motherName = document.getElementById("txtMotherName").value;

    if (IsNullOrEmpty(fatherName) && IsNullOrEmpty(motherName) ) {
        errorMessage += "Anne veya baba adından bir tanesi dolu olmalıdır.\n";
    }

    var fatherPhoneNumber = document.getElementById("txtFatherPhoneNumber").value;
    var motherPhoneNumber = document.getElementById("txtMotherPhoneNumber").value;

    if (IsNullOrEmpty(fatherName) && !IsNullOrEmpty(fatherPhoneNumber)) {
        errorMessage += "Baba telefon numarası dolu olduğu için baba adı girilmelidir.\n";
    }

    if (IsNullOrEmpty(motherName) && !IsNullOrEmpty(motherPhoneNumber)) {
        errorMessage += "Anne telefon numarası dolu olduğu için anne adı girilmelidir.\n";
    }

    if (!IsNullOrEmpty(fatherName) && IsNullOrEmpty(fatherPhoneNumber)) {
        if (IsNullOrEmpty(motherName) || IsNullOrEmpty(motherPhoneNumber)) {
            errorMessage += "Baba telefon numarası ya da anne ad ve telefon numarası dolu olmalıdır.\n";
        }
    }

    if (!IsNullOrEmpty(motherName) && IsNullOrEmpty(motherPhoneNumber)) {
        if (IsNullOrEmpty(fatherName) || IsNullOrEmpty(fatherPhoneNumber)) {
            errormessage += "anne telefon numarası ya da baba ad ve telefon numarası dolu olmalıdır.\n";
        }
    }

    var isValidBirthday = false;
    var day = document.getElementById("txtDay").value;
    var month = document.getElementById("txtMonth").value;
    var year = document.getElementById("txtYear").value;

    if (!IsNullOrEmpty(day))
        isValidBirthday = true;

    if (!IsNullOrEmpty(month))
        isValidBirthday = true;

    if (!IsNullOrEmpty(year))
        isValidBirthday = true;

    if (isValidBirthday) {
        if (IsNullOrEmpty(day))
            errorMessage += "Gün boş bırakılamaz.\n";

        if (IsNullOrEmpty(month))
            errorMessage += "Ay boş bırakılamaz.\n";

        if (IsNullOrEmpty(year))
            errorMessage += "Yıl boş bırakılamaz.\n";
    }

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    window["studentList"] = null;
    return true;
}

