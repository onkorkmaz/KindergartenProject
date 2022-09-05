window.onload = function () {
    loadData();
    //document.getElementById("btnSubmit").disabled = "disabled";
};

function loadData() {

    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllWorkers', jsonData, successFunctionGetWorksers, errorFunction);

}

function successFunctionGetWorksers(obje) {

    var entityList = obje;
    if (entityList != null) {

        var tbody = "";
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"#\"><img src =\"/img/icons/update1.png\" onclick='updateCurrentRecord(\"" + entityList[i].EncryptId + "\")'/></a>";
            tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";
            tbody += "</td>";

            tbody += "<td>" + entityList[i].Name + "</td>";
            tbody += "<td>" + entityList[i].Surname + "</td>";
            if (entityList[i].IsManager) {
                tbody += "<td><img src='img/icons/active.png' width='25' height ='25' /></td>";
            }
            else {
                tbody += "<td>&nbsp;</td>";

            }
            tbody += "<td>" + entityList[i].Price + "</td>";
            tbody += "<td>" + entityList[i].PhoneNumber + "</td>";

            if (entityList[i].IsTeacher)
                tbody += "<td><img src='img/icons/active.png' width='25' height ='25' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='20' height ='20' /></td>";

            if (entityList[i].IsActive)
                tbody += "<td><img src='img/icons/active.png' width='25' height ='25' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='20' height ='20' /></td>";

            tbody += "<td>" + convertToJavaScriptDate(entityList[i].UpdatedOn) + "</td>";
            tbody += "</tr> ";
        }

        document.getElementById("tbWorkers").innerHTML = tbody;

    }
}

function validateAndSave()
{
    if (!validate())
        return false;

    var id = document.getElementById("hdnId").value;
    var name = document.getElementById("txtName").value;
    var surname = document.getElementById("txtSurname").value;
    var isManager = document.getElementById("chcIsManager").checked;
    var price = document.getElementById("txtPrice").value;
    var isActive = document.getElementById("chcIsActive").checked;
    var isTeacher = document.getElementById("chcIsTeacher").checked;
    var phoneNumber = document.getElementById("txtPhoneNumber").value;

    var workerEntity = {};
    workerEntity["Name"] = name;
    workerEntity["Surname"] = surname;
    workerEntity["IsManager"] = isManager;
    if (price == '') {
        price = 0;
    }
    workerEntity["Price"] = price;
    workerEntity["PhoneNumber"] = phoneNumber;

    workerEntity["IsActive"] = isActive;
    workerEntity["IsTeacher"] = isTeacher;

    var jsonData = "{ encryptId:" + JSON.stringify(id) + ", workerEntity: " + JSON.stringify(workerEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/InsertOrUpdateWorker', jsonData, successFunctionInsertOrUpdateWorker, errorFunction);

    return false;

}

function validate() {
    var errorMessage = "";

    obje = document.getElementById("txtName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "İsim boş bırakılamaz\n";

    obje = document.getElementById("txtSurname").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Soyisim boş bırakılamaz\n";

    //obje = document.getElementById("txtPrice").value;
    //if (IsNullOrEmpty(obje))
    //    errorMessage += "Tutar boş bırakılamaz\n";



    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function successFunctionInsertOrUpdateWorker(obje) {

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
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteWorker', jsonData, successFunctionDeletePaymentType, errorFunction);
    }

}

function successFunctionDeletePaymentType(obje) {
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
    CallServiceWithAjax('/KinderGartenWebService.asmx/UpdateWorker', jsonData, successFunctionUpdateWorker, errorFunction);

}

function successFunctionUpdateWorker(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("txtName").value = entity.Name;
        document.getElementById("txtSurname").value = entity.Surname;
        document.getElementById("chcIsManager").checked = entity.IsManager;
        document.getElementById("txtPrice").value = entity.Price;
        document.getElementById("chcIsActive").checked = entity.IsActive;
        document.getElementById("chcIsTeacher").checked = entity.IsTeacher;
        document.getElementById("txtPhoneNumber").checked = entity.PhoneNumber;

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
    //document.getElementById("btnSubmit").disabled = "disabled";

}

function cancel() {
    setDefaultValues();
    return false;
}
