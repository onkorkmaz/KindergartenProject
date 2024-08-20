window.onload = function () {

    loadData();
};

function loadData() {

    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAuthorityTypeList', jsonData, successFunctionGetAuthorityTypeList, errorFunction);

}

function successFunctionGetAuthorityTypeList(obje) {

    if (!obje.HasError && obje.Result) {

        var entityList = obje.Result;
        if (entityList != null) {

            var tbody = "";
            for (var i in entityList) {

                tbody += "<tr>";
                tbody += "<td>";
                tbody += "<a href = \"#\"><img src =\"/img/icons/update1.png\" onclick='updateCurrentRecord(\"" + entityList[i].Id + "\")'/></a>";
                tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].Id + "\")' /></a>";
                tbody += "</td>";

                tbody += "<td>" + entityList[i].Name + "</td>";

                tbody += "<td>" + entityList[i].Description + "</td>";

                if (entityList[i].IsActive)
                    tbody += "<td><img src='/img/icons/active.png' width='25' height ='25' /></td>";
                else
                    tbody += "<td><img src='/img/icons/passive.png' width='20' height ='20' /></td>";

                tbody += "</tr> ";
            }

            document.getElementById("tbAuthorityTypeList").innerHTML = tbody;

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
    var name = document.getElementById("txtAuthorityType").value;
    var description = document.getElementById("txtDescription").value;
    var isActive = document.getElementById("chcIsActive").checked;


    var authorityTypeEntity = {};
    authorityTypeEntity["Name"] = name;
    authorityTypeEntity["IsActive"] = isActive;
    authorityTypeEntity["Description"] = description;


    var jsonData = "{ id:" + JSON.stringify(id) + ", authorityTypeEntity: " + JSON.stringify(authorityTypeEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/InsertOrUpdateAuthorityType', jsonData, successFunctionInsertOrUpdateAuthorityType, errorFunction);

    return false;

}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtAuthorityType").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Yetki Türü boş bırakılamaz\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function successFunctionInsertOrUpdateAuthorityType(obje) {

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
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteAuthorityType', jsonData, successFunctionDeleteAuthorityType, errorFunction);
    }

}

function successFunctionDeleteAuthorityType(obje) {
    if (!obje.HasError && obje.Result) {
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
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAuthorityTypeWithId', jsonData, successFunctionGetAuthorityTypeWithId, errorFunction);

}

function successFunctionGetAuthorityTypeWithId(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("hdnId").value = entity.Id;
        document.getElementById("txtAuthorityType").value = entity.Name;
        document.getElementById("txtDescription").value = entity.Description;
        document.getElementById("chcIsActive").checked = entity.IsActive;
        document.getElementById("btnSubmit").value = "Güncelle";
        document.getElementById("btnSubmit").disabled = "";

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function setDefaultValues() {
    document.getElementById("hdnId").value = "";
    document.getElementById("txtAuthorityType").value = "";
    document.getElementById("txtDescription").value = "";
    document.getElementById("chcIsActive").checked = true;
    document.getElementById("btnSubmit").value = "Kaydet";
}

function cancel() {
    setDefaultValues();
    return false;
}
