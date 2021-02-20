window.onload = function () {

    initializeValue();
};

function initializeValue() {

    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        txtSearchStudent_Change(searchValue);
    }
    else {
        loadData();
    }
}

function GetStudentList() {

    var studentList = window["studentList"];
    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('KinderGartenWebService.asmx/GetAllStudent', jsonData, successGetAllStudent, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

}

function loadData() {

    successFunctionSearchStudent(null);
}

function txtSearchStudent_Change(searchValue) {

    if ((document.getElementById('tBodyStudentList') != null))
        successFunctionSearchStudent(searchValue);

    SetCacheData("searchValue", searchValue);
}

function successGetAllStudent(obje) {

    var entityList = obje;
    if (entityList != null) {
        window["studentList"] = obje;
    }
}

function successFunctionSearchStudent(search) {

    var objects = GetStudentList();
    var entityList = [];

    if (!IsNullOrEmpty(search)) {

        var toSearch = replaceTurkichChar(search.toLocaleLowerCase('tr-TR'));

        for (var i = 0; i < objects.length; i++) {

            if (objects[i]["CitizenshipNumber"] != null && replaceTurkichChar(objects[i]["CitizenshipNumber"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
                entityList.push(objects[i]);
            }

            else if (objects[i]["FullName"] != null && replaceTurkichChar(objects[i]["FullName"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
                entityList.push(objects[i]);
            }

            else if (objects[i]["FatherName"] != null && replaceTurkichChar(objects[i]["FatherName"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
                entityList.push(objects[i]);
            }

            else if (objects[i]["MotherName"] != null && replaceTurkichChar(objects[i]["MotherName"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
                entityList.push(objects[i]);
            }

            else if (objects[i]["FatherPhoneNumber"] != null && replaceTurkichChar(objects[i]["FatherPhoneNumber"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
                entityList.push(objects[i]);
            }

            else if (objects[i]["MotherPhoneNumber"] != null && replaceTurkichChar(objects[i]["MotherPhoneNumber"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
                entityList.push(objects[i]);
            }
        }
    }
    else {
        entityList = objects;
    }

    if (entityList != null) {

        var tbody = "";
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"AddStudent.aspx?Id=" + entityList[i].EncryptId + "\"><img src =\"img/icons/update1.png\"/></a>";
            tbody += "<a href = \"#\"><img src =\"img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";
            tbody += "</td>";
            if (IsNullOrEmpty(entityList[i].CitizenshipNumber))
                tbody += "<td></td>";
            else
                tbody += "<td>" + entityList[i].CitizenshipNumber + "</td>";
            tbody += "<td>" + entityList[i].FullName + "</td>";
            tbody += "<td>" + entityList[i].DateFormat + "</td>";
            tbody += "<td>" + entityList[i].FatherInfo + "</td>";
            tbody += "<td>" + entityList[i].MotherInfo + "</td>";

            if (entityList[i].IsStudent)
                tbody += "<td>&nbsp;<img src='img/icons/student.png' width='20' height ='20' /></td>";
            else
                tbody += "<td>&nbsp;<img src='img/icons/interview.png' width='23' height ='23' /></td>";


            if (entityList[i].IsActive)
                tbody += "<td><img src='img/icons/active.png' width='20' height ='20' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='23' height ='23' /></td>";


            tbody += "</tr> ";
        }
        document.getElementById("tBodyStudentList").innerHTML = tbody;
    }
}


function deleteCurrentRecord(id) {

    if (confirm('Silme işlemine devam etmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('KinderGartenWebService.asmx/DeleteStudent', jsonData, successFunctionDeleteStudent, errorFunction);
    }
}

function successFunctionDeleteStudent(obje) {
    if (!obje.HasError && obje.Result) {
        window["studentList"] = null;
        if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value))
            successFunctionSearchStudent(document.getElementById("txtSearchStudent").value);
        else
            successFunctionSearchStudent(null);

        callDeleteInformationMessage();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}