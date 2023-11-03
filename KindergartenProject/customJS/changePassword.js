window.onload = function () {
};


function txtUserName_Change(value) {

    var userName = document.getElementById("txtUserName").value;
    var id = document.getElementById("hdnId").value;

    if (!IsNullOrEmpty(userName)) {
        var jsonData = "{ id:" + JSON.stringify(id) + ", userName: " + JSON.stringify(userName) + "  }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/ControlUserName', jsonData, successFunctionControlClassName, errorFunction);
    }

}

function successFunctionControlClassName(obje) {
    if (!obje.HasError) {

        var hasUserName = obje.Result;
        if (hasUserName) {
            alert("Kullanıcı adı uygun değildir.");
            document.getElementById("txtUserName").value = "";
            return;   
        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function validateAndSave()
{
    if (!validate())
        return false;


    var id = document.getElementById("hdnId").value;
    var name = document.getElementById("txtUserName").value;
    var password = document.getElementById("txtPassword").value;
    var passwordRepeat = document.getElementById("txtPasswordRepeat").value;
   
    var adminEntity = {};
    adminEntity["UserName"] = name;
    adminEntity["Password"] = password;
    
    var jsonData = "{ id:" + JSON.stringify(id) + ", adminEntity: " + JSON.stringify(adminEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/UpdateAdmin', jsonData, successFunctionUpdateAdmin, errorFunction);

    return false;

}

function successFunctionUpdateAdmin(obje) {
    if (!obje.HasError) {
        callInsertOrUpdateInformationMessage("hdnId");
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtUserName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Kullanıcı boş bırakılamaz\n";

    var password = document.getElementById("txtPassword").value;
    if (IsNullOrEmpty(password))
        errorMessage += "Şifre boş bırakılamaz\n";

    var passwordRepeat = document.getElementById("txtPasswordRepeat").value;
    if (IsNullOrEmpty(passwordRepeat))
        errorMessage += "Şifre tekrarı boş bırakılamaz\n";


    if (!IsNullOrEmpty(password) && !IsNullOrEmpty(passwordRepeat) && password != passwordRepeat) {
        errorMessage += "Şifre ve tekrarı birbirne eşit olmalıdır";
    }

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}


function setDefaultValues() {
    document.getElementById("txtUserName").value = "";
    document.getElementById("txtPassword").value = "";
    document.getElementById("txtPasswordRepeat").value = "";
}

function cancel() {
    setDefaultValues();
    return false;
}
