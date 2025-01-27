﻿window.onload = function () {

    loadData();
    toggleMenu();
};

function onChangeChcOldInterview() {
    interviewStudent();
}

function onClassNameChanged() {

    if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value)) {
        loadData();
    }
    else {
        if (document.getElementById("drpClassList").value > 0) {
            loadData();
        }
        else {
            activeStudent();
        }
    }
}

function setDefaultValues(studentList) {

    //var allObje = document.getElementById("lblAllStudent");
    var activeObje = document.getElementById("lblActiveStudent");
    var interviewObje = document.getElementById("lblInterview");
    var passiveObje = document.getElementById("lblPassiveStudent");

    var activeCount = 0;
    var passiveCount = 0;
    var interviewCount = 0;


    for (var i = 0; i < studentList.length; i++) {

        if (studentList[i].IsActive && studentList[i].IsStudent) {
            activeCount++;
        }
        else if (!studentList[i].IsStudent && studentList[i].IsActive) {
            interviewCount++;
        }
        else if (!studentList[i].IsActive) {
            passiveCount++;
        }
    }

    setLabel(activeCount, activeObje, "Aktif");
    setLabel(interviewCount, interviewObje, "Görüşme");
    setLabel(passiveCount, passiveObje, "Pasif");

}

function setLabel(listCount, obje, text) {
    obje.innerHTML = "<div style='cursor: pointer;'>" + text + "&nbsp;Öğrenci Sayısı : " + listCount + "</div>";
}

function loadData() {

    var search = document.getElementById("txtSearchStudent").value;
    var objects = GetStudentList();
    setDefaultValues(objects);

    if (!IsNullOrEmpty(search)) {
        var list = GetStudentList();
        entityList = GetFilterStudent(list, search);
    }

    else if ((search != null && search.trim() == '')) {
        activeStudent();
        return;
    }
    else {
        entityList = objects;
    }

    if (document.getElementById("drpClassList") != null && !IsNullOrEmpty(document.getElementById("drpClassList").value)) {
        if (document.getElementById("drpClassList").value > 0) {

            var newEntityList = [];

            for (var i = 0; i < entityList.length; i++) {

                if (entityList[i]["ClassId"] == document.getElementById("drpClassList").value && entityList[i]["IsActive"] == true) {
                    newEntityList.push(entityList[i]);
                }
            }
            drawList(newEntityList);
        }
        else {
            drawList(entityList);
        }
    }
    else {
        drawList(entityList);
    }
}

function drawList(entityList) {

    var tbody = "";

    if (entityList == null || entityList.length <= 0) {
        window["tbody"] = tbody;
        document.getElementById("tBodyStudentList").innerHTML = tbody;
        return;
    }

    if (entityList != null) {
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"/ogrenci-guncelle/" + entityList[i].Id + "\"><img title='Güncelle' src =\"/img/icons/update1.png\"/></a> &nbsp; ";

            if (entityList[i].IsStudent == true) {
                tbody += "<a href = \"/odeme-plani-detay/" + entityList[i].Id + "\"><img title = 'Ödeme detayı' src =\"/img/icons/paymentPlan.png\"/></a> &nbsp; ";
                tbody += "<a href = \"/email-gonder/" + entityList[i].Id + "\"><img title = 'Email gönder' src =\"/img/icons/email4.png\"/></a> &nbsp; ";
            }

            tbody += "</td>";
            tbody += "<td style='cursor: pointer;' onclick =_onDetailRow(\"" + entityList[i].Id + "\") id='tdPlus_" + entityList[i].Id + "' >+</td>";

            tbody += "<td>" + entityList[i].FullName + "</td>";

            var parentInfo = entityList[i].ParentName + " - ";
            parentInfo += "<a href='tel:" + entityList[i].ParentPhoneNumber + "'>" + entityList[i].ParentPhoneNumber + "</a>";

            tbody += "<td>" + parentInfo + "</td>";

            if (entityList[i].IsActive) {
                if (entityList[i].IsStudent)
                    tbody += "<td>&nbsp;<img src='/img/icons/student3.png' width='20' height ='20' /></td>";
                else
                    tbody += "<td>&nbsp;<a href = \"#\"><img title='Öğrenciye Çevir' src='/img/icons/smile.png' width='23' height ='23' onclick='convertStudent(\"" + entityList[i].Id + "\")' /></a></td>";
            }
            else {
                tbody += "<td>&nbsp;<img src='/img/icons/passive.png' width='20' height ='20' /></td>";
            }

            if (entityList[i].IsInterview) {
                tbody += "<td>" + entityList[i].InterviewWithFormat + "</td>";
            }
            else {
                tbody += "<td> </td>";
            }

            tbody += "</tr> ";

            tbody += _getDetailRow(entityList[i], 2, 8, entityList[i].Id);

        }

        window["tbody"] = tbody;

        document.getElementById("tBodyStudentList").innerHTML = tbody;
    }
}

function deleteCurrentRecord(id) {

    if (confirm('Kayıt silinecektir, işleme devam etmek istediğinize emin misiniz?')) {
        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteStudent', jsonData, successFunctionDeleteStudent, errorFunction);
    }
    else
        return false;
}

function convertStudent(id) {

    if (confirm('Görüşme olan kaydı öğrenciye çevirmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/ConvertStudent', jsonData, successFunctionConvertStudent, errorFunction);
    }
}

function successFunctionDeleteStudent(obje) {
    if (!obje.HasError && obje.Result) {
        window["tbody"] = null;
        GetActiveStudentList();

        callDeleteInformationMessage();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function successFunctionConvertStudent(obje) {
    if (!obje.HasError && obje.Result) {
        window["tbody"] = null;
        if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value))
            loadData();
        else
            loadData();

        alert("Görüşme başarılı bir şekilde öğrenciye çevrilmiştir.");

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function allStudent() {

    document.getElementById("drpClassList").value = "-1";
    var objects = GetStudentList();

    drawList(objects, true);
    setMenuBold(1);
}

function activeStudent() {

    var classId = document.getElementById("drpClassList").value;

    setVisibleItems(StudentListType.ActiveStudent);

    var divOldInterview = document.getElementById("divOldInterview");
    divOldInterview.style.display = "none";

    var objects = GetStudentList();
    var entityList = [];

    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsActive"] === true && objects[i]["IsStudent"] == true) {

            if (classId != undefined && classId > 0) {
                if (objects[i]["ClassId"] == classId) {
                    entityList.push(objects[i]);
                }
            }
            //Sınıf ataması yapılmayan öğrenciler
            else if (classId != undefined && classId == -2) {
                if (IsNullOrEmpty(objects[i]["ClassId"]) || objects[i]["ClassId"] == 0) {
                    entityList.push(objects[i]);
                }
            }
            else {
                entityList.push(objects[i]);
            }
        }
    }

    drawList(entityList);
    setMenuBold(2);

}

function interviewStudent() {

    setVisibleItems(StudentListType.InterviewStudent);

    var chcInterview = document.getElementById("chcInterview");

    document.getElementById("drpClassList").value = "-1";
    var objects = GetStudentList();
    var entityList = [];

    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsStudent"] === false && objects[i]["IsActive"] === true) {

            //entityList.push(objects[i]);

            var interviewDate = objects[i].InterviewDateWithFormat;
            var parts = interviewDate.split('-');

            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);

            var _myInterviewDate = new Date();
            _myInterviewDate.setHours(0, 0, 0, 0);


            if (parts.length == 3) {
                _myInterviewDate = new Date(parts[0], parts[1] - 1, parts[2]);
            }

            if (chcInterview.checked) {
                if (_myInterviewDate != null && _myInterviewDate.getTime() < currentDate.getTime()) {
                    entityList.push(objects[i]);
                }
            }
            else if (IsNullOrEmpty(interviewDate) || _myInterviewDate.getTime() >= currentDate.getTime()) {
                entityList.push(objects[i]);
            }
        }
    }

    drawList(entityList);
    setMenuBold(3);

}

function setVisibleItems(studentListType) {

    var divOldInterview = document.getElementById("divOldInterview");
    divOldInterview.style.display = "none";

    var lblClassName = document.getElementById("lblClassName");
    lblClassName.style.display = "none";

    var divClassList = document.getElementById("divClassList");
    divClassList.style.display = "none";


    if (studentListType == StudentListType.ActiveStudent) {
        lblClassName.style.display = "";
        divClassList.style.display = "";
    }
    else if (studentListType == StudentListType.PassiveStudent) {

    }
    else if (studentListType == StudentListType.InterviewStudent) {
        divOldInterview.style.display = "";
    }
}


function passiveStudent() {

    document.getElementById("drpClassList").value = "-1";

    setVisibleItems(StudentListType.PassiveStudent);

    var objects = GetStudentList();
    var entityList = [];


    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsActive"] === false) {
            entityList.push(objects[i]);
        }
    }
    drawList(entityList);
    setMenuBold(4);

}

function setMenuBold(menuId) {

    //var allObje = document.getElementById("lblAllStudent");
    var activeObje = document.getElementById("lblActiveStudent");
    var interviewObje = document.getElementById("lblInterview");
    var passiveObje = document.getElementById("lblPassiveStudent");

    //removeBold(allObje);
    removeBold(activeObje);
    removeBold(interviewObje);
    removeBold(passiveObje);

    //if (menuId == 1) {
    //    addBold(allObje);
    //}
    //else 
    if (menuId == 2) {
        addBold(activeObje);
    }
    else if (menuId == 3) {
        addBold(interviewObje);
    }
    else if (menuId == 4) {
        addBold(passiveObje);
    }

}

function removeBold(obje) {
    var html = obje.innerHTML;
    html = html.replace(/<b>/g, "");
    html = html.replace(/<\/b>/g, "");
    html = html.replace(/<h4>/g, "");
    html = html.replace(/<\/h4>/g, "");
    html = html.replace(/<u>/g, "");
    html = html.replace(/<\/u>/g, "");

    obje.innerHTML = html;

}

function addBold(obje) {
    var html = obje.innerHTML;
    html = "<div style='cursor: pointer;'><b><h4><u>" + html + "</u></h4></b></div>";
    obje.innerHTML = html;
}