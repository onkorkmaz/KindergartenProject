window.onload = function () {
    loadData();
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

            tbody += "<td>" + entityList[i].FullName + "</td>";
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

            tbody += "<td><table cellpadding='4' border='1'><tr><td><b>BD Anaokulu<b></td><td>|</td><td><b>BD Eğitim Mer.</b></td><td><b>Pembe Kule</b></td></tr>";
            var anaokulu = "<img src='img/icons/negative.png' width='20' height ='20' />";
            var egitimMerkezi = "<img src='img/icons/negative.png' width='20' height ='20' />";
            var pembeKule = "<img src='img/icons/negative.png' width='20' height ='20' />";

            var relEntityList = entityList[i].AdminProjectTypeRelationEntityList;
            for (var r in relEntityList) {
                if (relEntityList[r].ProjectTypeId == 1 && relEntityList[r].HasAuthority) {
                    anaokulu = "<img src='img/icons/positive.png' width='20' height ='20' />";
                }

                if (relEntityList[r].ProjectTypeId == 2 && relEntityList[r].HasAuthority) {
                    egitimMerkezi = "<img src='img/icons/positive.png' width='20' height ='20' />";
                }
                if (relEntityList[r].ProjectTypeId == 3 && relEntityList[r].HasAuthority) {
                    pembeKule = "<img src='img/icons/positive.png' width='20' height ='20' />";
                }
            }

            tbody += "<tr><td style='text-align: center;'>" + anaokulu + "</td><td></td><td style='text-align: center;'>" + egitimMerkezi + "</td><td style='text-align: center;'>" + pembeKule + "</td></tr>";

            tbody += "</tr></table></td>";
            if (entityList[i].IsActive)
                tbody += "<td style='text-align: center;'><img src='img/icons/active.png' width='25' height ='25' /></td>";
            else
                tbody += "<td style='text-align: center;'><img src='img/icons/passive.png' width='20' height ='20' /></td>";


            tbody += "<td>" + convertToJavaScriptDate(entityList[i].UpdatedOn) + "</td>";
            tbody += "</tr> ";
        }

        document.getElementById("tbAdminList").innerHTML = tbody;

    }
}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtFullName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "İsim-Soyisim boş bırakılamaz\n";

    obje = document.getElementById("txtUserName").value;
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
    var fullName = document.getElementById("txtFullName").value;
    var password = document.getElementById("txtPassword").value;
    var isActive = document.getElementById("chcIsActive").checked;
    var authorityTypeId = document.getElementById("drpAuthorityType").value;

    var objeBenimDunyamAnaokulChecked = document.getElementById("chcBenimDunyamAnaokulu").checked;
    var objeBenimDunyamEgitimMerkeziChecked = document.getElementById("chcBenimDunyamEgitimMerkezi").checked;
    var objePembeKuleChecked = document.getElementById("chcPembeKule").checked;


    var adminEntity = {};
    adminEntity["UserName"] = name;
    adminEntity["FullName"] = fullName;
    adminEntity["Password"] = password;
    adminEntity["IsActive"] = isActive;
    adminEntity["AuthorityTypeId"] = authorityTypeId;

    var adminProjectTypeRelationEntityList = [];
    var adminProjectTypeRelationEntity = {};
    adminProjectTypeRelationEntity.AdminId = id;
    adminProjectTypeRelationEntity.ProjectTypeId = 1;
    adminProjectTypeRelationEntity.HasAuthority = objeBenimDunyamAnaokulChecked;
    adminProjectTypeRelationEntity.IsActive = true;
    adminProjectTypeRelationEntityList.push(adminProjectTypeRelationEntity);

    adminProjectTypeRelationEntity = {};
    adminProjectTypeRelationEntity.AdminId = id;
    adminProjectTypeRelationEntity.ProjectTypeId = 2;
    adminProjectTypeRelationEntity.HasAuthority = objeBenimDunyamEgitimMerkeziChecked;
    adminProjectTypeRelationEntity.IsActive = true;
    adminProjectTypeRelationEntityList.push(adminProjectTypeRelationEntity);

    adminProjectTypeRelationEntity = {};
    adminProjectTypeRelationEntity.AdminId = id;
    adminProjectTypeRelationEntity.ProjectTypeId = 3;
    adminProjectTypeRelationEntity.HasAuthority = objePembeKuleChecked;
    adminProjectTypeRelationEntity.IsActive = true;
    adminProjectTypeRelationEntityList.push(adminProjectTypeRelationEntity);

    adminEntity["AdminProjectTypeRelationEntityList"] = adminProjectTypeRelationEntityList;

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
        document.getElementById("txtFullName").value = entity.FullName;

        var relEntityList = entity.AdminProjectTypeRelationEntityList;

        for (var r in relEntityList) {
            if (relEntityList[r].ProjectTypeId == 1) {
                document.getElementById("chcBenimDunyamAnaokulu").checked = relEntityList[r].HasAuthority;
            }

            if (relEntityList[r].ProjectTypeId == 2 ) {
                document.getElementById("chcBenimDunyamEgitimMerkezi").checked = relEntityList[r].HasAuthority;
            }

            if (relEntityList[r].ProjectTypeId == 3) {
                document.getElementById("chcPembeKule").checked = relEntityList[r].HasAuthority;
            }
        }

        document.getElementById("btnSubmit").value = "Güncelle";
        document.getElementById("btnSubmit").disabled = "";

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function setDefaultValues() {
    document.getElementById("hdnId").value = "";
    document.getElementById("txtUserName").value = "";
    document.getElementById("txtPassword").value = "";
    document.getElementById("drpAuthorityType").value = "0";
    document.getElementById("chcBenimDunyamAnaokulu").checked = false;
    document.getElementById("chcBenimDunyamEgitimMerkezi").checked = false;
    document.getElementById("chcPembeKule").checked = false;
    document.getElementById("chcIsActive").checked = true;
    document.getElementById("txtFullName").value = "";
    document.getElementById("btnSubmit").value = "Kaydet";

}

function cancel() {
    setDefaultValues();
    return false;
}
