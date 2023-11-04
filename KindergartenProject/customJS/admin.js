window.onload = function () {
    loadData();
    //document.getElementById("btnSubmit").disabled = "disabled";
};

function loadData() {

    var isOnlyActiveStudent = document.getElementById("chcOnlyActive").checked;
    var jsonData = "{ isOnlyActive: " + JSON.stringify(isOnlyActiveStudent) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllAdmin', jsonData, successFunctionGetAdminList, errorFunction);

}

function successFunctionGetAdminList(obje) {

    var entityList = obje;
    if (entityList != null) {

        var tbody = "";
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"#\"><img src =\"/img/icons/update1.png\" onclick='updateCurrentRecord(\"" + entityList[i].Id + "\")'/></a>";
            tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].Id + "\")' /></a>";
            tbody += "</td>";

            tbody += "<td>" + entityList[i].UserName + "</td>";

            var isUpperAdmin = entityList[i].EntranceAdminInfo != null && (entityList[i].EntranceAdminInfo.OwnerStatus == 1 || entityList[i].EntranceAdminInfo.OwnerStatus == 2);


            if (isUpperAdmin) {
                tbody += "<td>" + entityList[i].Password + "</td>";
            }
            else {
                tbody += "<td>******</td>";
            }

            tbody += "<td>" + entityList[i].AuthorityDescription + "</td>";

            if (isUpperAdmin) {
                tbody += "<td>" + entityList[i].OwnerStatusDescription + "</td>";
            }
            else {
                tbody += "<td>******</td>";
            }

            if (entityList[i].IsActive)
                tbody += "<td><img src='img/icons/active.png' width='25' height ='25' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='20' height ='20' /></td>";


            tbody += "<td>" + convertToJavaScriptDate(entityList[i].UpdatedOn) + "</td>";
            tbody += "</tr> ";
        }

        document.getElementById("tbAdminList").innerHTML = tbody;

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

    obje = document.getElementById("drpAuthorityType").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Yetki türü boş bırakılamaz\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}


function validateAndSave() {
    if (!validate())
        return false;

    var id = document.getElementById("hdnId").value;
    var name = document.getElementById("txtUserName").value;
    var password = document.getElementById("txtPassword").value;
    var isActive = document.getElementById("chcIsActive").checked;
    var authorityTypeId = document.getElementById("drpAuthorityType").value;

    var adminEntity = {};
    adminEntity["UserName"] = name;
    adminEntity["Password"] = password;
    adminEntity["IsActive"] = isActive;
    adminEntity["AuthorityTypeId"] = authorityTypeId;

  
    var jsonData = "{ id:" + JSON.stringify(id) + ", adminEntity: " + JSON.stringify(adminEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/InsertOrUpdateAdmin', jsonData, successFunctionInsertOrUpdateAdmin, errorFunction);

    return false;

}

function successFunctionInsertOrUpdateAdmin(obje) {

    if (!obje.HasError && obje.Result) {
        loadData();
        callInsertOrUpdateInformationMessage("hdnId");

        setDefaultValues();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function deleteCurrentRecord(id) {

    if (confirm('Silme işlemine devam etmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteAdmin', jsonData, successFunctionDeleteAdmin, errorFunction);
    }

}

function successFunctionDeleteAdmin(obje) {
    if (!obje.HasError) {
        loadData();
        callDeleteInformationMessage();

        setDefaultValues();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function updateCurrentRecord(id) {

    document.getElementById("hdnId").value = id;
    var jsonData = "{ id: " + JSON.stringify(id) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAdminWithId', jsonData, successFunctionGetAdminWithId, errorFunction);

}

function successFunctionGetAdminWithId(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("txtUserName").value = entity.UserName;
        document.getElementById("txtPassword").value = entity.Password;
        document.getElementById("txtPasswordRepeat").value = entity.Password;
        document.getElementById("chcIsActive").checked = entity.IsActive;
        document.getElementById("drpAuthorityType").value = entity.AuthorityTypeId;

        document.getElementById("btnSubmit").value = "Güncelle";
        document.getElementById("btnSubmit").disabled = "";

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function setDefaultValues() {
    document.getElementById("hdnId").value = "";
    document.getElementById("txtName").value = "";
    document.getElementById("txtSurname").value = "";

    document.getElementById("txtPrice").value = "";
    document.getElementById("txtPhoneNumber").value = "";

    document.getElementById("chcIsActive").checked = true;
    document.getElementById("chcIsManager").checked = false;
    document.getElementById("btnSubmit").value = "Kaydet";

}

function cancel() {
    setDefaultValues();
    return false;
}
