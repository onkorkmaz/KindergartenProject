function GetStudentEntity(citizenshipNumber)
{
    if (!IsNullOrEmpty(citizenshipNumber)) {
        var jsonData = "{ citizenshipNumber: " + JSON.stringify(citizenshipNumber) + " }";
        CallServiceWithAjax('KinderGartenWebService.asmx/GetStudentEntity',
            jsonData,
            successFunctionGetStudentEntity,
            errorFunction);
    }
}

function txtCitizenshipNumber_Change(citizenshipNumber) {
    GetStudentEntity(citizenshipNumber);
}

function successFunctionGetStudentEntity(result) {

    if (!result.HasError) {
        var entity = result.Result;
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
            document.getElementById("txtSpokenPrice").value = entity.SpokenPrice;
            document.getElementById("txtNotes").value = entity.Notes;
            document.getElementById("txtBirthday").value = entity.BirthdayWithFormat2;
            document.getElementById("txtDateOfMeeting").value = entity.DateOfMeetingWithFormat2;
            document.getElementById("txtEmail").value = entity.Email;
            document.getElementById("btnSubmit").value = "Güncelle";

        } else {

            document.getElementById("hdnId").value = "";
            document.getElementById("txtName").value = "";
            document.getElementById("txtSurname").value = "";
            document.getElementById("txtMiddleName").value = "";
            document.getElementById("txtMotherName").value = "";
            document.getElementById("txtFatherName").value = "";
            document.getElementById("txtFatherPhoneNumber").value = "";
            document.getElementById("txtMotherPhoneNumber").value = "";
            document.getElementById("chcIsActive").checked = true;
            document.getElementById("txtSpokenPrice").value = "";
            document.getElementById("txtNotes").value = "";
            document.getElementById("txtBirthday").value = "";
            document.getElementById("txtDateOfMeeting").value = "";
            document.getElementById("txtEmail").value = "";
            document.getElementById("btnSubmit").value = "Kaydet";
        }
    }
    else {
        alert("Hata var !!! Error : " + result.ErrorDescription);
    }
}

function isEmailAddress(str) {

    var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    return str.match(pattern);
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
            errorMessage += "Anne telefon numarası ya da baba ad ve telefon numarası dolu olmalıdır.\n";
        }
    }

    var email = document.getElementById("txtEmail").value;
    if (!IsNullOrEmpty(email) && !isEmailAddress(email)) {
        errorMessage += "Email bilgisi doğru formatta girilmemiştir.\n";

    }

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    window["studentList"] = null;
    return true;
}

