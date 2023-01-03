window.onload = function () {

    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        successFunctionSearchStudent(searchValue);
    }
    else {
        successFunctionSearchStudent(null, true);
    }
};

function txtSearchStudent_Change(searchValue) {

    successFunctionSearchStudent(searchValue);
    SetCacheData("searchValue", searchValue);
}

function GetStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudent', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

}

function onClassNameChanged() {

    if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value)) {
        successFunctionSearchStudent(document.getElementById("txtSearchStudent").value);
    }
    else {
        if (document.getElementById("drpClassList").value > 0) {
            successFunctionSearchStudent(null);
        }
        else {
            activeStudent();
        }
    }
}


function successFunctionCurrentPage(obje) {

    var entityList = obje;
    if (entityList != null) {
        window["studentList"] = obje;
    }
}

function setDefaultValues(studentList) {

    var allObje = document.getElementById("lblAllStudent");
    var activeObje = document.getElementById("lblActiveStudent");
    var interviewObje = document.getElementById("lblInterview");
    var passiveObje = document.getElementById("lblPassiveStudent");

    setLabel(studentList.length, allObje, "Toplam");

    var activeCount = 0;
    var passiveCount = 0;
    var interviewCount = 0;


    for (var i = 0; i < studentList.length; i++) {

        if (studentList[i].IsActive && studentList[i].IsStudent) {
            activeCount++;
        }
        else if (!studentList[i].IsStudent && studentList[i].IsActive)
        {
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
    obje.innerHTML = "<div style='cursor: pointer;'>" + text + "&nbsp;Öğrenci Sayısı : " + listCount +"</div>";
}

function successFunctionSearchStudent(search,isLoadedFirst) {

    var objects = GetStudentList();
    setDefaultValues(objects);

    if (isLoadedFirst || (search != null && search.trim() == '')) {
        activeStudent();
        return;
    }
    else if (!IsNullOrEmpty(search)) {
        entityList = GetFilterStudent(studentList, search);
    }
    else {
        entityList = objects;
    }

    if (document.getElementById("drpClassList") != null && !IsNullOrEmpty(document.getElementById("drpClassList").value)) {
        if (document.getElementById("drpClassList").value > 0) {

            var newEntityList = [];

            for (var i = 0; i < entityList.length; i++) {

                if (entityList[i]["ClassId"] == document.getElementById("drpClassList").value) {
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
            tbody += "<td style='cursor: pointer;' onclick =onDetailRow(\"" + entityList[i].EncryptId + "\") >+</td>";
            tbody += "<td>";
            tbody += "<a href = \"/ogrenci-guncelle/" + entityList[i].EncryptId + "\"><img title='Güncelle' src =\"/img/icons/update1.png\"/></a> &nbsp; ";       
            //tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" title ='Sil' onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";

            if (entityList[i].IsStudent == true) {
                tbody += "<a href = \"/odeme-plani-detay/" + entityList[i].EncryptId + "\"><img title = 'Ödeme detayı' src =\"/img/icons/paymentPlan.png\"/></a> &nbsp; ";
                tbody += "<a href = \"/email-gonder/" + entityList[i].EncryptId + "\"><img title = 'Email gönder' src =\"/img/icons/email4.png\"/></a> &nbsp; ";
            }

            var parentInfo = entityList[i].MotherInfo;
            if (IsNullOrEmpty(parentInfo))
                parentInfo += " - " + entityList[i].FatherInfo;
           

            tbody += "</td>";
            tbody += "<td>" + entityList[i].FullName + "</td>";
            //tbody += "<td>" + entityList[i].BirthdayWithFormat + "</td>";
            tbody += "<td>" + parentInfo + "</td>";
            if (entityList[i].SchoolClass != undefined && entityList[i].SchoolClass != null && entityList[i].SchoolClass != '') {
                tbody += "<td>" + entityList[i].SchoolClass + "</td>";
            }
            else {
                tbody += "<td> - </td>";

            }

            if (entityList[i].IsActive) {
                if (entityList[i].IsStudent)
                    tbody += "<td>&nbsp;<img src='img/icons/student3.png' width='20' height ='20' /></td>";
                else
                    tbody += "<td>&nbsp;<a href = \"#\"><img title='Öğrenciye Çevir' src='/img/icons/interview.png' width='23' height ='23' onclick='convertStudent(\"" + entityList[i].EncryptId + "\")' /></a></td>";
            }
            else {
                tbody += "<td>&nbsp;<img src='img/icons/passive.png' width='20' height ='20' /></td>";
            }

            if (entityList[i].IsInterview)
                tbody += "<td>&nbsp;<img src='img/icons/paymentOk.png' width='20' height ='20' /></td>";
            else
                tbody += "<td> </td>";

            if (entityList[i].IsInterview) {
                tbody += "<td>" + entityList[i].InterviewWithFormat + "</td>";
            }
            else {
                tbody += "<td> </td>";
            }

            
            tbody += "</tr> ";

            tbody += "<tr style='display: none;' id='tr" + entityList[i].EncryptId + "' >";
            {
                tbody += "<td colspan=2></td >";
                tbody += "<td colspan=6>";
                {
                    tbody += "<table border='1' width='100%' cellpadding='8'>";
                    {
                        tbody += "<tr><td width='150'><b>TCKN</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].CitizenshipNumberStr + "</td></tr>";

                        tbody += "<tr><td width='150'><b>Konuşulan ücret</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].SpokenPriceStr + "</td></tr>";
                        tbody += "<tr><td width='150'><b>Sınıf</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].ClassName + "</td></tr>";

                        tbody += "<tr><td width='150'><b>Ana Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].MainTeacher + "</td></tr>";
                        tbody += "<tr><td width='150'><b>Yardımcı Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].HelperTeacher + "</td></tr>";

                        tbody += "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" + entityList[i].DateOfMeetingWithFormat + "</td></tr>";
                        tbody += "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" + entityList[i].EmailStr + "</td></tr>";
                        tbody += "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" + entityList[i].NotesStr + "</td></tr>";
                        if (entityList[i].SchoolClass != undefined && entityList[i].SchoolClass != null && entityList[i].SchoolClass != '') {
                            tbody +="<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'>" + entityList[i].SchoolClass + "</td></tr>";
                        }
                        else {
                            tbody += "<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'> - </td></tr>";

                        }


                        if (entityList[i].IsActive)
                            tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/active.png' width='20' height ='20' /></td></tr>";
                        else
                            tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/passive.png' width='20' height ='20' /></td></tr>";

                        tbody += "<tr><td><input type='submit' value='Sil' id='btnDelete' class='btn btn-danger' onclick='return deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></td><td>&nbsp;</td><td>&nbsp;</td></tr>";

                       

                    }
                    tbody += "</table>";
                }
                tbody += "</td > ";
            }
            tbody += "</tr>";

        }

        window["tbody"] = tbody;

        document.getElementById("tBodyStudentList").innerHTML = tbody;
    }
}


function onDetailRow(id) {
    var row = document.getElementById("tr" + id);
    row.style.display = row.style.display === 'none' ? '' : 'none';
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
        window["studentList"] = null;
        window["tbody"] = null;
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

function successFunctionConvertStudent(obje) {
    if (!obje.HasError && obje.Result) {
        window["studentList"] = null;
        window["tbody"] = null;
        if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value))
            successFunctionSearchStudent(document.getElementById("txtSearchStudent").value);
        else
            successFunctionSearchStudent(null);

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

    var text = document.getElementById("lblActiveStudent").innerHTML;


    document.getElementById("drpClassList").value = "-1";

    var objects = GetStudentList();
    var entityList = [];

    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsActive"] === true && objects[i]["IsStudent"] == true) {
            entityList.push(objects[i]);
        }
    }

    drawList(entityList);
    setMenuBold(2);

}

function interviewStudent() {

    document.getElementById("drpClassList").value = "-1";
    var objects = GetStudentList();
    var entityList = [];

    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsStudent"] === false && objects[i]["IsActive"] === true ) {
            entityList.push(objects[i]);
        }
    }

    drawList(entityList);
    setMenuBold(3);

}

function passiveStudent() {

    document.getElementById("drpClassList").value = "-1";
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

    var allObje = document.getElementById("lblAllStudent");
    var activeObje = document.getElementById("lblActiveStudent");
    var interviewObje = document.getElementById("lblInterview");
    var passiveObje = document.getElementById("lblPassiveStudent");

    removeBold(allObje);
    removeBold(activeObje);
    removeBold(interviewObje);
    removeBold(passiveObje);

    if (menuId == 1) {
        addBold(allObje);
    }
    else if (menuId == 2) {
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