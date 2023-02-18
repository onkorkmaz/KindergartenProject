window.onload = function () {

    loadData();
};

function loadData() {

    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetClassList', jsonData, successFunctionGetClassList, errorFunction);

}

function txtClass_Change(value) {

    var className = document.getElementById("txtClassName").value;

    if (!IsNullOrEmpty(className)) {
        var jsonData = "{ className: " + JSON.stringify(className) + "  }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/ControlClassName', jsonData, successFunctionControlClassName, errorFunction);
    }

}

function successFunctionControlClassName(obje) {
    if (!obje.HasError) {

        var entityList = obje.Result;
        if (entityList != null) {
            if (confirm("Bu isimde bir sınıf mevcut! Güncellee ile devam etmek ister misiniz?")) {
                successFunctionUpdateClass(obje);
            }
            else {
                document.getElementById("txtClassName").value = "";
                return;
            }
        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function successFunctionGetClassList(obje) {

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
                tbody += "<td>" + entityList[i].WarningOfStudentCount + "</td>";
                tbody += "<td>" + entityList[i].StudentCount + "</td>";
                tbody += "<td>" + entityList[i].MainTeacherInfo;
                if (!entityList[i].IsActiveMainTeacher) {
                    tbody += "&nbsp; <img src='/img/icons/passive.png' width='15' height='15' />";
                }
                tbody +="</td>";
                tbody += "<td>" + entityList[i].HelperTeacherInfo;
                if (!entityList[i].IsActiveHelperTeacer) {
                    tbody += "&nbsp; <img src='/img/icons/passive.png' width='15' height='15' />";
                }

                tbody += "</td>";

                if (entityList[i].IsActive)
                    tbody += "<td><img src='/img/icons/active.png' width='25' height ='25' /></td>";
                else
                    tbody += "<td><img src='/img/icons/passive.png' width='20' height ='20' /></td>";

                
                tbody += "<td>" + entityList[i].TeacherOutGoing + "</td>";
                tbody += "<td>" + entityList[i].StudentIncome + "</td>";
                tbody += "<td>" + entityList[i].StudentCurrentIncome + "</td>";
                tbody += "</tr> ";
            }

            document.getElementById("tbClassList").innerHTML = tbody;

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
    var name = document.getElementById("txtClassName").value;
    var description = document.getElementById("txtDescription").value;
    var mainTeacherId = document.getElementById("drpMainTeacher").value;
    var helperTeacherId = document.getElementById("drpHelperTeacher").value;
    var warningOfStudentCount = document.getElementById("txtWarningOfStudentCount").value;
    var isActive = document.getElementById("chcIsActive").checked;

    if (IsNullOrEmpty(warningOfStudentCount))
        warningOfStudentCount = 0;

    var classEntity = {};
    classEntity["Name"] = name;
    classEntity["MainTeacherId"] = mainTeacherId;
    classEntity["HelperTeacherId"] = helperTeacherId;
    classEntity["WarningOfStudentCount"] = warningOfStudentCount;
    classEntity["IsActive"] = isActive;
    classEntity["Description"] = description;


    var jsonData = "{ id:" + JSON.stringify(id) + ", classEntity: " + JSON.stringify(classEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/InsertOrUpdateClass', jsonData, successFunctionInsertOrUpdateClass, errorFunction);

    return false;

}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtClassName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Sınıf Adı boş bırakılamaz\n";


    //obje = document.getElementById("drpMainTeacher").value;
    //if (IsNullOrEmpty(obje))
    //    errorMessage += "Ana öğretmen Boş bırakılamaz\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function successFunctionInsertOrUpdateClass(obje) {

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
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteClass', jsonData, successFunctionDeleteClass, errorFunction);
    }

}

function successFunctionDeleteClass(obje) {
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
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetClassWithId', jsonData, successFunctionGetClassWithId, errorFunction);

}

function successFunctionGetClassWithId(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("hdnId").value = entity.Id;
        document.getElementById("txtClassName").value = entity.Name;
        document.getElementById("txtWarningOfStudentCount").value = entity.WarningOfStudentCount;
        document.getElementById("txtDescription").value = entity.Description;
        document.getElementById("drpMainTeacher").value = entity.MainTeacherId;
        document.getElementById("drpHelperTeacher").value = entity.HelperTeacherId;
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
    document.getElementById("txtClassName").value = "";
    document.getElementById("txtDescription").value = "";
    document.getElementById("txtWarningOfStudentCount").value = "";
    document.getElementById("drpMainTeacher").value = 0;
    document.getElementById("drpHelperTeacher").value = 0;
    document.getElementById("chcIsActive").checked = true;
    document.getElementById("btnSubmit").value = "Kaydet";

}

function cancel() {
    setDefaultValues();
    return false;
}
