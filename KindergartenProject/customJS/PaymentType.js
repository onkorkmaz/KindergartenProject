window.onload = function () {
    loadData();
    document.getElementById("btnSubmit").disabled = "disabled";
};

function loadData() {

    var jsonData = "{  }";
    CallServiceWithAjax('KinderGartenWebService.asmx/GetPaymentAllPaymentType', jsonData, successFunctionGetPaymentAllPaymentType, errorFunction);

}

function successFunctionGetPaymentAllPaymentType(obje) {

    var entityList = obje;
    if (entityList != null) {

        var tbody = "";
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"#\"><img src =\"img/icons/update1.png\" onclick='updateCurrentRecord(\"" + entityList[i].EncryptId + "\")'/></a>";
            //tbody += "<a href = \"#\"><img src =\"img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";
            tbody += "</td>";

            tbody += "<td>" + entityList[i].Name + "</td>";
            tbody += "<td>" + entityList[i].Amount + "</td>";
            if (entityList[i].IsActive)
                tbody += "<td><img src='img/icons/active.png' width='25' height ='25' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='20' height ='20' /></td>";

            tbody += "<td>" + convertToJavaScriptDate(entityList[i].UpdatedOn) + "</td>";
            tbody += "</tr> ";
        }

        document.getElementById("tbPaymentTypeList").innerHTML = tbody;

    }
}

function validateAndSave()
{
    if (!validate())
        return false;


    var id = document.getElementById("hdnId").value;
    var name = document.getElementById("txtPaymentType").value;
    var amount = document.getElementById("txtAmount").value;
    var isActive = document.getElementById("chcIsActive").checked;

    var paymentTypeEntity = {};
    paymentTypeEntity["Name"] = name;
    paymentTypeEntity["Amount"] = amount;
    paymentTypeEntity["IsActive"] = isActive;

    var jsonData = "{ encryptId:" + JSON.stringify(id)+", paymentTypeEntity: " + JSON.stringify(paymentTypeEntity) + " }";
    CallServiceWithAjax('KinderGartenWebService.asmx/InsertOrUpdatePaymentType', jsonData, successFunctionInsertOrUpdatePaymentType, errorFunction);

    return false;

}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtPaymentType").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Ödeme türü boş bırakılamaz\n";

    obje = document.getElementById("txtAmount").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Tutar boş bırakılamaz\n";



    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function successFunctionInsertOrUpdatePaymentType(obje) {

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
        CallServiceWithAjax('KinderGartenWebService.asmx/DeletePaymentType', jsonData, successFunctionDeletePaymentType, errorFunction);
    }

}

function successFunctionDeletePaymentType(obje) {
    if (!obje.HasError && obje.Result) {
        loadData();
        callDeleteInformationMessage();

        document.getElementById("hdnId").value = "";
        document.getElementById("txtPaymentType").value = "";
        document.getElementById("txtAmount").value = "";
        document.getElementById("chcIsActive").checked = true;
        document.getElementById("btnSubmit").value = "Kaydet";

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function updateCurrentRecord(id) {

    document.getElementById("hdnId").value = id;
    var jsonData = "{ id: " + JSON.stringify(id) + " }";
    CallServiceWithAjax('KinderGartenWebService.asmx/UpdatePaymentType', jsonData, successFunctionUpdatePaymentType, errorFunction);

}

function successFunctionUpdatePaymentType(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("txtPaymentType").value = entity.Name;
        document.getElementById("txtAmount").value = entity.Amount;
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
    document.getElementById("txtPaymentType").value = "";
    document.getElementById("txtAmount").value = "";
    document.getElementById("chcIsActive").checked = true;
    document.getElementById("btnSubmit").value = "Kaydet";
    document.getElementById("btnSubmit").disabled = "disabled";

}

function cancel() {
    setDefaultValues();
    return false;
}
