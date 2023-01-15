window.onload = function () {
    loadData();
    //document.getElementById("btnSubmit").disabled = "disabled";
};

function loadData() {

    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllIncomeAndExpenseType', jsonData, successFunctionGetIncomeAndExpenseType, errorFunction);

}

function successFunctionGetIncomeAndExpenseType(obje) {

    var entityList = obje;
    if (entityList != null) {

        var tbodyIncome = "";
        var tbodyExpense= "";

        for (var i in entityList) {

            var tbody = "";            

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"#\"><img src =\"/img/icons/update1.png\" onclick='updateCurrentRecord(\"" + entityList[i].EncryptId + "\")'/></a>";
            tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";
            tbody += "</td>";

            tbody += "<td>" + entityList[i].Name + "</td>";
            
            if (entityList[i].IsActive)
                tbody += "<td><img src='img/icons/active.png' width='25' height ='25' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='20' height ='20' /></td>";

            if (entityList[i].Type==1)
                tbody += "<td>Gelir</td>";
            else if (entityList[i].Type == 2)
                tbody += "<td>Gider</td>";
            else if (entityList[i].Type == 3)
                tbody += "<td style='color:red;'>Çalışan Gideri</td>";

            tbody += "</tr> ";


            if (entityList[i].Type == 1) {
                tbodyIncome += tbody;
            }
            else {
                tbodyExpense += tbody;

            }
        }

        document.getElementById("tbIncome").innerHTML = tbodyIncome;
        document.getElementById("tbExpense").innerHTML = tbodyExpense;


    }
}

function validateAndSave()
{
    if (!validate())
        return false;

    var id = document.getElementById("hdnId").value;
    var name = document.getElementById("txtName").value;
    var type = document.getElementById("drpType").value;
    var isActive = document.getElementById("chcIsActive").checked;


    var IncomeAndExpenseTypeEntity = {};
    IncomeAndExpenseTypeEntity["Name"] = name;
    IncomeAndExpenseTypeEntity["Type"] = type;
    IncomeAndExpenseTypeEntity["IsActive"] = isActive;
    var jsonData = "{ encryptId:" + JSON.stringify(id) + ", IncomeAndExpenseTypeEntity: " + JSON.stringify(IncomeAndExpenseTypeEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/InsertOrUpdateIncomeAndExpenseTypeEntity', jsonData, successFunctionInsertOrUpdateIncomeAndExpenseTypeEntity, errorFunction);

    return false;

}

function validate() {
    var errorMessage = "";

    obje = document.getElementById("txtName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "İsim boş bırakılamaz\n";

    obje = document.getElementById("drpType").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Tip Seçilmelidir\n";
    else if (obje != 1 && obje != 2 && obje != 3) {
        errorMessage += "Tip Seçilmelidir\n";
    }


    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function successFunctionInsertOrUpdateIncomeAndExpenseTypeEntity(obje) {

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
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteIncomeAndExpenseType', jsonData, successFunctionDeletePaymentType, errorFunction);
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
    var jsonData = "{ id: " + JSON.stringify(id) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/UpdateIncomeAndExpenseType', jsonData, successFunctionUpdateWorker, errorFunction);

}

function successFunctionUpdateWorker(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("txtName").value = entity.Name;

        document.getElementById("drpType").value = entity.Type;


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
    document.getElementById("txtName").value = "";
    document.getElementById("drpType").value = 1;

    document.getElementById("chcIsActive").checked = true;
    document.getElementById("btnSubmit").value = "Kaydet";
    //document.getElementById("btnSubmit").disabled = "disabled";

}

function cancel() {
    setDefaultValues();
    return false;
}
